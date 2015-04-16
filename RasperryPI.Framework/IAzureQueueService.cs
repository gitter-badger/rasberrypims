namespace RasperryPI.Framework
{
    public interface IAzureQueueService
    {
        bool AddTollMessage(TollMessage tollMessage);

        void RemoveTollMessage(TollMessage tollMessage);
    }
}