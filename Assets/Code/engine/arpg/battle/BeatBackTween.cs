using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeatBackTween {
    public GameObject go;
    public List<Vector3> vs = new List<Vector3>();
    public List<float> ds = new List<float>();
    public float delta;
    public int index;
    public bool end;
    public int updatecount=0;
    public void reset(GameObject go, Vector3 dirforce)
    {
        vs.Clear();
        ds.Clear();

        Vector3 v = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z);
        this.go = go;
        vs.Add(v);
        vs.Add(v + dirforce * BattleConfig.hitBackPoint1);
        vs.Add(v + dirforce * BattleConfig.hitBackPoint2);
        vs.Add(v + dirforce * BattleConfig.hitBackPoint3);
        vs.Add(v + dirforce);

        ds.Add(BattleConfig.hitBackTime1 / 2);
        ds.Add(BattleConfig.hitBackTime1 / 2);
        ds.Add(BattleConfig.hitBackTime2 / 2);
        ds.Add(BattleConfig.hitBackTime2 / 2);

        this.delta = 0f;
        this.index = 0;
        v = vs[0];

        end = false;
    }

    public void update()
    {
        if (end) return;
        updatecount++;
        if (Application.platform == RuntimePlatform.WindowsEditor) if (updatecount % 2 == 0) return;
        delta += Time.deltaTime;
        if(delta>=ds[index]) delta=ds[index];

        go.transform.position = vs[index] + (vs[index + 1] - vs[index]) * delta / ds[index];
        if (delta >= ds[index])
        {
            delta = 0f;
            index++;
            if (index >= 4)
            {
                end = true;
                updatecount = 0;
            }
        }
        
    }

}
