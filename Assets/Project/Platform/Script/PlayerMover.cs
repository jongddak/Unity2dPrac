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

    [SerializeField] float x; //  좌우 값 확인용 

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
        // 땅에서만 움직였을때만 힘을 받게 하지 않으면 벽에 붙어있을 수 있음 벽타기 ?
        rb2d.AddForce(Vector2.right * x * movePower, ForceMode2D.Force);
        if (rb2d.velocity.x > maxSpeed)  // 오른쪽  최고속도 제한
        {

            rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
        }
        else if (rb2d.velocity.x < -maxSpeed)  // 왼쪽 최고속도 제한
        {
            rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
        }
        if (rb2d.velocity.y < -maxFallSpeed) // 떨어지는 속도 
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x * 10f, -maxFallSpeed);
        }



        if (x < 0) // 플레이어 스프라이트 뒤집기
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

        if (hit.collider != null) // 시네머신용 폴리곤 콜라이더도 감지하니 주의 
        {
            isGrounded = true;
            Debug.Log("점프가능");

        }
        else
        {
            isGrounded = false;
            Debug.Log("불가능");
        }
    }

    private void AnimPlayer()
    {
        int temp;
        if (rb2d.velocity.y > 0.01f)  // 트랜지션 대신 
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
