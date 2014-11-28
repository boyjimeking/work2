using System.Collections;
using System.Collections.Generic;
using System.Xml;
using engine;
    public class Templates {
        private Hashtable xmlDatas = new Hashtable();
        private Dictionary<int, CharAnimation> charAnimations = new Dictionary<int, CharAnimation>();
        private Dictionary<int, SkillTemplate> skills = new Dictionary<int, SkillTemplate>();
        private Dictionary<int, CharTemplate> chars = new Dictionary<int, CharTemplate>();
        private Dictionary<int, SkillEffectTemplate> skillEffects = new Dictionary<int, SkillEffectTemplate>();
        private Dictionary<int, DungeonTemplate> dungeons = new Dictionary<int, DungeonTemplate>();
        private Dictionary<int, SceneTemp> scenes = new Dictionary<int, SceneTemp>();
        private Dictionary<int, CharDataTemplate> charDatas = new Dictionary<int, CharDataTemplate>();
        private Dictionary<int, Script> scripts = new Dictionary<int, Script>();

        private int count;
        private int currentIndex;
        private LoadingManager.loadXmlCallBack callBack;

        public void init() {
            count = 9;
            currentIndex = 0;
            
        }
        public void loadxml(LoadingManager.loadXmlCallBack callBack)
        {
            App.coroutine.StartCoroutine(onload());
            this.callBack = callBack;
        }
        public IEnumerator onload()
        {
            initTemp("char_anim", "character", charAnimations);
            update();
              yield return 2;
            initTemp("skill_effect", "effect", skillEffects);
            update();
              yield return 2;
            initTemp("char_template", "character", chars);
            update();
              yield return 2;
            initTemp("skill", "skill", skills);
            update();
              yield return 2;
            initTemp("dungeon_template", "dungeon", dungeons);
            update();
              yield return 2;
            initTemp("scene", "RECORD", scenes);
            update();
              yield return 2;
            initTemp("char_data", "char", charDatas);
            update();
              yield return 2;
            initTemp<CommonTemp>("common", "config", null);
            update();
              yield return 2;
            initTemp("scripts", "script", scripts);
            update();
              yield return 2;
            loadxmlfinish();
        }
        public void update(){
            currentIndex++;
            LoadingManager.instance.onProgress((float)currentIndex / (float)count, 1);
        }
        private void loadxmlfinish()
        {
            foreach (SkillEffectTemplate template in skillEffects.Values)
            {
                if (template.id > 0)
                {
                    template.postRead(this);
                    if (skills.ContainsKey(template.id))
                    {
                        skills[template.id].effect = template;
                        template.skillTemplate = skills[template.id];
                    }
                }
            }
            foreach (CharAnimation anim in charAnimations.Values)
            {
                anim.postRead(this);
            }
            if (callBack != null) {
                callBack();
                callBack = null;
            }
        }

        private void initTemp<T>(string xmlName, string tagName, Dictionary<int, T> container) where T : BaseTemp, new()
        {
            XmlDocument xml = new XmlDocument();
            string xmlText = App.res.loadText("Local/data/xml/" + xmlName);
            xml.LoadXml(xmlText);
            XmlNodeList list = xml.GetElementsByTagName(tagName);
            string typeName = typeof(T).ToString();
            if (!xmlDatas.Contains(typeName)) {
                xmlDatas.Add(typeName, container);
            }
            for (int i = 0, max = list.Count; i < max; i++) {
                XmlElement e = list.Item(i) as XmlElement;
                if (e != null) {
                    T temp = new T();
                    temp.read(e);
                    if(container != null)
                        container.Add(temp.id, temp);
                }
            }
            currentIndex++;
         }

        public T getTemp<T>(int id) where T : BaseTemp {
            Dictionary<int, T> container = getTemps<T>();
            if (container != null && container.ContainsKey(id)) {
                return container[id];
            }
            return null;
        }

        public Dictionary<int, T> getTemps<T>() where T : BaseTemp {
            string name = typeof (T).ToString();
            Dictionary<int, T> container = xmlDatas[name] as Dictionary<int, T>;
            return container;
        }
    }

