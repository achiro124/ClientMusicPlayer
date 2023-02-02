using MusicPlayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AudioPlayerService
{
    public class ServiceAudioPlayer : IServiceAudioPlayer
    {

        SqlConnection connection;
        SqlCommand sqlCommand;
        SqlConnectionStringBuilder connectionStringBuilder;
        List<Audio> audios= new List<Audio>();

        public ServiceAudioPlayer()
        {
            ConnectToBd();
        }

        public void ConnectToBd()
        {
            connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder.DataSource = "DESKTOP-FGS36Q5";
            connectionStringBuilder.InitialCatalog = "DateBaseAudioPlayer";
            connectionStringBuilder.Encrypt = true;
            connectionStringBuilder.TrustServerCertificate = true;
            connectionStringBuilder.ConnectTimeout = 30;
            connectionStringBuilder.AsynchronousProcessing = true;
            connectionStringBuilder.MultipleActiveResultSets = true;
            connectionStringBuilder.IntegratedSecurity = true;

            connection = new SqlConnection(connectionStringBuilder.ToString());
        }

        public byte[] GetAudioFile(string title)
        {
            string pathSourceFile = @"D:\Audios\" + title + ".mp3";
           // string pathCompressedFile = @"D:\Audios\" + title + ".gz";
           // GZip zip = new GZip
           // {
           //     SourceFile = pathSourceFile,
           //     CompressedFile = pathCompressedFile
           // };

            //zip.Compress();
            byte[] compressAudio = File.ReadAllBytes(pathSourceFile);
            return compressAudio;
        }

        public List<Audio> GetAudioList()
        {
            try
            {
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "SELECT * FROM Audio";
                sqlCommand.CommandType = System.Data.CommandType.Text;

                connection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    audios.Add(new Audio
                    {
                        Id = Convert.ToInt32(reader[0]),
                        Title = reader[1].ToString(),
                        Image = (byte[])reader[2],
                        Group = reader[3].ToString(),
                        Genre = (GenreType)reader[4]
                    });
                }
                return audios;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
    }
}
