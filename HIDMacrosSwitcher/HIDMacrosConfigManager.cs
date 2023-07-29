using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace HIDMacrosSwitcher
{
    class HIDMacrosConfigManager
    {
        public Config ReadConfigFromXml(string xmlFilePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Config));
                using (FileStream stream = new FileStream(xmlFilePath, FileMode.Open))
                {
                    return (Config)serializer.Deserialize(stream);
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }
        public void SaveConfigToXml(string xmlFilePath, Config config)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            using (FileStream stream = new FileStream(xmlFilePath, FileMode.Create))
            {
                serializer.Serialize(stream, config);
            }
        }
        public Macros ReadMacrosFromXml(string xmlFilePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            using (FileStream stream = new FileStream(xmlFilePath, FileMode.Open))
            {
                return (Macros)serializer.Deserialize(stream);
            }
        }
        public void SaveMacrosToXml(string xmlFilePath, Macros macros)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            using (FileStream stream = new FileStream(xmlFilePath, FileMode.Open))
            {
                serializer.Serialize(stream, macros);
            }
        }
    }
}
