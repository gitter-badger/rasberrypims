using System;

namespace RasperryPI.Framework
{
    public class AzureQueueService : MarshalByRefObject, IAzureQueueService
    {
        /// <summary>
        ///     Queue Repository
        /// </summary>
        private readonly IQueueRepository _queueRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AzureQueueService" /> class.
        /// </summary>
        /// <param name="queueRepository">queue Repository</param>
        public AzureQueueService(IQueueRepository queueRepository)
        {
            _queueRepository = queueRepository;
        }

        public bool AddTollMessage(TollMessage tollMessage)
        {
            return _queueRepository.AddTollMessage(tollMessage);
        }

        /// <summary>
        ///     Remove PurchaseReceipt Message
        /// </summary>
        /// <param name="queueMessage">PurchaseReceipt Queue Message</param>
        public void RemoveTollMessage(TollMessage queueMessage)
        {
            _queueRepository.RemoveTollMessage(queueMessage);
        }
    }
}