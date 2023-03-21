using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayerLibrary
{
    public class UserAlboms
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
        public string Title { get; set; }

        [DataMember]
        public List<Audio> ListAudio { get; set; }

    }
}
