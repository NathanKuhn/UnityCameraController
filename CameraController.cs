using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float moveSpeed = 10.0f;
    public float sensitivity = 5.0f;
    public float smoothing = 1.0f;

    private Vector2 mouseLook;
    private Vector2 smoothV;

    private bool camLock;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        camLock = true;
    }

    private void Update() {

        if (camLock) {

            /* Rotation */
            var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
            smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
            mouseLook += smoothV;

            mouseLook.y = Mathf.Clamp(mouseLook.y, -90, 90);

            Quaternion yaw_q = Quaternion.AngleAxis(mouseLook.x, Vector3.up);
            Quaternion pitch_q = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);

            transform.localRotation = yaw_q * pitch_q;

            /* Movement */
            float translation = 0; // W S
            float straffe = 0; // A D
            float hover = 0; // SPACE

            if (Input.GetKey(KeyCode.W)) { translation = 1f; } else if (Input.GetKey(KeyCode.S)) { translation = -1f; }
            if (Input.GetKey(KeyCode.D)) { straffe = 1f; } else if (Input.GetKey(KeyCode.A)) { straffe = -1f; }
            if (Input.GetKey(KeyCode.Space)) { hover = 1f; } else if (Input.GetKey(KeyCode.LeftShift)) { hover = -1f; }

            Vector3 translationVector = transform.localRotation * new Vector3(straffe, 0.0f, translation);
            Vector3 moveVector = new Vector3(0.0f, hover, 0.0f) + translationVector;
            moveVector *= moveSpeed * Time.deltaTime;

            transform.Translate(moveVector, Space.World);

            if (Input.GetKeyDown(KeyCode.Escape)) {
                Cursor.lockState = CursorLockMode.None;
                camLock = false;
            }
        }

        /* Lock the camera if the window is clicked */
        if (Input.GetMouseButtonDown(0)) {
            Cursor.lockState = CursorLockMode.Locked;
            camLock = true;
        }

    }
}
