using System.Collections.Generic;
using Akka.Actor;
using AkkaDevice.Dtos;

namespace AkkaDevice.DeviceDataRouting.Vibration
{
    public class VibrationDeviceGroup : ReceiveActor
    {
        private readonly Dictionary<string, IActorRef> _noiseDeviceActors = new();
        
        public VibrationDeviceGroup()
        {
            Receive<IDeviceData>(RouteMessage);
        }
        
        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new VibrationDeviceGroup());
        }

        private void RouteMessage(IDeviceData message)
        {
            if (!_noiseDeviceActors.TryGetValue(message.SerialNumber, out var actor))
            {
                actor = Context.ActorOf(VibrationDeviceActor.Props());
                _noiseDeviceActors[message.SerialNumber] = actor;
            }

            actor.Tell(message);
        }
    }
}