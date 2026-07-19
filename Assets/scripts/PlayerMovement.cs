using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 4f;
    public float gravity = -20f;
    public float jumpHeight = 1.2f;

    [Header("Rotation 90")]
    public float turnDuration = 0.15f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private bool isTurning = false;
    private float startYRotation;
    private float targetYRotation;
    private float turnTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleGroundCheck();
        HandleTurning();
        HandleMovement();
        HandleGravityAndJump();
    }

    void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }
    }

    void HandleTurning()
    {
        if (!isTurning)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartTurn(-90f);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                StartTurn(90f);
            }
        }
        else
        {
            turnTimer += Time.deltaTime;
            float t = Mathf.Clamp01(turnTimer / turnDuration);

            float newY = Mathf.LerpAngle(startYRotation, targetYRotation, t);
            transform.rotation = Quaternion.Euler(0f, newY, 0f);

            if (t >= 1f)
            {
                isTurning = false;
                transform.rotation = Quaternion.Euler(0f, targetYRotation, 0f);
            }
        }
    }

    void StartTurn(float angle)
    {
        isTurning = true;
        turnTimer = 0f;
        startYRotation = transform.eulerAngles.y;
        targetYRotation = startYRotation + angle;
    }

    void HandleMovement()
    {
        float moveInput = 0f;

        if (Input.GetKey(KeyCode.W))
            moveInput = 1f;
        else if (Input.GetKey(KeyCode.S))
            moveInput = -1f;

        Vector3 move = transform.forward * moveInput;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void HandleGravityAndJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}