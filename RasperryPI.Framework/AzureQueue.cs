using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace RasperryPI.Framework
{
    public interface IAzureQueue<T>
    {
        /// <summary>
        ///     Ensures the exist.
        /// </summary>
        void EnsureExist();

        /// <summary>
        ///     Check if queue exists
        /// </summary>
        /// <returns>True if exists</returns>
        bool Exists();

        /// <summary>
        ///     Clears this instance.
        /// </summary>
        void Clear();

        /// <summary>
        ///     Adds the message.
        /// </summary>
        /// <param name="message">The message.</param>
        void AddMessage(T message);

        /// <summary>
        ///     Gets the message.
        /// </summary>
        /// <returns>the queue message.</returns>
        T GetMessage();

        /// <summary>
        ///     Gets the messages.
        /// </summary>
        /// <param name="maxMessagesToReturn">The max messages to return.</param>
        /// <returns>returns collection of queue message.</returns>
        IEnumerable<T> GetMessages(int maxMessagesToReturn);

        /// <summary>
        ///     Deletes the message.
        /// </summary>
        /// <param name="message">The message.</param>
        void DeleteMessage(T message);
    }

    public class AzureQueue<T> : IAzureQueue<T> where T : IQueueMessage
    {
        /// <summary>
        ///     Cloud StorageAccount
        /// </summary>
        private readonly CloudStorageAccount _account;

        /// <summary>
        ///     cloud queue.
        /// </summary>
        private readonly CloudQueue _queue;

        /// <summary>
        ///     Time to live visibility.
        /// </summary>
        private readonly TimeSpan _timeToLive;

        /// <summary>
        ///     Time Span for visibility.
        /// </summary>
        private readonly TimeSpan _visibilityTimeout;

        /// <summary>
        ///     Initializes a new instance of the AzureQueue class.
        /// </summary>
        public AzureQueue()
            : this(CloudStorageAccount.Parse(TollConfiguration.GetConfigurationItem("storageconnectionstring")))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the AzureQueue class.
        /// </summary>
        /// <param name="account">The account.</param>
        public AzureQueue(CloudStorageAccount account)
            : this(account, typeof(T).Name.ToLower())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the AzureQueue class.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="queueName">Name of the queue.</param>
        public AzureQueue(CloudStorageAccount account, string queueName)
            : this(
                account, queueName,
                TimeSpan.FromSeconds(Convert.ToInt32(TollConfiguration.GetConfigurationItem("QueueMessageVisibilityTimeout"))),
                TimeSpan.FromDays(Convert.ToInt32(TollConfiguration.GetConfigurationItem("QueueMessageTimeToLiveInDays"))))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the AzureQueue class.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="visibilityTimeout">The visibility timeout.</param>
        /// <param name="timeToLive">The time to live </param>
        public AzureQueue(CloudStorageAccount account, string queueName, TimeSpan visibilityTimeout, TimeSpan timeToLive)
        {
            _account = account;
            _visibilityTimeout = visibilityTimeout;
            _timeToLive = timeToLive;

            CloudQueueClient client = _account.CreateCloudQueueClient();
            client.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(5), 5);

            _queue = client.GetQueueReference(queueName);
        }

        /// <summary>
        ///     Adds the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void AddMessage(T message)
        {
            // string serializedMessage = new JavaScriptSerializer().Serialize(message);
            byte[] serializedMessage = Serialize(message);
            _queue.AddMessage(new CloudQueueMessage(serializedMessage), _timeToLive);
        }

        /// <summary>
        ///     Gets the message.
        /// </summary>
        /// <returns>queue message.</returns>
        public T GetMessage()
        {
            CloudQueueMessage message = _queue.GetMessage(_visibilityTimeout);

            if (message == null)
            {
                return default(T);
            }

            return GetDeserializedMessage(message);
        }

        /// <summary>
        ///     Gets the messages.
        /// </summary>
        /// <param name="maxMessagesToReturn">The max messages to return.</param>
        /// <returns>get messages for queue.</returns>
        public IEnumerable<T> GetMessages(int maxMessagesToReturn)
        {
            IEnumerable<CloudQueueMessage> messages;
            messages = _queue.GetMessages(maxMessagesToReturn, _visibilityTimeout);
            return messages.Select(GetDeserializedMessage);
        }

        /// <summary>
        ///     Ensures the exist.
        /// </summary>
        public void EnsureExist()
        {
            _queue.CreateIfNotExists();
        }

        /// <summary>
        ///     Check if queue exists
        /// </summary>
        /// <returns>True if exists</returns>
        public bool Exists()
        {
            return _queue.Exists();
        }

        /// <summary>
        ///     Clears this instance.
        /// </summary>
        public void Clear()
        {
            _queue.Clear();
        }

        /// <summary>
        ///     Deletes the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void DeleteMessage(T message)
        {
            _queue.DeleteMessage(message.MessageId, message.PopReceipt);
        }

        /// <summary>
        ///     Gets the de-serialized message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>get de-serialized message.</returns>
        private static T GetDeserializedMessage(CloudQueueMessage message)
        {
            // var deserializedMessage = new JavaScriptSerializer().Deserialize<T>(message.AsString);
            T deserializedMessage = Deserialize(message.AsBytes);
            deserializedMessage.MessageId = message.Id;
            deserializedMessage.PopReceipt = message.PopReceipt;
            deserializedMessage.DequeueCount = message.DequeueCount;

            return deserializedMessage;
        }

        /// <summary>
        ///     Serialize the message
        /// </summary>
        /// <param name="message">message to be serialized</param>
        /// <returns>the serialized message</returns>
        private static byte[] Serialize(T message)
        {
            byte[] data = default(byte[]);

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, message);
                data = stream.ToArray();
            }

            return data;
        }

        /// <summary>
        ///     returns de-serialized message.
        /// </summary>
        /// <param name="message">message to  be de-serialized</param>
        /// <returns>the de serialized message</returns>
        private static T Deserialize(byte[] message)
        {
            object o = default(object);
            using (var stream = new MemoryStream(message))
            {
                var formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                o = formatter.Deserialize(stream);
            }

            return (T)o;
        }
    }
}