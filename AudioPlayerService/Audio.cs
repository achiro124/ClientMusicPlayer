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
    [DataContract]
    public class Audio: INotifyPropertyChanged
    {
        [DataMember]
        private bool favorite = false;

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public byte[] Image { get; set; }

        [DataMember]
        public string Group { set; get; }

        [DataMember]
        public GenreType Genre { get; set; }

        [DataMember]
        public string Path { get; set; }

        [DataMember]
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
