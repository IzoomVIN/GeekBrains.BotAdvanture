using Pool;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerHeadController : MonoBehaviour
    {
        [SerializeField] private GameObject scripts;
        [SerializeField] private GameObject _bulletStartPoint;
        [Header("Rotate vertical")]
        [SerializeField] private float min = -8;
        [SerializeField] private float max = 16;

        private PlayerManager _manager;
        private Animator _animator;
        private Vector3 _startLocalPosition;
        private Quaternion _rotation;
        private PoolManager _pool;
        private string _bulletTag = "BulletPlayer";

        private const string
            AttackName = "Attack";
    
        void Start()
        {
            _manager = scripts.GetComponent<PlayerManager>();
            _animator = GetComponent<Animator>();
            _startLocalPosition = transform.localPosition;
            _pool = PoolManager.Instance;
        }

        private void Update()
        {
            if (!_manager.Attack) return;
            
            AnimatorLogic();
        }

        private void FixedUpdate()
        {
            RotationLogic();
        }

        // Calculate and set rotation of head dependent of camera direction
        private void RotationLogic()
        {
            var mouseY = Mathf.Clamp(_manager.MouseY, min, max);
        
            var rotX = Quaternion.AngleAxis(_manager.MouseX, Vector3.up);
            var rotY = Quaternion.AngleAxis(mouseY, Vector3.left);
        
        
            var quatTo = _manager.StartRotation * rotX * rotY;
            quatTo.Normalize();
        
        
            _rotation = Quaternion.Slerp(
                transform.rotation, 
                quatTo, 
                Time.fixedDeltaTime * _manager.RotationSpeed
            );

            transform.rotation = _rotation;
        }

        private void Attack()
        {
            var point = _bulletStartPoint.transform;
            var bullet = _pool.GetElementByTag(_bulletTag);

            bullet.transform.SetPositionAndRotation(point.position, point.rotation);
            bullet.SetActive(true);
        }

        private void AnimatorLogic()
        {
            _animator.ResetTrigger(AttackName);
            _animator.SetTrigger(AttackName);
        }

        private void AnimationEnd()
        {
            transform.localPosition = _startLocalPosition;
        }
    }
}
