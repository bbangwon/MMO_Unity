using UnityEngine;

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

        //��Ȯ�� 0�� ������ ���� ��찡 ����. �ſ� ���������� �������� ����
        if (dir.magnitude < 0.0001f)
        {
            _state = PlayerState.Idle;
        }
        else
        {
            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);

            transform.position += moveDist * dir.normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        // �ִϸ��̼�
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
        if (_state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1.0f);

        LayerMask mask = LayerMask.GetMask("Wall");
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, mask))
        {
            _destPos = hit.point;
            _state = PlayerState.Moving;
        }
    }
}