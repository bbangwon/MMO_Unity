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

    // Start is called before the first frame update
    void Start()
    {
        _stat = GetComponent<PlayerStat>();

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

        //��Ȯ�� 0�� ������ ���� ��찡 ����. �ſ� ���������� �������� ����
        if (dir.magnitude < 0.1f)
        {
            _state = PlayerState.Idle;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            nma.Move(moveDist * dir.normalized);

            //�����ġ���� ���̸� ��
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                _state = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        // �ִϸ��̼�
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

    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    void OnMouseClicked(Define.MouseEvent @event)
    {
        //UI�� Ŭ���Ǿ� �ִ� �������� Ȯ��
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (_state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1.0f);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _mask))
        {
            _destPos = hit.point;
            _state = PlayerState.Moving;

            if(hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                Debug.Log("Monster Click!");
            }
            else
            {
                Debug.Log("Ground Click!");
            }
        }
    }

    void OnRunEvent(string seq)
    {
        Debug.Log($"�ѹ� �ѹ� string {seq}");
    }

    void OnRunEvent(int seq)
    {
        Debug.Log($"�ѹ� �ѹ� int {seq}");
    }


}
