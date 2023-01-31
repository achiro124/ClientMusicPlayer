using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    [DataContract]
    public class Audio
    {
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
