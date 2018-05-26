using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Elven_Path
{
    public enum Direction
    {
        Up, Down,
        Right, Left
    }

    public class PlayerManager : MonoBehaviour
    {
        //public static PlayerManager instance;
        public static Action<int, SpiritType> TakeDamageEvent;
        //public static Action<int> UpdateScoreEvent;

        [SerializeField] private CharacterProfile startCharacter;
        [SerializeField] private GameObject characterObject;
        [SerializeField] private LayerMask blockLayer;

        private List<GameObject> heroes = new List<GameObject>();
        public List<Character> characters = new List<Character>();
        public List<Vector3> trains = new List<Vector3>();

        public float speed = 0.1f;

        private bool newHero = false;
        private bool isMoving = false;
        private bool isBattle = false;

        private Vector3 direction = Vector3.right;
        private Vector3 mDirection = Vector3.right;
        private Direction theDirection = Direction.Right;
        private Direction newDirection = Direction.Right;

        public SaveScore saved;

        #region Singleton
        private void Awake()
        {
            //instance = this;
        }
        private void OnEnable()
        {
            TakeDamageEvent += TakeDamage;
            GameManager.EndBattleEvent += Victory;
        }
        private void OnDisable()
        {
            TakeDamageEvent -= TakeDamage;
            GameManager.EndBattleEvent -= Victory;
        }
        private void Start()
        {
            GameObject em = Instantiate(characterObject, new Vector2( GameManager.startX + 12, GameManager.startY - 6), transform.rotation, GameManager.heroParent);
            em.GetComponent<SpriteRenderer>().sprite = startCharacter.profileSprite;

            characters.Add  (new Character());
            trains.Add(new Vector3(GameManager.startX + 12, GameManager.startY - 6, 0));
            heroes.Add(em);

            characters[0].myGridX = 12;
            characters[0].myGridY = 6;

            characters[0].Hp = startCharacter.heart;
            characters[0].type = startCharacter.type;
            characters[0].sword = startCharacter.sword;
            characters[0].shield = startCharacter.shield;
            characters[0].profileSprite = startCharacter.profileSprite;

            string tmp = "{HP : " + characters[0].Hp + " } {ATK : " + characters[0].sword + " } {DEF : " + characters[0].shield + " } {TYPE : " + characters[0].type + " } {HERO : " + heroes.Count + " }";
            UIPlayManager.UpdatePlayerStatusTextEvent.Invoke(tmp);
            UIPlayManager.UpdateHighScoreTextEvent.Invoke(false, saved.high_score.ToString());
        }
        #endregion

        private void Update()
        {
            //Debug.DrawRay(heroes[0].transform.position, direction, Color.green);
            MovementController();
            SwitchHeroController();
            if (!isMoving && !isBattle)
            {
                GameManager.GenAnythingEvent.Invoke();
                StartCoroutine(DelayMove());
            }
        }
        public void AddHero(GameObject hero)
        {
            newHero = true;
            GameObject em = Instantiate(characterObject, heroes[heroes.Count - 1].transform.position, transform.rotation, GameManager.heroParent);
            em.GetComponent<SpriteRenderer>().sprite = GameManager.characters[hero].profileSprite;

            trains.Add(trains[trains.Count - 1]);

            heroes.Add(em);
            characters.Add(new Character());

            characters[characters.Count - 1].Hp = GameManager.characters[hero].heart;
            characters[characters.Count - 1].type = GameManager.characters[hero].type;
            characters[characters.Count - 1].sword = GameManager.characters[hero].sword;
            characters[characters.Count - 1].shield = GameManager.characters[hero].shield;
            characters[characters.Count - 1].profileSprite = GameManager.characters[hero].profileSprite;
            characters[characters.Count - 1].myGridX = characters[(characters.Count == 2) ? 0 : characters.Count - 2].myGridX;
            characters[characters.Count - 1].myGridY = characters[(characters.Count == 2) ? 0 : characters.Count - 2].myGridY;

            GameManager.characters.Remove(hero);
            Destroy(hero);
        }

        #region System
        private GameObject target;
        private bool Detect_Alive()
        {
            if (Physics2D.Raycast(heroes[0].transform.position, direction, 1f).collider == null)
                return false;
            target = Physics2D.Raycast(heroes[0].transform.position, direction, 1f).collider.gameObject;
            return true;
        }
        private bool NextHero()
        {
            GameManager.gridFieldXY[characters[characters.Count - 1].myGridX, characters[characters.Count - 1].myGridY] = false;
            GameObject em = heroes[0];

            for (int i = heroes.Count - 1; i > 0; i--)
            {
                heroes[i].transform.position = trains[i - 1];
                characters[i].myGridX = characters[i - 1].myGridX;
                characters[i].myGridY = characters[i - 1].myGridY;
            }

            heroes.RemoveAt(0);
            characters.RemoveAt(0);
            if (heroes.Count > 0)
            {
                trains.RemoveAt(trains.Count - 1);
                string tmp = "{ HP : " + characters[0].Hp + "} {ATK : " + characters[0].sword + "} {DEF : " + characters[0].shield + "} {TYPE : " + characters[0].type + "}";
                UIPlayManager.UpdatePlayerStatusTextEvent.Invoke(tmp);
                Destroy(em);
                return true;
            }
            else
            {
                print("Game Over");
                return false;
            }
        }
        private int score = 0;
        private void UpdateScore()
        {
            for (int i = 0; i < characters.Count; i++)
                score += characters[i].Hp;
            UIPlayManager.UpdateScoreTextEvent.Invoke(score.ToString());
            if (score > saved.high_score)
            {
                saved.high_score = score;
                UIPlayManager.UpdateHighScoreTextEvent.Invoke(true, score.ToString());
            }
        }
        private void GameOver()
        {
            UIPlayManager.TurnOnEndPanelEvent.Invoke();
            SoundFxManager.instance.Sound_OnLoseEvent.Invoke();
        }
        #endregion
        #region Battle
        private void StartBattle()
        {
            //GameManager.StartBattleEvent.Invoke(target);
            string tmp = "ATK : " + characters[0].sword + "\n" + "DEF : " + characters[0].shield + "\n TYPE : " + characters[0].type;
            UIPlayManager.BeginUpdatePlayerBattlePanelEvent.Invoke(characters[0].profileSprite, characters[0].heart.ToString(), tmp);
            SoundFxManager.instance.Sound_OnStartBattleEvent.Invoke();
            Invoke("StrikeBack", 0.5f);
        }
        public void TakeDamage(int i, SpiritType type)
        {
            if (type == characters[0].type)
                i = i * 2;
            UIPlayManager.ShakePlayerPanelEvent.Invoke();
            characters[0].Hp -= (i - characters[0].shield > 0) ? i : 1;
            UIPlayManager.UpdatePlayerBattleHeartEvent.Invoke(characters[0].Hp.ToString());

            string tmp = "{HP : " + characters[0].Hp + " } {ATK : " + characters[0].sword + " } {DEF : " + characters[0].shield + " } {TYPE : " + characters[0].type + " } {HERO : " + heroes.Count + " }";
            UIPlayManager.UpdatePlayerStatusTextEvent.Invoke(tmp);

            StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(1f);
            if (!characters[0].isDie) StrikeBack();
            else NextHeroToStrikeBack();
        }

        private void StrikeBack()
        {
            GameManager.TakeDamageEvent.Invoke(characters[0].sword, characters[0].type);
            SoundFxManager.instance.Sound_OnAttackEvent.Invoke();
        }

        private void NextHeroToStrikeBack()
        {
            GameManager.gridFieldXY[characters[characters.Count - 1].myGridX, characters[characters.Count - 1].myGridY] = false;
            GameObject em = heroes[0];

            for(int i = heroes.Count - 1; i > 0; i--)
            {
                heroes[i].transform.position = trains[i - 1];
                characters[i].myGridX = characters[i - 1].myGridX;
                characters[i].myGridY = characters[i - 1].myGridY;
            }

            heroes.RemoveAt(0);
            characters.RemoveAt(0);
            if(heroes.Count > 0)
            {
                trains.RemoveAt(trains.Count - 1);
                string tmp = "{ HP : " + characters[0].Hp + "} {ATK : " + characters[0].sword + "} {DEF : " + characters[0].shield + "} {TYPE : " + characters[0].type + "}";
                UIPlayManager.UpdatePlayerStatusTextEvent.Invoke(tmp);
                tmp = "ATK : " + characters[0].sword + "\n" + "DEF : " + characters[0].shield + "\n TYPE : " + characters[0].type;
                UIPlayManager.BeginUpdatePlayerBattlePanelEvent.Invoke(characters[0].profileSprite, characters[0].heart.ToString(), tmp);
                Invoke("StrikeBack", 0.5f);
                SoundFxManager.instance.Sound_OnDieEvent.Invoke();
                Destroy(em);
            }
            else
            {
                print("Game Over");
                GameOver();
            }
        }
        private void Victory()
        {
            isBattle = false;
            speed = (speed > 0.1f) ? speed - 0.05f : speed - speed * 0.1f;
            SoundFxManager.instance.Sound_OnVictoryEvent.Invoke();
            UpdateScore();
        }
        #endregion
        #region Controller
        private void MovementController()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (theDirection == Direction.Down) return;
                newDirection = Direction.Up;

                direction = transform.up;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (theDirection == Direction.Up) return;
                newDirection = Direction.Down;

                direction = transform.up * -1;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (theDirection == Direction.Left) return;
                newDirection = Direction.Right;

                direction = transform.right;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (theDirection == Direction.Right) return;
                newDirection = Direction.Left;

                direction = transform.right * -1;
            }
        }
        private void SwitchHeroController()
        {
            if (heroes.Count <= 1 || isBattle) return;
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C))
            {
                print("Before   " + characters[0].myGridX + "  :   " + characters[0].myGridY + "  :   " + heroes[0].transform.position);
                print("Before   " + characters[1].myGridX + "  :   " + characters[1].myGridY + "  :   " + heroes[1].transform.position);

                var em = heroes[1];
                Character tmp = characters[1];
                int xx = tmp.myGridX;
                int yy = tmp.myGridY;

                int nxx = characters[0].myGridX;
                int nyy = characters[0].myGridY;

                tmp.myGridY = characters[1].myGridY;
                tmp.myGridX = characters[1].myGridX;

                heroes[1] = heroes[0];
                heroes[0] = em;

                characters[1] = characters[0];
                characters[1].myGridX = xx;
                characters[1].myGridY = yy;
                characters[0] = tmp;
                characters[0].myGridX = nxx;
                characters[0].myGridY = nyy;

                heroes[0].transform.position = trains[0];
                heroes[1].transform.position = trains[1];

                print("After    " + characters[0].myGridX + "  :   " + characters[0].myGridY + "  :   " + heroes[0].transform.position);
                print("After    " + characters[1].myGridX + "  :   " + characters[1].myGridY + "  :   " + heroes[1].transform.position);

                string tmp1 = "{HP : " + characters[0].Hp + " } {ATK : " + characters[0].sword + " } {DEF : " + characters[0].shield + " } {TYPE : " + characters[0].type + " } {HERO : " + heroes.Count + " }";
                UIPlayManager.UpdatePlayerStatusTextEvent.Invoke(tmp1);
            }
        }
        #endregion
        #region Movement
        private IEnumerator DelayMove()
        {
            isMoving = true;
            yield return new WaitForSeconds(speed);
            Moving();
        }
        private void Moving()
        {
            if (!CheckNewDirection())
            {
                GameOver();
                return;
            }
            if (Detect_Alive())
            {
                switch (target.tag)
                {
                    case "Hero":
                        AddHero(target);
                        string tmp = "{HP : " + characters[0].Hp + " } {ATK : " + characters[0].sword + " } {DEF : " + characters[0].shield + " } {TYPE : " + characters[0].type + " } {HERO : " + heroes.Count + " }";
                        UIPlayManager.UpdatePlayerStatusTextEvent.Invoke(tmp);
                        break;
                    case "Enemy":
                        isBattle = true;
                        isMoving = false;
                        GameManager.StartBattleEvent.Invoke(target);
                        StartBattle();
                        return;
                    case "Animal":
                        characters[0].Hp += (GameManager.characters[target].type == characters[0].type) ? 2 : 1;
                        string tmp1 = "{HP : " + characters[0].Hp + " } {ATK : " + characters[0].sword + " } {DEF : " + characters[0].shield + " } {TYPE : " + characters[0].type + " } {HERO : " + heroes.Count + " }";
                        UIPlayManager.UpdatePlayerStatusTextEvent.Invoke(tmp1);
                        SoundFxManager.instance.Sound_OnHealingEvent.Invoke();
                        GameManager.gridFieldXY[GameManager.characters[target].myGridX, GameManager.characters[target].myGridY] = false;
                        GameManager.characters.Remove(target);
                        Destroy(target);
                        break;
                    default: break;
                }
            }
            theDirection = newDirection;
            mDirection = direction;

            if (newHero)
            {
                for (int i = heroes.Count - 2; i > 0; i--)
                {
                    trains[i] = trains[i - 1];
                    heroes[i].transform.position = trains[i];
                }
                trains[0] += mDirection;
                heroes[0].transform.position = trains[0];
                newHero = false;
            }
            else
            {
                for (int i = heroes.Count - 1; i > 0; i--)
                {
                    trains[i] = trains[i - 1];
                    heroes[i].transform.position = trains[i];
                }
                trains[0] += mDirection;
                heroes[0].transform.position = trains[0];
                GameManager.gridFieldXY[characters[characters.Count - 1].myGridX, characters[characters.Count - 1].myGridY] = false;
            }

            Vector2 pos;
            int oldX, oldY;
            int newX, newY;

            oldX = characters[0].myGridX;
            oldY = characters[0].myGridY;
            characters[0].myGridX += Mathf.CeilToInt(mDirection.x);
            characters[0].myGridY -= Mathf.CeilToInt(mDirection.y);
            GameManager.gridFieldXY[characters[0].myGridX, characters[0].myGridY] = true;

            isMoving = false;

            for (int i = 1; i < heroes.Count; i++)
            {
                newX = characters[i].myGridX;
                newY = characters[i].myGridY;
                characters[i].myGridX = oldX;
                characters[i].myGridY = oldY;
                oldX = newX;
                oldY = newY;

                pos.x = GameManager.startX + characters[i].myGridX;
                pos.y = GameManager.startY - characters[i].myGridY;
                trains[i] = pos;
                heroes[i].transform.position = pos;
            }
        }
        private bool CheckNewDirection()
        {
            //print("use");
            switch (newDirection)
            {
                case Direction.Up:
                    if (Physics2D.Raycast(heroes[0].transform.position, direction, 1f, blockLayer).collider != null)
                    {
                        if (!NextHero()) return false;
                        if(Physics2D.Raycast(heroes[0].transform.position, Vector2.left, 1f, blockLayer).collider != null)
                        {
                            theDirection = Direction.Right;
                            newDirection = Direction.Right;
                            direction = Vector3.right;
                        }
                        else
                        {
                            theDirection = Direction.Left;
                            newDirection = Direction.Left;
                            direction = Vector3.left;
                        }
                    }
                    break;
                case Direction.Down:
                    if (Physics2D.Raycast(heroes[0].transform.position, direction, 1f, blockLayer).collider != null)
                    {
                        if (!NextHero()) return false;
                        if (Physics2D.Raycast(heroes[0].transform.position, Vector2.right, 1f, blockLayer).collider != null)
                        {
                            theDirection = Direction.Left;
                            newDirection = Direction.Left;
                            direction = Vector3.left;
                        }
                        else
                        {
                            theDirection = Direction.Right;
                            newDirection = Direction.Right;
                            direction = Vector3.right;
                        }
                    }
                    break;
                case Direction.Right:
                    if (Physics2D.Raycast(heroes[0].transform.position, direction, 1f, blockLayer).collider != null)
                    {
                        if (!NextHero()) return false;
                        if (Physics2D.Raycast(heroes[0].transform.position, Vector2.up, 1f, blockLayer).collider != null)
                        {
                            theDirection = Direction.Down;
                            newDirection = Direction.Down;
                            direction = Vector3.down;
                        }
                        else
                        {
                            theDirection = Direction.Up;
                            newDirection = Direction.Up;
                            direction = Vector3.up;
                        }
                    }
                    break;
                case Direction.Left:
                    if (Physics2D.Raycast(heroes[0].transform.position, direction, 1f, blockLayer).collider != null)
                    {
                        if (!NextHero()) return false;
                        if (Physics2D.Raycast(heroes[0].transform.position, Vector2.down, 1f, blockLayer).collider != null)
                        {
                            theDirection = Direction.Up;
                            newDirection = Direction.Up;
                            direction = Vector3.up;
                        }
                        else
                        {
                            theDirection = Direction.Down;
                            newDirection = Direction.Down;
                            direction = Vector3.down;
                        }
                    }
                    break;
                default: return true;
            }
            return true;
        }
        #endregion
    }
}