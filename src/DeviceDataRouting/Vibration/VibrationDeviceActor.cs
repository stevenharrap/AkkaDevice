using System;
using Akka.Actor;
using AkkaDevice.Dtos;

namespace AkkaDevice.DeviceDataRouting.Vibration
{
    public class VibrationDeviceActor : ReceiveActor
    {
        public VibrationDeviceActor()
        {
            Receive<VibrationData>(ReceiveVibrationData);
        }
        
        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new VibrationDeviceActor());
        }

        private void ReceiveVibrationData(VibrationData message)
        {
            Console.WriteLine($"Received vibration message {message.SerialNumber}");
        }
    }
}