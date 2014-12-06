using UnityEngine;
using System.Collections;
using WellFired;
using engine;

public class PlayBornSript : MonoBehaviour
{
        private Camera mainCamera;
        private Camera scriptCamera;
        private Transform player;
        private Vector3 initPos = new Vector3(13, 3, -2f);

        private Vector3 originalPosition;
        public Quaternion origRotate;
        private GameObject mainCameraObject;

        void Awake()
        {
            mainCamera = CameraManager.Main;
            mainCameraObject = mainCamera.gameObject;
            App.suspend = true;           
            App.input.setJoyEnabled(false);
           // UIManager.Instance.uiCamera.gameObject.SetActive(false);
            scriptCamera = gameObject.GetComponent<Camera>();
            BattleUI.instance.setActive(false);
        }

        public void onBegin(Transform player, bool isFrist=false)
        {
            this.player = player;
            if (isFrist)
            {
                player.position = initPos;
                player.transform.eulerAngles = new Vector3(0, -180, 0);
            }
            this.gameObject.SetActive(true);
            player.gameObject.SetActive(false);
            mainCameraObject.SetActive(false);
            App.boot.GetComponent<AudioListener>().enabled = false;
        }


        public void playSript(int talkSingle)
        {
            //OnPalyEnd();
            ScriptManager.instance.showNextDilage(talkSingle);
        }

        public IEnumerator recover(float delay)
        {
            yield return new WaitForSeconds(delay);
            Debug.Log("recover");
            App.boot.GetComponent<AudioListener>().enabled = true;
            App.suspend = false;
            BattleUI.instance.setActive(true);
            
            Player.instance.HpBar.Owner = Player.instance;
            App.sceneManager.onEnterSequenceEnded();
            App.input.setJoyEnabled(true);
            CameraManager.Main.gameObject.SetActive(true);
            CameraManager.CameraFollow.makeFollow();



            foreach (FightCharacter c in BattleEngine.scene.getFriends())
            {
                c.HpBar.Parent.SetActive(true);
            }
            foreach (FightCharacter c in BattleEngine.scene.getEnemies())
            {
                c.HpBar.Parent.SetActive(true);
            }

            DestoryParent();
        }
        private void DestoryParent()
        {
            Transform top=null;
            Transform parent = this.transform.parent;
            while (parent != null)
            {
                top = parent;
                parent = parent.parent;
            }
            GameObject.Destroy(top.gameObject);
        }

        private bool ended=false;
        public void OnPalyEnd()
        {
            if (ended) return;
            Transform trans = transform.parent.parent.Find("scene1/scene1/man_enter");
            if (trans != null)
            {
                player.position = trans.position;
                trans.gameObject.SetActive(false);
            }
            CameraManager.CameraFollow.target = player;
            CameraManager.CameraFollow.makeFollow();
            origRotate = mainCameraObject.transform.rotation;
            originalPosition = mainCameraObject.transform.position;

            ended = true;
            player.gameObject.SetActive(true);
            mainCameraObject.SetActive(true);
            mainCameraObject.transform.position = gameObject.transform.position;
            mainCameraObject.transform.localRotation = gameObject.transform.localRotation;
            gameObject.GetComponent<Animator>().StopPlayback();
            scriptCamera.enabled = false;
            

            Hashtable ht1 = iTween.Hash("rotation", origRotate, "easeType", iTween.EaseType.linear, "time", 1.5f);
            iTween.RotateTo(mainCameraObject, ht1);
            Hashtable ht2 = iTween.Hash("position", originalPosition, "easeType", iTween.EaseType.linear, "time", 1.5f);
            iTween.MoveTo(mainCameraObject, ht2);


            //ScriptManager.instance.CloseScript();
            App.coroutine.StartCoroutine(recover(1.5f));
        }

        public void doPalyEnd()
        {
            ScriptManager.instance.CloseScript();
            OnPalyEnd();
        }
}
