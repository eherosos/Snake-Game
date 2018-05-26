using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elven_Path
{
    public class TestMove : MonoBehaviour
    {
        #region Value
        private bool isMoving = false;
        private Vector3 mDirection = Vector3.right;
        private Vector3 direction = Vector3.right;
        public float speed = 0.1f;
        private Direction theDirection = Direction.Right;
        #endregion

        private void Update()
        {
            MovementController();
            if (!isMoving)
            {
                StartCoroutine(Moving());
            }
        }

        #region Movement-System
        private IEnumerator Moving()
        {
            isMoving = true;
            float gridPos = 0;
            mDirection = direction;

            do
            {
                gridPos += speed;
                transform.position += mDirection * speed;
                yield return new WaitForEndOfFrame();
            } while (gridPos < 1);
            
            isMoving = false;
            transform.position += mDirection * (1 - gridPos);
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