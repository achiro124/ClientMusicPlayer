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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {

        public User user { get; set; }
        ServiceAudioPlayerClient client;

        public RegistrationWindow(ServiceAudioPlayerClient client)
        {
            InitializeComponent();
            this.client = client;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            client.Registration("Fadeev","ttt");
            DialogResult = false;
        }
    }
}
