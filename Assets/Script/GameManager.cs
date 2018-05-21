using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elven_Path
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public bool isPause { get; set; }
        

        private void Awake()
        {
            instance = this;

            isPause = false;
        }

        [SerializeField] private float minX, maxX, minY, maxY;
        private Vector2 vec;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            for (int y = 0; y < maxY - minY + 1; y++)
            {
                for (int x = 0; x < maxX - minX + 1; x++)
                {
                    vec.x = minX + x;
                    vec.y = minY + y;
                    Gizmos.DrawCube(vec, Vector3.one * 0.5f);
                }
            }
        }
    }
}