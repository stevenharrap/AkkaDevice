using System;

namespace AkkaDevice.Dtos
{
    public class SoundData : IDeviceData
    {
        public DeviceType DeviceType { get; } = DeviceType.Sound;
        public string SerialNumber { get; init; }
        public DateTime UtcStartTime { get; init; }
        public double Laq { get; init; }
    }
}