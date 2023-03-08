using AudioPlayerLibrary;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayerService
{
    [DataContract]
    public class DbAudioPlayerContext: DbContext
    {
        public DbAudioPlayerContext() : base()
        {
            this.Database.Connection.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = AudioPlayer; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False; MultipleActiveResultSets=True";


        }

        [DataMember]
        public DbSet<Audio> Audios { get; set; }

        [DataMember]
        public DbSet<User> Users { get; set; }
    }
}
