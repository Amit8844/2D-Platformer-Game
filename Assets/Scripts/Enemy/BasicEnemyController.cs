using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
   private enum State
   {
       Walking,
       Knockback,
       Dead
   }

   private State currentState;
   
   [SerializeField]
   private float
        groundCheckDistance,
        wallCheckDistance,
        movementSpeed,
        maxHealth,
        KnockbackDuration;

   [SerializeField]
   private Transform
          groundCheck,
          wallCheck;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private Vector2 KnockbackSpeed;

    private float 
               currentHealth,
               KnockbackStartTime;

   private int 
            facingDirection,
            damageDirection;

   private Vector2 movement;

   private bool
           groundDetected,
           wallDetected;

   private GameObject alive; 
   private Rigidbody2D aliveRb;
   private Animator aliveAnim;


   private  void Start() 
   {
       alive = transform.Find("Alive").gameObject;
       aliveRb = alive.GetComponent<Rigidbody2D>();
       aliveAnim = alive.GetComponent<Animator>();

       facingDirection = 1;
   }    

   private void Update()
   {
       switch (currentState)
       {
           case State.Walking:
              UpdateWalkingState();
              break;
           case State.Knockback:
              UpdateKnockbackState();
              break;
           case State.Dead:
              UpdateDeadState();
              break;      
       }
   }
   
   //--Walking State-----------
   private void EnterWalkingState()
   {

   }

   private void UpdateWalkingState()
   {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);


          if (!groundDetected || wallDetected)
          {
              //Flip
              Flip();
          }        
          else
          {
            //Move
            movement.Set(movementSpeed * facingDirection, aliveRb.velocity.y);
            aliveRb.velocity = movement;
          }
   }

   private void ExitWalkingState()
   {

   }
   //------KNOCKBACK STATE-----------
   private void EnterKnockbackState()
   {
           KnockbackStartTime = Time.time;
           movement.Set(KnockbackSpeed.x * damageDirection, KnockbackSpeed.y);
           aliveRb.velocity = movement;
           aliveAnim.SetBool("Knockback", true);
   }

   private void UpdateKnockbackState()
   {
       if(Time.time >= KnockbackStartTime + KnockbackDuration)
       {
           SwitchState(State.Walking);
       }
   }
   
   private void ExitKnockbackState()
   {
      aliveAnim.SetBool("Knockback",false);
   }

   //-----DEAD STATE-------------------

   private void EnterDeadState()
     {
           Destroy(gameObject);
     }

     private void UpdateDeadState()
     {

     }

     private void ExitDeadState()
     {

     }

     //---OTHER FUNCTIONS---------------------------------------

     private void Damage(float[] attackDeatails)
     {
           currentHealth -= attackDeatails[0];

           if (attackDeatails[1] > alive.transform.position.x)
           {
               damageDirection = -1;
           }
           else
           {
               damageDirection =1;

           }
           //hit
            if (currentHealth > 0.0f)
            {
                SwitchState(State.Knockback);

            }
            else if(currentHealth <= 0.0f)
            {
                SwitchState(State.Dead);
            }
     }

      private void Flip()
      {
          facingDirection *= -1;
          alive.transform.Rotate(0.0f, 180.0f, 0.0f);
      }


     private void SwitchState(State state)
     {
         switch (currentState)
         {
             case State.Walking:
                 ExitWalkingState();
                 break;
             case State.Knockback:
                 ExitKnockbackState();
                 break;
            case State.Dead:
                 ExitDeadState();
                 break;

         }

          switch (state)
         {
             case State.Walking:
                 EnterWalkingState();
                 break;
             case State.Knockback:
                 EnterKnockbackState();
                 break;
            case State.Dead:
                 EnterDeadState();
                 break;

         }

         currentState = state;
     }
     private void OnDrawGizmos()
      {
         Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
         Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
      }
}
