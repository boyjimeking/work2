using System.Collections.Generic;
using System.Xml;
using AnimationOrTween;

namespace engine {
    public enum DungeonTriggerType {
        door, monsterSpawnTrigger
    }
    public class DungeonTemplate : BaseTemp {

        public class RandomBox {
            public int id;
            public string name;
            public int rate;
            public string wait;
            public int preTrigger;
           
            public void parse(XmlElement e)
            {
                id = Utility.toInt(e.GetAttribute("id"));
                rate = Utility.toInt(e.GetAttribute("appRate"));
                name = e.GetAttribute("name");
                wait = e.GetAttribute("wait");
                preTrigger = Utility.toInt(e.GetAttribute("preTrigger"));
            }
        }
       
        public class SpawnGroup{
            public float x,y,z;
            public float radius;
            public int[] monsters;//id,count,id,count,...
            public int bossID;
            public float bossx, bossy, bossz;

            public void parse(XmlElement e) {
                string[] parts = e.GetAttribute("pos").Split(',');
                x = Utility.toFloat(parts[0]);
                y = Utility.toFloat(parts[1]);
                z = Utility.toFloat(parts[2]);
                radius = Utility.toFloat(e.GetAttribute("radius"));


                parts = e.GetAttribute("monster").Split(',');
                int length = parts.Length;
                monsters = new int[length * 2];
                int index = 0;
                for (int i = 0; i < length; i++) {
                    string[] value = parts[i].Split(':');
                    monsters[index++] = Utility.toInt(value[0]);
                    monsters[index++] = Utility.toInt(value[1]);
                }
                bossID = 0;
                string v = e.GetAttribute("bossID");
                if (!string.IsNullOrEmpty(v)) {
                    bossID = Utility.toInt(v);
                    string[] parts2 = e.GetAttribute("bossPos").Split(',');
                    if (parts2.Length >= 3) {
                        bossx = Utility.toFloat(parts2[0]);
                        bossy = Utility.toFloat(parts2[1]);
                        bossz = Utility.toFloat(parts2[2]);
                    }
                }
            }
           
        }
        public class DungeonRoom {
            public List<SpawnGroup> groups=new List<SpawnGroup>();
            public DungeonTrigger doorTrigger;
            public DungeonTrigger monsterSpawnTrigger;

            public void parse(XmlElement e, Dictionary<int, DungeonTrigger> triggers) {
                parseMonster(e.GetElementsByTagName("monster"));
                parseTriggers(e.GetElementsByTagName("triggers"), triggers);
            }
            private void parseTriggers(XmlNodeList list, Dictionary<int, DungeonTrigger> triggers) {
                int length = list.Count;
                if (length !=1) return;
                list = (list.Item(0) as XmlElement).GetElementsByTagName("trigger");
                length = list.Count;
                for (int i = 0; i < length; i++) {
                    XmlElement e = list.Item(i) as XmlElement;
                    DungeonTrigger trigger = new DungeonTrigger();
                    trigger.parse(e);
                    triggers[trigger.sequence] = trigger;
                    if (trigger.type == (int)DungeonTriggerType.door) {
                        doorTrigger = trigger;
                    } else if (trigger.type == (int)DungeonTriggerType.monsterSpawnTrigger) {
                        monsterSpawnTrigger = trigger;
                    }
                    
                }
            }
            public void parseMonster(XmlNodeList list) {
                int length = list.Count;
                for (int i = 0; i < length; i++) {
                    XmlElement e = list.Item(i) as XmlElement;
                    SpawnGroup group = new SpawnGroup();
                    group.parse(e);
                    groups.Add(group);
                }
            }

        }

        public class DungeonTrigger {
            public string name;
            public int type;
            public string animation, effect;
            public int sequence;
 			public string state,enterRoomTriggerName;

            public void parse(XmlElement e) {
                name = e.GetAttribute("name");
                type = Utility.toInt(e.GetAttribute("type"));
                animation = e.GetAttribute("animation");
                effect = e.GetAttribute("effect");
                sequence = Utility.toInt(e.GetAttribute("sequence"));
 				state = e.GetAttribute("state");
                enterRoomTriggerName = e.GetAttribute("enterRoom");
            }

            public bool isDoorTrigger() {
                return (DungeonTriggerType) type == DungeonTriggerType.door;
            }
        }
      
        public DungeonRoom[] rooms;
        public Dictionary<int, DungeonTrigger> triggers;
        public List<RandomBox> boxs = new List<RandomBox>();
        public string triggerPrefab;
        public string sound;
       

        public override void read(XmlElement e) {
            base.read(e);
            name = e.GetAttribute("name");
            description = e.GetAttribute("description");
            triggerPrefab = e.GetAttribute("triggerPrefab");
            triggers = new Dictionary<int, DungeonTrigger>();
            parseRoom(e.GetElementsByTagName("room"), triggers);
            parseBox(e.GetElementsByTagName("box"));
            sound = e.GetAttribute("sound");
            
        }
        protected void parseRoom(XmlNodeList list, Dictionary<int, DungeonTrigger> triggers) {
            int length = list.Count;
            rooms = new DungeonRoom[length];
            for (int i = 0; i < length; i++) {
                XmlElement e = list.Item(i) as XmlElement;
                DungeonRoom room = new DungeonRoom();
                room.parse(e, triggers);
                rooms[i] = room;
            }
        }
        protected void parseBox(XmlNodeList list)
        {
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                XmlElement e = list.Item(i) as XmlElement;
                RandomBox box = new RandomBox();
                box.parse(e);
                boxs.Add(box);
            }
        }
        
    }

}
