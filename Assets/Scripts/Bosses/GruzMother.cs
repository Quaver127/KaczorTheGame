using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class GruzMother : MonoBehaviour
{
    public static GruzMother instance;
    
    [SerializeField] private GameObject gateL;
    [SerializeField] private GameObject gateR;
    
    public GameObject portal;
    
    public Animator gateL_Animator;
    public Animator gateR_Animator;
    
    [Header("Sleep")]
    [SerializeField] float sleepMovementSpeed;
    [SerializeField] Vector2 sleepMovementDirection;
    
    [Header("Idel")]
    [SerializeField] float idelMovementSpeed;
    [SerializeField] Vector2 idelMovementDirection;

    [Header("AttackUpNDown")]
    [SerializeField] float attackMovementSpeed;
    [SerializeField] Vector2 attackMovementDirection;

    [Header("AttackPlayer")]
    [SerializeField] float attackPlayerSpeed;
    [SerializeField] Transform player;
    
    [Header("Health")]
    [SerializeField] private float maxHealth;
    private float currentHealth;
    public bool isInvincible = false;
    private bool isHitted = false;

    [Header("Other")]
    [SerializeField] Transform goundCheckUp;
    [SerializeField] Transform goundCheckDown;
    [SerializeField] Transform goundCheckWall;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    private bool isTouchingUp;
    private bool isTouchingDown;
    private bool isTouchingWall;
    private bool hasPlayerPositon;

    private Vector2 playerPosition;

    private bool facingLeft = true;
    private bool goingUp = true;
    private Rigidbody2D enemyRB;
    private Animator enemyAnim;
    
    private bool hasBeenKilled = false;

    void Awake()
    {
        if (hasBeenKilled)
        {
            Debug.Log("Already Killed");
            Destroy(gameObject);
        }
    }
    void Start()
    {
        currentHealth = maxHealth;
        
        idelMovementDirection.Normalize();
        attackMovementDirection.Normalize();
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingUp = Physics2D.OverlapCircle(goundCheckUp.position, groundCheckRadius, groundLayer); 
        isTouchingDown = Physics2D.OverlapCircle(goundCheckDown.position, groundCheckRadius, groundLayer); 
        isTouchingWall = Physics2D.OverlapCircle(goundCheckWall.position, groundCheckRadius, groundLayer);
    }

    void RandomStatePicker()
    {
        int randomState = Random.Range(0, 2);
        if (randomState == 0)
        {
            enemyAnim.SetTrigger("AttackUpNDown");
        }
        else if (randomState == 1)
        {
            enemyAnim.SetTrigger("AttackPlayer");
        }
    }


    public void SleepState()
    {
        if (enemyAnim.GetBool("isSleeping"))
        {
            enemyRB.velocity = sleepMovementSpeed * sleepMovementDirection;
        }
    }
   public void IdelState()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        enemyRB.velocity = idelMovementSpeed * idelMovementDirection;
    } 
   public void AttackUpNDownState()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        enemyRB.velocity = attackMovementSpeed * attackMovementDirection;
    }

    public void AttackPlayerState()
    {
       
        if (!hasPlayerPositon)
        {
            FlipTowardsPlayer();
             playerPosition = player.position - transform.position;
            playerPosition.Normalize();
            hasPlayerPositon = true;
        }
        if (hasPlayerPositon)
        {
            enemyRB.velocity = attackPlayerSpeed * playerPosition;
           
        }
        

        if (isTouchingWall || isTouchingDown)
        {
            //play Slam animation
            enemyAnim.SetTrigger("Slamed");
            enemyRB.velocity = Vector2.zero;
            hasPlayerPositon = false;
        }
    }

    void FlipTowardsPlayer()
    {
        float playerDirection = player.position.x - transform.position.x;

        if (playerDirection>0 && facingLeft)
        {
            Flip();
        }
        else if (playerDirection<0 && !facingLeft)
        {
            Flip();
        }
    }

    void ChangeDirection()
    {
        goingUp = !goingUp;
        idelMovementDirection.y *= -1;
        attackMovementDirection.y *= -1;
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        idelMovementDirection.x *= -1;
        attackMovementDirection.x *= -1;
        transform.Rotate(0, 180, 0);
    }
    
    public void ApplyDamage(float damage)
    {
        Debug.Log("ApplyDamage called with: " + damage);

        if (isInvincible)
        {
            Debug.Log("But boss is invincible");
            return;
        }

        currentHealth += damage;
        Debug.Log("Boss health is now: " + currentHealth);

        //enemyAnim.SetTrigger("Hit");

        StartCoroutine(HitTime());

        if (currentHealth <= 0)
        {
            StartCoroutine(OpenGates());
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enemyAnim.GetBool("isSleeping") && other.CompareTag("Player"))
        {
            gateL.SetActive(true);
            gateR.SetActive(true);
            gateL_Animator.Play("GateL_Open");
            gateR_Animator.Play("GateR_Open");
            Debug.Log("Player entered boss area. Wake up!");
            enemyAnim.SetBool("isSleeping", false);
            enemyAnim.SetTrigger("Idle");
            IdelState();
            Destroy(GetComponent<CircleCollider2D>());
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && currentHealth > 0)
        {
            collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(1, transform.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(goundCheckUp.position, groundCheckRadius);
        Gizmos.DrawWireSphere(goundCheckDown.position, groundCheckRadius);
        Gizmos.DrawWireSphere(goundCheckWall.position, groundCheckRadius);
    }
    
    IEnumerator HitTime()
    {
        isHitted = true;
        isInvincible = true;
        yield return new WaitForSeconds(0.2f);
        isHitted = false;
        isInvincible = false;
    }

    IEnumerator OpenGates()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
        gateL_Animator.Play("GateL_Close");
        gateR_Animator.Play("GateR_Close");
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Boss defeated!");
        gateL.SetActive(false);
        gateR.SetActive(false);
        enemyRB.velocity = Vector2.zero;
        this.enabled = false;
        hasBeenKilled = true;
        portal.SetActive(true);
    }
    
}
