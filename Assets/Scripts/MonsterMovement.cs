using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); //  клавиши

        if (moveX > 0)
        {
            animator.SetBool("isFacingRight", true);
            spriteRenderer.flipX = true;
        }
        else if (moveX < 0)
        {
            animator.SetBool("isFacingRight", false);
            spriteRenderer.flipX = false;
        }
    }
}
