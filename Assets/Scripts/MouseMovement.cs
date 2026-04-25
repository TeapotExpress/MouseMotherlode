using UnityEngine;

public class MouseMovement : MonoBehaviour
{

    //
    //Ten skrypcior jest tylko do poruszania graczem gdy nie jest w kopalni!! 
    //Podmienimy go na inny, gdy gracz zacznie kopać okii?? 
    //ඞ

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
        rb2d.linearVelocity = new Vector3(x*speed,0,0); //<- dałem zmianę velocity, bo gracz raaaaaczej nie ma z czym kolidować na tej scenie i nic go nie potrąci lool. Przy add force, movement byłby dziwny imo!!
        animator.SetBool("Move", x != 0);
        spriteRenderer.flipX = x < 0;
    }
}
