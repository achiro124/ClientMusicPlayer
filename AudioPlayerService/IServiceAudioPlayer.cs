using AudioPlayerLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AudioPlayerService
{
    [ServiceContract(CallbackContract = typeof(IServerAudioCallback))]
    public interface IServiceAudioPlayer
    {
        [OperationContract]
        [CyclicReferencesAware(true)]
        List<Audio> GetAudioList(int userId);

        [OperationContract]
        [CyclicReferencesAware(true)]
        List<Audio> GetFavoriteAudioList(int userId);

        [OperationContract]
        byte[] GetAudioFile(string title);

        [OperationContract]
        [CyclicReferencesAware(true)]
        User Registration(string login, string password);

        [OperationContract]
        [CyclicReferencesAware(true)]
        User Authorization(string login, string password);

        [OperationContract(IsOneWay = true)]
        void EditUserIcon(int userId, byte[] icon);

        [OperationContract(IsOneWay = true)]
        void AddFavoriteAudio(int userId,int audioId);

        [OperationContract(IsOneWay = true)]
        void DeleteFavoriteAudio(int userId, int audioId);

        [OperationContract]
        [CyclicReferencesAware(true)]
        int AddUserPlaylist(int userId, string title);

        [OperationContract]
        [CyclicReferencesAware(true)]
        List<UserPlaylist> GetUserPlaylist(int userId);

        [OperationContract(IsOneWay = true)]
        void DeleteUserPlaylist(int userId, int audiolistId);

        [OperationContract(IsOneWay = true)]
        void EditUserPlaylist(int audiolistId, string title);

        [OperationContract(IsOneWay = true)]
        void AddAudioUserPlaylist(int audiolistId, int audioId);

        [OperationContract(IsOneWay = true)]
        void DeleteAudioUserPlaylist(int audiolistId, int audioId);

    }
    public interface IServerAudioCallback
    {
       // [OperationContract(IsOneWay = true)]
       // void UsersCallback(List<string> names, List<int> listId);

    }
}
