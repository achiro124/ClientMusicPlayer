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
        public static RoutedCommand NextAudio { get; set; }
        public static RoutedCommand PrevAudio { get; set; }
        public static RoutedCommand CycleAudioList { get; set; }        

        static MediaPlayerCommands()
        {
            PlayOrPause = new RoutedCommand("PlayOrPause", typeof(Button));
            NextAudio = new RoutedCommand("NextAudio", typeof(Button));
            PrevAudio = new RoutedCommand("PrevAudio", typeof(Button));
            CycleAudioList = new RoutedCommand("CycleAudioList", typeof(Button));
            
        }
    }
}
