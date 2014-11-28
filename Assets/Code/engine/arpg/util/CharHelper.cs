using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
namespace engine {
    public class CharHelper {

        /// <summary>
        /// ////////////character body part cache,mostly left_hand,right_hand, for fast switching weapon//////////////////////
        /// </summary>
        private static Dictionary<string, Dictionary<string, string>> bodyPath = new Dictionary<string, Dictionary<string, string>>();
        public static Transform getBodyPart(GameObject go, string career, string partName) {
            Transform hand;
            string handPath = getBodyPartPath(career, partName);
            if (handPath != null) {
                hand = go.transform.FindChild(handPath).transform;
            } else {
                hand = findBodyPart(go.transform, partName);
                handPath = calcPath(hand);
                cacheBodyPartPath(career, partName, handPath);
            }
            return hand;
        }
        private static string calcPath(Transform hand) {
            List<string> path = new List<string>();
            path.Add(hand.name);
            while (hand.parent != null) {
                path.Add(hand.parent.name);
                hand = hand.parent;
            }
            StringBuilder builder = new StringBuilder();
            for (int j = path.Count - 2; j > -1; j--) {
                builder.Append(path[j]);
                if (j != 0) builder.Append("/");
            }
            return builder.ToString();
        }
        private static string getBodyPartPath(string career, string partName) {
            if (bodyPath.ContainsKey(career)) {
                Dictionary<string, string> path = bodyPath[career];
                if (path.ContainsKey(partName)) return path[partName];
            }
            return null;
        }
        private static void cacheBodyPartPath(string career, string partName, string partPath) {
            Dictionary<string, string> path = null;
            if (bodyPath.ContainsKey(career)) path = bodyPath[career];
            else {
                path = new Dictionary<string, string>();
                bodyPath[career] = path;
            }
            path[partName] = partPath;

        }
        private static Transform findBodyPart(Transform body, string part) {
            Transform result;
            if (body.name == part) return body;
            if (body.childCount > 0) {
                foreach (Transform child in body) {
                    result = findBodyPart(child, part);
                    if (result != null) return result;
                }
            }

            return null;
        }

    }

}
