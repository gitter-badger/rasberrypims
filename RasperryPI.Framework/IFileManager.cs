using System.IO;

namespace RasperryPI.Framework
{
    public interface IFileManager
    {
        string PersisteFile(Stream stream, string userId);

        bool PersisteErrorFile(Stream stream, string referenceId, string userId);

        Stream ReadFile(string referenceId, string userId);

        Stream ReadErrorFile(string referenceId, string userId);

        void DeleteFile(string fileName);
    }
}