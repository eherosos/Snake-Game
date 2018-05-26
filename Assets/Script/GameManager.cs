//using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace Elven_Path
{
    public class SendInt : UnityEvent<int> { }

    public class GameManager : MonoBehaviour
    {
        //public static UnityEvent GenAnythingEvent = new UnityEvent();

        public static System.Action GenAnythingEvent;
        public static System.Action<GameObject> StartBattleEvent;
        public static System.Action EndBattleEvent;
        public static System.Action<int, SpiritType> TakeDamageEvent;

        private Vector2 vec;

        public bool isPause { get; set; }
        public bool isBattle { get; set; }
        public bool isOver { get; set; }
        private bool isTimeToGen = false;

        public CharacterProfile[] heroes;
        public CharacterProfile[] enemies;
        public CharacterProfile[] animals;

        public GameObject heroPrefs, enemyPrefs, animalPrefs;

        public Transform mHeroParent, mEnemyParent, mAnimalParent;
        public static Transform heroParent { get; private set; }
        public static Transform enemyParent { get; private set; }
        public static Transform animalParent { get; private set; }

        public static bool[,] gridFieldXY;
        public const float startX = -11;
        public const float startY = 5;
        public int countX, countY;

        private int wave = 1;

        public static Dictionary<GameObject, Character> characters = new Dictionary<GameObject, Character>();

        [TextArea(0, 20)]
        public string debugText;

        #region Singleton
        private void Awake()
        {
            //instance = this;
            heroParent = mHeroParent;
            enemyParent = mEnemyParent;
            animalParent = mAnimalParent;

            //GenAnythingEvent.AddListener(Gen_Anything);

            characters = new Dictionary<GameObject, Character>();

            gridFieldXY = new bool[countX, countY];

            //print(gridFieldXY.Length);

            isPause = false;
            
        }
        private void OnEnable()
        {
            GenAnythingEvent += Gen_Anything;
            TakeDamageEvent += TakeDamage;

            StartBattleEvent += BattleBegin;
            EndBattleEvent += BattleFinish;
        }
        private void OnDisable()
        {
            GenAnythingEvent -= Gen_Anything;
            TakeDamageEvent -= TakeDamage;

            StartBattleEvent -= BattleBegin;
            EndBattleEvent -= BattleFinish;
        }
        private void Start()
        {
            StartCoroutine(Gen());
        }
        #endregion

        private void Update()
        {
            debugText = "";
            //for (int y = countY - 1; y > -1; y--)
            for (int y = 0; y < countY; y++)
            {
                for(int x = 0; x < countX; x++)
                {
                    if (gridFieldXY[x,y]) debugText += " * ";
                    else debugText += " - ";
                }
                debugText += '\n';
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

        #region Gen
        public void Gen_Anything()
        {
            if (isBattle || isOver || isPause) return;
            if (isTimeToGen)
            {
                wave++;
                UIPlayManager.UpdateWaveTextEvent.Invoke(wave.ToString());
                SoundFxManager.instance.Sound_OnGenerateEvent.Invoke();
                isTimeToGen = false;
                Gen_Hero();
                Gen_Enemy();
                Gen_Animal();
                StartCoroutine(Gen());
            }
        }
        private void Gen_Hero()
        {
            //if (Random.Range(0, 1f) < 0.3f) return;
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
            int tmp_r = Random.Range(0, ((wave < heroes.Length) ? wave : heroes.Length));

            em.GetComponent<SpriteRenderer>().sprite = heroes[tmp_r].profileSprite;

            Character tmp = new Character();
            tmp.profileSprite = heroes[tmp_r].profileSprite;
            tmp.heart = heroes[tmp_r].heart;
            tmp.sword = heroes[tmp_r].sword;
            tmp.shield = heroes[tmp_r].shield;
            tmp.type = heroes[tmp_r].type;
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
            int tmp_r = Random.Range(0, ((wave < enemies.Length) ? wave : enemies.Length));

            em.GetComponent<SpriteRenderer>().sprite = enemies[tmp_r].profileSprite;

            Character tmp = new Character();
            tmp.profileSprite = enemies[tmp_r].profileSprite;
            tmp.heart = enemies[tmp_r].heart;
            tmp.sword = enemies[tmp_r].sword;
            tmp.shield = enemies[tmp_r].shield;
            tmp.type = enemies[tmp_r].type;
            tmp.myGridX = tmp_x;
            tmp.myGridY = tmp_y;
            characters.Add(em, tmp);
        }
        private void Gen_Animal()
        {
            if (wave < 10) return;
            if (Random.Range(0, 1f) < 0.75f) return;
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
            int tmp_r = Random.Range(0, ((wave < animals.Length) ? wave : animals.Length));

            em.GetComponent<SpriteRenderer>().sprite = animals[tmp_r].profileSprite;
            Character tmp = new Character();
            tmp.profileSprite = enemies[tmp_r].profileSprite;
            tmp.heart = enemies[tmp_r].heart;
            tmp.type = enemies[tmp_r].type;
            tmp.myGridX = tmp_x;
            tmp.myGridY = tmp_y;
            characters.Add(em, tmp);
        }
        #endregion

        #region Battle
        private GameObject target;
        public void BattleBegin(GameObject enemy)
        {
            target = enemy;
            Character tmp = characters[enemy];
            UIPlayManager.EnableBattlePanelEvent.Invoke(true);
            string tmp2 = "ATK : " + tmp.sword + "\n" + "DEF : " + tmp.shield + "\n" + "TYPE : " + tmp.type;
            UIPlayManager.BeginUpdateEnemyBattlePanelEvent.Invoke(tmp.profileSprite, tmp.heart.ToString(), tmp2);
        }
        public void TakeDamage(int damage,SpiritType type)
        {
            //print("Enemy Hurt");
            if (type == characters[target].type)
                damage = damage * 2;
            UIPlayManager.ShakeEnemyPanelEvent.Invoke();
            characters[target].Hp -= (damage - characters[target].shield > 0) ? damage : 1;
            UIPlayManager.UpdateEnemyBattleHeartEvent.Invoke(characters[target].Hp.ToString());
            StartCoroutine(Delay());
        }
        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.5f);
            if (!characters[target].isDie) StrikeBack();
            else Reward();
        }
        private void StrikeBack()
        {
            PlayerManager.TakeDamageEvent.Invoke(characters[target].sword, characters[target].type);
            SoundFxManager.instance.Sound_OnAttackEvent.Invoke();
        }
        private void Reward()
        {
            //print("Reward");
            GameManager.gridFieldXY[characters[target].myGridX, characters[target].myGridY] = false;
            characters.Remove(target);
            Destroy(target);
            EndBattleEvent.Invoke();
            UIPlayManager.EnableBattlePanelEvent.Invoke(false);
        }
        public void BattleFinish() { UIPlayManager.EnableBattlePanelEvent.Invoke(false); }
        #endregion

    }
}