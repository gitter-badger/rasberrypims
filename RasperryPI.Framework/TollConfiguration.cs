﻿using System.Configuration;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace RasperryPI.Framework
{
    public static class TollConfiguration
    {
        /// <summary>
        ///     Gets the configuration item.
        /// </summary>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <returns>
        ///     Details of configuration item
        /// </returns>
        public static string GetConfigurationItem(string key)
        {
            //if (RoleEnvironment.IsAvailable)
            //{
            //    return RoleEnvironment.GetConfigurationSettingValue(key);
            //}

            return ConfigurationManager.AppSettings[key];
        }
    }
}