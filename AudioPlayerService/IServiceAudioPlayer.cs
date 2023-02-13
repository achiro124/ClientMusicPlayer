using MusicPlayer;
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
        List<Audio> GetAudioList(int userId);

        [OperationContract]
        byte[] GetAudioFile(string title);

        [OperationContract]
        User Registration(string login, string password);

        [OperationContract]
        User Authorization(string login, string password);

        [OperationContract(IsOneWay = true)]
        void EditUserIcon(int userId, byte[] icon);

        [OperationContract(IsOneWay = true)]
        void AddFavoriteAudio(int userId,int audioId);

        [OperationContract(IsOneWay = true)]
        void DeleteFavoriteAudio(int userId, int audioId);



    }
    public interface IServerAudioCallback
    {
       // [OperationContract(IsOneWay = true)]
       // void UsersCallback(List<string> names, List<int> listId);

    }
}
