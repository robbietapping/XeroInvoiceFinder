using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XeroInvoiceFinder
{
    public class SettingsReader
    {

        private static SettingsReader _settingsReader;

        public static SettingsReader Instance()
        {
            if (_settingsReader == null)
            {
                _settingsReader = new SettingsReader();
            }
            return _settingsReader;
        }


        public SettingsConfig GetSettings()
        {
            try
            {
                using (var sr = new StreamReader($"{System.Environment.CurrentDirectory}\\Settings.json"))
                {
                    var contents = sr.ReadToEnd();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<SettingsConfig>(contents);

                }
            }catch(Exception ex)
            {
                throw new Exception(String.Format("There was an error trying to convert the settings file, it may be malformed: {0}", ex.Message));
            }
        }
    }



    public class SettingsConfig
    {
        public string clientId { get; set; }
        public string clientSecret { get; set; }
    }

}
