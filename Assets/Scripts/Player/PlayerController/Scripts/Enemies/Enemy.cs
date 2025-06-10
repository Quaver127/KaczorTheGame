using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public CharacterController2D controller;
    public float life = 10;

    private Rigidbody2D rb;
    private bool facingRight = true;

    public float speed = 5f;
    public bool isInvincible = false;
    private bool isHitted = false;

    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;

    private Transform currentTarget;

    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = pointB; // Start moving toward point B
    }

    void FixedUpdate()
    {
        if (life <= 0)
        {
            GetComponent<Animator>().SetBool("IsDead", true);
            StartCoroutine(DestroyEnemy());
            return;
        }

        if (!isHitted && life > 0 && Mathf.Abs(rb.velocity.y) < 0.5f)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        float direction = currentTarget.position.x - transform.position.x;
        rb.velocity = new Vector2(Mathf.Sign(direction) * speed, rb.velocity.y);

        // Flip sprite if needed
        if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
        {
            Flip();
        }

        // Switch target if close
        if (Mathf.Abs(direction) < 0.2f)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void ApplyDamage(int damage)
    {
        if (!isInvincible)
        {
            float direction = damage / Mathf.Abs(damage);
            damage = Mathf.Abs(damage);
            GetComponent<Animator>().SetBool("Hit", true);
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
