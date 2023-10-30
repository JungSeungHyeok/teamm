using UnityEngine;

public class FixedUpdateColliders : MonoBehaviour
{
    private Animator animator;
    private Collider myCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        if (animator != null && animator.HasState(0, Animator.StringToHash("Obstacle-29")))
        {
            animator.Play("Obstacle-29");
        }

        if (animator != null && animator.HasState(0, Animator.StringToHash("Obstacle-14")))
        {
            animator.Play("Obstacle-14");
        }
    }
}
