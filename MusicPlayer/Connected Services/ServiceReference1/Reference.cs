﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MusicPlayer.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://Microsoft.ServiceModel.Samples", ConfigurationName="ServiceReference1.IAudioPlayerService")]
    public interface IAudioPlayerService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Microsoft.ServiceModel.Samples/IAudioPlayerService/GetListAudio", ReplyAction="http://Microsoft.ServiceModel.Samples/IAudioPlayerService/GetListAudioResponse")]
        string[] GetListAudio();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Microsoft.ServiceModel.Samples/IAudioPlayerService/GetListAudio", ReplyAction="http://Microsoft.ServiceModel.Samples/IAudioPlayerService/GetListAudioResponse")]
        System.Threading.Tasks.Task<string[]> GetListAudioAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IAudioPlayerServiceChannel : MusicPlayer.ServiceReference1.IAudioPlayerService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AudioPlayerServiceClient : System.ServiceModel.ClientBase<MusicPlayer.ServiceReference1.IAudioPlayerService>, MusicPlayer.ServiceReference1.IAudioPlayerService {
        
        public AudioPlayerServiceClient() {
        }
        
        public AudioPlayerServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AudioPlayerServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AudioPlayerServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AudioPlayerServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string[] GetListAudio() {
            return base.Channel.GetListAudio();
        }
        
        public System.Threading.Tasks.Task<string[]> GetListAudioAsync() {
            return base.Channel.GetListAudioAsync();
        }
    }
}
