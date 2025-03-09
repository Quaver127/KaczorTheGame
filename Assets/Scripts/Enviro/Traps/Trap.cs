using UnityEngine;
using UnityEngine.TextCore.Text;

public class Trap : MonoBehaviour
{
   
    public float bounceForce = 20f;  // Adjust as needed
    public int damage = 1;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered trap");
            HandlePlayerBounce(collision.gameObject);
            
        }
    }

    private void HandlePlayerBounce(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Debug.Log("Applying bounce force!");
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.velocity = new Vector2(rb.velocity.x, bounceForce);
        }
        else
        {
            Debug.LogError("No Rigidbody2D found on Player!");
        }
    }
}