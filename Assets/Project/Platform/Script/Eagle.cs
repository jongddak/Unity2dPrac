using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    public enum State { Idle, Trace, Return, Attack, Dead, Size }  // 열거형 끝에 Size 를 두면 열거형의 크기를 체크 가능 
    private State curstate = State.Idle;

    private BaseState[] States = new BaseState[(int)State.Size];  // 열거형 크기만큼의 배열 생성 


    [SerializeField] GameObject Player;
    [SerializeField] float MoveSpeed;
    [SerializeField] Vector2 startPos;


    [SerializeField] float traceRange;
    [SerializeField] float attackRange;


    private void Awake()
    {
        States[(int)State.Idle] = new IdelState(this);   // 상태 배열 초기화 
        States[(int)State.Trace] = new TraceState(this);
        States[(int)State.Return] = new ReturnState(this);
        States[(int)State.Attack] = new AttackState(this);
        States[(int)State.Dead] = new DeadState(this);
    }

    private void Start()
    {
        startPos = transform.position;
        Player = GameObject.FindGameObjectWithTag("Player");

        States[(int)curstate].Enter();
    }

    private void OnDestroy()
    {
        States[(int)curstate].Exit();
    }
    private void Update()
    {
        States[(int)curstate].Update();   // 열거형 번호(배열기준)에 맞는 상태를 실행 , 현재 상태만 실행 
    }

    public void ChangeState(State state)   // 현재 상태에서 나가고 다른 상태에 진입 
    {
        States[(int)curstate].Exit();
        curstate = state;
        States[(int)curstate].Enter();
    }
    private class EagleState : BaseState  // 상태별 가지고 있는 공통 기능 구현용 
    {
        public Eagle eagle;

        public EagleState(Eagle eagle)
        {
            this.eagle = eagle;  // eagle을 state 가 서로 참조할 수 있게
        }
    }

    private class IdelState : EagleState   // 각각의 상태를 클래스로 만들어서 캡슐화 , 상태간 간섭을 없앨수 있고 나중에 상태를 추가해도 영향이 없음 
    {
        public IdelState(Eagle eagle) : base(eagle) // 참조용 생성자 
        {
        }
        public override void Update()
        {
          if (Vector2.Distance(eagle.transform.position, eagle.Player.transform.position) < eagle.traceRange) 
            {
                eagle.ChangeState(State.Trace);
            }
        }
        // 상속에서 버추얼로 작성해서 할게 없다면 작성 안하면 됨 

    }
 
    private class TraceState : EagleState 
    {
        private GameObject Player;
        private float MoveSpeed;
        public TraceState(Eagle eagle) : base(eagle)
        {
        }
        public override void Enter()
        {
            Player = eagle.Player;
            MoveSpeed = eagle.MoveSpeed;
        }

        public override void Update()
        {
         
           eagle.transform.position = Vector2.MoveTowards(eagle.transform.position, eagle.Player.transform.position, eagle.MoveSpeed * Time.deltaTime);

           // 다른 상태로 전환
           if (Vector2.Distance(eagle.transform.position, eagle.Player.transform.position) > eagle.traceRange)
           {
                eagle.ChangeState(State.Idle);
           }
           else if (Vector2.Distance(eagle.transform.position,eagle.Player.transform.position) < eagle.attackRange)
           {
                eagle.ChangeState(State.Attack);
           }
        }
    }
   
    private class ReturnState : EagleState 
    {
        public ReturnState(Eagle eagle) : base(eagle)
        {
        }

        public override void Enter()
        {

        }
        public override void Update()
        {

        }
        public override void Exit()
        {

        }
    }
   
    private class AttackState : EagleState 
    {
        public AttackState(Eagle eagle) : base(eagle)
        {
        }

        public override void Enter()
        {

        }
        public override void Update()
        {

        }
        public override void Exit()
        {

        }
    }
   
    private class DeadState : EagleState
    {
        public DeadState(Eagle eagle) : base(eagle)
        {
        }

        public override void Enter()
        {

        }
        public override void Update()
        {

        }
        public override void Exit()
        {

        }
    }




}
//using UnityEngine;
//using UnityEngine.UI;

//public class Eagle : MonoBehaviour
//{
//    public enum State { Idle, Trace, Return, Attack, Dead, Size }
//    [SerializeField] State curState = State.Idle;
//    private BaseState[] states = new BaseState[(int)State.Size];

//    [SerializeField] GameObject player;
//    [SerializeField] float traceRange;
//    [SerializeField] float attackRange;
//    [SerializeField] float moveSpeed;
//    [SerializeField] Vector2 startPos;

//    [SerializeField] Text stateText;

//    private void Awake()
//    {
//        states[(int)State.Idle] = new IdleState(this);
//        states[(int)State.Trace] = new TraceState(this);
//        states[(int)State.Return] = new ReturnState(this);
//        states[(int)State.Attack] = new AttackState(this);
//        states[(int)State.Dead] = new DeadState(this);
//    }

//    private void Start()
//    {
//        startPos = transform.position;
//        player = GameObject.FindGameObjectWithTag("Player");

//        states[(int)curState].Enter();
//    }

//    private void OnDestroy()
//    {
//        states[(int)curState].Exit();
//    }

//    private void Update()
//    {
//        states[(int)curState].Update();

//        stateText.text = curState.ToString();
//    }

//    public void ChangeState(State nextState)
//    {
//        states[(int)curState].Exit();
//        curState = nextState;
//        states[(int)curState].Enter();
//    }

//    private class EagleState : BaseState
//    {
//        public Eagle eagle;

//        public EagleState(Eagle eagle)
//        {
//            this.eagle = eagle;
//        }
//    }

//    private class IdleState : EagleState
//    {
//        public IdleState(Eagle eagle) : base(eagle)
//        {
//        }

//        public override void Update()
//        {
//            // Idle 행동만 구현
//            // 가만히 있기

//            // 다른 상태로 전환
//            if (Vector2.Distance(eagle.transform.position, eagle.player.transform.position) < eagle.traceRange)
//            {
//                eagle.ChangeState(State.Trace);
//            }
//        }
//    }

//    private class TraceState : EagleState
//    {
//        public TraceState(Eagle eagle) : base(eagle)
//        {
//        }

//        public override void Update()
//        {
//            // Trace 행동만 구현
//            eagle.transform.position = Vector2.MoveTowards(eagle.transform.position, eagle.player.transform.position, eagle.moveSpeed * Time.deltaTime);

//            // 다른 상태로 전환
//            if (Vector2.Distance(eagle.transform.position, eagle.player.transform.position) > eagle.traceRange)
//            {
//                eagle.ChangeState(State.Return);
//            }
//            else if (Vector2.Distance(eagle.transform.position, eagle.player.transform.position) < eagle.attackRange)
//            {
//                eagle.ChangeState(State.Attack);
//            }
//        }
//    }

//    private class ReturnState : EagleState
//    {
//        public ReturnState(Eagle eagle) : base(eagle)
//        {
//        }

//        public override void Update()
//        {
//            // Return 행동만 구현
//            eagle.transform.position = Vector2.MoveTowards(eagle.transform.position, eagle.startPos, eagle.moveSpeed * Time.deltaTime);

//            if (Vector2.Distance(eagle.transform.position, eagle.startPos) < 0.01f)
//            {
//                eagle.ChangeState(State.Idle);
//            }
//        }
//    }

//    private class AttackState : EagleState
//    {
//        public AttackState(Eagle eagle) : base(eagle)
//        {
//        }

//        public override void Update()
//        {
//            // Attack 행동만 구현
//            Debug.Log("공격");

//            if (Vector2.Distance(eagle.transform.position, eagle.player.transform.position) > eagle.attackRange)
//            {
//                eagle.ChangeState(State.Trace);
//            }
//        }
//    }

//    private class DeadState : EagleState
//    {
//        public DeadState(Eagle eagle) : base(eagle)
//        {
//        }
//    }


//    #region BaseStatePattern
//    /*
//    private void Idle()
//    {
//        // Idle 행동만 구현
//        // 가만히 있기

//        // 다른 상태로 전환
//        if (Vector2.Distance(transform.position, player.transform.position) < traceRange)
//        {
//            curState = State.Trace;
//        }
//    }

//    private void Trace()
//    {
//        // Trace 행동만 구현
//        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);

//        // 다른 상태로 전환
//        if (Vector2.Distance(transform.position, player.transform.position) > traceRange)
//        {
//            curState = State.Return;
//        }
//        else if (Vector2.Distance(transform.position, player.transform.position) < attackRange)
//        {
//            curState = State.Attack;
//        }
//    }

//    private void Return()
//    {
//        // Return 행동만 구현
//        transform.position = Vector2.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);

//        if (Vector2.Distance(transform.position, startPos) < 0.01f)
//        {
//            curState = State.Idle;
//        }
//    }

//    private void Attack()
//    {
//        // Attack 행동만 구현
//        Debug.Log("공격");

//        if (Vector2.Distance(transform.position, player.transform.position) > attackRange)
//        {
//            curState = State.Trace;
//        }
//    }

//    private void Dead()
//    {
//        // Dead 행동만 구현
//        Debug.Log("죽음");
//    }
//    */
//    #endregion
//}
