using System;
using Akka.Actor;
using Akka.Routing;
using AkkaDevice.DeviceDataRouting.Noise;
using AkkaDevice.DeviceDataRouting.Vibration;
using AkkaDevice.Dtos;

namespace AkkaDevice.DeviceDataRouting
{
    public class DeviceDataRouter : ReceiveActor
    {
        private IActorRef _noiseDeviceRouter;
        private IActorRef _vibrationDeviceRouter;
        private int _messageCount = 0;
        
        public DeviceDataRouter()
        {
            Receive<IDeviceData>(ReceiveDeviceData);
        }

        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new DeviceDataRouter());
        }

        protected override void PreStart()
        {
            var deviceDataMessageMapping = new ConsistentHashMapping(message => message is IDeviceData deviceDataMessage
                ? deviceDataMessage.SerialNumber
                : null);
            
            _noiseDeviceRouter = Context.ActorOf(
                NoiseDeviceGroup.Props()
                    .WithRouter(new ConsistentHashingPool(5)
                        .WithHashMapping(deviceDataMessageMapping)));
            _vibrationDeviceRouter = Context.ActorOf(
                VibrationDeviceGroup.Props()
                    .WithRouter(new ConsistentHashingPool(5)
                        .WithHashMapping(deviceDataMessageMapping)));
        }
        
        private void ReceiveDeviceData(IDeviceData message)
        {
            if (_messageCount % 20 == 0)
            {
                Console.WriteLine($"******* Device Data Router has {_messageCount} messages. ********");
            }
            
            _messageCount++;
            
            //Console.WriteLine($"Received device message {message.SerialNumber}");
            switch (message.DeviceType)
            {
                case DeviceType.Sound:
                    _noiseDeviceRouter.Tell(message);
                    return;
                case DeviceType.Vibration:
                    _vibrationDeviceRouter.Tell(message);
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}