using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayerLibrary
{
    [DataContract]
    public class Audio : INotifyPropertyChanged
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public int AudioId { get; set; }
        [DataMember]
        public string Title { get; set; } = "";
        [DataMember]
        public byte[] Image { get; set; }
        [DataMember]
        public string Group { get; set; } = "";
        [DataMember]
        public GenreType GenreType { get; set; }

        [DataMember]
        private bool favorite = false;

        [DataMember]
        public bool IsFavorites
        {
            get
            {
                return favorite;
            }
            set
            {
                favorite = value;
                PropertyChange("IsFavorites");
            }
        }
        [DataMember]
        public List<User> Users { get; set; } = new List<User>();

        public event PropertyChangedEventHandler PropertyChanged;
        private void PropertyChange([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        [DataMember]
        public string Path { get; set; } = "";

        [DataMember]
        public List<UserPlaylist> userPlaylists { get; set; } = new List<UserPlaylist>();


    }

    public enum GenreType
    {
        Pop,
        Rock,
        Hip_Hop,
        Jazz
    }
}
