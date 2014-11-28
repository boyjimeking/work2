using UnityEngine;
using System.Collections;
namespace engine {
    public class CharacterBinding : Binding {

        protected float pushPower = 2.0f;
        private static Vector3 tmp = new Vector3(1, 0, 1);

        void OnTriggerEnter(Collider other) {
            Debug.Log("charactercollider othter:" +data+"*****"+ other);
        }
        //void OnControllerColliderHit(ControllerColliderHit hit)// Character can push an object.
        //{

        //    if (hit.collider.name == "wall" || hit.gameObject.name == "wall") {
        //        Debug.Log("hit wall");
        //    }
        //    Rigidbody body = hit.collider.attachedRigidbody;
        //    if (body == null || body.isKinematic) {
        //        return;
        //    }
        //    if (hit.moveDirection.y < -0.3) {
        //        return;
        //    }

        //    Vector3 pushDir = Vector3.Scale(hit.moveDirection, tmp);
        //    body.velocity = pushDir * pushPower;
        //}
    }
}

