using UnityEngine;

public class mouseViewController : MonoBehaviour
{

    public float mouseSensitivity = 100f;

    public Transform playerBody;

    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //lock cursor to center of screen and make it disappear.
    }

    // Update is called once per frame
    void Update()
    {
        mouseMovement();    //Calls the mouseMovement function.
    }

    void mouseMovement()    //Allows the player to alter their view by moving the mouse.
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; //multiplying by time.deltatime accounts for framerate - moving at the same speed whether it is high or low fps
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation = xRotation - mouseY; //modify the xRotation by taking the current rotational position and subtracting mouse movement since the last frame.
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Define boundaries for the xRotation so that the player cannot rotate all the way to see behind them.

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
