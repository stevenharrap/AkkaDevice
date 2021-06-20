using System;

namespace AkkaDevice.Dtos
{
    public enum DeviceType
    {
        Sound,
        Vibration
    }
    
    public interface IDeviceData
    {
        DeviceType DeviceType { get; }
        string SerialNumber { get; }
        DateTime UtcStartTime { get; }
    }
}