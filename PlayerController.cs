using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool canJump = true;
    int groundMask = 1<<8; // this is a “bitshift”
 // Update is called once per frame


    bool isIdle;
    bool isLeft;
    int isIdleKey = Animator.StringToHash("isIdle");
    int isJumpKey = Animator.StringToHash("isJump");
    
    void Start()
    {

    }

    void Update()
    {
        Animator a = GetComponent<Animator>(); 
        a.SetBool(isIdleKey, isIdle);
        a.SetBool(isJumpKey, !canJump);

        SpriteRenderer r = GetComponent<SpriteRenderer>();
        r.flipX = isLeft;

    }

    private void FixedUpdate()
    {
        isIdle = true;


        Vector2 physicsVelocity = Vector2.zero;
        Rigidbody2D r = GetComponent<Rigidbody2D>();

        // move left
        if (Input.GetKey(KeyCode.A))
        {
            physicsVelocity.x -= 7;
            isIdle = false;
            isLeft = true;

        }
        // move right
        if (Input.GetKey(KeyCode.D))
        {
            physicsVelocity.x += 7;
            isIdle = false;
            isLeft = false;
        }

        // jump
        if (Input.GetKey(KeyCode.W))
        {
            if (canJump)
            {
                r.velocity = new Vector2(physicsVelocity.x, 14);
                GetComponent<AudioSource>().Play();
                canJump = false;
                isIdle = false;
            }
        }
        // Test the ground immediately below the Player
        // and if it tagged as a Ground layer, then we allow the
        // Player to jump again. The capsule collider is 4.8 units
        // high, so 2.5 units “down” from its centre will be just
        // touching the floor when we are on the ground.
        if (Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y),-Vector2.up, 2.5f, groundMask))
        {
            canJump = true;
        }

        // apply the updated velocity to the rigid body
        r.velocity = new Vector2(physicsVelocity.x, r.velocity.y);

    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coins"))
        {
            
            Destroy(other.gameObject);
        }
    }
}
