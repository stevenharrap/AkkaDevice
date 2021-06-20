using System;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaDevice.DeviceDataRouting;
using AkkaDevice.Dtos;

namespace AkkaDevice.DevicesConsole
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var actorSystem = ActorSystem.Create("akka-device-data-system");
            var deviceDataRouterActor = actorSystem.ActorOf(DeviceDataRouter.Props());

            //SendTwo(deviceDataRouterActor);
            await SendTwoHundred(deviceDataRouterActor);
           
            Console.WriteLine("Crt-c to close the system");
            Console.ReadLine();
        }

        private static void SendTwo(IActorRef deviceDataRouterActor)
        {
            deviceDataRouterActor.Tell(new SoundData
            {
                SerialNumber = "00045402",
                UtcStartTime = DateTime.Now,
                Laq = 34
            });
            deviceDataRouterActor.Tell(new VibrationData()
            {
                SerialNumber = "00666644",
                UtcStartTime = DateTime.Now,
                Hz = 453
            });
        }

        private static async Task SendTwoHundred(IActorRef deviceDataRouterActor)
        {
            var devices = new []
            {
                new { serial = "DT_S01", type = DeviceType.Sound },
                new { serial = "DT_V06", type = DeviceType.Vibration },
                new { serial = "DT_S02", type = DeviceType.Sound },
                new { serial = "DT_V07", type = DeviceType.Vibration },
                new { serial = "DT_S03", type = DeviceType.Sound },
                new { serial = "DT_V08", type = DeviceType.Vibration },
                new { serial = "DT_S04", type = DeviceType.Sound },
                new { serial = "DT_V09", type = DeviceType.Vibration },
            };

            for (var i = 0; i < 200; i++)
            {
                var device = devices[i % 8];
                switch (device.type)
                {
                    case DeviceType.Sound:
                        deviceDataRouterActor.Tell(new SoundData
                        {
                            SerialNumber = device.serial, 
                            UtcStartTime = DateTime.Now,
                            Laq = i * 20.25
                        });
                        break;
                        
                    case DeviceType.Vibration:
                        deviceDataRouterActor.Tell(new VibrationData
                        {
                            SerialNumber = device.serial, 
                            UtcStartTime = DateTime.Now,
                            Hz = 334.6 * i
                        });
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                await Task.Delay(50);
            }
        } 
    }
}