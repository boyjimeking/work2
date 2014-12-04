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
            // moving objects need rigidbody
            Rigidbody body = model.addOnce<Rigidbody>();
            body.useGravity = false;
            body.isKinematic = true;
            body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
        getMeshRenderer();
       
    }
    public void enableCollider(bool v) {
        if (colliders != null) {
            foreach (BoxCollider c in colliders) {
                c.enabled = v;
            }
        }
    }
    public MeshRenderer renderer;
    public Material[] orignalMaterial;
    public Texture[] originalMainTExture;
    public MeshRenderer getMeshRenderer()
    {
        if (renderer != null) return renderer;
        renderer = model.GetComponentInChildren<MeshRenderer>();
        if (renderer == null) return null;
        int length = renderer.materials.Length;
        orignalMaterial = new Material[length];
        originalMainTExture = new Texture[length];
        Shader behindWallShader = Shader.Find("BehindWall2");
        Color c = new Color(1, 1, 0, 1);
        for (int i = 0; i < length; i++)
        {
            orignalMaterial[i] = renderer.materials[i];
            orignalMaterial[i].shader = behindWallShader;
            orignalMaterial[i].SetColor("_AtmoColor", c);
            originalMainTExture[i] = orignalMaterial[i].mainTexture;
        }
        return renderer;
    }
}
}

