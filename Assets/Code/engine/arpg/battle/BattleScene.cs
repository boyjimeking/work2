using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace engine {
    public class BattleScene:Scene {

        protected List<Bullet> bullets = new List<Bullet>();
        protected List<Effect> effects = new List<Effect>();
        protected List<FightCharacter> friends = new List<FightCharacter>();
        protected List<FightCharacter> enemies = new List<FightCharacter>();
        protected List<FightCharacter> deads = new List<FightCharacter>();

        public DungeonData dungeonData;
        protected bool endableAttackLOS=false;
        public bool checkMonsterGroup = false;
        public bool friendAllDie = false;
        public bool pausing = false;

        public List<FightCharacter> getEnemies() { return enemies; }
        public List<FightCharacter> getFriends() { return friends; }

       
        public virtual void reset(DungeonData data) {
            this.dungeonData = data;

            bullets.Clear();
            effects.Clear();
            friends.Clear();
            enemies.Clear();
            deads.Clear();

            checkMonsterGroup = false;


        }
        public virtual void clear()
        {
            dungeonData.clear();

            foreach (FightCharacter c in friends)
            {
                c.clear();
            }
            foreach (FightCharacter c in enemies)
            {
                c.clear();
            }
            foreach (FightCharacter c in deads)
            {
                c.clear();
            }

            bullets.Clear();
            effects.Clear();
            friends.Clear();
            enemies.Clear();
            deads.Clear();

        }
        public void addFriend(FightCharacter c) {
            friends.Add(c);
        }
        public void removeFriend(FightCharacter c) {
            friends.Remove(c);
        }
        public void addEnemy(FightCharacter c) {
            enemies.Add(c);
        }
        public void removeEnemy(FightCharacter c) {
            enemies.Remove(c);
        }
        public FightCharacter getEmemy(int templateId)
        {
            for (var i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].charTemplate.id == templateId)
                {
                    return enemies[i];
                }
            }
            return null;
        }

        public virtual void update()
        {
            
            for (int i = friends.Count - 1; i > -1; i--) {
                friends[i].update();
                if (friends[i].isDead()) {
                    deads.Add(friends[i]);
                    friends.RemoveAt(i);
                }
            }

            for (int i = enemies.Count - 1; i > -1; i--) {
                enemies[i].update();
                if (enemies[i].isDead())
                {
                    deads.Add(enemies[i]);
                    enemies.RemoveAt(i);
                }
            }

            for (int i = bullets.Count - 1; i > -1; i--) {
                Bullet bullet = bullets[i];
                bullet.update();
                if (bullet.completed) bullets.RemoveAt(i);
            }
            for (int i = effects.Count - 1; i > -1; i--) {
                Effect effect = effects[i];
                effect.update();
                if (effect.completed) effects.RemoveAt(i);
            }

            for (int i = deads.Count - 1; i > -1; i--) {
                if (deads[i].destroyed) deads.RemoveAt(i);
                else deads[i].updateDead();
            }

                //if the current room spawned any enemies and are cleared,check next group if any.
            if (checkMonsterGroup )
            {
                bool allEnemyDead = true;
                for (int i = enemies.Count - 1; i > -1; i--)
                {
                    if (!enemies[i].isDead())
                    {
                        allEnemyDead = false;
                        break;
                    }
                }
                if (allEnemyDead)
                {
                    nextGroup();                  
                    //checkMonsterGroup = false;
                }              
            }
            
            if (friends.Count == 0)
            {
                if (!friendAllDie)
                {
                    friendAllDie = true;
                    DungeonFinish.instance.show(false, null);
                }
            }
            else
            {
                friendAllDie = false;
            }
            ArrawManager.instance.update();
        }
        public void addBullet(Bullet bullet) {
            bullets.Add(bullet);
        }
        public void addEffect(Effect effect) {
            effects.Add(effect);
        }
        /// <summary>
        /// when a monster is already within attack range,it's in obstacle mode,
        /// so if endableAttackLOS is true,the method may return null.
        /// </summary>
        /// <param name="enemy"></param>
        /// <returns></returns>
        public FightCharacter findNearestTarget(FightCharacter enemy, FightCharacter oldtarget = null, bool checkNum = true, float distance = float.MaxValue)
        {
            List<FightCharacter> targetList;
            if (enemy is PetCharacter) {
                targetList = enemies;
            }
            else if (enemy is MonsterCharacter) {
                targetList = friends;
            } else {
                targetList = enemies;
            }
            FightCharacter target = null;
           // float attackRange = enemy.getAttackRange();
            Vector3 position = enemy.transform.position;
            for (int i = 0, max = targetList.Count; i < max; i++) {
                FightCharacter fc = targetList[i];
                if (fc.isDead() || !fc.model.activeSelf || (fc.cc != null && fc.cc.enabled == false) )
                    continue;
                if(fc == oldtarget && (!checkNum || (checkNum && targetList.Count > 1)))
                    continue;
                float temp=Vector3.Distance(fc.transform.position, position);
                if (temp < distance) {
                    if (endableAttackLOS) {
                        NavMeshHit hit;
                        if (NavMesh.Raycast(position, fc.transform.position, out hit, 1)) {
                            continue;
                        }
                    }
                   
                    target = fc;
                    distance = temp;
                }
            }
            return target;
        }

        public virtual Vector3 getTriggerPos() {
            return Vector3.down;
        }

        // angle -1 represent circle range, else is vector range
        public bool judgeInRange(Transform target, Transform self, float radius, float angle = -1) {
            if (radius <= 0)
                return false;
            Vector3 tv = target.position;
            Vector3 sv = self.position;
            if(Vector3.Distance(tv, sv) > radius)
                return false;
            if (angle > 0) {
                if (Vector3.Dot(tv, sv) <= 0)
                    return false;
                Vector3 dir = tv - sv;
                if (Vector3.Angle(dir, self.forward) < angle)
                    return true;
                return false;
            }
            return true;
        }

        //pause characters ai only
        public void pauseAI(bool v) {
             for (int i = 0; i <enemies.Count; i++) {
                 enemies[i].suspendAI = v;
             }
             for (int i = 0; i < friends.Count; i++) {
                 friends[i].suspendAI = v;
             }
        }

        //pause everything:animator,ai,agent
        public void pause(bool v) {
            pausing = v;
            for (int i = 0; i < enemies.Count; i++) {
                enemies[i].pause(v);
            }
            for (int i = 0; i < friends.Count; i++) {
                friends[i].pause(v);
            }   
        }
      
        public void shieldCharacter(bool shield = true)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].model.SetActive(!shield);
            }
            for (int i = 0; i < friends.Count; i++)
            {
                friends[i].model.SetActive(!shield);
            }  
        }

        //is a and b in the same team?
        public bool isDifferentTeam(IAttackable a, IAttackable b) {
            return a.getTeam().teamNo != b.getTeam().teamNo;
        }

        public virtual void prepareRoom() {

        }
        
        protected virtual bool nextGroup() {
            if (dungeonData.hasNextGroup())
            {
                dungeonData.nextGroup();
                enemies.Clear();
                prepareRoom();
                return true;
            }
            return false;
        }
        public void nextRoom() {
            dungeonData.nextRoom();
            prepareRoom();
        }
        public bool hasNextRoom() {
            return dungeonData.hasNextRoom();
        }

        public bool hasNextTrigger(bool isPlayer=false) {
            return dungeonData.hasNextTrigger(isPlayer);
        }
        public bool isRoomCleared() {
            for (int i = 0, max = enemies.Count; i < max; i++) {
                if (!enemies[i].isDead()) return false;
            }
            return !dungeonData.hasNextGroup();
        }
    }

}
