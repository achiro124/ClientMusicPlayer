using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AudioPlayerLibrary
{
    [DataContract]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public int UserId { get; set; }
        [Required]
        [DataMember]
        public string Login { get; set; } = "";
        [Required]
        [DataMember]
        public string Password { get; set; } = "";
        [DataMember]
        public byte[] Icon { get; set; }
        [DataMember]
        public List<Audio> FavoriteAudio { get; set; } = new List<Audio>();

        [DataMember]
        public List<UserAlboms> UserAlboms { get; set; } = new List<UserAlboms>();
    }
}
