using UnityEngine;
using System.Collections;
using engine;

public class PlayerGhost {
    private class Dummy : MonoBehaviour {
        void NewEvent(AnimationEvent e) {
            //do nothing
        }
    }
    public static PlayerGhost instance = new PlayerGhost();

    private GameObject model;
    private GameObject weapon;
    private ParticleSystem ps;
    //private Animator animator;

    private float shadowTime;
    private bool firstTime = true;
    private CameraFollowing follow;

    public void reset()
    {
        model = Engine.res.createSingle(Naming.EffectPath + "move");
        model.name = "__PlayerGhost";
        model.SetActive(false);
        ps = model.getChildComponent<ParticleSystem>("sudu");
        follow = CameraManager.Main.GetComponent<CameraFollowing>();
        
    }

    public void reset(GameObject model, GameObject weapon) {
        this.model = model;
        this.weapon = weapon;
        model.name = "__PlayerGhost";
        firstTime = true;
        

        Shader newShader = Shader.Find("Particles/Additive");
        Color tintColor = new Color(9 / 255f, 141 / 255f, 4 / 255f, 8 / 255f);
        Material[] m = model.GetComponentInChildren<SkinnedMeshRenderer>().materials;
        for (int i = 0; i < m.Length; i++) {
            m[i].mainTexture = null;
            m[i].shader = newShader;
            m[i].SetColor("_TintColor", tintColor);
        }
        MeshRenderer[] renders = weapon.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r2 in renders) {
            r2.material.mainTexture = null;
            r2.material.shader = newShader;
            r2.material.SetColor("_TintColor", tintColor);
        }

        //this.animator = model.addOnce<Animator>();
        RuntimeAnimatorController runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Engine.res.loadObject("Local/controller/"+Player.instance.charTemplate.controller));
        //animator.runtimeAnimatorController = runtimeAnimatorController;
        
        model.addOnce<Dummy>();

        model.SetActive(false);
    }

    
    public bool update() {
        if (firstTime) {
            firstTime = false;
            //animator.speed = 0;
        }
      
        shadowTime -= Time.deltaTime;
        if (shadowTime <= 0) {
            model.SetActive(false);
            return false;
        }
        return true;
    }
    public void resetRushShadow() {
        model.transform.position = Player.instance.transform.position;
        model.transform.forward = Player.instance.transform.forward;
        shadowTime = 1f;
        model.SetActive(true);
        ps.time = 0;
        follow.rushDelay = BattleConfig.rushDelay;
        //if (firstTime) {
        //    animator.Play(Hash.atk1StartState, 0, 0);
        //}
    }

}

