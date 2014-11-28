using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using engine;

public class BoomBox
{
    public DungeonTemplate.RandomBox box;
    public GameObject model;
    public bool boomed;
}
public class BoomBoxManager
{

    public static BoomBoxManager instance = new BoomBoxManager();
    public Dictionary<int, List<BoomBox>> triggerBoxs = new Dictionary<int, List<BoomBox>>();

    private List<BoomBox> boxs = new List<BoomBox>();
       

    private static string resbegin = "Local/break/";

    public void init(DungeonTemplate template,GameObject go)
    {
        if (go == null) return;
        Vector3 v = Vector3.zero;
        foreach (DungeonTemplate.RandomBox box in template.boxs)
        {
            if (Random.Range(0, 100) > box.rate) continue;
            Transform transform = getBoxTransForm(go, box.name);
            if (transform != null)
            {
                Transform[] allChildren = transform.GetComponentsInChildren<Transform>();
                foreach (Transform child in allChildren)
                {
                    BoomBox item = new BoomBox();
                    item.box = box;
                    GameObject model = App.res.createSingle(resbegin + box.wait);
                    model.animation.Stop();
                    model.addOnce<Binding>().data = item;
                    item.model = model;
                    v = child.position;
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(v, out hit, 100, 1))
                    {
                        v = hit.position;
                    }
                    model.transform.position = v;
                    model.transform.eulerAngles += child.eulerAngles;
                    boxs.Add(item);
                    List<BoomBox> boxList;
                    if (triggerBoxs.ContainsKey(box.preTrigger)) {
                        boxList = triggerBoxs[box.preTrigger];
                    }
                    else {
                        boxList = new List<BoomBox>();
                        triggerBoxs.Add(box.preTrigger, boxList);
                    }
                    boxList.Add(item);
                }
            }         
        }
    }
    public Transform getBoxTransForm(GameObject go,string name)
    {
        Transform[] allChildren = go.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.gameObject.name == name)
            {
                return child;
            }
        }
        return null;    
    }
    public void destroy(BoomBox item)
    {
        if (item.boomed) return;
        item.boomed = true;
        if (Random.Range(0, 100) > 50)
        {
            DropManager.instance.dropGold(item.model.transform, Random.Range(3, 15));
        }
        //item.model.animation.Play();
        ExploderManager.instance.exploderDefault(item.model);
        //GameObject.Destroy(item.model, 2f);
        boxs.Remove(item);
    }

    protected void OnExplosion(float timeMS, ExploderObject.ExplosionState state)
    { 
    
    }

    public void clear()
    {
        for (int i = boxs.Count - 1; i >= 0; i--)
        {
            GameObject.Destroy(boxs[i].model);
        }
        boxs.Clear();
    }
}
