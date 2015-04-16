namespace RasperryPI.Framework
{
    /// <summary>
    ///     Interface for QueueMessage
    /// </summary>
    public interface IQueueMessage
    {
        /// <summary>
        ///     Gets or sets MessageId
        /// </summary>
        string MessageId { get; set; }

        /// <summary>
        ///     Gets or sets PopReceipt
        /// </summary>
        string PopReceipt { get; set; }

        /// <summary>
        ///     Gets or sets De-queue Count
        /// </summary>
        int DequeueCount { get; set; }
    }
}