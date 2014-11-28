using UnityEngine;
using System.Collections;
namespace engine {
    //fighter info in a dungeon
    public class FighterInfo {
        public int instanceId;
        public int templateId;//char template id
        public DataPoint dataPoint;//position or other info in dungeon template configuration

        public int hp = 10000;

        

        public FighterInfo(int instanceId, int templateId) {
            this.instanceId = instanceId;
            this.templateId = templateId;
            
        }

    }
}

