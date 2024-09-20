using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.Events;

public class StatePlayerMover : MonoBehaviour
{
    public enum moverState { Idle, Run, Jump,Dead, Size }

    

    private BaseState[] States = new BaseState[(int)moverState.Size];

    [SerializeField] moverState curplayerState;

    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] float movePower;
    [SerializeField] float maxSpeed;
    [SerializeField] float maxFallSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] int Hp;
     
    [SerializeField] Animator animator;

    [SerializeField] float x; //  �¿� �� Ȯ�ο� 
    
    [SerializeField] SpriteRenderer spriteRenderer;

    public UnityEvent OnDead;


    private static int idel = Animator.StringToHash("Idel");
    private static int jump = Animator.StringToHash("Jump");
    private static int run = Animator.StringToHash("Run");
    private static int fall = Animator.StringToHash("Fall");

    private int CurAnim;

    private bool isGrounded = false;

    //  �� ���¸��� �ִϸ��̼� ��� 
    // ���̵� ������Ʈ���� �ִϸ��̼Ǹ� 
    // ������ 
    private void Awake()
    {
        States[(int)moverState.Idle] = new IdelState(this);
        States[(int)moverState.Run] = new RunState(this);
        States[(int)moverState.Jump] = new JumpState(this);
        
        States[(int)moverState.Dead] = new DeadState(this);

        curplayerState = moverState.Idle;

        Hp = 1;
    }
    private void Start()
    {
        States[(int)curplayerState].Enter();
    }
    private void Update()
    {
        x = Input.GetAxis("Horizontal");
        States[(int)curplayerState].Update();
        GroundCheck();
        AnimPlayer();

        if (Hp == 0) 
        {
            ChangemoverState(moverState.Dead);
        }
    }
    private void OnDestroy() 
    {
        States[(int)curplayerState].Exit();
    }
    public void ChangemoverState(moverState moverstate) 
    {
        States[(int)curplayerState].Exit();
        curplayerState =moverstate;
        States[(int)curplayerState].Enter();
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
    private void AnimPlayer()    // ���� ���¸� �Ű������� �ٲܱ�?
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
    private class PlayerState : BaseState
    {
        public StatePlayerMover player;

        public PlayerState(StatePlayerMover player)
        {
            this.player = player;   
        }
    }

    private class IdelState : PlayerState
    {
        
        public IdelState(StatePlayerMover player) : base(player)
        {
        }
        public override void Update() 
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.ChangemoverState(moverState.Jump);  // �����̽� ������ �������·� ��ȯ
            }
            else if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f) 
            {
                player.ChangemoverState(moverState.Run);
            }
           
        }
    }
    private class RunState : PlayerState
    {
        public RunState(StatePlayerMover player) : base(player)
        {
        }

        public override void Update() 
        {
            player.rb2d.AddForce(Vector2.right * player.x * player.movePower, ForceMode2D.Force);
            if (player.rb2d.velocity.x > player.maxSpeed)  // ������  �ְ�ӵ� ����
            {

                player.rb2d.velocity = new Vector2(player.maxSpeed, player.rb2d.velocity.y);
            }
            else if (player.rb2d.velocity.x < -player.maxSpeed)  // ���� �ְ�ӵ� ����
            {
                player.rb2d.velocity = new Vector2(-player.maxSpeed, player.rb2d.velocity.y);
            }
            if (player.rb2d.velocity.y < -player.maxFallSpeed) // �������� �ӵ� 
            {
                player.rb2d.velocity = new Vector2(player.rb2d.velocity.x * 10f, -player.maxFallSpeed);
            }



            if (player.x < 0) // �÷��̾� ��������Ʈ ������
            {

                player.spriteRenderer.flipX = true;
            }
            else
            {
                player.spriteRenderer.flipX = false;
            }

            if (Mathf.Abs(Input.GetAxis("Horizontal")) < 0.01f)
            {
                player.ChangemoverState(moverState.Idle);
            }
            else if (Input.GetKeyDown(KeyCode.Space)) 
            {
                player.ChangemoverState(moverState.Jump);
            }
        }

    }
    private class JumpState : PlayerState
    {
        public JumpState(StatePlayerMover player) : base(player)
        {
        }
        public override void Enter()
        {
            if (player.isGrounded == false)
            {
                player.ChangemoverState(moverState.Idle);
            }
            else
            {
                player.rb2d.AddForce(Vector2.up * player.jumpPower, ForceMode2D.Impulse);
                player.ChangemoverState(moverState.Idle);
            }
        }
    }

    private class DeadState : PlayerState
    {
        public DeadState(StatePlayerMover player) : base(player)
        {
        }
        public override void Enter() 
        {
            player.OnDead?.Invoke();
            Destroy(player.gameObject);
            // �״� �ִϸ��̼� �߰��ϸ� ������
        }
    }


}
