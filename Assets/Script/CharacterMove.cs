using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float moveSpeed = 5f; // Default deðer atandý
    private Animator anim;
    private Rigidbody2D rb2d;
    private float moveHorizontal; // Sýnýf seviyesinde tanýmlandý

    public bool facingRight;
    public float JumpForce;
    public bool isGrounded;
    public bool canDoubleJump;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CharacterMovement();
        CharacterAnimation();
        CharacterAttack();
        CharacterRunAttack();
        CharacterJump();
    }

    void CharacterMovement()
    {
        moveHorizontal = Input.GetAxis("Horizontal"); // Deðer burada güncelleniyor
        rb2d.velocity = new Vector2(moveHorizontal * moveSpeed, rb2d.velocity.y);
    }

    void CharacterAnimation()
    {
        if (moveHorizontal > 0) // Deðiþkene buradan eriþiliyor
        {
            anim.SetBool("IsRunning", true);
        }
        if (moveHorizontal == 0)
        {
            anim.SetBool("IsRunning", false);
        }
        if(moveHorizontal < 0)
        {
            anim.SetBool("IsRunning", true);
        }
        if (facingRight == false && moveHorizontal > 0)
        {
            CharacterFlip();
        }
        if (facingRight == true && moveHorizontal < 0)
        {
            CharacterFlip();
        }
    }
    void CharacterFlip()
    {
        facingRight =!facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
    void CharacterAttack()
    {
        if (Input.GetKeyDown(KeyCode.E)&&moveHorizontal == 0)
        {
            anim.SetTrigger("IsAttack");
        }
    }
    void CharacterRunAttack()
    {
        if (Input.GetKeyDown(KeyCode.E)&&moveHorizontal>0||Input.GetKeyDown(KeyCode.E)&&moveHorizontal<0)
        {
            anim.SetTrigger("IsRunAttack");
        }
    }
    void CharacterJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("IsJump", true);
            if (isGrounded)
            {
                rb2d.velocity = Vector2.up * JumpForce; // Normal zýplama
                canDoubleJump = true; // Double jump yapmaya izin ver
            }
            else if (canDoubleJump)
            {
                float doubleJumpForce = JumpForce / 1.5f; // Double jump için geçici güç
                rb2d.velocity = Vector2.up * doubleJumpForce;

                canDoubleJump = false; // Double jump bitti
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        anim.SetBool("IsJump", false);

        if(col.gameObject.tag == "Grounded")
        {
            isGrounded = true;
        }
    }
    void OnCollisionStay2D(Collision2D col)
    {
        anim.SetBool("IsJump",false);

        if(col.gameObject.tag == "Grounded")
        {
            isGrounded = true;

        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        anim.SetBool("IsJump", true);

        if(col.gameObject.tag == "Grounded")
        {
            isGrounded = false;
        }
    }
}
