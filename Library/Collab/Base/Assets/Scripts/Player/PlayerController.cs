using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        static PlayerController instance;

        [Range(1,50)]
        [SerializeField] float startingSpeed = 8;
        [SerializeField] float currentSpeed;
        [SerializeField] float speedMultipiler = 1;
        [SerializeField] float jumpMultipiler = 4;
        [SerializeField] Rigidbody2D rigidbody2D;
        [SerializeField] bool isDead = false;
        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            { 
                Destroy(gameObject);
            }

            rigidbody2D = GetComponent<Rigidbody2D>();

            currentSpeed = startingSpeed * speedMultipiler;

            rigidbody2D.velocity = new Vector2(currentSpeed, rigidbody2D.velocity.y);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("z"))
            {
                Jump();
            }
            if (!isDead)
            {
                rigidbody2D.velocity = new Vector2(currentSpeed, rigidbody2D.velocity.y);
            }
        }

        void Jump()
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpMultipiler);

        }

    }
}