using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MusicPlayer
{
    public class MediaPlayerCommands
    {
        public static RoutedCommand PlayOrPause { get; set; }

        static MediaPlayerCommands()
        {
            PlayOrPause = new RoutedCommand("PlayOrPause", typeof(Button));
        }
    }
}
