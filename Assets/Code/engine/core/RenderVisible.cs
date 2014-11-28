using UnityEngine;
using System.Collections;
using engine;

public class RenderVisible : MonoBehaviour {
    public FightCharacter c;
    void OnBecameInvisible()
    {
        c.becameVisible = false;
    }

    void OnBecameVisible()
    {
        c.becameVisible = true;
    }
}
