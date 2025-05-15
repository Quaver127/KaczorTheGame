using UnityEngine;

public class PlayTest : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("AnimDuck");  // Exact name of your clip
    }

    void OnEnable()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        // Reset and trigger animation when re-enabled
        animator.ResetTrigger("playAnim"); // optional safety
        animator.SetTrigger("playAnim");
    }
}