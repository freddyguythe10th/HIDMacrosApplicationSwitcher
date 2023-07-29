using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIDMacrosSwitcher
{
    using System.Xml.Serialization;

    [XmlRoot("Config")]
    public class Config
    {
        [XmlElement("General")]
        public General General { get; set; }

        [XmlElement("Macros")]
        public Macros Macros { get; set; }

        [XmlElement("Devices")]
        public Devices Devices { get; set; }

        [XmlElement("Map")]
        public Map Map { get; set; }
    }

    public class Devices
    {
        [XmlElement("Name")]
        public string Name;
        [XmlElement("SystemID")]
        public string SystemID;
    }

    public class Map
    {
    }

    public class General
    {
        [XmlElement("Language")]
        public string Language { get; set; }

        [XmlElement("ScriptLanguage")]
        public string ScriptLanguage { get; set; }

        [XmlElement("ProcBegin")]
        public string ProcBegin { get; set; }

        [XmlElement("ProcEnd")]
        public string ProcEnd { get; set; }

        [XmlElement("MinimizeToTray")]
        public int MinimizeToTray { get; set; }

        [XmlElement("StartMinimized")]
        public int StartMinimized { get; set; }

        [XmlElement("AllowScriptGUI")]
        public int AllowScriptGUI { get; set; }
    }

    public class Macro
    {
        [XmlElement("Device")]
        public string Device { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("KeyCode")]
        public int KeyCode { get; set; }

        [XmlElement("Action")]
        public string Action { get; set; }

        [XmlElement("Command")]
        public string Command { get; set; }
        // Add other properties as needed
    }

    public class Macros
    {
        [XmlElement("Macro")]
        public List<Macro> MacroList { get; set; }
    }

    // Define classes for other elements like Devices and Map if needed.

}
