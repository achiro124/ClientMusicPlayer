using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayerLibrary
{
    [DataContract]
    public class UserPlaylist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public int AlbomId { get; set; }

        [Required]
        [DataMember]
        public User User { get; set; }

        [Required]
        [DataMember]
        private string title;
        [Required]
        [DataMember]
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
                PropertyChange("Title");
            }
        }

        [DataMember]
        public List<Audio> ListAudio { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void PropertyChange([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
