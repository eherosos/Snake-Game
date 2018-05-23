using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elven_Path
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private Vector2 vec;

        public bool isPause { get; set; }
        public bool isBattle { get; set; }
        public bool isOver { get; set; }
        private bool isTimeToGen = false;

        public CharacterProfile[] heroes;
        public CharacterProfile[] enemies;
        public CharacterProfile[] animals;

        [SerializeField] private GameObject heroPrefs, enemyPrefs, animalPrefs;
        public Transform heroParent, enemyParent, animalParent;

        public bool[,] gridFieldXY;
        public float startX, startY;
        public int countX, countY;

        public Dictionary<GameObject, Character> characters = new Dictionary<GameObject, Character>();

        [TextArea(0, 20)]
        public string debugText;

        private void Awake()
        {
            instance = this;

            gridFieldXY = new bool[countX, countY];

            print(gridFieldXY.Length);

            isPause = false;
            
        }
        private void Start()
        {
            StartCoroutine(Gen());
        }

        private void Update()
        {
            debugText = "";
            for (int y = countY - 1; y > -1; y--)
            //for (int y = 0; y < countY; y++)
            {
                for(int x = 0; x < countX; x++)
                {
                    if (gridFieldXY[x,y]) debugText += " * ";
                    else debugText += " - ";
                }
                debugText += '\n';
            }

            if (isBattle || isOver || isPause) return;
            if (isTimeToGen)
            {
                isTimeToGen = false;
                Gen_Hero();
                Gen_Enemy();
                StartCoroutine(Gen());
            }
        }

        private IEnumerator Gen()
        {
            yield return new WaitForSeconds(5f);
            isTimeToGen = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            for(int y = 0; y < countY; y++)
            {
                for(int x = 0; x < countX; x++)
                {
                    vec.x = startX + x;
                    vec.y = startY - y;
                    Gizmos.DrawCube(vec, Vector3.one * 0.25f);
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

        private int wave = 1;

        private void Gen_Hero()
        {
            int tmp_x;
            int tmp_y;
            do
            {
                tmp_x = Random.Range(0, countX);
                tmp_y = Random.Range(0, countY);
            } while (gridFieldXY[tmp_x, tmp_y]);
            gridFieldXY[tmp_x, tmp_y] = true;

            vec.x = startX + tmp_x;
            vec.y = startY - tmp_y;
            
            GameObject em = Instantiate(heroPrefs, vec, transform.rotation, heroParent);
            int tmp_r = Random.Range(0, ((wave > heroes.Length) ? wave : heroes.Length));

            em.GetComponent<SpriteRenderer>().sprite = heroes[tmp_r].profileSprite;

            Character tmp = new Character();
            tmp.profileSprite = heroes[tmp_r].profileSprite;
            tmp.heart = heroes[tmp_r].heart;
            tmp.sword = heroes[tmp_r].sword;
            tmp.shield = heroes[tmp_r].shield;
            tmp.myGridX = tmp_x;
            tmp.myGridY = tmp_y;
            characters.Add(em, tmp);
        }
        private void Gen_Enemy()
        {
            int tmp_x;
            int tmp_y;
            do
            {
                tmp_x = Random.Range(0, countX);
                tmp_y = Random.Range(0, countY);
            } while (gridFieldXY[tmp_x, tmp_y]);
            gridFieldXY[tmp_x, tmp_y] = true;

            vec.x = startX + tmp_x;
            vec.y = startY - tmp_y;

            GameObject em = Instantiate(enemyPrefs, vec, transform.rotation, enemyParent);
            int tmp_r = Random.Range(0, ((wave > enemies.Length) ? wave : enemies.Length));

            em.GetComponent<SpriteRenderer>().sprite = enemies[tmp_r].profileSprite;

            Character tmp = new Character();
            tmp.profileSprite = enemies[tmp_r].profileSprite;
            tmp.heart = enemies[tmp_r].heart;
            tmp.sword = enemies[tmp_r].sword;
            tmp.shield = enemies[tmp_r].shield;
            tmp.myGridX = tmp_x;
            tmp.myGridY = tmp_y;
            characters.Add(em, tmp);
        }
        private void Gen_Animal()
        {
            int tmp_x;
            int tmp_y;
            do
            {
                tmp_x = Random.Range(0, countX);
                tmp_y = Random.Range(0, countY);
            } while (gridFieldXY[tmp_x, tmp_y]);
            gridFieldXY[tmp_x, tmp_y] = true;

            vec.x = startX + tmp_x;
            vec.y = startY - tmp_y;

            GameObject em = Instantiate(animalPrefs, vec, transform.rotation, animalParent);
            int tmp_r = Random.Range(0, ((wave > animals.Length) ? wave : animals.Length));

            em.GetComponent<SpriteRenderer>().sprite = animals[tmp_r].profileSprite;

            Character tmp = new Character();
            tmp.profileSprite = animals[tmp_r].profileSprite;
            tmp.heal = animals[tmp_r].heal;
            tmp.heart = animals[tmp_r].heart;
            tmp.sword = animals[tmp_r].sword;
            tmp.shield = animals[tmp_r].shield;
            tmp.myGridX = tmp_x;
            tmp.myGridY = tmp_y;
            characters.Add(em, tmp);
        }

    }
}