using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using engine;
public class DropItem
{
    public bool pickupEnable = false;
    public float stayTime = 1.5f;//掉落后等待时间
    public GameObject model;
}

public class WaitDropItem
{
    public static float dropSpace = 0.1f;
    public Vector3 position;
    public float delay; 
}

public class DropManager {

    public static DropManager instance = new DropManager();
    private float flySpeed = 10f;
    private float hideDistance = 0.5f;
    private List<WaitDropItem> wditems = new List<WaitDropItem>();
    private List<DropItem> items = new List<DropItem>();
    private List<DropItem> flyingItems = new List<DropItem>();
    public void drop(DropItem dropItem) {
        items.Add(dropItem);
    }

    public void clear()
    {
        foreach (DropItem item in items)
        {
            GameObject.Destroy(item.model);
        }
        foreach (DropItem item in flyingItems)
        {
            GameObject.Destroy(item.model);
        }
        items.Clear();
        flyingItems.Clear();
        wditems.Clear();
    }

    public void update() {
        float deltaTime = Time.deltaTime;
        int length = wditems.Count;
        for (var i = length - 1; i > -1; i--)
        {
            WaitDropItem wd = wditems[i];
            wd.delay -= deltaTime;
            if (wd.delay <= 0)
            {
                doDrop(wd);
                wditems.RemoveAt(i);
            }
        }

        length = items.Count;
        for (int i = length - 1; i > -1; i--) { 
            DropItem item = items[i];
            item.stayTime -= deltaTime;
            if (item.stayTime <= 0)//等待时间过期,添加到fly列表
            {
                items.RemoveAt(i);
                flyingItems.Add(item);
            }
        }
        Vector3 playerPosition = Player.instance.model.transform.position;
        length = flyingItems.Count;
        for (int i = length - 1; i > -1; i--)
        {
            DropItem item = flyingItems[i];
            Vector3 itemPosition = item.model.transform.position;
            item.model.transform.position += (new Vector3(playerPosition.x, playerPosition.y+0.4f, playerPosition.z) - itemPosition).normalized * flySpeed * deltaTime;
            if (Vector3.SqrMagnitude(itemPosition - playerPosition) < Mathf.Pow(hideDistance, 2))
            {
                flyingItems.RemoveAt(i);
                GameObject.Destroy(item.model);
            }
        }
    }

    private void doDrop(WaitDropItem wd)
    {
        DropItem item = new DropItem();
        GameObject model = App.res.createSingle("Local/prefab/mogu/Coin");
        model.transform.position = wd.position;
        model.addOnce<Binding>().data = item;
        item.model = model;
        //掉落路线
        iTween.MoveTo(model, iTween.Hash("position", new Vector3(wd.position.x+Random.Range(0,1.2f), wd.position.y, wd.position.z+Random.Range(0,1.2f)), "time", 0.3f, "easetype", iTween.EaseType.linear));
        items.Add(item);
        App.coroutine.StartCoroutine(setPickupEnable(item));
    }

    /**掉落效果过程中不可捡起*/
    private IEnumerator setPickupEnable(DropItem item)
    {
        yield return new WaitForSeconds(0.6f);
        item.pickupEnable = true;
    } 

    public void dropGold(Transform transf, int count) {
        if (count > 10) count = 10;//暂定最多掉10个金币模型
        for (var i = 0; i < count; i++)
        {
            WaitDropItem wd = new WaitDropItem();
            wd.delay = i * WaitDropItem.dropSpace;
            wd.position = transf.position;
            wditems.Add(wd);
        }
    }

    /**玩家捡起物品*/
    public void pickupDrop(DropItem item)
    {
        if (!item.pickupEnable) return;
        if (flyingItems.Contains(item)) return;
        items.Remove(item);
        GameObject.Destroy(item.model);
    }
}
