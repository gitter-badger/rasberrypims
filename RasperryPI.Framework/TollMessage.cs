using System;

namespace RasperryPI.Framework
{
    [Serializable]
    public class TollMessage : IQueueMessage
    {
        public string VehicleNo { get; set; }

        public string ImageUrl { get; set; }

        public string DeviceId { get; set; }

        public DateTime CaptureTime { get; set; }

        public string GeoLocation { get; set; }

        public string PopReceipt { get; set; }

        public int DequeueCount { get; set; }

        public string MessageId { get; set; }
    }

    [Serializable]
    public class TollImageMessage : IQueueMessage
    {
        public string FileName { get; set; }

        public string PopReceipt { get; set; }

        public int DequeueCount { get; set; }

        public string MessageId { get; set; }
    }
}