using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundcheckScript : MonoBehaviour
{
    PlayerController playerController;


    private void Start()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
    }



    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            playerController.onJumpCooldown = true;
            playerController.triggerJumpCooldown();
        }
    }
}
