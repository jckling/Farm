using UnityEngine;

namespace Farm.Inventory
{
    public class ItemBounce : MonoBehaviour
    {
        private Transform spriteTrans;
        private BoxCollider2D coll;

        public float gravity = -4.0f;
        private bool isGround;
        private float distance;
        private Vector2 direction;
        private Vector3 targetPos;

        #region Event Functions

        private void Awake()
        {
            spriteTrans = transform.GetChild(0);
            coll = GetComponent<BoxCollider2D>();
            coll.enabled = false;
        }

        private void Update()
        {
            Bounce();
        }

        #endregion

        public void InitBounceItem(Vector3 target, Vector2 dir)
        {
            coll.enabled = false;
            direction = dir;
            targetPos = target;
            distance = Vector3.Distance(target, transform.position);

            spriteTrans.position += Vector3.up * 1.5f; // player 头顶
        }

        private void Bounce()
        {
            isGround = spriteTrans.position.y <= transform.position.y;

            if (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                transform.position += (Vector3)direction * (distance * -gravity * Time.deltaTime);
            }

            if (!isGround)
            {
                spriteTrans.position += Vector3.up * (gravity * Time.deltaTime);
            }
            else
            {
                spriteTrans.position = transform.position;
                coll.enabled = true;
            }
        }
    }
}