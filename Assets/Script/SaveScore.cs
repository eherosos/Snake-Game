using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elven_Path
{
    [CreateAssetMenu(fileName = "Saved", menuName = "Create a save")]
    public class SaveScore : ScriptableObject
    {
        public int high_wave;
        public int high_score;
    }
}