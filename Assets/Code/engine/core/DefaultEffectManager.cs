using UnityEngine;
using System.Collections;
namespace engine {
    public class DefaultEffectManager {
        public bool shakingCamera;
        public CameraShake cameraShaker;
        public float shakingY;
        public void shakeCamera(float magnitude, float duration) {
            if (cameraShaker == null) {
                cameraShaker = Camera.main.gameObject.GetComponent<CameraShake>();
            }
            if (cameraShaker != null) {
                if (shakingCamera) return;
                shakingCamera = true;
                //cameraShaker.shakeCamera();//magnitude, duration);
                cameraShaker.shakeCameraLinear();
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

