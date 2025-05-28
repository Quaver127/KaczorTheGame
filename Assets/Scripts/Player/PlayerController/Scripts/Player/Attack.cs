using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public float atkRadius = 20f;
	public float dmgValue = 4;
	public GameObject throwableObject;
	public Transform attackCheck;
	private Rigidbody2D m_Rigidbody2D;
	public Animator animator;
	public bool canAttack = true;
	public bool isTimeToCheck = false;

	public float chargeTime;
	public bool isCharging = false;
	
	
	public GameObject cam;
	public GameObject damageTrigger;
	
	public GameObject attackParticles;
	private Animator attackAnimator;
	
	public GameObject chargeParticles;
	private Animator chargeAnimator;

	public AudioSource chargedScream;
	
	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
	{
		attackAnimator = attackParticles.GetComponent<Animator>();
		chargeAnimator = chargeParticles.GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update()
    {
	    
		if (Input.GetKeyDown(KeyCode.X) && canAttack)
		{
			attackParticles.SetActive(true);             
			attackAnimator.Play("AttackAnim", 0, 0f); 
			canAttack = false;
			animator.SetBool("IsAttacking", true);
			damageTrigger.SetActive(true);
			StartCoroutine(AttackCooldown());
		}

		if (Input.GetKey(KeyCode.X) && chargeTime < 2)
		{
			isCharging = true;
			if (isCharging)
			{
				chargeTime += Time.deltaTime;
			}
		}

		if (Input.GetKeyUp(KeyCode.X) && chargeTime >= 2)
		{
			chargeParticles.SetActive(true);
			damageTrigger.SetActive(true);
			attackAnimator.Play("Scream20", 0, 0f); 
			chargedScream.Play();
			isCharging = false;
			chargeTime = 0;
			StartCoroutine(ChargeDmg());
			chargeAnimator.SetBool("IsChargeAttacking", true);
			StartCoroutine(AttackCooldown());
			StartCoroutine(DmgReset());

		}
		else if (Input.GetKeyUp(KeyCode.X) && chargeTime < 2)
		{
			chargeTime = 0;
		}

		if (Input.GetKeyDown(KeyCode.V))
		{
			GameObject throwableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f,-0.2f), Quaternion.identity) as GameObject; 
			Vector2 direction = new Vector2(transform.localScale.x, 0);
			throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction; 
			throwableWeapon.name = "ThrowableWeapon";
		}
		
	}

    
	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(0.01f);
		yield return new WaitForSeconds(0.25f);
		damageTrigger.SetActive(false);
		attackParticles.SetActive(false); 
		canAttack = true;
		yield return new WaitForSeconds(0.8f);
		chargeParticles.SetActive(false);
	}

	IEnumerator ChargeDmg()
	{
		yield return new WaitForSeconds(0.01f);
		dmgValue = 8;
	}
	IEnumerator DmgReset()
	{
		yield return new WaitForSeconds(1f);
		dmgValue = 4;
	}

	public void DoDashDamage()
	{
		dmgValue = Mathf.Abs(dmgValue);
		Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, atkRadius);
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.tag == "Enemy")
			{
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					dmgValue = -dmgValue;
				}
				collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
				cam.GetComponent<CameraFollow>().ShakeCamera();
			}
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, atkRadius);
		
	}
}
