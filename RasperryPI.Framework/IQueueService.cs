namespace RasperryPI.Framework
{
    public interface IQueueService
    {
        bool AddTollMessage(TollMessage tollMessage);

        void RemoveTollMessage(TollMessage tollMessage);
    }
}