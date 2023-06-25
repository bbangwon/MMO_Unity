using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 10f;
    Vector3 _destPos;

    public enum PlayerState
    {
        Idle,
        Moving,
        Die
    }

    PlayerState _state = PlayerState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
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
            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
            nma.Move(moveDist * dir.normalized);

            //배꼽위치에서 레이를 쏨
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                _state = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        // 애니메이션
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", _speed);
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

    void OnMouseClicked(Define.MouseEvent @event)
    {
        //UI가 클릭되어 있는 상태인지 확인
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (_state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1.0f);

        LayerMask mask = LayerMask.GetMask("Wall");
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, mask))
        {
            _destPos = hit.point;
            _state = PlayerState.Moving;
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
