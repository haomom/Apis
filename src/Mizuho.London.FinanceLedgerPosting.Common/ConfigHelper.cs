using System;
using System.Configuration;
using Mizuho.London.Common.Interfaces.Configuration;

namespace Mizuho.London.FinanceLedgerPosting.Common
{
    public class ConfigHelper : IConfigHelper
    {
        /// <summary>   
        /// Gets a specific config property   
        /// </summary>   
        /// <param name="key">the property to get</param>   
        /// <param name="type">type of class asking - to get right assembly</param>   
        /// <returns>value</returns>   
        public string GetConfigProperty(string key, Type type)
        {
            var config = GetConfig(type);
            return config.AppSettings.Settings[key].Value;
        }

        /// <summary>   
        /// Get the configuration for the supplied type   
        /// </summary>   
        /// <param name="type">type of class</param>   
        /// <returns>a configuration</returns>   
        private static Configuration GetConfig(Type type)
        {
            var dllLocation = type.Assembly.Location + ".config";

            if (dllLocation == null)
                throw new Exception("Could not find config file, add .config in DLL location");

            //create config  
            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = dllLocation };
            var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            return config;
        }
    }
}
