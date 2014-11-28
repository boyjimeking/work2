using System.Collections;
using System.Collections.Generic;

namespace editor
{
    public enum RoleCareer
    {
        CareerBegin = 0,
        CareerSword = 1, //剑士
        CareerArcher = 2,   //弓箭手
        CareerMagician = 3,  //法师
        CareerEnd
    }

    //bundle的类型
    public enum EBundleType
    {
        eBundleShader,
        eBundleCommon,      //Common bundle
        eBundleScene,           //场景的 bundle
        eBundleMusic,           //音乐的bundle
        /*eBundlePicture,         //icon跟场景背景的bundle
        eBundleMusic,           //音乐的bundle
        eBundleWing,            //翅膀bundle
        eBundleSelectRole,      //选角用到的bundle
        eBundleShader,          //常驻内存的shader资源
        eBundleWeapon,          //武器bundle
        eBundleWeaponEffect,    //武器bundle 特效
        eBundleUI,              //UI bundle
        eBundleUIEffect,        //UI bundle 特效
        eBundleTaskEffect,      //任务bundle 特效
        eBundleNPC,             //NPC的 bundle
        eBundleBattleGoBulin,	//哥布林中用到的特殊资源
        eBundleBattleEffect,    //战斗专用的effect特效
        eBundleRoleEffect,      //战斗使用的对应角色prefab
        eBundleMonster,         //怪物bundle
        eBundleMulti,           //多人副本：战斗bundle
        eBundleRaid,            //关卡：战斗bundle
        eBundleGobulin,         //哥布林：战斗bundle
        eBundleTower,           //爬塔：战斗bundle 
        eBundleReward,          //悬赏战斗 bundle
        eBundleScenario,        //剧情战斗 bundle
        eBundleBoss,            //Boss战斗 bundle 
        eBundlePanduoLa,        //潘多拉战斗 bundle 
        eBundlePet,				//宠物模型
        eBundleConfig,          //配置文件
        eBundleWeaponDecorate,  //神器的bundle
        eBundleEnd,
        eBundleOther = 1000,  //没有划分类别的bundle，使用Resources.Load*/
    }

    public class BundleItem
    {
        public uint bundleID;       //bundle的ID索引
        public string bundleName;
        public RoleCareer career;
        public EBundleType bundleType;
        public int mapID;
        public List<string> subPrefabs;  //该bundle对应的所有子Prefab
        public int bundleVersion;

        public BundleItem()
        {
            subPrefabs = new List<string>();
        }
    }

    public class BundleConfig
    {
        public string path = null;
        private static BundleConfig _instance;      
        private Dictionary<int, BundleItem> _allItems;
        private Dictionary<EBundleType, List<BundleItem>> _bundleByTypeDic;     //根据bundle类型进行加载的预取件，必须线性时间查找


        public BundleConfig()
        {
            _allItems = new Dictionary<int, BundleItem>();
            _bundleByTypeDic = new Dictionary<EBundleType, List<BundleItem>>();
        }

        public static BundleConfig Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new BundleConfig();
                return _instance;
            }
        }

        public List<BundleItem> getBundleItemByType(EBundleType type)
        {
            if (_bundleByTypeDic.ContainsKey(type))
            {
                return _bundleByTypeDic[type];
            }
            return null;
        }

        public List<BundleItem> AllBundleItems
        {
            get { return new List<BundleItem>(_allItems.Values); }
        }

        public string RootName
        {
            get { return "RECORDS"; }
        }

        public void appendAttribute(int key, string name, string value)
        {
            BundleItem item;
            if (!_allItems.ContainsKey(key))
            {
                item = new BundleItem();
                _allItems.Add(key, item);
            }
            item = _allItems[key];

            switch (name)
            {
                case "ID":
                    item.bundleID = uint.Parse(value);
                    break;
                case "bundleName":
                    item.bundleName = trimPath(value);
                    break;
                case "type":
                    item.bundleType = (EBundleType)int.Parse(value);
                    addBundle(item);
                    break;
                case "career":
                    item.career = (RoleCareer)int.Parse(value);
                    break;
                case "version":
                    item.bundleVersion = int.Parse(value);
                    break;
                case "MapID":
                    item.mapID = int.Parse(value);
                    break;
                case "prefabs":
                    string[] arrStr = value.Split(';');
                    foreach (string str in arrStr)
                    {
                        item.subPrefabs.Add(str);
                    }                  
                    break;
            }
        }

        private string trimPath(string origName)
        {
            if (origName.IndexOf('/') != -1)
                return origName.Substring(origName.LastIndexOf('/') + 1);
            return origName;
        }

        private void addBundle(BundleItem item)
        {
            List<BundleItem> items;
            if (_bundleByTypeDic.ContainsKey(item.bundleType))
            {
                items = _bundleByTypeDic[item.bundleType];
            }
            else
            {
                items = new List<BundleItem>();
                _bundleByTypeDic.Add(item.bundleType, items);
            }
            items.Add(item);
        }
    }
}
