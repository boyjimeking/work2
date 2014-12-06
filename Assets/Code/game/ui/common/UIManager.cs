using UnityEngine;
using System.Collections.Generic;
    public class UIManager {
        private static UIManager instance;

        private GameObject uiRoot;
        private bool enable;
        private bool active = true;
        private List<Transform> oldNoActives = new List<Transform>();

        public GameObject cameraObject;
        public Camera uiCamera;

        private UIManager() {
            enable = true;
        }

        public void create(string uiName, Transform parent = null) {
            
        }

        public void init(Transform rootTrans) {
            uiRoot = rootTrans.gameObject;
            Transform t = rootTrans.FindChild("Camera");
            if (t)
            {
                cameraObject = t.gameObject;
                uiCamera = cameraObject.camera;
                int v = uiCamera.eventMask;
            }
                
        }

        public void hideUI(bool show, string path = null)
        {
            foreach (Transform t in RootTrans)
            {
                if (t.name != "Camera" && t.name != "loadingPanel(Clone)" && t.name != path)
                {
                    Debug.LogError(t.name);
                    t.gameObject.SetActive(show);
                }
            }
        }

      
        public static UIManager Instance {
            get {
                if(instance == null)
                    instance = new UIManager();
                return instance;
            }
        }

         public Transform RootTrans {
            get { return uiRoot.transform; }
        }

         public bool Active
         {
             get
             {
                 return active;
             }
             set
             {
                 active = value;
                 if (active)
                 {
                     foreach (Transform t in UIManager.Instance.RootTrans)
                     {
                         if (!oldNoActives.Contains(t)) t.gameObject.SetActive(true);
                     }
                 }
                 else
                 {
                     oldNoActives.Clear();
                     foreach (Transform t in UIManager.Instance.RootTrans)
                     {
                         if(t.transform == uiCamera.transform) continue;
                         if (!t.gameObject.activeSelf) oldNoActives.Add(t);
                         t.gameObject.SetActive(false);
                     }
                 }
                 App.input.setJoyEnabled(active);
                 
             }
         }

        public bool Enable {
            get {
                return enable;                
            }
            set {
                enable = value;
                UICamera camera = cameraObject.GetComponent<UICamera>();
                if (enable) {
                    camera.eventReceiverMask = 32;
                }
                else {
                    camera.eventReceiverMask = 0;
                    HeroController controller = Player.instance.controller as HeroController;
                    if (controller != null) {
                        controller.disableMove();
                    }
                }
                App.input.setJoyEnabled(enable);
            }
        }
    }
