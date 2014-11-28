using UnityEngine;
using System.Collections;
namespace engine {
    public class Team {
        //the team the ai is on
        public int teamNo;
        //if checked off ai will use dynamic teams feature
        public bool dynamicTeams;
        //will attack anyone and be attacked by anyone
        public bool hostile;

        public Team clone() {
            Team copy = new Team();
            copy.teamNo = teamNo;
            copy.dynamicTeams = dynamicTeams;
            copy.hostile = hostile;
            return copy;
        }
    }

}
