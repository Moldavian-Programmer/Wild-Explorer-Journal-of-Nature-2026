using UnityEngine;

public class MouseLookRightClick : MonoBehaviour
{
    [Header("Target")]
    public Transform playerBody;

    [Header("Third Person Camera")]
    public float distance = 4f;
    public float height = 1.8f;
    public float sideOffset = 0f;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 220f;
    public bool requireRightClick = true;
    public float minPitch = -30f;
    public float maxPitch = 60f;

    [Header("Smoothing")]
    public float positionSmooth = 12f;
    public float rotationSmooth = 12f;

    private float yaw = 0f;
    private float pitch = 15f;
    private bool mouseLocked = false;

    void Start()
    {
        if (playerBody == null)
        {
            Debug.LogWarning("Player Body is not assigned on MouseLookRightClick.");
            return;
        }

        yaw = playerBody.eulerAngles.y;
        pitch = 15f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void LateUpdate()
    {
        if (playerBody == null)
            return;

        HandleMouseLock();

        bool canRotate = !requireRightClick || Input.GetMouseButton(1);

        if (canRotate && mouseLocked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetCenter = playerBody.position + Vector3.up * height;
        Vector3 targetOffset = targetRotation * new Vector3(sideOffset, 0f, -distance);
        Vector3 desiredPosition = targetCenter + targetOffset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSmooth * Time.deltaTime);

        Quaternion lookRotation = Quaternion.LookRotation(targetCenter - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSmooth * Time.deltaTime);
    }

    void HandleMouseLock()
    {
        if (requireRightClick)
        {
            if (Input.GetMouseButtonDown(1))
            {
                mouseLocked = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (Input.GetMouseButtonUp(1))
            {
                mouseLocked = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else
        {
            mouseLocked = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mouseLocked = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}