using UnityEngine;

public class MouseMovement : MonoBehaviour
{

    private Rigidbody2D rb2d;
    private Animator animator;
    public SpriteRenderer spriteRenderer;
    public float speed = 5;

    
    void Start()
    {
    }
    private void Awake()
    {
   
        animator = GetComponent<Animator>();
        rb2d=GetComponent<Rigidbody2D>();   
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        rb2d.linearVelocity = new Vector3(x*speed,0,0);
        animator.SetBool("Move", x != 0);
        spriteRenderer.flipX = x < 0;
    }
}
