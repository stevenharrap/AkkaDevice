using System.Collections.Generic;
using Akka.Actor;
using AkkaDevice.Dtos;

namespace AkkaDevice.DeviceDataRouting.Noise
{
    public class NoiseDeviceGroup : ReceiveActor
    {
        private readonly Dictionary<string, IActorRef> _noiseDeviceActors = new();
        
        public NoiseDeviceGroup()
        {
            Receive<IDeviceData>(RouteMessage);
        }
        
        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new NoiseDeviceGroup());
        }

        private void RouteMessage(IDeviceData message)
        {
            if (!_noiseDeviceActors.TryGetValue(message.SerialNumber, out var actor))
            {
                actor = Context.ActorOf(NoiseDeviceActor.Props());
                _noiseDeviceActors[message.SerialNumber] = actor;
            }

            actor.Tell(message);
        }
    }
}