using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 7.5f;
    public float gravity = 9.81f;
    public float jumpHeight = 3.0f;
    public Transform groundCheck;
    public LayerMask groundMask;

    private float horizontalInput;
    private float verticalInput;
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private float groundDistance = 0.35f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        ReadInput();
        CheckGround();
        Movement();
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    private void ReadInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void Movement()
    {
        // Resetear velocidad vertical si está en el suelo
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        // Movimiento en plano XZ
        Vector3 forwardMovement = transform.forward * verticalInput;
        Vector3 rightMovement = transform.right * horizontalInput;
        Vector3 movementDirection = Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f);

        characterController.Move(movementDirection * playerSpeed * Time.deltaTime);

        // Saltar
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }

        // Aplicar gravedad
        velocity.y -= gravity * Time.deltaTime;

        // Movimiento vertical
        characterController.Move(velocity * Time.deltaTime);
    }
}