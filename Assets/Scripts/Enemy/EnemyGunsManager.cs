using UnityEngine;

namespace Enemy
{
    public class EnemyGunsManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] gunObjects;
        [SerializeField] private AnimationClip gunClip;

        private EnemyGunController[] _guns;
        private float _attackDelta;

        void Awake()
        {
            InitGuns();
            InitAttackDelta();
        }

        private void InitGuns()
        {
            _guns = new EnemyGunController[gunObjects.Length];

            for (int i = 0; i < gunObjects.Length; i++)
            {
                _guns[i] = gunObjects[i].GetComponent<EnemyGunController>();
            }
        }

        private void InitAttackDelta()
        {
            _attackDelta = gunClip.length/_guns.Length;
        }
        void Update()
        {
            Attack();
        }

        private void Attack()
        {
            foreach (var gun in _guns)
            {
                if (gun.IsLive) gun.AttackAnimate(true);
            }
        }
    }
}
