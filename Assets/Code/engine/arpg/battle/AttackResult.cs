using UnityEngine;
using System.Collections;

namespace engine
{
    public enum AttackState
    {
        none, critical, eva
    }
    public class AttackResult
    {
        public AttackResult(int damage) {
            this.damage = damage;
        }
        public int damage;
        public int attkerState;
        public int targetState;
        public AttackState state;
    }
}
