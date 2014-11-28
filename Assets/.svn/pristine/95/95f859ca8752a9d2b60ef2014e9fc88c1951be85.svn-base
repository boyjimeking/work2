using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace engine {
    public enum SceneType {
        Home,
        Dungeon,
        Elite,
        Tower,
        Arena
    }

    public class SceneTemp :BaseTemp {
        public string sceneName;
        public string displayName;
        public SceneType type;
        public int chapter;
        public string icon;
        public Vector3 iconPos;
        public Vector3 initPos;
        public Vector3 finalPos;
        public Vector3 playerRotate;
        public float initView;
        public float finalView;
        public float transitTime;
        public float moveTime;
        public float slowScale;

        public List<string> loadedBundles; //when player come into scene, which bundle need loaded

        public override void read(XmlElement e) {
            base.read(e);
            sceneName = e.GetAttribute("sceneName");
            displayName = e.GetAttribute("displayName");
            type = (SceneType)Utility.toInt(e.GetAttribute("type"));
            chapter = Utility.toInt(e.GetAttribute("chapter"));
            icon = e.GetAttribute("icon");
            iconPos = Utility.toVector3(e.GetAttribute("iconPos"));
            initPos = Utility.toVector3(e.GetAttribute("initPos"));
            finalPos = Utility.toVector3(e.GetAttribute("finalPos"));
            float rotate = Utility.toFloat(e.GetAttribute("playerRotate"));
            playerRotate = new Vector3(0, rotate, 0);
            initView = Utility.toFloat(e.GetAttribute("initView"));
            finalView = Utility.toFloat(e.GetAttribute("finalView"));
            transitTime = Utility.toFloat(e.GetAttribute("transitTime"));
            moveTime = Utility.toFloat(e.GetAttribute("moveTime"));
            slowScale = Utility.toFloat(e.GetAttribute("slowScale"));
        }       
    }

}
