using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    private float xRotationCamera;
    private float xRotationRocketLauncher = 90f;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotationCamera -= mouseY;
        xRotationCamera = Mathf.Clamp(xRotationCamera, -90f, 90f);

        xRotationRocketLauncher -= mouseY;
        xRotationRocketLauncher = Mathf.Clamp(xRotationRocketLauncher, 0f, 180f);

        transform.localRotation = Quaternion.Euler(xRotationCamera, 0f, 0f);
    }
}
