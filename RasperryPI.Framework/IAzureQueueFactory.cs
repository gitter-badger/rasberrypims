namespace RasperryPI.Framework
{
    /// <summary>
    ///     defines factory for azure queue.
    /// </summary>
    public interface IAzureQueueFactory
    {
        /// <summary>
        ///     Returns the queue with the matching type.
        /// </summary>
        /// <typeparam name="T">Type of the queue.</typeparam>
        /// <returns>Instance of the queue.</returns>
        IAzureQueue<T> GetQueue<T>() where T : IQueueMessage;
    }
}