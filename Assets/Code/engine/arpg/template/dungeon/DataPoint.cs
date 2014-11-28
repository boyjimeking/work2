using UnityEngine;
using System.Collections;
using System.Xml;
namespace engine{
    public enum DataPointType {
        none,monster
    }
    public class DataPoint:BaseTemp {
        public DataPointType type;
        public int templateId;
        public float x, y, z;//monster initial position

        public override void read(XmlElement e) {
            base.read(e);
            type = toDataPointType(e.GetAttribute("type"));
            if (type == DataPointType.monster) {
                templateId = Utility.toInt(e.GetAttribute("template"));
            }
            string position=e.GetAttribute("position");
            if(!string.IsNullOrEmpty(position)){
                string[] xyz=position.Split(',');
                if (xyz.Length != 3) throw new System.Exception("invalid datapoint:" + position);
                x = Utility.toFloat(xyz[0].Trim());
                y = Utility.toFloat(xyz[1].Trim());
                z = Utility.toFloat(xyz[2].Trim());
            }
        }

        public DataPointType toDataPointType(string value) {
            if (value == "monster") return DataPointType.monster;

            return DataPointType.none;
        }
    }

}
