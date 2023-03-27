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
        int AddUserAudiolist(int userId, string title);

        [OperationContract]
        [CyclicReferencesAware(true)]
        List<UserAlboms> GetUserAudiolist(int userId);

        [OperationContract(IsOneWay = true)]
        void DeleteUserAudiolist(int userId, int audiolistId);

    }
    public interface IServerAudioCallback
    {
       // [OperationContract(IsOneWay = true)]
       // void UsersCallback(List<string> names, List<int> listId);

    }
}
