using UnityEngine;

[RequireComponent(
    typeof(Rigidbody), 
    typeof(Collider), 
    typeof(Animator))]
public class PlayerControllerOld : MonoBehaviour
{
    [SerializeField] private float jumpSpeed = 200;
    [SerializeField] private float runCoefficient = 2f;
    
    private Rigidbody _rigidBody;
    private Quaternion _rotation;
    private Animator _animator;
    private Vector3 _movement;
    private float _horizontalMove, _verticalMove, _jump, _run, _attack;
    private bool _onPlace;
    private const string IsMoveName = "isMove", JumpName = "isJump", 
        MoveSpeedName = "MoveSpeed", IsAttackName = "isAttack"; 
    
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        _rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        _verticalMove = GetInt(Input.GetAxis("Vertical"));
        _horizontalMove = GetInt(Input.GetAxis("Horizontal"));
        _run = GetInt(Input.GetAxis("Run"));
        _jump = GetInt(Input.GetAxis("Jump"));
        _attack = GetInt(Input.GetAxis("Fire1"));
        
    }

    private void FixedUpdate()
    {
        // AttackLogic();
        if (_onPlace)
        {
            MovementLogic();
            // JumpLogic();
        }
    }

    private void AttackLogic()
    {
        _animator.SetBool(IsAttackName, _attack > 0);
    }
    
    private void JumpLogic()
    {
        bool jump = _jump != 0;
        _animator.SetBool(JumpName, jump);

        if (jump)
        {
            Vector3 jumpForce = Vector3.up * jumpSpeed;
            _rigidBody.AddForce(jumpForce);
        }
    }

    private void MovementLogic()
    {
        if (_horizontalMove != 0f || _verticalMove != 0f)
        {
            _rotation = CameraControllerOld.Instance.rotationForPlayer;
            
            _movement.Set(_horizontalMove, 0f, _verticalMove);
            _movement.Normalize ();
        }

        var animSpeed = _run != 0f ? runCoefficient : 1;
        _animator.SetFloat(MoveSpeedName, animSpeed);

        SetBoolMoveAnimation();
    }

    private void RotateLogic(Quaternion rotation)
    {
        _rigidBody.rotation = rotation;
    }

    private void SetBoolMoveAnimation()
    {
        var animMove = (_horizontalMove != 0f || _verticalMove != 0f);
        _animator.SetBool(IsMoveName, animMove);
    }

    private static int GetInt(float number) => number < 0 ? -1 : number == 0 ? 0 : 1;

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Place"))
            _onPlace = true;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Place"))
            _onPlace = false;
    }
    
    void OnAnimatorMove ()
    {
        _rigidBody.MovePosition (_rigidBody.position + _rotation * _movement * _animator.deltaPosition.magnitude);
        _rigidBody.MoveRotation (_rotation);
    }
}
