using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Collections.Generic;


public class CharacterController2D : MonoBehaviour
{
	public PauseMenu pauseCheck;
	
	[SerializeField] private float m_JumpForce = 400f;							
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	
	[SerializeField] private bool m_AirControl = false;							
	[SerializeField] private LayerMask m_WhatIsGround;							
	[SerializeField] private Transform m_GroundCheck;							
	[SerializeField] private Transform m_WallCheck;								

	const float k_GroundedRadius = .2f; 
	private bool m_Grounded;            
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  
	private Vector3 velocity = Vector3.zero;
	private float limitFallSpeed = 25f;

	[Separator(5,5)]
	
	[ColoredHeader("Checks", "#FFFFFF", "#000000", -2.5f, 0f)]
	public bool canWallJump = true;
	public bool canDoubleJump = true; 
	[SerializeField] private float m_DashForce = 25f;
	private bool canDash = true;
	private bool isDashing = false; 
	private bool m_IsWall = false; 
	private bool isWallSliding = false; 
	private bool oldWallSlidding = false; 
	private float prevVelocityX = 0f;
	private bool canCheck = false; 
	
	[Separator(5,5)]
	
	
	public static bool dashUnlocked = true;
	public static bool doubleJumpUnlocked = true;
	public static bool wallJumpUnlocked = true;
	
	[Separator(5,5)]
	
	[ColoredHeader("Life Settings", "#FFFFFF", "#B72828",-2.5f, 0f)]
	public HealthUI healthUI;
	public int life = 3;
	public int currentHealth;
	public bool invincible = false; 
	
	[Separator(5,5)]
	
	[Header("Healing")]
	private bool isHealingTriggered = false;
	public bool healing;
	private float healTimer;
	[SerializeField] float timeToHeal;
	
	private AnimatorOverrideController overrideController;
	
	[Separator(5,5)]
	
	[ColoredHeader("Taunt Settings", "#FFFFFF", "#397048",-2.5f, 0f)] 
	public Animator Duck;
	public AudioSource audioSource;
	public AudioClip clip;
	public List<AnimationClip> tauntClips;
	[SerializeField] int totalTaunts = 15;
	private bool isTaunting = false;
	public bool canTaunt = true;
	
	[Separator(5,5)]
	
	[ColoredHeader("Mana Settings", "#FFFFFF", "#0000CC",-2.5f, 0f)]
	[SerializeField] Image manaStorage;
	[SerializeField] public float mana;
	[SerializeField] private float manaDrainSpeed;
	[SerializeField] public float manaGain;
	
	private Animator animator;
	public ParticleSystem particleJumpUp; 
	public ParticleSystem particleJumpDown; 
	public bool canMove = true;
	private float jumpWallStartX = 0;
	private float jumpWallDistX = 0; 
	private bool limitVelOnWallJump = false; 
	
	[Header("Events")]
	[Space]

	public UnityEvent OnFallEvent;
	public UnityEvent OnLandEvent;

	[Separator(5,5)]
	[Header("Misc Objects")] 
	public AudioSource fallSound;
	

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
			audioSource.ignoreListenerPause = true;

			m_Rigidbody2D = GetComponent<Rigidbody2D>();
			animator = GetComponent<Animator>();

			if (OnFallEvent == null)
				OnFallEvent = new UnityEvent();

			if (OnLandEvent == null)
				OnLandEvent = new UnityEvent();
		
	}

	private void Start()
	{
		overrideController = new AnimatorOverrideController(Duck.runtimeAnimatorController);
		Duck.runtimeAnimatorController = overrideController;
		
		currentHealth = life;
		healthUI.SetMaxHearts(life);

		Mana = mana;
		
		manaStorage.fillAmount = Mana;
	}

	private void Update()
	{
		
		if (Input.GetKeyDown(KeyCode.T) && m_Grounded && canTaunt)
		{
			StartCoroutine(TauntCooldown());
		}
		
		Heal();

		if (pauseCheck.isPaused)
		{
			AudioListener.pause = true;
			animator.SetBool("canMove", false);
		}
		else
		{
			AudioListener.pause = false;
			animator.SetBool("canMove", true);
			
		}
	}
	

	private void FixedUpdate()
	{
		
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;

				if (!wasGrounded)
				{
					OnLandEvent.Invoke();
					
					if (m_Rigidbody2D.velocity.y < -0.1f && !m_IsWall && !isDashing)
					{
						fallSound.Play();
					}

					if (!m_IsWall && !isDashing) 
						particleJumpDown.Play();

					if (doubleJumpUnlocked)
					{
						canDoubleJump = true;
					}

					if (m_Rigidbody2D.velocity.y < 0f)
						limitVelOnWallJump = false;
				}
			}
		}

		m_IsWall = false;

		if (!m_Grounded)
		{
			OnFallEvent.Invoke();
			Collider2D[] collidersWall = Physics2D.OverlapCircleAll(m_WallCheck.position, k_GroundedRadius, m_WhatIsGround);
			for (int i = 0; i < collidersWall.Length; i++)
			{
				if (collidersWall[i].gameObject != null)
				{
					isDashing = false;
					m_IsWall = true;
				}
			}
			prevVelocityX = m_Rigidbody2D.velocity.x;
		}

		if (limitVelOnWallJump)
		{
			if (m_Rigidbody2D.velocity.y < -0.5f)
				limitVelOnWallJump = false;
			jumpWallDistX = (jumpWallStartX - transform.position.x) * transform.localScale.x;
			if (jumpWallDistX < -0.5f && jumpWallDistX > -1f) 
			{
				canMove = true;
			}
			else if (jumpWallDistX < -1f && jumpWallDistX >= -2f) 
			{
				canMove = true;
				m_Rigidbody2D.velocity = new Vector2(10f * transform.localScale.x, m_Rigidbody2D.velocity.y);
			}
			else if (jumpWallDistX < -2f) 
			{
				limitVelOnWallJump = false;
				m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
			}
			else if (jumpWallDistX > 0) 
			{
				limitVelOnWallJump = false;
				m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
			}
		}
	}

	public void Heal()
	{
		bool canHeal =
			Input.GetKey(KeyCode.F)
			&& currentHealth < life
			&& Mana > 0
			&& !animator.GetBool("IsAttacking")
			&& !animator.GetBool("IsJumping")
			&& !animator.GetBool("IsDashing")
			&& !animator.GetBool("IsDoubleJumping")
			&& !animator.GetBool("IsWallSliding")
			&& Mathf.Abs(m_Rigidbody2D.velocity.x) < 0.01f
			&& Mathf.Abs(m_Rigidbody2D.velocity.y) < 0.01f;

		if (canHeal)
		{
			animator.SetBool("IsHealing", true); 

			healing = true;
			healTimer += Time.deltaTime;

			if (healTimer >= timeToHeal)
			{
				currentHealth++;
				healTimer = 0;
				healthUI.UpdateHearts(currentHealth);
			}

			Mana -= Time.deltaTime * manaDrainSpeed;
		}
		else
		{
			animator.SetBool("IsHealing", false); 
			healing = false;
			healTimer = 0;
		}
		
		bool wasHealing = animator.GetBool("IsHealing");
		animator.SetBool("IsHealing", canHeal);

		if (canHeal && !wasHealing)
		{
			Debug.Log("Healing started");
		}
	}
	
	public void PlayRandomTaunt()
	{
		if (tauntClips == null || tauntClips.Count == 0) return;

		int index = Random.Range(0, tauntClips.Count);
     
		audioSource.PlayOneShot(clip);
		overrideController["Taunt"] = tauntClips[index];
        
		Duck.SetTrigger("Taunt");
	}
	public void Move(float move, bool jump, bool dash)
	{
		if (canMove) {
			
			if (dash && canDash && !isWallSliding && dashUnlocked)
			{
				StartCoroutine(DashCooldown());
			}
			
			if (isDashing)
			{
				m_Rigidbody2D.velocity = new Vector2(transform.localScale.x * m_DashForce, 0);
			}
			
			else if (m_Grounded || m_AirControl)
			{
				if (m_Rigidbody2D.velocity.y < -limitFallSpeed)
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -limitFallSpeed);
				
				Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
				
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

				
				if (move > 0 && !m_FacingRight && !isWallSliding)
				{
					
					Flip();
				}
				
				else if (move < 0 && m_FacingRight && !isWallSliding)
				{
					
					Flip();
				}
			}
			
			if (m_Grounded && jump)
			{
				
				animator.SetBool("IsJumping", true);
				animator.SetBool("JumpUp", true);
				m_Grounded = false;
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
				if (doubleJumpUnlocked == true)
				{
					canDoubleJump = true;	
				}
				
				particleJumpDown.Play();
				particleJumpUp.Play();
			}
			else if (!m_Grounded && jump && canDoubleJump && !isWallSliding )
			{
				canDoubleJump = false;
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce / 1.2f));
				animator.SetBool("IsDoubleJumping", true);
			}

			else if (m_IsWall && !m_Grounded && wallJumpUnlocked)
			{
				if (!oldWallSlidding && m_Rigidbody2D.velocity.y < 0 || isDashing)
				{
					isWallSliding = true;
					m_WallCheck.localPosition = new Vector3(-m_WallCheck.localPosition.x, m_WallCheck.localPosition.y, 0);
					Flip();
					StartCoroutine(WaitToCheck(0.1f));
					if (doubleJumpUnlocked)
					{
						canDoubleJump = true;
					}
					animator.SetBool("IsWallSliding", true);
				}
				isDashing = false;

				if (isWallSliding)
				{
					if (move * transform.localScale.x > 0.1f)
					{
						StartCoroutine(WaitToEndSliding());
					}
					else 
					{
						oldWallSlidding = true;
						m_Rigidbody2D.velocity = new Vector2(-transform.localScale.x * 2, -5);
					}
				}

				if (jump && isWallSliding)
				{
					animator.SetBool("IsJumping", true);
					animator.SetBool("JumpUp", true); 
					m_Rigidbody2D.velocity = new Vector2(0f, 0f);
					m_Rigidbody2D.AddForce(new Vector2(transform.localScale.x * m_JumpForce *1.2f, m_JumpForce));
					jumpWallStartX = transform.position.x;
					limitVelOnWallJump = true;
					if (doubleJumpUnlocked)
					{
						canDoubleJump = true;	
					}
					isWallSliding = false;
					animator.SetBool("IsWallSliding", false);
					oldWallSlidding = false;
					m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
					canMove = false;
				}
				else if (dash && canDash)
				{
					isWallSliding = false;
					animator.SetBool("IsWallSliding", false);
					oldWallSlidding = false;
					m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
					if (doubleJumpUnlocked)
					{ 
						canDoubleJump = true;
					}
					StartCoroutine(DashCooldown());
				}
			}
			else if (isWallSliding && !m_IsWall && canCheck) 
			{
				isWallSliding = false;
				animator.SetBool("IsWallSliding", false);
				oldWallSlidding = false;
				m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
				if (doubleJumpUnlocked)
				{
					canDoubleJump = true;
				}
			}
		}
	}


	private void Flip()
	{
		
		m_FacingRight = !m_FacingRight;

		
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Coin"))
		{
			Destroy(other.gameObject);
		}
		else if (other.gameObject.CompareTag("Diamond"))
		{
			Destroy(other.gameObject);
		}
		
		Trap trap = other.GetComponent<Trap>();
		if (trap&& trap.damage > 0)
		{
			if (!invincible)
			{
				TakeSpikeDamage(trap.damage);
				if (currentHealth <= 0)
				{
					StartCoroutine(WaitToDead());
				}
				else 
				{
					StartCoroutine(MakeInvincible(0.5f));
				}	
			}
			
		}
	}

	public float Mana
	{
		get { return mana; }
		set
		{
			if (mana != value)
			{
				mana = Mathf.Clamp(value, 0f, 1f);
				manaStorage.fillAmount = Mana;
			}
		}
	}

	private void TakeSpikeDamage(int damage)
	{
		animator.SetBool("Hit", true);
		currentHealth -= damage;
		healthUI.UpdateHearts(currentHealth);
	}
	public void ApplyDamage(int damage, Vector3 position) 
	{
		if (!invincible)
		{
			animator.SetBool("Hit", true);
			
			currentHealth -= damage;
			healthUI.UpdateHearts(currentHealth);
			
			Vector2 damageDir = Vector3.Normalize(transform.position - position) * 40f ;
			m_Rigidbody2D.velocity = Vector2.zero;
			m_Rigidbody2D.AddForce(damageDir * 10);
			if (currentHealth <= 0)
			{
				StartCoroutine(WaitToDead());
			}
			else
			{
				StartCoroutine(Stun(0.25f));
				StartCoroutine(MakeInvincible(1f));
			}
		}
	}
	
	IEnumerator DashCooldown()
	{
		animator.SetBool("IsDashing", true);
		isDashing = true;
		canDash = false;
		yield return new WaitForSeconds(0.1f);
		isDashing = false;
		yield return new WaitForSeconds(0.5f);
		canDash = true;
	}
	
	IEnumerator Stun(float time) 
	{
		canMove = false;
		yield return new WaitForSeconds(time);
		canMove = true;
	}

	IEnumerator TauntCooldown()
	{
		audioSource.pitch = 1;
		PlayRandomTaunt();
		m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
		canMove = false;
		invincible = true;
		isTaunting = true;
		canTaunt = false;
		yield return new WaitForSeconds(0.45f);
		isTaunting = false;
		m_Rigidbody2D.constraints = RigidbodyConstraints2D.None;
		yield return new WaitForSeconds(0.01f);
		m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		canMove = true;
		yield return new WaitForSeconds(0.2f);
		invincible = false;
		//yield return new WaitForSeconds(0.1f);
		canTaunt = true;
	}
	
	IEnumerator MakeInvincible(float time) 
	{
		invincible = true;
		yield return new WaitForSeconds(time);
		invincible = false;
	}
	IEnumerator WaitToMove(float time)
	{
		canMove = false;
		yield return new WaitForSeconds(time);
		canMove = true;
	}

	IEnumerator WaitToCheck(float time)
	{
		canCheck = false;
		yield return new WaitForSeconds(time);
		canCheck = true;
	}

	IEnumerator WaitToEndSliding()
	{
		yield return new WaitForSeconds(0.1f);
		if (doubleJumpUnlocked)
		{
			canDoubleJump = true;
		}
		isWallSliding = false;
		animator.SetBool("IsWallSliding", false);
		oldWallSlidding = false;
		m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
	}

	IEnumerator WaitToDead()
	{
		//m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
		canMove = false;
		animator.SetBool("IsDead", true);
		invincible = true;
		GetComponent<Attack>().enabled = false;
		yield return new WaitForSeconds(0.4f);
		m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
		yield return new WaitForSeconds(1.1f);
		SceneManager.LoadSceneAsync("MainWorld");
	}
}