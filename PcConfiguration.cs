using System.Configuration;

namespace CentralPowerMonitor.Configuration
{
    public class PcConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("pcs")]
        public PcCollection Pcs => (PcCollection)this["pcs"];
    }

    [ConfigurationCollection(typeof(PcElement))]
    public class PcCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PcElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PcElement)element).Alias;
        }
    }

    public class PcElement : ConfigurationElement
    {
        [ConfigurationProperty("alias", IsRequired = true, IsKey = true)]
        public string Alias
        {
            get { return (string)this["alias"]; }
            set { this["alias"] = value; }
        }

        [ConfigurationProperty("ipAddress", IsRequired = true)]
        public string IpAddress
        {
            get { return (string)this["ipAddress"]; }
            set { this["ipAddress"] = value; }
        }
    }
}

