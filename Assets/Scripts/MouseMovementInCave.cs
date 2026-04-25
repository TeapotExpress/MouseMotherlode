using UnityEngine;

public class MouseMovementInCave : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private Animator animator;
    [SerializeField] private SpriteRenderer _spr;
    public float maxSpeed = 10;
    public float speed = 10;
    public float jumpForce = 10;

    bool canJump = false;


    bool lastLook = false;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");

        if(rb2d.linearVelocity.x<maxSpeed)
            rb2d.AddForce(new Vector3(x*speed, 0, 0), ForceMode2D.Force);

        if(Input.GetKeyDown(KeyCode.Space)&&canJump)
        {
            canJump=false;
            rb2d.AddForce(new Vector3(0, jumpForce, 0),ForceMode2D.Impulse);
        }

        animator.SetBool("Move", x!=0);

        if(x!=0)
            lastLook = x < 0;

        _spr.flipX = lastLook;
    }
}
