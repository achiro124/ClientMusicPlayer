﻿using MusicPlayer;
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
        List<Audio> GetAudioList();
        [OperationContract]
        byte[] GetAudioFile(string title);

    }
    public interface IServerAudioCallback
    {
       // [OperationContract(IsOneWay = true)]
       // void UsersCallback(List<string> names, List<int> listId);

    }
}