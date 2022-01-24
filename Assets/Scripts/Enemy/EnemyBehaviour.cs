using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
   [SerializeField]
   float moveSpeed =1f;

   Rigidbody2D myRigidbody;
   public Collider2D myBoxCollider;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        
    }
    
    void Update()
    {
        
        if(IsFacingRight())
        {
            myRigidbody.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
             myRigidbody.velocity = new Vector2(-moveSpeed, 0f);
        }
    }
    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;       //Epsilon checking for very smaller value
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), transform.localScale.y);
    }

    void OnCollisionExit2D(Collision2D other) {
    
    }
}
