using System.Collections.Generic;
using UnityEngine;
using System.Collections;
namespace engine {
    public enum CharacterState {
        idle, attack, hit, down, stand, chase,dead
    }

   

    public class Character {
        public string instanceName;//used for debug only
        private static int instanceNameId;
        private CharacterType type;
        protected Binding binding;

        public CharacterState state=CharacterState.idle;

        public GameObject model;
        public Transform transform;
        public Animator animator;
        public AvatarController controller;
        public GameObject charName;
       
        public CharData data;
        public CharTemplate charTemplate;
        public Dictionary<int, SkillState> skillStates; 

        public AI ai;

        public NavMeshAgent agent;
        public float bodyRadius;

        public CapsuleCollider cc;

        public bool destroyed;
        public virtual void onDestroy() {
            destroyed = true;
        }

        public virtual void reset(GameObject model, CharData data,AI ai) {
           
            this.model = model;
            this.transform = model.transform;
            this.data = data;
            this.charTemplate = data.charTemplate;
            this.type = charTemplate.type;
            this.ai = ai;
            this.instanceName = charTemplate.name + instanceNameId++;
            this.state = CharacterState.idle;

            binding = model.addOnce<Binding>();
            binding.data = this;

            resetAgent();
            resetRigidBody();
            resetAnimator();

            //make sure capsule collider center is zero
            cc = model.GetComponent<CapsuleCollider>();
            if (cc != null) cc.center = Vector3.zero;

            skillStates = new Dictionary<int, SkillState>();
        }
        protected void resetAnimator() {
            this.animator = model.addOnce<Animator>();
            RuntimeAnimatorController runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Engine.res.loadObject(getAnimatorControllerName()));
            animator.runtimeAnimatorController = runtimeAnimatorController;

            if (controller == null) {
                controller = createAvatarController();
            }
            controller.reset(this);
        }
        protected virtual void resetAgent() {
            agent = model.addOnce<NavMeshAgent>();
            agent.speed = data.moveSpeed;
            agent.acceleration = 100000;
            agent.angularSpeed = charTemplate.angularSpeed;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
            CapsuleCollider cc = model.GetComponent<CapsuleCollider>();
            agent.stoppingDistance = cc.radius;
            agent.radius = agent.stoppingDistance;
            bodyRadius = agent.stoppingDistance;
            agent.enabled = false;
            agent.updateRotation = false;
        }

        protected void resetRigidBody() {
            Rigidbody body = model.addOnce<Rigidbody>();
            body.useGravity = false;
            body.isKinematic = true;
        }

        protected virtual AvatarController createAvatarController() {
            return new MonsterController();
        }

        public void setAgentEnable(bool v) {
            agent.enabled = v;
        }
        public virtual void setObstacleMode(bool v) {
            agent.enabled = !v;
            agent.updatePosition = !v;
            agent.updateRotation = !v;
        }

        protected virtual string getAnimatorControllerName() {
            if(string.IsNullOrEmpty(charTemplate.controller)) return "Local/controller/MonsterController";
            return "Local/controller/"+charTemplate.controller;
        }

        public void learnSkill(int skillId, int skillLevel) {
            data.learnSkill(skillId, skillLevel);
        }
        public LearnedSkill getLearnedSkill(int skillId) {
            return data.getLearnedSkill(skillId);
        }


        public void agentMoveTo(Vector3 destination) {
            setObstacleMode(false);
            agent.updatePosition = true;
            agent.updateRotation = true;
            
            agent.SetDestination(destination);
            
            controller.setBool(Hash.runBool, true);
        }
        public void agentMoveTo(FightCharacter c) {
            agentMoveTo(c.transform.position);
        }
        public virtual void agentStop(bool playIdle = true) {
            if (agent.enabled) {
                //agent.Stop(true);
                agent.updatePosition = false;
                agent.updateRotation = false;
                agent.enabled = false;
            }
            
            if (playIdle) {
                controller.setBool(Hash.runBool, false);
            }
        }
        public bool inAttackRange(Character target, float atkRange) {
            if (atkRange == 0) atkRange = bodyRadius;
            float distance = Vector3.Distance(transform.position , target.transform.position);
            return distance < atkRange + target.bodyRadius;
        }
        public bool inMoveRange(Character target) {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            return distance < data.moveRanage;
        }
        
        private static Vector3 lookatEulerAngles = new Vector3();
        public void lookat(FightCharacter c) {
            transform.LookAt(c.transform);
            lookatEulerAngles.Set(0, transform.eulerAngles.y, 0);
            transform.eulerAngles = lookatEulerAngles;
        }
        public void lookat(Vector3 v) {
            transform.LookAt(v);
            lookatEulerAngles.Set(0, transform.eulerAngles.y, 0);
            transform.eulerAngles = lookatEulerAngles;
        }


        public Transform getTagPoint(string name) {
            Transform[] allChildren = model.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren) {
                if (child.gameObject.name == name) {
                    return child;
                }
            }
            return null;
        }

        public bool isPlayer() {
            return type == CharacterType.player;
        }

        public bool isHero() {
            return type == CharacterType.hero;
        }

        public bool isMonster() {
            return type == CharacterType.monster;
        }

        public bool isBoss() {
            return type == CharacterType.boss;
        }


        public virtual void update() {
            //do nothing by default

            //updateAttack();

            //if (data.moveSpeedMultiplier != 1) {
            //    data.moveSpeedMultiplier = Mathf.Lerp(data.moveSpeedMultiplier, 1, Time.deltaTime * 100);
            //}
           
        }

        public Vector3 Position {
            get { return model.transform.position; }
        }

        public virtual void onBorn() {
            
        }
    }

}
