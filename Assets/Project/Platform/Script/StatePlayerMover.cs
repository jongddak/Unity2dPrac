using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatePlayerMover : MonoBehaviour
{
    public enum moverState { Idle, Run, Jump,Dead, Size }

    

    private BaseState[] States = new BaseState[(int)moverState.Size];

    [SerializeField] moverState curplayerState;
    [SerializeField] Slider slider;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] float movePower;
    [SerializeField] float maxSpeed;
    [SerializeField] float maxFallSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] int Hp;
     
    [SerializeField] Animator animator;

    [SerializeField] float x; //  좌우 값 확인용 
    
    [SerializeField] SpriteRenderer spriteRenderer;

    public UnityEvent OnDead;

    public UnityEvent OnEagleStep;


    private static int idel = Animator.StringToHash("Idel");
    private static int jump = Animator.StringToHash("Jump");
    private static int run = Animator.StringToHash("Run");
    private static int fall = Animator.StringToHash("Fall");

    private int CurAnim;

    private bool isGrounded = false;
    private bool underMonster = false;

    //  각 상태마다 애니메이션 재생 
    // 아이들 업데이트에선 애니메이션만 
    // 점프는 
    private void Awake()
    {
        States[(int)moverState.Idle] = new IdelState(this);
        States[(int)moverState.Run] = new RunState(this);
        States[(int)moverState.Jump] = new JumpState(this);
        
        States[(int)moverState.Dead] = new DeadState(this);

        curplayerState = moverState.Idle;

        Hp = 3;
        slider.maxValue = 3;
        slider.minValue = 0;
        slider.value = 3;

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
        stepMob();
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

        if (hit.collider != null) // 시네머신용 폴리곤 콜라이더도 감지하니 주의 
        {
            isGrounded = true;
            Debug.Log("점프가능");
            if (hit.collider.tag == "Mob")
            {
                underMonster = true;
                Debug.Log("몬스터 밟기 가능");
            }
            else 
            {
                underMonster = false;
            }

        }
        else
        {
            isGrounded = false;
            Debug.Log("불가능");
        }
 
    }

    public void stepMob() 
    {
        if (underMonster == true) 
        {
            OnEagleStep?.Invoke();
        }
    }

    private void AnimPlayer()    // 현재 상태를 매개변수로 바꿀까?
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
    public void Heal() 
    {
        Debug.Log("체력 1회복");
        Hp += 1;
        slider.value += 1;
    }
    public void TakeHit() 
    {
        Debug.Log("몬스터에게 맞음!");
        Hp -= 1;
        slider.value -= 1;
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
                player.ChangemoverState(moverState.Jump);  // 스페이스 누르면 점프상태로 변환
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
            if (player.rb2d.velocity.x > player.maxSpeed)  // 오른쪽  최고속도 제한
            {

                player.rb2d.velocity = new Vector2(player.maxSpeed, player.rb2d.velocity.y);
            }
            else if (player.rb2d.velocity.x < -player.maxSpeed)  // 왼쪽 최고속도 제한
            {
                player.rb2d.velocity = new Vector2(-player.maxSpeed, player.rb2d.velocity.y);
            }
            if (player.rb2d.velocity.y < -player.maxFallSpeed) // 떨어지는 속도 
            {
                player.rb2d.velocity = new Vector2(player.rb2d.velocity.x * 10f, -player.maxFallSpeed);
            }



            if (player.x < 0) // 플레이어 스프라이트 뒤집기
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
            
        }
    }


}
