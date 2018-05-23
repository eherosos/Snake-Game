//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace Elven_Path
{
    public enum SpiritType
    {
        Red_Spirit, Green_Spirit, Blue_Spirit
    }

    [CreateAssetMenu(fileName = "Character Name", menuName = "Make a character")]
    public class CharacterProfile : ScriptableObject
    {
        public Sprite profileSprite;
        public int heart;
        public int sword;
        public int shield;
        public int heal;
        public SpiritType type;
    }

    public class Character// : MonoBehaviour
    {
        public bool isDie = false;

        public Sprite profileSprite;
        protected int heart;
        public int Hp
        {
            get { return heart; }
            set
            {
                heart = value;
                if (heart <= 0)
                {
                    isDie = true;
                    GameManager.instance.gridFieldXY[myIndex.x, myIndex.y] = false;
                }
            }
        }
        public int sword;
        public int shield;
        public int heal;
        public SpiritType type;
        public Vector2Int myIndex;
    }
}