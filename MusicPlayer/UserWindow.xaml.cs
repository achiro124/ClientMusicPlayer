using AudioPlayerService;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MusicPlayer
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        //public User user { get; set; }
       // public ServiceAudioPlayerClient client;
        public UserWindow()
        {
            InitializeComponent();
           // this.user = user;
           // this.client = client;
           // spUser.DataContext = user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
          //  OpenFileDialog openFileDialog = new OpenFileDialog();
          //  openFileDialog.Multiselect = true;
          //  openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
          //  openFileDialog.RestoreDirectory = true;
          //  openFileDialog.Filter = "Images files (*.jpeg;*.jpg;*.png)|*.jpg;*.png;*.jpeg|All files (*.*)|*.*";
          //  if (openFileDialog.ShowDialog() == true)
          //  {
          //      FileInfo fileInf = new FileInfo(openFileDialog.FileName);
          //      byte[] icon = File.ReadAllBytes(fileInf.FullName);
          //      user.Icon = icon;
          //      client.EditUserIcon(user.UserId,icon);
          //      ElUserIcon.DataContext = user;
          //  }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            this.Owner.Close();
            this.Close();
        }
    }
}
