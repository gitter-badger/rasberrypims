namespace RasperryPI.Framework
{
    public interface IQueueRepository
    {
        bool AddTollMessage(TollMessage tollMessage);
        
        void RemoveTollMessage(TollMessage tollMessage);
    }
}