using System.Collections;
using engine;
using UnityEngine;
using WellFired;

public class PlayerBorn : MonoBehaviour {
    private Vector3 velocity;

    private float initView = 37f;
    private float finalView = 60f;
    private float transitTime = 1f;
    private Vector3 initPos = new Vector3(21, 0, -15);
    private Vector3 finalPos = new Vector3(15, 0, -15.5f);
    private float moveTime = 2f;
    private float scaleTime = 0.6f;

    private Camera controlCamera;
    private Camera mainCamera;
    private Transform target;
    private bool canMove;  

    private USSequencer bornSEQ;
    private USTimelineContainer container;
    private USTimelineObjectPath path;
    private USSendMessageEvent sendMsg;
   
    void Awake() {
        UIManager.Instance.Enable = false;
        SceneTemp temp = App.template.getTemp<SceneTemp>(1);
        initView = temp.initView;
        //finalView = temp.finalView;
        finalView = Camera.main.fieldOfView;
        transitTime = temp.transitTime;
        initPos = temp.initPos;
        finalPos = temp.finalPos;
        moveTime = temp.moveTime;
        scaleTime = temp.slowScale;
        bornSEQ = transform.parent.GetComponent<USSequencer>();
        container = transform.parent.Find("Container").GetComponent<USTimelineContainer>();
        path = transform.parent.Find("Container/Path").GetComponent<USTimelineObjectPath>();
        sendMsg = transform.parent.Find("Container/timeLine/SendMsg").GetComponent<USSendMessageEvent>();
        controlCamera = GetComponent<Camera>();
        mainCamera = Camera.main;
        mainCamera.gameObject.SetActive(false);
        controlCamera.fieldOfView = initView;
        controlCamera.transform.position = path.FirstNode.Position;
        controlCamera.transform.LookAt(initPos);
        canMove = false;
        Vector3 face = finalPos - initPos;
        velocity = face.normalized * face.magnitude / moveTime;
    }

    void Update() {
        if(canMove){
            Player.instance.beginMove(velocity);
        }
    }

    void playNext() {
        //target.transform.localEulerAngles = playerRotate;
        canMove = false;
        Player.instance.setMoveState(false);
        CameraManager.CameraFollow.target = target;
        CameraManager.CameraFollow.makeFollow();
        Vector3 lookTarget = target.position + Vector3.up * CameraManager.CameraFollow.lookHeight;
        Hashtable ht1 = iTween.Hash("position",mainCamera.transform.position,"easeType", iTween.EaseType.linear,
            "looktarget", lookTarget, "oncomplete", "onEnd", "time", transitTime);
        iTween.MoveTo(controlCamera.gameObject, ht1);
        Hashtable ht2 = iTween.Hash("from", initView, "to", finalView, "time", transitTime, "onupdate", "changeView");
        iTween.ValueTo(controlCamera.gameObject, ht2);
    }

    public void onBegin(Transform player) {
        target = player;
        target.position = initPos;
        target.transform.LookAt(finalPos);
        container.AffectedObject = controlCamera.transform;
        path.LookAtTarget = target;
        sendMsg.receiver = gameObject;
        bornSEQ.Play();
        Time.timeScale = scaleTime;
        App.suspend = true;
        canMove = true;
        Player.instance.setMoveState(true);
        App.input.setJoyEnabled(false);
    }

    public void onEnd() {
        Time.timeScale = 1f;
        App.suspend = false;
        UIManager.Instance.Enable = true;
        mainCamera.gameObject.SetActive(true);
        Player.instance.HpBar.Owner = Player.instance;
        Destroy(transform.parent.gameObject);

        App.sceneManager.onEnterSequenceEnded();
        App.input.setJoyEnabled(true);
    }

    void changeView(float value) {
        controlCamera.fieldOfView = value;
    }
}