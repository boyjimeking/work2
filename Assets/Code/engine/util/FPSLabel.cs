﻿using UnityEngine;
using System.Collections;
namespace engine {
    public class FPSLabel : MonoBehaviour {

        float deltaTime = 0.0f;

        void Update() {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        }
        GUIStyle style = new GUIStyle();
        void OnGUI() {
            int w = Screen.width, h = Screen.height;

            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 80;
            style.normal.textColor = Color.white;// new Color(1, 1, 1, 1.0f);
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
        }
    }
}
