using System;

namespace RasperryPI.Framework
{
    /// <summary>
    ///     Azure queue repository.
    /// </summary>
    public class AzureQueueRepository : IQueueRepository
    {
        /// <summary>
        ///     queue factory instance.
        /// </summary>
        private readonly IAzureQueueFactory _queueFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AzureQueueRepository" /> class.
        /// </summary>
        /// <param name="queueFactory"> queue factory. </param>
        [CLSCompliant(false)]
        public AzureQueueRepository(IAzureQueueFactory queueFactory)
        {
            _queueFactory = queueFactory;
        }

        public bool AddTollMessage(TollMessage publishMessage)
        {
            IAzureQueue<TollMessage> publishAzureQueue = _queueFactory.GetQueue<TollMessage>();
            publishAzureQueue.EnsureExist();
            publishAzureQueue.AddMessage(publishMessage);
            return true;
        }

        public void RemoveTollMessage(TollMessage queueMessage)
        {
            var purchaseAzureQueue = _queueFactory.GetQueue<TollMessage>();
            purchaseAzureQueue.EnsureExist();
            purchaseAzureQueue.DeleteMessage(queueMessage);
        }

        public TollMessage ReadTollMessage()
        {
            var purchaseAzureQueue = this._queueFactory.GetQueue<TollMessage>();
            purchaseAzureQueue.EnsureExist();
            return purchaseAzureQueue.GetMessage();
        }
    }
}