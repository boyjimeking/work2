//----------------------------------------------
//            Behaviour Machine
// Copyright © 2014 Anderson Campos Cardoso
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using BehaviourMachine;

namespace BehaviourMachineEditor {

    /// <summary>
    /// Wrapper class for GlobalBlackboardEditor.
    /// <seealso cref="BehaviourMachine.GlobalBlackboard" />
    /// </summary>
    [CustomEditor(typeof(GlobalBlackboard))]
    public class GlobalBlackboardEditor : InternalGlobalBlackboardEditor {}
}