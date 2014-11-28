using UnityEngine;

public class DoorAnim : MonoBehaviour {

    private Animator animator;
    private bool beginLoop;

    void Awake() {
        animator = GetComponent<Animator>();
        beginLoop = true;
    }

    void Start() {
        animator.StartRecording(0);
    }

    void loopEvent() {
        if (beginLoop) {
            animator.StopRecording();
            animator.StartPlayback();
            beginLoop = false;
        }
    }

    public void stopPlay() {
        animator.StopPlayback();
    }

    public void beginPlay() {
        animator.StartPlayback();
    }
}
