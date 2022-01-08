using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;

    //deklrasi animasi
    private Animator anim;

    //deklarasi agar bisa flip
    private float horizontalInput;

    private void Awake()
    {
        // Mengambil referensi untuk rigidbody dan animator dari objek  
        body = GetComponent<Rigidbody2D>();

        // Proses sekali diawal permainan
        anim = GetComponent<Animator>();

        boxCollider = GetComponent<BoxCollider2D>();
    }


    private void Update()
    {  
        //Memeriksa input horizontal untuk mementukan animasi bergerak kekiri dan kekanan
        horizontalInput = Input.GetAxis("Horizontal");

        // Agar dapat melompat dengan mempertahankan kecepatan horizontal
        
        
        //Flip player ketika bergerak ke kiri atau kekanan
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(1,1,1); //TransformScale
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1,1,1);

        //Set animator parameter
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //Wall Jump Logic
        if (wallJumpCooldown < 0.2f)
        {
           
            // Untuk bisa berjalan ke kiri dan ke kanan
            body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 7;

            if (Input.GetKey(KeyCode.Space))
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;

    }

    private void Jump()
    {
        if(isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if(onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 15, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.y);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 6, 14);

            wallJumpCooldown = 0;
            
        }
        
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
