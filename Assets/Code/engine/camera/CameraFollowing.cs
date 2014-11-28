﻿/*
 * Camera Following, modified frow smoothFollow.js by ljx
 */
using UnityEngine;

namespace engine {
    public class CameraFollowing : MonoBehaviour {
        public float rushDelay = 0f;//主角冲刺之后镜头缓动时间
        public Transform target;
        public float xOffset = 10.5f;
        public float zOffset = 10.0f;
        public float height = 5.0f;
        public float lookHeight = 1f;
        public float hightSmooth = 0.5f;
        public float xzSmooth = 0.5f;

        private Vector3 lastPos;
        private Vector3 temp;
        private GameObject lookAtObj;

        void LateUpdate() {
            if (target == null)
                return;
            doUpdate();
        }

        public void makeFollow() {
            doUpdate(false);
        }

        private void quickLocate() {
            temp = target.position;
            temp.x += xOffset;
            temp.y += height;
            temp.z += zOffset;
            if(lookAtObj == null)
                lookAtObj = new GameObject();
            lookAtObj.transform.position = temp;
            lookAtObj.transform.LookAt(target.position + Vector3.up*lookHeight);
        }

        private void doUpdate(bool notLerp = true) {
            quickLocate();
            temp = target.position;
            float offsetX = xOffset;
            float offsetY = height;
            float offsetZ = zOffset;
            if (rushDelay <= 0 && notLerp) {
                offsetX = Mathf.Lerp(transform.position.x - temp.x, xOffset, xzSmooth);
                offsetY = Mathf.Lerp(transform.position.y - temp.y, height, hightSmooth);
                offsetZ = Mathf.Lerp(transform.position.z - temp.z, zOffset, xzSmooth);
            }
            lastPos = temp;
            temp.x = temp.x + offsetX;
            temp.y = temp.y + offsetY;
            temp.z = temp.z + offsetZ;
            if (rushDelay > 0) {
                rushDelay -= Time.deltaTime;
                transform.position += (temp - transform.position) * Time.deltaTime / rushDelay;
            }
            else {
                transform.position = temp;
                transform.rotation = lookAtObj.transform.rotation;
                //transform.LookAt(target.position + Vector3.up*lookHeight);
            }
        }
    }
}

