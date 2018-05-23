using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elven_Path
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public bool isPause { get; set; }
        public bool isBattle { get; set; }

        public CharacterProfile[] heroes;
        public CharacterProfile[] enemies;
        public CharacterProfile[] animals;

        public Transform heroParent, enemyParent, animalParent;

        public bool[,] gridFieldXY;

        private void Awake()
        {
            instance = this;

            gridFieldXY = new bool[countX, countY];

            isPause = false;
        }
        [SerializeField] private int countX, countY;
        public float startX, startY;
        private Vector2 vec;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            for(int y = 0; y < countY; y++)
            {
                for(int x = 0; x < countX; x++)
                {
                    vec.x = startX + x;
                    vec.y = startY - y;
                    Gizmos.DrawCube(vec, Vector3.one * 0.5f);
                }
            }
        }

        public Vector2 CheckRandomGridPos()
        {
            Vector2 pos = new Vector2();
            do
            {
                pos.x = Random.Range(0, countX);
                pos.y = Random.Range(0, countY);
            } while (gridFieldXY[(int)pos.x, (int)pos.y]);

            pos.x = startX + pos.x;
            pos.y = startY - pos.y;

            return pos;
        }

        public Vector2 GetGridPos(int x,int y)
        {

            return new Vector2();
        }

    }
}