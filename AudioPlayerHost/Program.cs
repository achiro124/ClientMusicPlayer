using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayerHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(AudioPlayerService.ServiceAudioPlayer)))
            {
                host.Open();
                Console.WriteLine("Хост стартовал");
                Console.ReadLine();
            }
        }
    }
}
