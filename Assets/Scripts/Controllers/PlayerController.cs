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

    PlayerState _state = PlayerState.Idle;

    public enum CursorType
    {
        None,
        Attack,
        Hand
    }

    CursorType _cursorType = CursorType.None;
    Texture2D _attackIcon;
    Texture2D _handIcon;

    // Start is called before the first frame update
    void Start()
    {
        _stat = GetComponent<PlayerStat>();

        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        _handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");

        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
    }

    void UpdateIdle()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0);
    }

    void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;

        //정확히 0이 나오지 않을 경우가 있음. 매우 작은값으로 도착여부 판정
        if (dir.magnitude < 0.1f)
        {
            _state = PlayerState.Idle;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            nma.Move(moveDist * dir.normalized);

            //배꼽위치에서 레이를 쏨
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if(Input.GetMouseButton(0) == false)
                    _state = PlayerState.Idle;

                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        // 애니메이션
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", _stat.MoveSpeed);
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
        UpdateMouseCursor();

        switch (_state)
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
            default:
                break;
        }
    }

    void UpdateMouseCursor()
    {
        if (Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _mask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                if (_cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
                    _cursorType = CursorType.Attack;
                }
            }
            else
            {
                if(_cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);
                    _cursorType = CursorType.Hand;
                }
            }
        }
    }

    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    GameObject _lockTarget;
    void OnMouseEvent(Define.MouseEvent @event)
    {
        //UI가 클릭되어 있는 상태인지 확인
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (_state == PlayerState.Die)
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
                        _state = PlayerState.Moving;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            _lockTarget = hit.collider.gameObject;
                        else
                            _lockTarget = null;
                    }
                }
                break;
            case Define.MouseEvent.PointerUp:
                _lockTarget = null;
                break;
            case Define.MouseEvent.Click:
                break;
            default:
                break;
        }
    }

    void OnRunEvent(string seq)
    {
        Debug.Log($"뚜벅 뚜벅 string {seq}");
    }

    void OnRunEvent(int seq)
    {
        Debug.Log($"뚜벅 뚜벅 int {seq}");
    }


}
