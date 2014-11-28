using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class RemoveAnimEvents : MonoBehaviour {

    [MenuItem("Edit/Remove AnimationEvents")]
    public static void removeAnimationEvents() {
        GameObject o = Selection.activeGameObject;
        if (o == null) return;
        Animation anim = o.animation;
        if (anim == null) return;
        foreach (AnimationState s in anim) {
            AnimationEvent[] events = AnimationUtility.GetAnimationEvents(s.clip);
            if (events == null) continue;
            List<AnimationEvent> newEvents = new List<AnimationEvent>();
            foreach (AnimationEvent e in events) {
                string methodName = e.functionName;
                e.objectReferenceParameter = null;
                e.stringParameter = null;
                e.functionName = "handle";
                newEvents.Add(e);
            }
            AnimationUtility.SetAnimationEvents(s.clip, newEvents.ToArray());
        }
    }
}
