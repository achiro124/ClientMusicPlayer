using AudioPlayerLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Data.Entity;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Runtime.Remoting.Contexts;

namespace AudioPlayerService
{
    public class ServiceAudioPlayer : IServiceAudioPlayer
    {

        DbAudioPlayerContext _context = new DbAudioPlayerContext();
        public ServiceAudioPlayer()
        {
        }

        //Отправка пользователю аудиозаписи в виде байтов
        public byte[] GetAudioFile(string title)
        {
            string pathSourceFile = @"D:\Audios\" + title + ".mp3";
            byte[] compressAudio = File.ReadAllBytes(pathSourceFile);
            return compressAudio;
        }

        //Получение пользователем списка с музыкой
        public List<Audio> GetAudioList(int userId)
        {
            List<Audio> audioList = new List<Audio>();
            foreach(var item in _context.Audios.Include(x => x.Users))
            {
                item.Path = "";
                audioList.Add(item);
                audioList.Last().IsFavorites = _context.Users.FirstOrDefault(x => x.UserId == userId)?.FavoriteAudio
                                                             .FirstOrDefault(x => x.AudioId == item.AudioId) != null;
                
            }
            return audioList;
        }

        //Возвращение списка избранных в обратном порядке добавления их в бд
        public List<Audio> GetFavoriteAudioList(int userId)
       {
            List<Audio> favoriteAuidoList = new List<Audio>();
            foreach(var item in _context.Users.Include(x => x.FavoriteAudio).FirstOrDefault(x => x.UserId == userId).FavoriteAudio)
            {
                item.Path = "";
                favoriteAuidoList.Add(item);
                favoriteAuidoList.Last().IsFavorites = true;
            }
            return favoriteAuidoList;
            
       }

        //Регистрация пользователя
        public User Registration(string login, string password)
        {
            if (_context.Users.FirstOrDefault(x => x.Login == login) != null)
                return null;
            
            User user = new User
            {
                Login = login,
                Password = password,
                Icon = File.ReadAllBytes(@"D:\Diplom\MusicPlayer\Image\1.jpg")
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        //Авторизация пользователя
        public User Authorization(string login, string password)
        {
            if(_context.Users.FirstOrDefault(u => u.Login == login) != null && _context.Users.FirstOrDefault(u => u.Login == login).Password == password)
            {
                User user = _context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
                return user;
            }
            else
            {
                return null;
            }
        }

        //Редактирование иконки пользователя
        public void EditUserIcon(int userId, byte[] Icon)
        {
           
        }

        //Добавление аудио в избранное
        public void AddFavoriteAudio(int userId, int audioId)
        {
            Audio audio = _context.Audios.FirstOrDefault(x => x.AudioId == audioId);
            User user = _context.Users.FirstOrDefault(x => x.UserId == userId);

            _context.Users.FirstOrDefault(x => x.UserId == userId).FavoriteAudio.Add(audio);
            _context.Audios.FirstOrDefault(x => x.AudioId == audioId).Users.Add(user);
            _context.SaveChanges();
        }

        //Удаление аудио из избранного
        public void DeleteFavoriteAudio(int userId, int audioId)
        {
            Audio audio = _context.Audios.Include(st => st.Users).FirstOrDefault(x => x.AudioId == audioId);
            User user = _context.Users.Include(st => st.FavoriteAudio).FirstOrDefault(x => x.UserId == userId);

            _context.Users.Include(st => st.FavoriteAudio).FirstOrDefault(x => x.UserId == userId).FavoriteAudio.Remove(audio);
            _context.Audios.Include(st => st.Users).FirstOrDefault(x => x.AudioId == audioId).Users.Remove(user);
            _context.SaveChanges();
        }

        public int AddUserPlaylist(int userId, string title)
        {
            UserPlaylist userAlboms = new UserPlaylist
            {
                Title = title,
                User = _context.Users.FirstOrDefault(x => x.UserId == userId)
            };
            _context.Users.FirstOrDefault(x => x.UserId == userId).UserPlaylist.Add(userAlboms);
            _context.UserAlboms.Add(userAlboms);
            _context.SaveChanges();
            return _context.UserAlboms.FirstOrDefault(x => x.Title == title).AlbomId;
        }

        public List<UserPlaylist> GetUserPlaylist(int userId)
        {
            return _context.UserAlboms.Include(x => x.ListAudio).Include(x => x.User).Where(x => x.User.UserId == userId).ToList();
        }

        public void DeleteUserPlaylist(int userId, int audiolistId)
        {
            UserPlaylist userlist = _context.UserAlboms.Include(st => st.User).FirstOrDefault(x => x.AlbomId == audiolistId);
            User user = _context.Users.Include(st => st.UserPlaylist).FirstOrDefault(x => x.UserId == userId);

            _context.Users.Include(st => st.UserPlaylist).FirstOrDefault(x => x.UserId == userId).UserPlaylist.Remove(userlist);
            _context.UserAlboms.Remove(userlist);
            _context.SaveChanges();
        }

        public void EditUserPlaylist(int audiolistId, string title)
        {
            _context.UserAlboms.FirstOrDefault(x => x.AlbomId == audiolistId).Title= title;
            _context.SaveChanges();
        }

        public void AddAudioUserPlaylist(int audiolistId, int audioId)
        {
            _context.UserAlboms.Include(x => x.ListAudio).FirstOrDefault(x => x.AlbomId == audiolistId).ListAudio.Add(_context.Audios.FirstOrDefault(x => x.AudioId == audioId));
            _context.SaveChanges();
        }

        public void DeleteAudioUserPlaylist(int audiolistId, int audioId)
        {
            _context.UserAlboms.Include(x => x.ListAudio).FirstOrDefault(x => x.AlbomId == audiolistId).ListAudio.Remove(_context.Audios.FirstOrDefault(x => x.AudioId == audioId));
            _context.SaveChanges();
        }
    }
}
