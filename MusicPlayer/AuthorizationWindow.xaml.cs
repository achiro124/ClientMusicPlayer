using AudioPlayerService;
using MusicPlayer.ServiceAudio;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {

        public User user { get; set; }
        ServiceAudioPlayerClient client;

        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxLogin.Text != string.Empty && pswBox1.Password != string.Empty)
            {
                user = client.Authorization(txtBoxLogin.Text, pswBox1.Password);
                if(user != null)
                {
                    MainWindow mainWindow = new MainWindow(client,user);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    txtBlockError.Text = "Логин и(или) пароль заполнены неверно";
                }
            }
            else
            {
                txtBlockError.Text = "Необходимо заполнить все поля";
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            client = new ServiceAudioPlayerClient(new System.ServiceModel.InstanceContext(this));
        }
    }
}
