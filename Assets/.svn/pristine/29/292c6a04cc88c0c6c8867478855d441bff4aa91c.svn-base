using UnityEngine;
using System.Collections;
using engine;
public class EnterRoomTrigger : SceneTrigger {
    public Door door;//the door this trigger belongs to.
    public bool triggered;//one shot bool state

    public override void reset(GameObject model, DungeonTemplate.DungeonTrigger trigger) {
        this.model = model;
        BoxCollider c = model.addOnce<BoxCollider>();
        c.isTrigger = true;
        c.gameObject.addOnce<Binding>().data = this;
    }
    //public EnterRoomTrigger clone() {
    //    EnterRoomTrigger copy = new EnterRoomTrigger();
    //    copy.door = door;
    //   copy.model= GameObject.Instantiate(model, model.transform.position, model.transform.rotation) as GameObject;
    //   BoxCollider c =copy. model.addOnce<BoxCollider>();
    //   c.isTrigger = true;
    //   c.gameObject.addOnce<Binding>().data = copy;
      
    //   return copy;
    //}
}
