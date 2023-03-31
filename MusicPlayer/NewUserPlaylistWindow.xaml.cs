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
    /// Логика взаимодействия для NewAlbomWindow.xaml
    /// </summary>
    public partial class NewAlbomWindow : Window
    {
        public string TitleUserPlaylist { get; set; }
        public NewAlbomWindow()
        {
            InitializeComponent();
        }

        public NewAlbomWindow(string titleUserPlaylist)
        {
            InitializeComponent();

            txtTitle.Visibility = Visibility.Hidden;

            TitleUserPlaylist = titleUserPlaylist;
            txtBoxTitlePlaylist.Text = TitleUserPlaylist;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(txtBoxTitlePlaylist.Text != String.Empty)
            {
                TitleUserPlaylist = txtBoxTitlePlaylist.Text;
                DialogResult = true;
            }

        }
    }
}
