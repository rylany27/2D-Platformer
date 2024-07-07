using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        bool isWalk = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A);

        animator.SetBool("isWalk", isWalk);
        animator.SetBool("isGrounded", playerMovement.pubIsGrounded);
    }
}
