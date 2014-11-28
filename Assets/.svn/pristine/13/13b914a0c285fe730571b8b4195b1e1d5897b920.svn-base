/// <summary>
/// Floating text.
/// the GUI Text Floating system
/// </summary>

using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour {

	public GUISkin CustomSkin;// GUISkin
	public string Text = "";// Text
	public float LifeTime = 1;// Life time
	public bool FadeEnd = false;// Fade out at last 1 second before destroyed
	public Color TextColor = Color.white; // Text color
	public bool Position3D = false; // enabled when you need the text along with world 3d position
	public Vector2 Position; // 2D Position
	public int FontSize = 22;
	private float alpha = 1;
	private float timeTemp = 0;

    private GameObject floatLabel;
    private UILabel uiLabel;
	void Start () {
		timeTemp = Time.time;
		GameObject.Destroy(this.gameObject,LifeTime);
        GameObject.Destroy(floatLabel, LifeTime);
		if(Position3D && Camera.main){
			Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
			Position = new Vector2(screenPos.x,Screen.height - screenPos.y);
		}
	}

    public void reset(GameObject floatLabel, string damage) {
        this.floatLabel = floatLabel;
        this.uiLabel = floatLabel.GetComponent<UILabel>();
        if(string.Empty!=damage && damage!="")
            this.uiLabel.text = damage;
    }

	void Update () {
        if (floatLabel != null)
        {
            if (FadeEnd)
            {
                if (Time.time >= ((timeTemp + LifeTime) - 1))
                {
                    uiLabel.alpha = 1.0f - (Time.time - ((timeTemp + LifeTime) - 1));
                }
            }
            else
            {
                uiLabel.alpha = 1.0f - ((1.0f / LifeTime) * (Time.time - timeTemp));
            }
        }
	
		if(Position3D){
		    if (Camera.main == null)
		        Destroy(gameObject);
		    else {
		        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
                screenPos = UIManager.Instance.uiCamera.ScreenToWorldPoint(new Vector3(screenPos.x + 0 / 2, screenPos.y + 0 / 2, 0));
                if(floatLabel!=null)
                    floatLabel.transform.position = screenPos;
                //Position = new Vector2(screenPos.x, Screen.height - screenPos.y);
		    }
		}
	
	}


    //void OnGUI(){
		
        //GUI.color = new Color(GUI.color.r,GUI.color.g,GUI.color.b,alpha);
        //if(CustomSkin){
        //    GUI.skin = CustomSkin;
        //}
        //GUI.skin.label.fontSize = FontSize;
        //Vector2 textsize = GUI.skin.label.CalcSize(new GUIContent(Text));
        //Rect rect = new Rect(Position.x - (textsize.x/2), Position.y,textsize.x,textsize.y);

        //GUI.skin.label.normal.textColor = TextColor;
        //GUI.Label(rect,Text);

    //}
}
