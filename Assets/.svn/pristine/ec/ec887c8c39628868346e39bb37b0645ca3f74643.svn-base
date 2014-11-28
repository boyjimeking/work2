using UnityEngine;
using System.Collections;
using engine;

//base class for various trigger in the scene.
public class SceneTrigger  {
    public GameObject model;
    public string name;
    public DungeonTemplate.DungeonRoom room;
    public DungeonTemplate.DungeonTrigger trigger;

    public BoxCollider[] colliders;
    public virtual void reset(GameObject model, DungeonTemplate.DungeonTrigger trigger) {
        this.model = model;
        this.name = trigger.name;
        this.trigger = trigger;
        colliders = model.GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider c in colliders) {
            c.isTrigger = true;
            c.gameObject.addOnce<Binding>().data = this;
        }

    }

    public virtual void onTrigger() {
        model.SetActive(false);
        if (trigger == null) return;
        if (trigger.animation != null) {
            Animation anim = model.GetComponent<Animation>();
            if (anim != null) {
                anim.Play();
            }
        }

        if (trigger.effect != null) {
            GameObject effect = Engine.res.createSingle(trigger.effect);
            GameObject.Destroy(effect, 2f);
        }
    }
}
