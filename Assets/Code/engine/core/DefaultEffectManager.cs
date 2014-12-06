using UnityEngine;
using System.Collections;
namespace engine {
    public class DefaultEffectManager {
        public bool shakingCamera;
        public CameraShake cameraShaker;
        public float shakingY;
        public void shakeCamera(object efffect = null/*float magnitude, float duration*/)
        {
            if (cameraShaker == null) {
                cameraShaker = Camera.main.gameObject.GetComponent<CameraShake>();
            }
            if (cameraShaker != null) {
                if (shakingCamera ) return;
                CameraFollowing follow = CameraManager.Main.GetComponent<CameraFollowing>();
                if (follow.Rushing) return;
                shakingCamera = true;
                //cameraShaker.shakeCamera();//magnitude, duration);
                cameraShaker.shakeCameraLinear(efffect);
            }
        }

        //slow time effect
        private float slowNextTime,slowStopTime;
        private bool inSlowTime;
        public void checkSlowTimeScale() {
            float now = Time.realtimeSinceStartup;
            if (slowNextTime < now) {
                slowNextTime = now + BattleConfig.slowCD;
                slowStopTime = now + BattleConfig.slowDuration;
                Time.timeScale = BattleConfig.slowTimeScale;
                inSlowTime = true;
            }
        }
        public void update() {
            if (inSlowTime) {
                if (slowStopTime < Time.realtimeSinceStartup) {
                    Time.timeScale = 1f;
                    inSlowTime = false;
                }
            }
        }
    }
}

