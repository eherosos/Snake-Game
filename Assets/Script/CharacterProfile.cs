﻿//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace Elven_Path
{
    public enum SpiritType
    {
        Mortal,Wild,Spirit
        //,Red_Spirit, Green_Spirit, Blue_Spirit
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
        public int heart;
        public int Hp
        {
            get { return heart; }
            set
            {
                if (isDie) return;
                heart = value;
                if (heart <= 0)
                {
                    //Debug.Log("Die");
                    isDie = true;
                    heart = 0;
                    //GameManager.gridFieldXY[myGridX, myGridY] = false;
                }
            }
        }
        public int sword;
        public int shield;
        public int heal;
        public SpiritType type;
        public int myGridX;// { get; set; }
        public int myGridY;// { get; set; }
    }
}