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
        List<User> users= new List<User>();
        int nextUserId = 0;

        public ServiceAudioPlayer()
        {
            ConnectToBd();
            GetAllUsers();
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

        public void GetAllUsers()
        {
            try
            {
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "SELECT * FROM AudioPlayerUser";
                sqlCommand.CommandType = System.Data.CommandType.Text;

                connection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        UserId = Convert.ToInt32(reader[0]),
                        Login = reader[1].ToString(),
                        Icon = (byte[])reader[3],
                        Password = reader[2].ToString(),
                    });
                    nextUserId = Convert.ToInt32(reader[0]) + 1;
                }
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



        //Отправка пользователю аудиозаписи в виде байтов
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


        //Получение пользователем списка с музыкой
        public List<Audio> GetAudioList(int userId)
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
                        Genre = (GenreType)reader[4],
                    });
                }

                List<int> idFavoriteAudio = GetIdFavoriteAudio(userId);
                

                foreach(var aud in audios)
                {
                    foreach(var f in idFavoriteAudio)
                    {
                        if(aud.Id == f)
                        {
                            aud.Favorite = true;
                        }
                    }
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

        //Регистрация пользователя
        public User Registration(string login, string password)
        {

            if(users.FirstOrDefault(u => u.Login == login) != null)
            {
                return null;
            }

            try
            {
                User user = new User
                {
                    UserId = nextUserId,
                    Login = login,
                    Password = password,
                    Icon = File.ReadAllBytes(@"C:\\Users\\navfa\\source\\repos\\ClientMusicPlayer\\MusicPlayer\\Image\\1.jpg")
                };
                users.Add(user);
                nextUserId++;

                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "INSERT INTO AudioPlayerUser VALUES(@UserId, @Login, @Password, @Icon)";
                sqlCommand.Parameters.AddWithValue("UserId", user.UserId);
                sqlCommand.Parameters.AddWithValue("Login", user.Login);
                sqlCommand.Parameters.AddWithValue("Password", user.Password);
                sqlCommand.Parameters.AddWithValue("Icon", user.Icon);
                sqlCommand.CommandType = System.Data.CommandType.Text;
                connection.Open();
                sqlCommand.ExecuteNonQuery();

                return user;
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



        //Авторизация пользователя
        public User Authorization(string login, string password)
        {
            if(users.FirstOrDefault(u => u.Login == login) != null && users.FirstOrDefault(u => u.Login == login).Password == password)
            {
                return users.FirstOrDefault(u => u.Login == login);
            }
            else
            {
                return null;
            }
        }

        //Редактирование иконки пользователя
        public void EditUserIcon(int userId, byte[] Icon)
        {
            try
            {
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "UPDATE AudioPlayerUser SET Icon = @Icon WHERE UserId = @UserId";
                sqlCommand.Parameters.AddWithValue("UserId", userId);
                sqlCommand.Parameters.AddWithValue("Icon", Icon);

                sqlCommand.CommandType = System.Data.CommandType.Text;
                connection.Open();
                sqlCommand.ExecuteNonQuery();
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

        public void AddFavoriteAudio(int userId, int audioId)
        {
            try
            {
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "INSERT INTO FavoriteAudio VALUES(@UserId, @AudioId)";
                sqlCommand.Parameters.AddWithValue("UserId", userId);
                sqlCommand.Parameters.AddWithValue("AudioId", audioId);
                sqlCommand.CommandType = System.Data.CommandType.Text;
                connection.Open();
                sqlCommand.ExecuteNonQuery();
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


        public List<int> GetIdFavoriteAudio(int userId)
        {
            try
            {
                List<int> idFavorite = new List<int>();
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "SELECT AudioId FROM FavoriteAudio";
                sqlCommand.CommandType = System.Data.CommandType.Text;

                //connection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    idFavorite.Add(Convert.ToInt32(reader[0]));
                }

                return idFavorite;
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
