using UnityEngine;

public class CameraControllerOld : MonoBehaviour
{
    
    [SerializeField] private GameObject player;
    public float sensitivity = 1;
    
    [HideInInspector] public static CameraControllerOld Instance { get; private set; }
    [HideInInspector] public Quaternion rotationForPlayer;
    private float _mouseX;
    private float _mouseY;
    private Quaternion _startRotation;
    private Transform _myTransform;
    

    private void Awake()
    {
        _myTransform = transform;
        Instance = this;
    }

    private void Start()
    {
        _startRotation = transform.rotation;
    }

    // Update is called once per frame
    private void Update()
    {
        _mouseX += Input.GetAxis("Mouse X") * sensitivity;
        _mouseY += Input.GetAxis("Mouse Y") * sensitivity;
    }

    void FixedUpdate()
    {
        Vector3 playerPosition = player.transform.position;
        _mouseY = Mathf.Clamp(_mouseY, -50, 50);
        
        Quaternion rotX = Quaternion.AngleAxis(_mouseX, Vector3.up);
        Quaternion rotY = Quaternion.AngleAxis(-_mouseY, Vector3.right);

        rotationForPlayer = _startRotation * rotX;

        _myTransform.rotation = _startRotation * rotX * rotY;
        _myTransform.position = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
    }
}
