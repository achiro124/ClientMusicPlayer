using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AudioPlayerService;
using System.ComponentModel;
using AudioPlayerLibrary;
using MusicPlayer.AudioPlayerService;
using System.Collections.Generic;

namespace MusicPlayer


{
    public partial class MainWindow : Window
    {
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        private bool cycleAudioList = false;

        private DispatcherTimer timer = new DispatcherTimer();
        private ServiceAudioPlayerClient client;



        public ObservableCollection<Audio> audios;
        public ObservableCollection<Audio> favoritesAudios;
        public ObservableCollection<Audio> myAudiosList = new ObservableCollection<Audio>();
        public ObservableCollection<UserAlboms> userPlaylist;



        private (string, ListSortDirection) typeSort = ("Title", ListSortDirection.Ascending);



        //private int selectedAudioId;
        private int countAudio = 0;
        private User user;
        private int selectedItem;

        ListBox mainListBox;


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
            audios = new ObservableCollection<Audio>(client.GetAudioList(user.UserId));
            favoritesAudios = new ObservableCollection<Audio>(client.GetFavoriteAudioList(user.UserId));
            userPlaylist = new ObservableCollection<UserAlboms>(client.GetUserAudiolist(user.UserId));
            //cmUserPlaylist.ItemsSource = userPlaylist;


            mainListBox = audiosList;

            btnAddAudio.Visibility = Visibility.Hidden;
            audiosList.ItemsSource = audios;
            audiosListFavorites.ItemsSource = favoritesAudios;
            spUser.DataContext = user;
            listUserAlboms.ItemsSource = userPlaylist;
            audiosListUser.ItemsSource = myAudiosList;


            countAudio = new DirectoryInfo(@"Audios").GetFiles().Length;
        }

        //Пауза или старт аудио
        private void Play_Or_Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Image image = new Image();
            image.Height = 36;
            image.Width = 38;
            
            if (mediaPlayerIsPlaying)
            {
                mePlayer.Pause();
                mediaPlayerIsPlaying = false;

                
                image.Source = new BitmapImage(new Uri(@"/MusicPlayer;component/Image/ButtonPlay.png", UriKind.RelativeOrAbsolute));
                ButtonStart.Content = image;

            }
            else
            {
                mePlayer.Play();
                mediaPlayerIsPlaying = true;
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
            if (sliProgress.Value == sliProgress.Maximum && !userIsDraggingSlider)
            {
                if (cycleAudioList)
                {
                    mainListBox.SelectedIndex = mainListBox.SelectedIndex != mainListBox.Items.Count - 1 ? ++mainListBox.SelectedIndex : 0;
                }
                else
                {
                    if (mainListBox.SelectedIndex == mainListBox.Items.Count - 1)
                        return;
                    mainListBox.SelectedIndex = mainListBox.SelectedIndex + 1;
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
                    myAudiosList.Add(audio);
                }
            }
        }

        //Выбор аудио из списка
        //Обращение к серверу в целях получить аудиозапись
        private void Selected_Audio(object sender, EventArgs e)
        {
            Audio audio = mainListBox.SelectedItem as Audio;

            if (audio is null)
                return;

            if (countAudio == 15)
            {
                DeleteAudios();
                countAudio = 0;
            }

            if (audios.FirstOrDefault(x => x.AudioId == audio.AudioId)?.Path == "")
            {
                byte[] compressAudio = client.GetAudioFile(audio.Title);
                File.WriteAllBytes(@"Audios\" + audio.Title + ".mp3", compressAudio);
                FileInfo fileInfo = new FileInfo(@"Audios\" + audio.Title + ".mp3");
                audios.FirstOrDefault(x => x.AudioId == audio.AudioId).Path = fileInfo.FullName;

                if(favoritesAudios.FirstOrDefault(x => x.AudioId == audio.AudioId) != null)
                    favoritesAudios.FirstOrDefault(x => x.AudioId == audio.AudioId).Path = fileInfo.FullName;

                audio.Path = fileInfo.FullName;
                countAudio++;
            }
            mePlayer.Source = new Uri(audio.Path);
            textBlock_Title.Text = audio.Title;
            gridInfAudio.DataContext = audio;
        }

        //Команда на активацию цикла для аудио
        private void CycleAudioList_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (cycleAudioList)
            {
                cycleAudioList = false;
                ButtonCycle.Background = new SolidColorBrush(Color.FromRgb(45, 37, 47));
            }
            else
            {
                cycleAudioList = true;
                ButtonCycle.Background = new SolidColorBrush(Color.FromRgb(95, 74, 100));
            }
        }

        //Команда перехода на след. аудио
        private void NextAudio_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (cycleAudioList)
            {
                mainListBox.SelectedIndex = mainListBox.SelectedIndex != mainListBox.Items.Count - 1 ? ++mainListBox.SelectedIndex : 0;
            }
            else
            {
                if (mainListBox.SelectedIndex == mainListBox.Items.Count - 1)
                    return;
                mainListBox.SelectedIndex = mainListBox.SelectedIndex + 1;
            }
        }

        //Команда перехода на пред. аудио
        private void PrevAudio_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (cycleAudioList)
            {
                mainListBox.SelectedIndex = mainListBox.SelectedIndex != 0 ? --mainListBox.SelectedIndex : mainListBox.SelectedIndex = mainListBox.Items.Count - 1;
            }
            else
            {
                if (mainListBox.SelectedIndex == 0)
                    return;
                mainListBox.SelectedIndex = mainListBox.SelectedIndex - 1;
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

            foreach (var item in audios)
            {
                item.Path = "";
            }
        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
          //  UserWindow userWindow = new UserWindow(client,user);
          //  userWindow.Owner = this;
          //  userWindow.Show();
        }

        private void favorites_Click(object sender, RoutedEventArgs e)
        {
            audiosList.Visibility = Visibility.Hidden;
            audiosListUser.Visibility = Visibility.Hidden;
            audiosListFavorites.Visibility = Visibility.Visible;

            txtBlock.Text = "Избранное";
            btnAddAudio.Visibility = Visibility.Hidden;

            mainListBox = audiosListFavorites;
        }

        private void btnMain_Click(object sender, RoutedEventArgs e)
        {
            audiosList.Visibility = Visibility.Visible;
            audiosListUser.Visibility = Visibility.Hidden;
            audiosListFavorites.Visibility = Visibility.Hidden;


            audiosList.ItemsSource = audios;
            txtBlock.Text = "Главная";
            btnAddAudio.Visibility = Visibility.Hidden;

            mainListBox = audiosList;
        }

        private void myAudios_Click(object sender, RoutedEventArgs e)
        {
            audiosList.Visibility = Visibility.Hidden;
            audiosListFavorites.Visibility = Visibility.Hidden;
            audiosListUser.Visibility = Visibility.Visible;
            
            txtBlock.Text = "Мои аудиозаписи";
            btnAddAudio.Visibility = Visibility.Visible;

            mainListBox = audiosListUser;
        }

        private void btnFavorite_Click(object sender, RoutedEventArgs e)
        {
            Audio audio = gridInfAudio.DataContext as Audio;
            if (audio == null || audio.Group == "")
                return;

            if (!audio.IsFavorites)
            {
                audios.FirstOrDefault(x => x.AudioId == audio.AudioId).IsFavorites = true;
                client.AddFavoriteAudio(user.UserId, audio.AudioId);
                favoritesAudios.Insert(0, audios.FirstOrDefault(x => x.AudioId == audio.AudioId));
            }
            else
            {
                audios.FirstOrDefault(x => x.AudioId == audio.AudioId).IsFavorites = false;
                favoritesAudios.FirstOrDefault(x => x.AudioId == audio.AudioId).IsFavorites = false;
                client.DeleteFavoriteAudio(user.UserId, audio.AudioId);
                favoritesAudios.Remove(favoritesAudios.FirstOrDefault(x => x.AudioId == audio.AudioId));
            }
            
        }

        private void btnAddFavorite_Click(object sender, RoutedEventArgs e)
        {
            Audio audio = mainListBox.Items[selectedItem] as Audio;
          
            if (!audio.IsFavorites)
            {
                audios.FirstOrDefault(x => x.AudioId == audio.AudioId).IsFavorites = true;
                client.AddFavoriteAudio(user.UserId, audio.AudioId);
                favoritesAudios.Insert(0, audios.FirstOrDefault(x => x.AudioId == audio.AudioId));
            }
            else
            {
                audios.FirstOrDefault(x => x.AudioId == audio.AudioId).IsFavorites = false;
                favoritesAudios.FirstOrDefault(x => x.AudioId == audio.AudioId).IsFavorites = false;
                client.DeleteFavoriteAudio(user.UserId, audio.AudioId);
                favoritesAudios.Remove(favoritesAudios.FirstOrDefault(x => x.AudioId == audio.AudioId));
            }
        }

        private void audiosList_MouseMove(object sender, MouseEventArgs e)
        {
            var item = VisualTreeHelper.HitTest(mainListBox, Mouse.GetPosition(mainListBox))?.VisualHit;

            // find ListViewItem (or null)
            while (item != null && !(item is ListBoxItem))
                item = VisualTreeHelper.GetParent(item);

            if (item != null)
            {
                selectedItem = mainListBox.Items.IndexOf(((ListBoxItem)item).DataContext);
            }
        }

        private void UserPlaylist_MouseMove(object sender, MouseEventArgs e)
        {
            var item = VisualTreeHelper.HitTest(listUserAlboms, Mouse.GetPosition(listUserAlboms))?.VisualHit;

            // find ListViewItem (or null)
            while (item != null && !(item is ListBoxItem))
                item = VisualTreeHelper.GetParent(item);

            if (item != null)
            {
                selectedItem = listUserAlboms.Items.IndexOf(((ListBoxItem)item).DataContext);
            }
        }

        private void TxtBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtBoxSearch.Text == string.Empty)
            {
                audiosList.ItemsSource = audios;
            }
            else
            {
                audiosList.ItemsSource = audios?.Where(x =>  x.Group.ToLower().Contains(txtBoxSearch.Text.ToLower()) ||  
                                                            x.Title.ToLower().Contains(txtBoxSearch.Text.ToLower()));
            }
            txtBlock.Text = "Главная";
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            MenuItem menuItem;
            if (item != null) 
            {
                int k = cmSort.Items.IndexOf(item);
                switch (k)
                {
                    case 0:
                        menuItem = (MenuItem)cmSort.Items[0];
                        menuItem.FontWeight = FontWeights.Bold;
                        menuItem = (MenuItem)cmSort.Items[1];
                        menuItem.FontWeight = FontWeights.Normal;


                        typeSort.Item1 = "Title";
                        mainListBox.Items.SortDescriptions.Clear();
                        mainListBox.Items.SortDescriptions.Add(new SortDescription(typeSort.Item1, typeSort.Item2));
                        mainListBox.Items.Refresh();
                        break;
                    case 1:
                        menuItem = (MenuItem)cmSort.Items[1];
                        menuItem.FontWeight = FontWeights.Bold;
                        menuItem = (MenuItem)cmSort.Items[0];
                        menuItem.FontWeight = FontWeights.Normal;


                        typeSort.Item1 = "Group";
                        mainListBox.Items.SortDescriptions.Clear();
                        mainListBox.Items.SortDescriptions.Add(new SortDescription(typeSort.Item1, typeSort.Item2));
                        mainListBox.Items.Refresh();
                        break;
                    case 3:
                        menuItem = (MenuItem)cmSort.Items[3];
                        menuItem.FontWeight = FontWeights.Bold;
                        menuItem = (MenuItem)cmSort.Items[4];
                        menuItem.FontWeight = FontWeights.Normal;


                        typeSort.Item2 = ListSortDirection.Ascending;
                        mainListBox.Items.SortDescriptions.Clear();
                        mainListBox.Items.SortDescriptions.Add(new SortDescription(typeSort.Item1, typeSort.Item2));
                        mainListBox.Items.Refresh();
                        break;
                    case 4:
                        menuItem = (MenuItem)cmSort.Items[4];
                        menuItem.FontWeight = FontWeights.Bold;
                        menuItem = (MenuItem)cmSort.Items[3];
                        menuItem.FontWeight = FontWeights.Normal;


                        typeSort.Item2 = ListSortDirection.Descending;
                        mainListBox.Items.SortDescriptions.Clear();
                        mainListBox.Items.SortDescriptions.Add(new SortDescription(typeSort.Item1, typeSort.Item2));
                        mainListBox.Items.Refresh();
                        break;
                }
            }
        }

        private void btnSort_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cmSort.IsOpen = true;
        }

        private void btnEllipsisButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button button = sender as Button;

            if(button.ContextMenu == null)
            {
                MenuItem settingMenuItem = new MenuItem();
                settingMenuItem.Header = "Добавить в плейлист";
                settingMenuItem.Foreground = new SolidColorBrush(Colors.White);
                settingMenuItem.Background = new SolidColorBrush(Color.FromRgb(45, 37, 47));

                foreach (var item in userPlaylist)
                {
                    settingMenuItem.Items.Add(new MenuItem
                    {
                        Header = item.Title,
                        Foreground = new SolidColorBrush(Colors.White),
                        Background = new SolidColorBrush(Color.FromRgb(45, 37, 47))
                });
                }

                button.ContextMenu = new ContextMenu();
                button.ContextMenu.Background = new SolidColorBrush(Color.FromRgb(45, 37, 47));
                button.ContextMenu.Items.Add(settingMenuItem);
            }
            button.ContextMenu.IsOpen = true;
        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string text = textBlock.Text;
            txtBoxSearch.Text = text;
        }

        private void NewAlbom_Click(object sender, EventArgs e)
        {
            NewAlbomWindow newUserPlaylistWindow = new NewAlbomWindow();
            newUserPlaylistWindow.Owner = this;
            if(newUserPlaylistWindow.ShowDialog() == true)
            {
                UserAlboms newUserPlaylist = new UserAlboms
                {
                    User = this.user,
                    Title = newUserPlaylistWindow.TitleUserPlaylist
                };
                newUserPlaylist.AlbomId = client.AddUserAudiolist(user.UserId, newUserPlaylistWindow.TitleUserPlaylist);
                userPlaylist.Add(newUserPlaylist);
            }
        }

        private void DeleteUserPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            UserAlboms userAlboms = listUserAlboms.Items[selectedItem] as UserAlboms;
            if(userAlboms != null)
            {
                userPlaylist.Remove(userAlboms);
                client.DeleteUserAudiolist(user.UserId,userAlboms.AlbomId);
            }
        }

        private void btnEditAudioList_Click(object sender, RoutedEventArgs e)
        {
            NewAlbomWindow newUserPlaylistWindow = new NewAlbomWindow(userPlaylist[selectedItem].Title);
            newUserPlaylistWindow.Owner = this;
            if (newUserPlaylistWindow.ShowDialog() == true)
            {
                UserAlboms userAlboms = userPlaylist[selectedItem];
                userAlboms.Title = newUserPlaylistWindow.TitleUserPlaylist;
                client.EditUserAudiolist(userAlboms.AlbomId, userAlboms.Title);
            }
        }
    }
}
