using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class Enemy : MonoBehaviour {

	public CharacterController2D controller;
	public float life = 10;
	private bool isPlat;
	private bool isObstacle;
	private Transform fallCheck;
	private Transform wallCheck;
	public LayerMask turnLayerMask;
	private Rigidbody2D rb;


	public float distance = 1f;
	public LayerMask boxMask;
	private GameObject box;
	

	private bool facingRight = true;
	
	public float speed = 5f;

	public bool isInvincible = false;
	private bool isHitted = false;
	
	//Loot Table
	[Header("Loot")] public List<LootItem> lootTable = new List<LootItem>();

	void Awake () {
		fallCheck = transform.Find("FallCheck");
		wallCheck = transform.Find("WallCheck");
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up * transform.localScale.y, distance, boxMask );
		
		if (hit.collider != null && hit.collider.gameObject.tag == "Pushable")
		{
			box = hit.collider.gameObject;
			ApplyDamage(4);
		}
	}
	
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
        
		Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.down * transform.localScale.x * distance);
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (life <= 0) {
			transform.GetComponent<Animator>().SetBool("IsDead", true);
			StartCoroutine(DestroyEnemy());
		}

		isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Ground"));
		isObstacle = Physics2D.OverlapCircle(wallCheck.position, .2f, turnLayerMask);

		if (!isHitted && life > 0 && Mathf.Abs(rb.velocity.y) < 0.5f)
		{
			if (isPlat && !isObstacle && !isHitted)
			{
				if (facingRight)
				{
					rb.velocity = new Vector2(-speed, rb.velocity.y);
				}
				else
				{
					rb.velocity = new Vector2(speed, rb.velocity.y);
				}
			}
			else
			{
				Flip();
			}
		}
	}

	void Flip (){
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void ApplyDamage(int damage) {
		if (!isInvincible) 
		{
			float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
			transform.GetComponent<Animator>().SetBool("Hit", true);
			life -= damage;
			controller.Mana += controller.manaGain;
			rb.velocity = Vector2.zero;
			rb.AddForce(new Vector2(direction * 500f, 100f));
			StartCoroutine(HitTime());
		}
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" && life > 0)
		{
			collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(1, transform.position);
		}
	}

	IEnumerator HitTime()
	{
		isHitted = true;
		isInvincible = true;
		yield return new WaitForSeconds(0.1f);
		isHitted = false;
		isInvincible = false;
	}

	IEnumerator DestroyEnemy()
	{
		CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
		capsule.size = new Vector2(1f, 0.25f);
		capsule.offset = new Vector2(0f, -0.8f);
		capsule.direction = CapsuleDirection2D.Horizontal;
		yield return new WaitForSeconds(0.25f);
		rb.velocity = new Vector2(0, rb.velocity.y);
		yield return new WaitForSeconds(0.2f);
		Die();
		
	}

	public void Die()
	{
		foreach (LootItem lootItem in lootTable)
		{
			if (Random.Range(0f, 100f) <= lootItem.dropChance)
			{
				InstantiateLoot(lootItem.itemPrefab);
				break;	
			}
			
		}
		Destroy(gameObject);
	}

	void InstantiateLoot(GameObject loot)
	{
		if (loot)
		{
			GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);
			
			droppedLoot.GetComponent<SpriteRenderer>().color = Color.red;
		}
	}
}
