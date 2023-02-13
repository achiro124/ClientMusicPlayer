﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MusicPlayer.ServiceAudio;
using System.Windows.Markup;
using AudioPlayerService;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MusicPlayer
{
    public partial class MainWindow : Window
    {


        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        private bool cycleAudioList = false;

        private DispatcherTimer timer = new DispatcherTimer();
        public ServiceAudioPlayerClient client;
        private ObservableCollection<Audio> audios;
        //private ObservableCollection<Audio> favoritesAudios;
        //private string lastAudio = "";
        private int countAudio = 0;
        private User user;

        public MainWindow(ServiceAudioPlayerClient client, User user)
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;

            this.client = client;
            this.user = user;
        }

        //Инициализация службы во время загрузки окна приложения
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            audios = new ObservableCollection<Audio>(client.GetAudioList(user.UserId).ToList());
            audiosList.ItemsSource = audios;
            spUser.DataContext = user;
        }

        //Пауза или старт аудио
        private void Play_Or_Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (mediaPlayerIsPlaying)
            {
                mePlayer.Pause();
                mediaPlayerIsPlaying = false;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri(@"/MusicPlayer;component/Image/ButtonPlay.png", UriKind.RelativeOrAbsolute));
                ButtonStart.Content = image;

            }
            else
            {
                mePlayer.Play();
                mediaPlayerIsPlaying = true;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri(@"/MusicPlayer;component/Image/ButtonPause.png", UriKind.RelativeOrAbsolute));
                ButtonStart.Content = image;
                timer.Start();
            }
        }

        //Работа аудио записи на всем этапе работы программы
        private void timer_Tick(object sender, EventArgs e)
        {
            if ((mePlayer.Source != null) && (mePlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sliProgress.Value = mePlayer.Position.TotalSeconds;
                sliProgress.Minimum = 0;
                sliProgress.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;
            }
            if (sliProgress.Value == sliProgress.Maximum)
            {
                if (cycleAudioList)
                {
                    audiosList.SelectedIndex = audiosList.SelectedIndex != audios.Count - 1 ? ++audiosList.SelectedIndex : 0;
                }
                else
                {
                    if (audiosList.SelectedIndex == audios.Count - 1)
                        return;
                    audiosList.SelectedIndex = audiosList.SelectedIndex + 1;
                }
            }
        }

        //Изменение флага при перетаскивании ползнунка
        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }
        //Перемотка аудио записи
        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mePlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }

        //Изменение громкости звука по колесику мыши
        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            mePlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
            slVolume.Value = mePlayer.Volume;
            //textBlockVolumeStatus.Text = (Math.Floor(slVolume.Value * 100)).ToString();
        }

        //Измнение громкости звука через ползунок
        private void sliVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mePlayer.Volume = slVolume.Value;
            textBlockVolumeStatus.Text = (Math.Floor(slVolume.Value * 100)).ToString();
            if (slVolume.Value == 0)
            {
                imageVolume.Source = new BitmapImage(new Uri(@"/MusicPlayer;component/Image/Mute.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                imageVolume.Source = new BitmapImage(new Uri(@"/MusicPlayer;component/Image/Volume.png", UriKind.RelativeOrAbsolute));
            }

        }

        //Добавление новых аудио 
        private void NewAudio_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var file in openFileDialog.FileNames)
                {
                    FileInfo fileInf = new FileInfo(file);
                    Audio audio = new Audio { Title = fileInf.Name.TrimEnd(new char[] { '.', 'm', 'p', '3' }), Path = file };
                    audios.Add(audio);
                }
            }
        }

        //Выбор аудио из списка
        //Обращение к серверу в целях получить аудиозапись
        private void Selected_Audio(object sender, EventArgs e)
        {
            Audio audio = audiosList.SelectedItem as Audio;
            if (audio is null)
                return;

            if (audio.Path == null)
            {
                byte[] compressAudio = client.GetAudioFile(audio.Title);
                File.WriteAllBytes(@"Audios\" + audio.Title + ".mp3", compressAudio);
                FileInfo fileInfo = new FileInfo(@"Audios\" + audio.Title + ".mp3");
                audio.Path = fileInfo.FullName;
                countAudio++;
            }

            if (countAudio == 15)
            {
                DeleteAudios();
                countAudio = 0;
            }

            mePlayer.Source = new Uri(audio.Path);
            //lastAudio = audio.Title;
            textBlock_Title.Text = audio.Title;
            gridInfAudio.DataContext = audios[audiosList.SelectedIndex];
        }

        //Команда на активацию цикла для аудио
        private void CycleAudioList_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (cycleAudioList)
            {
                cycleAudioList = false;
                ButtonCycle.Background = new SolidColorBrush(Colors.White);
            }
            else
            {
                cycleAudioList = true;
                ButtonCycle.Background = new SolidColorBrush(Color.FromRgb(169, 169, 169));
            }
        }

        //Команда перехода на след. аудио
        private void NextAudio_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (cycleAudioList)
            {
                audiosList.SelectedIndex = audiosList.SelectedIndex != audios.Count - 1 ? ++audiosList.SelectedIndex : 0;
            }
            else
            {
                if (audiosList.SelectedIndex == audios.Count - 1)
                    return;
                audiosList.SelectedIndex = audiosList.SelectedIndex + 1;
            }
        }

        //Команда перехода на пред. аудио
        private void PrevAudio_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (cycleAudioList)
            {
                audiosList.SelectedIndex = audiosList.SelectedIndex != 0 ? --audiosList.SelectedIndex : audiosList.SelectedIndex = audios.Count - 1;
            }
            else
            {
                if (audiosList.SelectedIndex == 0)
                    return;
                audiosList.SelectedIndex = audiosList.SelectedIndex - 1;
            }
        }

        //Очистка кеша перед закрытием
        private void Window_Closed(object sender, EventArgs e)
        {
            DeleteAudios();
        }
        void DeleteAudios()
        {
            DirectoryInfo di = new DirectoryInfo("Audios");

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            foreach (var audio in audios)
            {
                audio.Path = null;
            }
        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            UserWindow userWindow = new UserWindow(client,user);
            userWindow.Show();
        }

        private void favorites_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnFavorite_Click(object sender, RoutedEventArgs e)
        {
            if(audiosList.SelectedIndex != -1)
            {
                int audioId = audios[audiosList.SelectedIndex].Id;
                if (!audios[audiosList.SelectedIndex].Favorite)
                {
                    audios[audiosList.SelectedIndex].Favorite = true;
                    client.AddFavoriteAudio(user.UserId, audioId);
                }
                else
                {
                    audios[audiosList.SelectedIndex].Favorite = false;
                    client.DeleteFavoriteAudio(user.UserId, audioId);
                }


            }
        }
    }
}
