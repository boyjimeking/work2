using UnityEngine;
using System.Collections;
namespace engine {
    public class HitEffect:Effect {

        private FightCharacter owner;
        private Color mainColor = Color.black;
        private float highlightTime;

        private float highlightTimer;
        private SkinnedMeshRenderer renderer;
        private float r, g, b,speed;

        private Material[] m;

        public void reset(FightCharacter owner,float duration,Color color) {
            this.owner = owner;
            renderer = owner.getSkinnedMeshRenderer();
            //bug sometime skinmeshrender is null //TODO check this
            if (renderer == null) {
                completed = true;
                return;
            }
            completed = false;

            if (m == null) {
                m = new Material[owner.originalMainTExture.Length];
                for(int i=0;i<m.Length;i++){
					m[i]=new Material(App.getShader("BeHit2")); //Shader.Find("BeHit2"));
                    m[i].mainTexture = owner.originalMainTExture[i];
                }
            }
            for(int i=0;i<m.Length;i++){
                m[i].SetColor("_Color", color);
            }
            renderer.materials=m;

            r = color.a;
            g = color.g;
            b = color.b;

            speed = (255-100)/255f / (duration*(1-0.2f));//fade out in half duration

            highlightTime = duration;
            highlightTimer = 0.0f;
        }
        
        public override void update() {
            if (completed) return;
            if (highlightTimer > highlightTime) {
                completed = true;
                highlightTime = 0.0f;
            } else if (highlightTimer >= highlightTime * 0.2f) {
                float delta = speed * Time.deltaTime;
                r -= delta;
                b -= delta;
                g -= delta;
                Color newColor=new Color(r, g, b, 1);
                for (int i = 0; i < m.Length; i++) {
                    m[i].SetColor("_Color", newColor);
                }
            }

            highlightTimer += Time.deltaTime;
        }
        public void setComplete() {
            completed = true;
        }
    }
}
