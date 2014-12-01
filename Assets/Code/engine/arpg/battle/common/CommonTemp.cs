using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace engine {
    public class CommonTemp : BaseTemp {
        //door anim configuration
        public static float toDoorTime;
        public static float doorOffset;
        public static float doorCameraY;
        public static float fromDoorTime;
        //boss born configuration
        public static float xDelta;
        public static float yDelta;
        public static float initAlpha;
        public static float tweenT;
        public static float alphaT;
        public static float nameT;
        public static float finalT;
        public static float bossOffset;
        public static float bossHeight;
        public static float bossLookY;
        public static float bossScale;
        //pet born configuration
        public static float petScale;
        public static float blackAlpha;
        public static float blackAlphaIOS;
        public static float tweenPetTime;
        public static float petView;
        public static float tweenNormalTime;
        public static float[] blackTimes = {0, 0};
        public static float[] effectTimes = { 0, 0};
        public static string[] effectPrefabs = { null, null };
        public static float[] bornTimes = { 0, 0 };
        public static bool[] rotates = {true, true};
        public static string[] eventNames = { "", "" };
        //battle win configuration
        public static float winOffset;
        public static float winHeight;
        public static float winView;
        public static float winLookY;
        public static Vector3 petPosOffset;
        public static float bossDieScale;
        public static float bossScaleTime;
        public static float showwindelay;
        public static int groundcount;

        public override void read(XmlElement e) {
            base.read(e);
            if (id == 1) {
                toDoorTime = Utility.toFloat(e.GetAttribute("toDoorTime"));
                doorOffset = Utility.toFloat(e.GetAttribute("doorOffset"));
                doorCameraY = Utility.toFloat(e.GetAttribute("doorCameraY"));
                fromDoorTime = Utility.toFloat(e.GetAttribute("fromDoorTime"));
            }
            else if (id == 100) {
                xDelta = Utility.toFloat(e.GetAttribute("xDelta"));
                yDelta = Utility.toFloat(e.GetAttribute("yDelta"));
                initAlpha = Utility.toFloat(e.GetAttribute("initAlpha"));
                tweenT = Utility.toFloat(e.GetAttribute("tweenT"));
                alphaT = Utility.toFloat(e.GetAttribute("alphaT"));
                nameT = Utility.toFloat(e.GetAttribute("nameT"));
                finalT = Utility.toFloat(e.GetAttribute("finalT"));
                bossOffset = Utility.toFloat(e.GetAttribute("bossOffset"));
                bossHeight = Utility.toFloat(e.GetAttribute("bossHeight"));
                bossLookY = Utility.toFloat(e.GetAttribute("bossLookY"));
                bossScale = Utility.toFloat(e.GetAttribute("bossScale"));
            }
            else if (id == 200) {
                petScale = Utility.toFloat(e.GetAttribute("petScale"));
                blackAlpha = Utility.toFloat(e.GetAttribute("blackAlpha"));
                blackAlphaIOS = Utility.toFloat(e.GetAttribute("blackAlphaIOS"));
                tweenPetTime = Utility.toFloat(e.GetAttribute("tweenPetTime"));
                petView = Utility.toFloat(e.GetAttribute("petView"));
                tweenNormalTime = Utility.toFloat(e.GetAttribute("tweenNormalTime"));
            }
            else if (id == 201) {
                blackTimes[0] = Utility.toFloat(e.GetAttribute("blackTime"));
                effectTimes[0] = Utility.toFloat(e.GetAttribute("effectTime"));
                effectPrefabs[0] = e.GetAttribute("effectPrefab");
                bornTimes[0] = Utility.toFloat(e.GetAttribute("bornTime"));
                eventNames[0] = e.GetAttribute("eventName");
                rotates[0] = Utility.toInt(e.GetAttribute("rotate")) == 1;
            }
            else if (id == 202) {
                blackTimes[1] = Utility.toFloat(e.GetAttribute("blackTime"));
                effectTimes[1] = Utility.toFloat(e.GetAttribute("effectTime"));
                effectPrefabs[1] = e.GetAttribute("effectPrefab");
                bornTimes[1] = Utility.toFloat(e.GetAttribute("bornTime"));
                eventNames[1] = e.GetAttribute("eventName");
                rotates[1] = Utility.toInt(e.GetAttribute("rotate")) == 1;
            }
            else if(id == 300){
                winOffset = Utility.toFloat(e.GetAttribute("winOffset"));
                winHeight = Utility.toFloat(e.GetAttribute("winHeight"));
                winView = Utility.toFloat(e.GetAttribute("winView"));
                winLookY = Utility.toFloat(e.GetAttribute("winLookY"));
                petPosOffset = Utility.toVector3(e.GetAttribute("petPosOffset"));
                bossDieScale = Utility.toFloat(e.GetAttribute("bossDieScale"));
                bossScaleTime = Utility.toFloat(e.GetAttribute("bossScaleTime"));
                showwindelay = Utility.toFloat(e.GetAttribute("showwindelay"));
                groundcount = Utility.toInt(e.GetAttribute("groundcount"));
            }          
        }
    }

}
