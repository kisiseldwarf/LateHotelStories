using UnityEngine;

namespace Scriptable_Objects.clues
{
    [CreateAssetMenu(fileName = "New Clue", menuName = "Clue", order = 0)]
    public class Clue : ScriptableObject
    {
        public string name;
        public string desc;
        public Sprite logo;
        public string hash;
        public string[] linkedCluesHash;
        public string[] textOnFound;
        public string[] textOnLinked;
        public Clue[] cluesOnFound;

        private void OnValidate()
        {
            hash = "";
            hash = Hash128.Compute(ToString()).ToString();
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}