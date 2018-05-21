//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace Elven_Path
{
    [CreateAssetMenu(fileName = "Character Name", menuName = "Make a character")]
    public class CharacterProfile : ScriptableObject
    {
        public Sprite profileSprite;
        public int heart;
        public int sword;
        public int shield;
    }
}