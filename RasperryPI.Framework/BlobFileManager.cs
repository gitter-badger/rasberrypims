using System;
using System.Collections.Generic;
using System.IO;

namespace RasperryPI.Framework
{
    public class BlobFileManager : IFileManager
    {
        private string _container;
        private string _connString;
        public BlobFileManager(string container, string connectionString)
        {
            _container = container;
            _connString = connectionString;
        }

        public string PersisteFile(Stream stream, string userId, string fileType)
        {
            try
            {
                var uid = Guid.NewGuid();
                var manager = new PhoenixAzureStorageHelper();
                var dictionary = new Dictionary<string, string>();
                dictionary.Add("UserId", userId);
                dictionary.Add("ReferenceId", uid.ToString());
                manager.UploadFile(stream, _container, uid.ToString() + fileType, _connString, dictionary);
                return uid.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool PersisteErrorFile(Stream stream, string referenceId, string userId)
        {
            try
            {
                var manager = new PhoenixAzureStorageHelper();
                var dictionary = new Dictionary<string, string>();
                dictionary.Add("UserId", userId);
                dictionary.Add("ReferenceId", referenceId);
                return manager.UploadFile(stream, _container, referenceId + "_errors.xlsx", _connString, dictionary); ;
            }
            catch (Exception ex)
            {
                // throw new PhoenixException(Layer.Business, "", "", ex);
            }
            return false;
        }

        public Stream ReadStreamImage(string url)
        {
            try
            {
                var manager = new PhoenixAzureStorageHelper();
                var dictionary = new Dictionary<string, string>();

                var files = url.Split("/".ToCharArray());
                var file = files[files.Length - 1];
                file = file.Replace(".png", ".jpg");
                //return null;
                return manager.DownloadFile(_container, file, _connString, dictionary);
            }
            catch (Exception ex)
            {
                // throw new PhoenixException(Layer.Business, "", "", ex);
            }
            return null;
        }
        public Stream ReadFile(string referenceId, string userId)
        {
            try
            {
                var manager = new PhoenixAzureStorageHelper();
                var dictionary = new Dictionary<string, string>();
                dictionary.Add("UserId", userId);
                dictionary.Add("ReferenceId", referenceId.ToString());
                //return null;
                return manager.DownloadFile(_container, referenceId.ToString() + ".xlsx", _connString, dictionary);
            }
            catch (Exception ex)
            {
                // throw new PhoenixException(Layer.Business, "", "", ex);
            }
            return null;
        }



        public Stream ReadErrorFile(string referenceId, string userId)
        {
            try
            {
                var manager = new PhoenixAzureStorageHelper();
                var dictionary = new Dictionary<string, string>();
                dictionary.Add("UserId", userId);
                dictionary.Add("ReferenceId", referenceId);
                //return null;
                return manager.DownloadFile(_container, referenceId + "_errors.xlsx", _connString, dictionary);
            }
            catch (Exception ex)
            {
                //throw new PhoenixException(Layer.Business, "", "", ex);
            }
            return null;
        }

        public void DeleteFile(string fileName)
        {
            var manager = new PhoenixAzureStorageHelper();
            manager.DeleteFile(_container, fileName, _connString);
        }

    }
}