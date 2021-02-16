using System;
using Pool;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Animator))]
    public class EnemyGunController : MonoBehaviour
    {
        [SerializeField] private GameObject bulletStartPoint;
        [SerializeField] private string attackBoolName;
        [SerializeField] private float deadTimeInSecond;

        private float _deadTime;
        private Vector3 _startPosition;
        private Quaternion _startRotation;
        private bool _deadFlag;

        public bool IsLive { get; private set; }
        private PoolManager _pool;
        private const string BulletTag = "BulletEnemy";
        
        private Animator _animator;

        private void OnEnable()
        {
            // _animator.StartPlayback();
            transform.SetPositionAndRotation(_startPosition, _startRotation);
            
            _deadTime = deadTimeInSecond;
            _animator.enabled = true;
            IsLive = true;
            _deadFlag = true;
        }

        // Start is called before the first frame update
        void Awake()
        {
            var trans = transform;
            _startPosition = trans.position;
            _startRotation = trans.rotation;
            
            _animator = GetComponent<Animator>();
            _pool = PoolManager.Instance;
        }

        private void AttackEvent()
        {
            var point = bulletStartPoint.transform;
            var bullet = _pool.GetElementByTag(BulletTag);

            bullet.transform.SetPositionAndRotation(point.position, point.rotation);
            bullet.SetActive(true);
        }

        public void AttackAnimate(bool isAttack)
        {
            _animator.SetBool(attackBoolName, isAttack);
        }

        public void Dead()
        {
            if (_deadFlag)
            {
                _deadFlag = false;
                IsLive = false;
                _animator.enabled = false;
                gameObject.AddComponent<Rigidbody>();
            }

            _deadTime -= Time.deltaTime;
            if(_deadTime <= 0) 
                Destroy(gameObject);
        }
    }
}
