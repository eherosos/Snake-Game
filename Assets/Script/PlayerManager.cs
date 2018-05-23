using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elven_Path
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance;

        [SerializeField] private CharacterProfile startCharacter;
        [SerializeField] private GameObject characterObject;
        [SerializeField] private LayerMask blockLayer;

        private List<GameObject> heroes = new List<GameObject>();
        public List<Character> characters = new List<Character>();

        public float speed = 0.1f;

        private bool newHero = false;
        private bool isMoving = false;
        private bool isBattle = false;

        private Vector3 direction = Vector3.right;
        private Vector3 mDirection = Vector3.right;
        private Direction theDirection = Direction.Right;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            GameObject em = Instantiate(characterObject, new Vector2( GameManager.instance.startX + 12, GameManager.instance.startY - 6), transform.rotation, GameManager.instance.heroParent);
            em.GetComponent<SpriteRenderer>().sprite = startCharacter.profileSprite;

            characters.Add  (new Character());
            heroes.Add(em);

            characters[0].myGridX = 12;
            characters[0].myGridY = 6;

            characters[0].Hp = startCharacter.heart;
            characters[0].sword = startCharacter.sword;
            characters[0].shield = startCharacter.shield;
            characters[0].profileSprite = startCharacter.profileSprite;
        }

        private void Update()
        {
            Debug.DrawRay(heroes[0].transform.position, direction, Color.green);

            MovementController();
            if (!isMoving && !isBattle)
            {
                CheckNewDirection();

                if (Detect_Alive())
                {
                    switch (target.tag)
                    {
                        case "Hero":
                            AddHero(target);
                            StartCoroutine(Moving());
                            break;
                        case "Enemy": isBattle = true;

                            break;
                        case "Animal":
                            break;
                        default: break;
                    }
                }
                else
                {
                    StartCoroutine(Moving());
                }
            }
        }

        public void AddHero(GameObject hero)
        {
            newHero = true;
            GameObject em = Instantiate(characterObject, heroes[heroes.Count - 1].transform.position, transform.rotation, GameManager.instance.heroParent);
            em.GetComponent<SpriteRenderer>().sprite = GameManager.instance.characters[hero].profileSprite;

            heroes.Add(em);
            characters.Add(new Character());

            characters[characters.Count - 1].Hp = GameManager.instance.characters[hero].heart;
            characters[characters.Count - 1].sword = GameManager.instance.characters[hero].sword;
            characters[characters.Count - 1].shield = GameManager.instance.characters[hero].shield;
            characters[characters.Count - 1].profileSprite = GameManager.instance.characters[hero].profileSprite;
            characters[characters.Count - 1].myGridX = characters[(characters.Count == 2) ? 0 : characters.Count - 2].myGridX;
            characters[characters.Count - 1].myGridY = characters[(characters.Count == 2) ? 0 : characters.Count - 2].myGridY;

            GameManager.instance.characters.Remove(hero);
            Destroy(hero);
        }

        #region CheckFront
        private GameObject target;
        private bool Detect_Alive()
        {
            if (Physics2D.Raycast(heroes[0].transform.position, direction, 1f).collider == null)
                return false;
            target = Physics2D.Raycast(heroes[0].transform.position, direction, 1f).collider.gameObject;
            return true;
        }
        #endregion

        #region Movement
        private IEnumerator Moving()
        {
            isMoving = true;
            float gridPos = 0;
            mDirection = direction;
            Vector2 pos;

            if (newHero)
            {
                do
                {
                    for (int i = 0; i < heroes.Count - 1; i++)
                    {
                        pos = heroes[i].transform.position;
                        heroes[i].transform.position += mDirection * speed;
                    }

                    gridPos += speed;
                    yield return new WaitForEndOfFrame();
                } while (gridPos < 1);
                newHero = false;
            }
            else
            {
                do
                {
                    for (int i = 0; i < heroes.Count; i++)
                    {
                        pos = heroes[i].transform.position;
                        heroes[i].transform.position += mDirection * speed;
                    }

                    gridPos += speed;
                    yield return new WaitForEndOfFrame();
                } while (gridPos < 1);
                GameManager.instance.gridFieldXY[characters[characters.Count - 1].myGridX, characters[characters.Count - 1].myGridY] = false;
            }

            int oldX, oldY;
            int newX, newY;

            oldX = characters[0].myGridX;
            oldY = characters[0].myGridY;
            characters[0].myGridX += Mathf.CeilToInt(mDirection.x);
            characters[0].myGridY += Mathf.CeilToInt(mDirection.y);
            GameManager.instance.gridFieldXY[characters[0].myGridX, characters[0].myGridY] = true;

            isMoving = false;

            for (int i = 1; i < heroes.Count; i++)
            {
                newX = characters[i].myGridX;
                newY = characters[i].myGridY;
                characters[i].myGridX = oldX;
                characters[i].myGridY = oldY;
                oldX = newX;
                oldY = newY;

                pos.x = GameManager.instance.startX + characters[i].myGridX;
                pos.y = GameManager.instance.startY - characters[i].myGridY;
                heroes[i].transform.position = pos;
            }
        }
        private void MovementController()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (theDirection == Direction.Down) return;
                theDirection = Direction.Up;

                direction = transform.up;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (theDirection == Direction.Up) return;
                theDirection = Direction.Down;

                direction = transform.up * -1;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (theDirection == Direction.Left) return;
                theDirection = Direction.Right;

                direction = transform.right;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (theDirection == Direction.Right) return;
                theDirection = Direction.Left;

                direction = transform.right * -1;
            }
        }
        private void CheckNewDirection()
        {
            switch (theDirection)
            {
                case Direction.Up:
                    if (Physics2D.Raycast(heroes[0].transform.position, direction, 1f, blockLayer).collider != null)
                    {
                        if(Physics2D.Raycast(heroes[0].transform.position, Vector2.left, 1f, blockLayer).collider != null)
                        {
                            theDirection = Direction.Right;
                            direction = Vector3.right;
                        }
                        else
                        {
                            theDirection = Direction.Left;
                            direction = Vector3.left;
                        }
                    }
                    break;
                case Direction.Down:
                    if (Physics2D.Raycast(heroes[0].transform.position, direction, 1f, blockLayer).collider != null)
                    {
                        if (Physics2D.Raycast(heroes[0].transform.position, Vector2.right, 1f, blockLayer).collider != null)
                        {
                            theDirection = Direction.Left;
                            direction = Vector3.left;
                        }
                        else
                        {
                            theDirection = Direction.Right;
                            direction = Vector3.right;
                        }
                    }
                    break;
                case Direction.Right:
                    if (Physics2D.Raycast(heroes[0].transform.position, direction, 1f, blockLayer).collider != null)
                    {
                        if (Physics2D.Raycast(heroes[0].transform.position, Vector2.up, 1f, blockLayer).collider != null)
                        {
                            theDirection = Direction.Down;
                            direction = Vector3.down;
                        }
                        else
                        {
                            theDirection = Direction.Up;
                            direction = Vector3.up;
                        }
                    }
                    break;
                case Direction.Left:
                    if (Physics2D.Raycast(heroes[0].transform.position, direction, 1f, blockLayer).collider != null)
                    {
                        if (Physics2D.Raycast(heroes[0].transform.position, Vector2.down, 1f, blockLayer).collider != null)
                        {
                            theDirection = Direction.Up;
                            direction = Vector3.up;
                        }
                        else
                        {
                            theDirection = Direction.Down;
                            direction = Vector3.down;
                        }
                    }
                    break;
                default: break;
            }
        }
        #endregion
    }
}