using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
public class BossEnemy : MonoBehaviour
{
    public CharacterController2D controller;
	public float life = 1;
	private bool isPlat;
	private bool isObstacle;
	private Rigidbody2D rb;

	public bool isInvincible = false;
	private bool isHitted = false;
	
	//Loot Table
	[Header("Loot")] public List<LootItem> lootTable = new List<LootItem>();

	void Awake () {
		
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (life <= 0) {
			transform.GetComponent<Animator>().SetBool("IsDead", true);
			StartCoroutine(DestroyEnemy());
		}
		
	}
	
	public void ApplyDamage(int damage) {
		if (!isInvincible) 
		{
			damage = Mathf.Abs(damage);
			life -= damage;
			controller.Mana += controller.manaGain;
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
