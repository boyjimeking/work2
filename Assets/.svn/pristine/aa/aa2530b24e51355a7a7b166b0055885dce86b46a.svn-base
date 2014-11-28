using UnityEngine;
using System.Collections;
using engine;

public class PlayerDie
{

    public static PlayerDie instance = new PlayerDie();

    public GameObject go;
    
    public void init()
    {
        go = Engine.res.createSingle("Local/prefab/battleui/playerdie");

    }
    public void clear()
    {
        
    }

    public void setActive(bool value)
    {
        if (go == null) init();
        go.SetActive(value);
    }
}
