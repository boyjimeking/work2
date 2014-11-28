using System.Xml;

namespace engine {
    public class BaseTemp{
        public int id;
        public string name;
        public string bundleName;
        public string description;
        public int resId;

        public virtual void read(XmlElement e) {
            id = Utility.toInt(e.GetAttribute("id"));
            bundleName = e.GetAttribute("bundleName");
        }
    }
}

