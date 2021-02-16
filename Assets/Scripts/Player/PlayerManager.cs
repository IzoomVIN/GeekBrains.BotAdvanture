using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Movement settings")] [SerializeField]
        private float jumpSpeed = 200;

        [SerializeField] private float rotationSpeed = 4;
        [SerializeField] private float runCoefficient = 2f;
        [SerializeField, Range(1, 10)] private float mouseSensitivity = 1;


        public float JumpSpeed => jumpSpeed;
        public float RotationSpeed => rotationSpeed;
        public float RunCoefficient => runCoefficient;
        public Vector3 Movement { get; private set; }
        public float HorizontalMove { get; private set; }
        public float VerticalMove { get; private set; }
        public float Jump { get; private set; }
        public float Run { get; private set; }
        public bool Attack { get; private set; }
        public float MouseX { get; private set; }
        public float MouseY { get; private set; }
        public Quaternion StartRotation { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            StartRotation = Quaternion.identity;
        }

        // Update is called once per frame
        void Update()
        {
            VerticalMove = GetInt(Input.GetAxis("Vertical"));
            HorizontalMove = GetInt(Input.GetAxis("Horizontal"));
            Run = GetInt(Input.GetAxis("Run"));
            Jump = GetInt(Input.GetAxis("Jump"));
            Attack = Input.GetKeyDown(KeyCode.Mouse0);

            MouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
            MouseY += Input.GetAxis("Mouse Y") * mouseSensitivity;
            MouseY = Mathf.Clamp(MouseY, -50, 50);

            Movement = new Vector3(HorizontalMove, 0f, VerticalMove);
            Movement.Normalize();
        }

        private void ChangeCamera()
        {
            
        }

        private static int GetInt(float number) => number < 0 ? -1 : number == 0 ? 0 : 1;
    }
}

