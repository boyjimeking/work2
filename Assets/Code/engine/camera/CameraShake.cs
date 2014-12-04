using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace engine {
    public class CameraShake : MonoBehaviour {

        protected float duration;
        protected float magnitude;

        public float deltay;//camera vibration in y diection.
        public float deltax;

        private int maxShakeCount;
        private int count;
        public void shake(float magnitude, float duration, int maxShakeCount = 0) {
            this.magnitude = magnitude;
            this.duration = duration;
            this.enabled = true;
            if (maxShakeCount != 0) {
                this.maxShakeCount = maxShakeCount;
            } else {
                this.maxShakeCount = 2;
            }
            count = 0;
        }
        void Start() {
            _fieldOfView = Camera.main.fieldOfView;
            this.enabled = false;
        }

        // Update is called once per frame
        int updatecount=0;
        void Update() {
            
            duration -= Time.deltaTime;
            if (duration > 0) {
                if (count > maxShakeCount) {
                    //BattleEngine.effect.shakingCamera = false;
                    this.enabled = false;
                } else {
                    deltax = Mathf.Sin(1000 * Time.time) * duration * magnitude;
                    deltay = Mathf.Sin(1000 * Time.time) * duration * magnitude;
                    count++;
                }

            } else {
                //BattleEngine.effect.shakingCamera = false;
                //this.enabled = false;
            }

            if (!linearEnd)
            {
                updatecount++;
                if (Application.platform == RuntimePlatform.WindowsEditor) {
                    if (updatecount % 2 == 0) return;
                }
                linearDelta += Time.deltaTime;
                //if(linearDelta>=0.035)  Debug.Log(linearDelta);
                CameraManager.Main.fieldOfView = vs[index] + (vs[index + 1] - vs[index]) * (linearDelta>ds[index]?ds[index]:linearDelta) / ds[index];
                //if (CameraManager.Main.fieldOfView <= 65) Debug.Log(linearDelta+" : "+ CameraManager.Main.fieldOfView);
                
                if (linearDelta >= ds[index]) {
                    linearDelta = 0f;
                    index++;
                    if (index >= ds.Count)
                    {
                        linearEnd = true;
                        updatecount = 0;
                        BattleEngine.effect.shakingCamera = false;
                        //this.enabled = false;
                    }
                }
            }
        }

        private float _fieldOfView;

       // public float cameraFarTime = 0.15f;
       // public iTween.EaseType cameraFarType = iTween.EaseType.easeOutElastic;
       // public float farNearRange = 2.1f;
       // public float maxFieldOfView = 10;
       // public float cameraNearTime = 0.04f;
       // public iTween.EaseType cameraNearType = iTween.EaseType.easeOutQuad;

        public void shakeCamera() {
            float fieldOfView = Camera.main.fieldOfView;
            if (_fieldOfView - fieldOfView - BattleConfig.farNearRange < BattleConfig.maxFieldOfView) {
                Hashtable ht = iTween.Hash("from", fieldOfView, "to", fieldOfView - BattleConfig.farNearRange, "time", BattleConfig.cameraNearTime,
                    "easeType", (iTween.EaseType)BattleConfig.cameraNearType, "onupdate", "changeCamera", "oncomplete", "farCamera");
                iTween.ValueTo(gameObject, ht);
            }
        }
        private void changeCamera(float fieldOfView) {
            Camera.main.fieldOfView = fieldOfView;
        }

        private void farCamera() {
            float fieldOfView = Camera.main.fieldOfView;
            Hashtable ht = iTween.Hash("from", fieldOfView, "to", _fieldOfView, "time", BattleConfig.cameraFarTime,
                 "easeType", (iTween.EaseType)BattleConfig.cameraFarType, "onupdate", "changeCamera");
            iTween.ValueTo(gameObject, ht);
        }

        private List<float> vs = new List<float>();
        private List<float> ds = new List<float>();
        private bool linearEnd;
        private float linearDelta;
        private int index;
        public void shakeCameraLinear(){
            vs.Clear();
            ds.Clear();
            vs.Add(_fieldOfView);
            vs.Add(_fieldOfView - BattleConfig.cameraShakePoint1);
            //vs.Add(_fieldOfView - BattleConfig.cameraShakePoint2);
            //vs.Add(_fieldOfView - BattleConfig.cameraShakePoint3);
            vs.Add(_fieldOfView);

            ds.Add(BattleConfig.cameraShakeTime1 / 2);
            ds.Add(BattleConfig.cameraShakeTime1 / 2);
            //ds.Add(BattleConfig.cameraShakeTime2 / 2);
            //ds.Add(BattleConfig.cameraShakeTime2 / 2);

            linearDelta = 0f;
            index = 0;
            linearEnd = false;
            this.enabled = true;
        }
    }
}

