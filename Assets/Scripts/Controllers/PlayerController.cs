using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    PlayerStat _stat;
    Vector3 _destPos;

    public enum PlayerState
    {
        Idle,
        Moving,
        Die,
        Skill,
    }

    [SerializeField]
    PlayerState _state = PlayerState.Idle;

    public PlayerState State
    {
        get => _state;
        set
        {
            _state = value;

            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case PlayerState.Idle:
                    anim.SetFloat("speed", 0);
                    anim.SetBool("attack", false);
                    break;
                case PlayerState.Moving:
                    anim.SetFloat("speed", _stat.MoveSpeed);
                    anim.SetBool("attack", false);
                    break;
                case PlayerState.Skill:
                    anim.SetBool("attack", true);
                    break;
                case PlayerState.Die: 
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _stat = GetComponent<PlayerStat>();

        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
    }

    void UpdateIdle()
    {
    }

    void UpdateMoving()
    {
        if(_lockTarget != null)
        {
            float distance = (_destPos - transform.position).magnitude;
            if(distance <= 1)
            {
                State = PlayerState.Skill;
                return;
            }
        }

        Vector3 dir = _destPos - transform.position;

        //Á¤È®È÷ 0ÀÌ ³ª¿ÀÁö ¾ÊÀ» °æ¿ì°¡ ÀÖÀ½. ¸Å¿ì ÀÛÀº°ªÀ¸·Î µµÂø¿©ºÎ ÆÇÁ¤
        if (dir.magnitude < 0.1f)
        {
            State = PlayerState.Idle;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            nma.Move(moveDist * dir.normalized);

            //¹è²ÅÀ§Ä¡¿¡¼­ ·¹ÀÌ¸¦ ½ô
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if(Input.GetMouseButton(0) == false)
                    State = PlayerState.Idle;

                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }

    void UpdateSkill()
    {
    }

    void OnHitEvent()
    {
        State = PlayerState.Idle;
    }

    void UpdateDie()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        //Local -> World
        //transform.TransformDirection

        //World -> Local
        //transform.InverseTransformDirection
        switch (State)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Skill:
                UpdateSkill();
                break;
            default:
                break;
        }
    }

    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    GameObject _lockTarget;
    void OnMouseEvent(Define.MouseEvent @event)
    {
        //UI°¡ Å¬¸¯µÇ¾î ÀÖ´Â »óÅÂÀÎÁö È®ÀÎ
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (State == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out RaycastHit hit, 100f, _mask);
        //Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1.0f);

        switch (@event)
        {
            case Define.MouseEvent.Press:
                {
                    if(_lockTarget != null)
                        _destPos = _lockTarget.transform.position;
                    else if(raycastHit)
                        _destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerDown:
                {
                    if(raycastHit)
                    {
                        _destPos = hit.point;
                        State = PlayerState.Moving;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            _lockTarget = hit.collider.gameObject;
                        else
                            _lockTarget = null;
                    }
                }
                break;
            default:
                break;
        }
    }

    void OnRunEvent(string seq)
    {
        Debug.Log($"¶Ñ¹÷ ¶Ñ¹÷ string {seq}");
    }

    void OnRunEvent(int seq)
    {
        Debug.Log($"¶Ñ¹÷ ¶Ñ¹÷ int {seq}");
    }


}
