using System;

namespace AkkaDevice.Dtos
{
    public class VibrationData : IDeviceData
    {
        public DeviceType DeviceType { get; } = DeviceType.Vibration;
        public string SerialNumber { get; init; }
        public DateTime UtcStartTime { get; init; }
        public double Hz { get; init; }
    }
}