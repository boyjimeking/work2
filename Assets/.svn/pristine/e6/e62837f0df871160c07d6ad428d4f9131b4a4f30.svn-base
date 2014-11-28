using UnityEngine;
using System.Collections;
using engine;

public class HeadController {

	// Use this for initialization
    public static HeadController instance = new HeadController();
    private CharData data;
    private GameObject ui_instance;
    public UISprite bg;
    public UISprite icon;
    public UILabel level;

    public void init()
    {
        ui_instance = Engine.res.createSingle("Local/prefab/battleui/playerHead");
        icon = ui_instance.getChildComponent<UISprite>("icon");
        level = ui_instance.getChildComponent<UILabel>("level");
        if (data != null)
        {
            setLevel(data.level);
            setIcon("common_zhanshi");
        }
    }
    public void clear() {
        
    }
    public void reset(CharData data){
        this.data = data;
        if (ui_instance == null) return;
        if (data != null)
        {
            setLevel(data.level);
            setIcon("common_zhanshi");
        }
    }

    public void setIcon(string _spriteName) {
        //icon.spriteName = _spriteName;
    }
    public void setLevel(int _level) {
        level.text = _level + "";
    }

    public void setActive(bool value){
        ui_instance.SetActive(value);
    }
}
