using System;
using Akka.Actor;
using AkkaDevice.Dtos;

namespace AkkaDevice.DeviceDataRouting.Noise
{
    public class NoiseDeviceActor : ReceiveActor
    {
        private string _serial;
        private int _messageCount;
        
        public NoiseDeviceActor()
        {
            Become(WaitingForFirstMessage);
        }

        private void WaitingForFirstMessage()
        {
            Receive<SoundData>(ReceiveFirstSoundData);
        }

        private void AcceptingSubsequentMessages()
        {
            Receive<SoundData>(ReceiveSoundData);
        }

        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new NoiseDeviceActor());
        }

        private void ReceiveFirstSoundData(SoundData message)
        {
            Console.WriteLine($"NoiseDeviceActor started for {message.SerialNumber}");
            _serial = message.SerialNumber;
            _messageCount++;
            Become(AcceptingSubsequentMessages);
        }
        
        private void ReceiveSoundData(SoundData message)
        {
            if (_serial != message.SerialNumber)
                throw new Exception($"NoiseDeviceActor for {_serial} received message for {message.SerialNumber}!");

            _messageCount++;
            
            Console.WriteLine($"NoiseDeviceActor: message for {message.SerialNumber}. Message count: {_messageCount}");
        }
    }
}