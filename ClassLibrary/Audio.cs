using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    public class Audio : INotifyPropertyChanged
    {
        private bool favorite = false;

        public int Id { get; set; }

        public string Title { get; set; }

        public byte[] Image { get; set; }

        public string Group { set; get; }

        public GenreType Genre { get; set; }

        public string Path { get; set; }

        public bool Favorite
        {
            get
            {
                return favorite;
            }
            set
            {
                favorite = value;
                PropertyChange("Favorite");
            }
        }

        public bool NotAddAudio { get; set; }
        

        public event PropertyChangedEventHandler PropertyChanged;


        private void PropertyChange([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }

    public enum GenreType
    {
        Blues,
        Country,
        Electronic,
        Pop,
        Rock,
        Hip_Hop
    }
}
