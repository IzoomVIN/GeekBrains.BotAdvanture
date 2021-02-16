using UnityEngine;

namespace Player
{
    public class CameraManager : MonoBehaviour
    {

        [SerializeField] private GameObject main;
        [SerializeField] private GameObject aim;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                aim.SetActive(true);
                main.SetActive(!aim.activeSelf);
            }
        
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                aim.SetActive(false);
                main.SetActive(!aim.activeSelf);
            }
        }
    }
}
