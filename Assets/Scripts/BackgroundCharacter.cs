using System.Collections;
using UnityEngine;

public class BackgroundCharacter : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spr;
    public float speed = 5;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb2d=GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
    }
    float target_x;
    IEnumerator FindPlace()
    {
        yield return new WaitForSeconds(8);
        ChooseVector();
        StartCoroutine(FindPlace());
    }
    void Start()
    {
        ChooseVector();
        StartCoroutine(FindPlace());
    }
    void ChooseVector()
    {
        target_x = Random.Range(-9f, 9f);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector3(target_x,transform.position.y,0),speed*Time.deltaTime);
        animator.SetBool("Move",Vector2.Distance(transform.position, new Vector2(target_x,0))>1.8f);
        spr.flipX = transform.position.x > target_x;
    }
}
