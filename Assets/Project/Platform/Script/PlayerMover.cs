using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] float movePower;
    [SerializeField] float maxSpeed;
    [SerializeField] float maxFallSpeed;
    [SerializeField] float jumpPower;

    [SerializeField] Animator animator;

    [SerializeField] float x; //  �¿� �� Ȯ�ο� 

    [SerializeField] SpriteRenderer spriteRenderer;


    private static int idel = Animator.StringToHash("Idel");
    private static int jump = Animator.StringToHash("Jump");
    private static int run = Animator.StringToHash("Run");
    private static int fall = Animator.StringToHash("Fall");

    private int CurAnim;

    private bool isGrounded = false;

    private void Update()
    {
        GroundCheck();
        AnimPlayer();
        x = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

    }
    private void FixedUpdate()
    {
        Move();

    }
    private void Move()
    {
        // �������� ������������ ���� �ް� ���� ������ ���� �پ����� �� ���� ��Ÿ�� ?
        rb2d.AddForce(Vector2.right * x * movePower, ForceMode2D.Force);
        if (rb2d.velocity.x > maxSpeed)  // ������  �ְ�ӵ� ����
        {

            rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
        }
        else if (rb2d.velocity.x < -maxSpeed)  // ���� �ְ�ӵ� ����
        {
            rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
        }
        if (rb2d.velocity.y < -maxFallSpeed) // �������� �ӵ� 
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x * 10f, -maxFallSpeed);
        }



        if (x < 0) // �÷��̾� ��������Ʈ ������
        {

            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

    }
    private void Jump()
    {
        if (isGrounded == false)
        {
            return;
        }
        else
        {
            rb2d.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }


    }
    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.2f);

        if (hit.collider != null) // �ó׸ӽſ� ������ �ݶ��̴��� �����ϴ� ���� 
        {
            isGrounded = true;
            Debug.Log("��������");

        }
        else
        {
            isGrounded = false;
            Debug.Log("�Ұ���");
        }
    }

    private void AnimPlayer()
    {
        int temp;
        if (rb2d.velocity.y > 0.01f)  // Ʈ������ ��� 
        {
            temp = jump;
           // animator.Play(jump);
        }
        else if (rb2d.velocity.y < -0.01f)
        {
            temp = fall;
           // animator.Play(fall);
        }
        else if (rb2d.velocity.sqrMagnitude < 0.01f)
        {   
            temp = idel;
          //  animator.Play(idel);
        }
        else
        {   
            temp = run;
          //  animator.Play(run);
        }

        if (CurAnim != temp) 
        {   
            CurAnim = temp;
            animator.Play(CurAnim);
        }
    }

}
