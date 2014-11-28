using UnityEngine;
using System.Collections;

public class RenderAngle : MonoBehaviour {

	public bool isRendering=false;
    private float lastTime=0;
    private float curtTime=0;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        isRendering = curtTime != lastTime ? true : false;
        lastTime = curtTime;
	}

    void OnWillRenderObject(){
        curtTime = Time.time;
    }
}
