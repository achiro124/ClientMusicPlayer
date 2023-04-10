using AudioPlayerLibrary;
using AudioPlayerService;
using MusicPlayer.AudioPlayerService;
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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {

        public User user { get; set; }
        ServiceAudioPlayerClient client;

        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(pswBox1.Password != string.Empty && pswBox2.Password != string.Empty && txtBoxLogin.Text != string.Empty)
            {

                if(txtBoxLogin.Text.Length < 3)
                {
                    txtBlockError.Text = "Логин должен содержать как минимум 3 символа";
                    return;
                }

                if(pswBox1.Password.Length < 5)
                {
                    txtBlockError.Text = "Пароль должен содержать как минимум 5 символов";
                    return;
                }


                if(pswBox1.Password == pswBox2.Password) 
                {

                    var login = txtBoxLogin.Text;
                    var password = Md5.HashPassword(pswBox1.Password);

                    user = client.Registration(login, password);
                    if (user != null)
                    {
                        MainWindow mainWindow = new MainWindow(client, user);
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        txtBlockError.Text = "Пользователь с таким логином уже существует";
                    }
                }
                else
                {
                    txtBlockError.Text = "Пароли не совпадают";
                }
            }
            else
            {
                txtBlockError.Text = "Заполните пустые поля";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            client = new ServiceAudioPlayerClient(new System.ServiceModel.InstanceContext(this));
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            this.Close();
        }
    }
}
