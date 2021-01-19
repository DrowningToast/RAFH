using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using level;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;

        [SerializeField] Rigidbody2D rigidbody2D;
        [SerializeField] SpriteRenderer spriteRender;

        [Range(1, 50)]
        [SerializeField] float startingSpeed = 8;
        [SerializeField] float currentSpeed;
        [SerializeField] float speedMultipiler = 1;
        [SerializeField] float jumpMultipiler = 4;
        [SerializeField] bool isDead = false;
        [SerializeField] bool isGround = false;
        [SerializeField] bool isSliding = false;

        public event Action<int> onLoadNewTileset;
        public event Action<int> onDeleteOldTileset;


        [SerializeField] LayerMask groundLayerMask;

        [SerializeField] int jumpCounter = 0;

        [SerializeField] Animator animator;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();

            animator = GetComponent<Animator>();

            currentSpeed = startingSpeed * speedMultipiler;
        }

        // Update is called once per frame
        void Update()
        {
            // If press jump (then) check availability and stop sliding
            if (Input.GetKeyDown("x") && jumpCounter <= 1 && jumpCounter != -1)
            {
                Jump();
                isSliding = false;
            }

            // If pressing (slide button) then slide when can
            if (!isGround || !Input.GetKey("z"))
            {
                isSliding = false;
            } 
            else if (Input.GetKey("z"))
            {
                isSliding = true;
                Slide();
            }

            // Correct the animation every frame
            ManageAnimation();
            if (!isDead)
            {
                rigidbody2D.velocity = new Vector2(currentSpeed, rigidbody2D.velocity.y);
            }
            animator.SetBool("Sliding", isSliding);

            
        }

        void ManageAnimation ()
        {
            if (!isGround && rigidbody2D.velocity.y < 0)
            {
                animator.SetBool("AirUp", false);
                animator.SetBool("AirDown", true);
            }else if (rigidbody2D.velocity.y > 2f)
            {
                animator.SetBool("AirUp", true);
            }
            animator.SetBool("Landing", isLanding());
        }

        void Jump()
        {
            jumpCounter += 1;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpMultipiler);

        }

        void Slide()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.tag)
            {
                // เมื่อสัมผัสพื้น
                case "ground":
                    isGround = true;
                    jumpCounter = 0;
                    animator.SetBool("AirUp", false);
                    animator.SetBool("AirDown", false);
                    break;
                // เมื่อแตะ load triggerbox
                case "Load":
                    // ปิด collider GameObject ตัวนั้นทิ้งป้องกันการโดนซ้ำ
                    collision.gameObject.SetActive(false);
                    // usedTile คือ index ของ tile ที่กำลังใช้งานอยู่แล้วโดยเราไม่ต้องการให้ tile มันต่อไปซ้ำกับ tile อันปัจจุบัน
                    int usedTile = TilesetsController.instance.tilesetPointer;
                    onLoadNewTileset.Invoke(usedTile);

                    break;
                case "Delete":
                    int target = collision.transform.parent.GetComponent<GridController>().index;
                    print($"Attempt delete : {target}");
                    onDeleteOldTileset.Invoke(target);
                    break;
                case "Obstacle":
                    print($"Obstacle : {collision.gameObject.name} in {collision.transform.parent.parent.gameObject.name}");
                    Destroy(gameObject);
                    print("Gameover");
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    break;
                case "BottomWall":
                    print($"Bottom Wall : {collision.gameObject.name}");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    break;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("ground"))
            {
                isGround = false;
            }
        }

        void OnTriggerStay2D (Collider2D collision)
        {
            if (collision.CompareTag("ground"))
            {
                isGround = true;
                jumpCounter = 0;
                animator.SetBool("AirUp", false);
                animator.SetBool("AirDown", false);
            }
            
        }

        private bool isLanding()
        {
            RaycastHit2D raycastHit = Physics2D.Raycast((Vector2)transform.position + new Vector2(0,0), Vector2.down, 2.5f, groundLayerMask);
            Color raycastColor;
            if (raycastHit.collider != null && raycastHit.collider.CompareTag("ground"))
            {
                if (rigidbody2D.velocity.y < 0)
                {
                    raycastColor = Color.green;
                }
                else
                {
                    raycastColor = Color.yellow;
                }
            }
            else
            {
                raycastColor = Color.red;
            }
            Debug.DrawRay((Vector2)transform.position + new Vector2(0, 0), Vector2.down * 2.5f, raycastColor);

            return raycastHit.collider != null && raycastHit.collider.CompareTag("ground");
        }

    }
}