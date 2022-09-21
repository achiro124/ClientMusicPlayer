using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace MusicPlayer
{
    public partial class MainWindow : Window
    {
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        List<Audio> audios = new List<Audio>();
        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = audios;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
        }
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
        private void timer_Tick(object sender, EventArgs e)
        {
            if ((mePlayer.Source != null) && (mePlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sliProgress.Value = mePlayer.Position.TotalSeconds;
                sliProgress.Minimum = 0;
                sliProgress.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;
            }
            if(sliProgress.Value == sliProgress.Maximum)
            {
                audiosList.SelectedIndex = audiosList.SelectedIndex != audios.Count - 1 ? ++audiosList.SelectedIndex : 0;
                mePlayer.Source = new Uri(audios[audiosList.SelectedIndex].Path);
                textBlock_Title.Text = audios[audiosList.SelectedIndex].Title;
            }
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mePlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            mePlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
            slVolume.Value = mePlayer.Volume;
            textBlockVolumeStatus.Text = (Math.Floor(slVolume.Value * 100)).ToString();
        }

        private void sliVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mePlayer.Volume = slVolume.Value;
            textBlockVolumeStatus.Text = (Math.Floor(slVolume.Value * 100)).ToString();
        }

        //  private void ChangeAudio_Click(object sender, EventArgs e)
        //  {
        //      Button button = sender as Button;
        //      for(int i = 0; i < listButtons.Count; i++)
        //      {
        //          if (button.Name == listButtons[i].Name)
        //          {
        //              mePlayer.Source = new Uri($"{audios[i].Path}", UriKind.RelativeOrAbsolute);
        //              slVolume.Value = 0.5;
        //              sliProgress.Value = 0;
        //              textBlock_Title.Text = audios[i].Title;
        //              textBlock_Musician.Text = audios[i].Musician;
        //              index = i;
        //              break;
        //          }
        //      }
        //  }

        private void NewAudio_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                foreach(var file in openFileDialog.FileNames)
                {
                    //mePlayer.Source = new Uri(openFileDialog.FileName);
                    FileInfo fileInf = new FileInfo(file);
                    Audio audio = new Audio(fileInf.Name.TrimEnd(new char[] {'.','m','p','3'}), file);
                    audios.Add(audio);

                }    
            }
        }

        private void Selected_Audio(object sender, EventArgs e)
        {
            Audio audio = audiosList.SelectedItem as Audio;
            if (audio is null) return;
            textBlock_Title.Text = audio.Title;
            mePlayer.Source = new Uri(audio.Path);
        }
    }
}
