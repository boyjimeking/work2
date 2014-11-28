using UnityEngine;
using System.Collections;
namespace engine{
    public enum WeaponType {
        Melee,Ranged
    }
public class Weapon  {
    public FightCharacter owner;
    public GameObject model;
    public GameObject effect;//weapon effect
    public WeaponType type;

    public int damageId;


    public BoxCollider[] colliders;
    public Weapon() { }
    public Weapon(FightCharacter owner, GameObject weapon,WeaponType type) {
        this.owner = owner;
        this.model = weapon;
        this.type = type;

        //weapon may have multi colliders,suck as sickle.
        //TODO find a better way for Binding.

        colliders = model.GetComponentsInChildren<BoxCollider>();
        if (colliders != null) {
            foreach (BoxCollider c in colliders) {
                c.enabled = false;
                c.isTrigger = true;
                c.gameObject.addOnce<Binding>().data = this;
            }
        }
       
    }
    public void enableCollider(bool v) {
        if (colliders != null) {
            foreach (BoxCollider c in colliders) {
                c.enabled = v;
            }
        }
    }
}
}

