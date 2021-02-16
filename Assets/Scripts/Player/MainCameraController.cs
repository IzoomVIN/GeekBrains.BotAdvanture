using UnityEngine;

namespace Player
{
    public class MainCameraController : MonoBehaviour
    {
    
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject scripts;
    
        private Transform _myTransform;
        private PlayerManager _manager;

        private void Start()
        {
            _myTransform = transform;
            _manager = scripts.GetComponent<PlayerManager>();
        }

        void FixedUpdate()
        {
            Vector3 playerPosition = player.transform.position;
        
            Quaternion rotX = Quaternion.AngleAxis(_manager.MouseX, Vector3.up);
            Quaternion rotY = Quaternion.AngleAxis(-_manager.MouseY, Vector3.right);

            _myTransform.rotation = _manager.StartRotation * rotX * rotY;
            _myTransform.position = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
        }
    }
}