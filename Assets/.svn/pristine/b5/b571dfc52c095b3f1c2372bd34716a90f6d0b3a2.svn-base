using UnityEngine;
using System.Collections;
namespace engine {
    //used for action bars on screen right bottom area.
    //app subclass this.
    public class EasyButtonEventHandler : MonoBehaviour {
        void OnEnable() {
            addListener();
        }
        void OnDisable() {
            removeListener();
        }
        void OnDestroy() {
            removeListener();
        }
        protected void addListener() {
            EasyButton.On_ButtonDown += onButtonDown;
        }
        protected void removeListener() {
            EasyButton.On_ButtonDown -= onButtonDown;
        }
        protected virtual void onButtonDown(string buttonName) {
            Debug.Log("clicked button:" + buttonName);
        }

    }
}

