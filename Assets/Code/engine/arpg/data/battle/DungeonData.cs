using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace engine {
    public class RoomState {
        public bool doorOpened;
    }
    public class DungeonData {
        public int instanceId;
        public int templateId;
        public int currentRoomIndex;
        public int currentGroupIndex;//current spawn group index in the room.
        public int totalGroupCount;
        public int currentTriggerIndex = 1;
        public int clearCount = 0;

        public DungeonTemplate template;

        public string unitySceneName;//unity editor saved scene name. we need this for steaming scene.

        public RoomState[] roomStates;


        public void clear()
        {
            
        }

        public DungeonData(int instanceId, int templateId) {
            this.instanceId = instanceId;
            this.templateId = templateId;
            template = App.template.getTemp<DungeonTemplate>(templateId);

            int length = template.rooms.Length;
            roomStates=new RoomState[length];
            for (int i = 0; i < length; i++) {
                roomStates[i] = new RoomState();
            }
        }
        public void nextRoom() {
            currentRoomIndex++;
            currentGroupIndex = 0;
        }
        public int nextGroup() {
            currentGroupIndex++;
            return currentGroupIndex;
        }
        public bool hasNextGroup() {
            return currentGroupIndex + 1 < totalGroupCount;
        }
        public bool hasNextRoom() {
            return currentRoomIndex + 1 < template.rooms.Length;
        }

        public bool hasNextTrigger(bool isPlayer) {
            if (isPlayer) {
                return template.triggers.ContainsKey(currentTriggerIndex);
            }
            if (clearCount > 0) {
                clearCount --;
                return template.triggers.ContainsKey(currentTriggerIndex);
            }
            return false;
        }

        public bool hasBoxTrigger() {
            return BoomBoxManager.instance.triggerBoxs.ContainsKey(currentTriggerIndex);
        }
    }
}

