using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    public enum State { Idle, Trace, Return, Attack, Dead, Size }  // ������ ���� Size �� �θ� �������� ũ�⸦ üũ ���� 
    private State curstate = State.Idle;

    private BaseState[] States = new BaseState[(int)State.Size];  // ������ ũ�⸸ŭ�� �迭 ���� 


    [SerializeField] GameObject Player;
    [SerializeField] float MoveSpeed;
    [SerializeField] Vector2 startPos;


    [SerializeField] float traceRange;
    [SerializeField] float attackRange;


    private void Awake()
    {
        States[(int)State.Idle] = new IdelState(this);   // ���� �迭 �ʱ�ȭ 
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
        States[(int)curstate].Update();   // ������ ��ȣ(�迭����)�� �´� ���¸� ���� , ���� ���¸� ���� 
    }

    public void ChangeState(State state)   // ���� ���¿��� ������ �ٸ� ���¿� ���� 
    {
        States[(int)curstate].Exit();
        curstate = state;
        States[(int)curstate].Enter();
    }
    private class EagleState : BaseState  // ���º� ������ �ִ� ���� ��� ������ 
    {
        public Eagle eagle;

        public EagleState(Eagle eagle)
        {
            this.eagle = eagle;  // eagle�� state �� ���� ������ �� �ְ�
        }
    }

    private class IdelState : EagleState   // ������ ���¸� Ŭ������ ���� ĸ��ȭ , ���°� ������ ���ټ� �ְ� ���߿� ���¸� �߰��ص� ������ ���� 
    {
        public IdelState(Eagle eagle) : base(eagle) // ������ ������ 
        {
        }
        public override void Update()
        {
          if (Vector2.Distance(eagle.transform.position, eagle.Player.transform.position) < eagle.traceRange) 
            {
                eagle.ChangeState(State.Trace);
            }
        }
        // ��ӿ��� ���߾�� �ۼ��ؼ� �Ұ� ���ٸ� �ۼ� ���ϸ� �� 

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

           // �ٸ� ���·� ��ȯ
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
//            // Idle �ൿ�� ����
//            // ������ �ֱ�

//            // �ٸ� ���·� ��ȯ
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
//            // Trace �ൿ�� ����
//            eagle.transform.position = Vector2.MoveTowards(eagle.transform.position, eagle.player.transform.position, eagle.moveSpeed * Time.deltaTime);

//            // �ٸ� ���·� ��ȯ
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
//            // Return �ൿ�� ����
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
//            // Attack �ൿ�� ����
//            Debug.Log("����");

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
//        // Idle �ൿ�� ����
//        // ������ �ֱ�

//        // �ٸ� ���·� ��ȯ
//        if (Vector2.Distance(transform.position, player.transform.position) < traceRange)
//        {
//            curState = State.Trace;
//        }
//    }

//    private void Trace()
//    {
//        // Trace �ൿ�� ����
//        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);

//        // �ٸ� ���·� ��ȯ
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
//        // Return �ൿ�� ����
//        transform.position = Vector2.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);

//        if (Vector2.Distance(transform.position, startPos) < 0.01f)
//        {
//            curState = State.Idle;
//        }
//    }

//    private void Attack()
//    {
//        // Attack �ൿ�� ����
//        Debug.Log("����");

//        if (Vector2.Distance(transform.position, player.transform.position) > attackRange)
//        {
//            curState = State.Trace;
//        }
//    }

//    private void Dead()
//    {
//        // Dead �ൿ�� ����
//        Debug.Log("����");
//    }
//    */
//    #endregion
//}
