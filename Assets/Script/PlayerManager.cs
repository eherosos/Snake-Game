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

        private List<Vector2Int> heroesIndex = new List<Vector2Int>();
        private List<GameObject> heroes = new List<GameObject>();
        private List<Character> characters = new List<Character>();

        public float speed = 0.1f;

        private bool newHero = false;
        private bool isMoving = false;
        private Vector3 direction = Vector3.right;
        private Vector3 mDirection = Vector3.right;
        private Direction theDirection = Direction.Right;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            GameObject em = Instantiate(characterObject, new Vector2(6,12), transform.rotation, GameManager.instance.heroParent);
            em.GetComponent<SpriteRenderer>().sprite = startCharacter.profileSprite;

            heroesIndex.Add (new Vector2Int(6, 12));
            characters.Add  (new Character());
            heroes.Add(em);

            characters[0].myIndex = heroesIndex[0];
            characters[0].Hp = startCharacter.heart;
            characters[0].sword = startCharacter.sword;
            characters[0].shield = startCharacter.shield;
            characters[0].profileSprite = startCharacter.profileSprite;
        }

        private void Update()
        {
            MovementController();
            if (!isMoving)
            {
                StartCoroutine(Moving());
            }
        }

        public void AddHero(CharacterProfile characterProfile)
        {
            newHero = true;
            GameObject em = Instantiate(characterObject, heroes[heroes.Count - 1].transform.position, transform.rotation, GameManager.instance.heroParent);
            em.GetComponent<SpriteRenderer>().sprite = characterProfile.profileSprite;

            heroesIndex.Add (heroesIndex[heroesIndex.Count - 1]);
            characters.Add  (new Character());
            heroes.Add(em);

            characters[characters.Count - 1].Hp = characterProfile.heart;
            characters[characters.Count - 1].sword = characterProfile.sword;
            characters[characters.Count - 1].shield = characterProfile.shield;
            characters[characters.Count - 1].myIndex = heroesIndex[heroesIndex.Count - 1];
            characters[characters.Count - 1].profileSprite = characterProfile.profileSprite;
        }

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
                GameManager.instance.gridFieldXY[heroesIndex[heroesIndex.Count - 1].x, heroesIndex[heroesIndex.Count - 1].y] = false;
            }

            Vector2Int newHeroIndex;
            Vector2Int oldHeroIndex;

            oldHeroIndex = heroesIndex[0];
            heroesIndex[0] += new Vector2Int(Mathf.CeilToInt(mDirection.x), Mathf.CeilToInt(mDirection.y));
            GameManager.instance.gridFieldXY[heroesIndex[0].x, heroesIndex[0].y] = true;

            for (int i = 1; i < heroes.Count; i++)
            {
                newHeroIndex = heroesIndex[i];
                heroesIndex[i] = oldHeroIndex;
                oldHeroIndex = newHeroIndex;
            }

            isMoving = false;

            for (int i = 0; i < heroes.Count; i++)
            {
                pos.x = GameManager.instance.startX + heroesIndex[i].x;
                pos.y = GameManager.instance.startY - heroesIndex[i].y;
                heroes[i].transform.position = pos;
                characters[i].myIndex = heroesIndex[i];
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
        #endregion
    }
}