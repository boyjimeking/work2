using UnityEngine;
using System.Collections;
namespace engine {
    public class DissolveEffect : Effect {
        private FightCharacter owner;
        //private SkinnedMeshRenderer renderer;
        private SkinnedMeshRenderer[] renderers;
        private bool _beginDissolve;             //开始肢解
        private float _playTime;                //播放时间
        private float playTimer;

        public void reset(FightCharacter owner, float duration) {
            this.owner = owner;
            renderers = owner.model.GetComponentsInChildren<SkinnedMeshRenderer>();
            ParticleSystem pa = owner.model.GetComponentInChildren<ParticleSystem>();
            if (pa != null) pa.Stop();
            //renderer = owner.getSkinnedMeshRenderer();
            completed = false;
            playTimer = duration;

            foreach (SkinnedMeshRenderer sr in renderers)
            {
                for (int i = 0; i < sr.materials.Length; i++)
                {
					sr.materials[i].shader = App.getShader("Custom/Dissolve");//Shader.Find("Custom/Dissolve");
                    Texture2D tex = Resources.Load("Local/picture/dissolve") as Texture2D;
                    sr.materials[i].SetTexture("_DissolveSrc", tex);
                    sr.materials[i].SetColor("_SpecColor", new Color(1f, 1f, 1f));
                }
            }
            _playTime = 0;
        }
        public override void update() {
            if (renderers == null || renderers.Length==0)
                return;
            if (completed) return;
            float ratio = _playTime / playTimer;

            foreach (SkinnedMeshRenderer sr in renderers)
            {
                if (sr != null)
                {

                    for (int i = 0; i < sr.materials.Length; i++)
                    {
                        sr.materials[i].SetFloat("_Amount", ratio);
                    }
                }
            }
            if (ratio >= 1) {
                completed = true;
            }
            _playTime += Time.deltaTime;
        }
       
    }
}

