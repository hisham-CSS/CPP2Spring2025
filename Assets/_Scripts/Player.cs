using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, ProjectActions.IOverworldActions
{
    ProjectActions input;
    CharacterController cc;

    [Header("Movement Variables")]
    [SerializeField] private float initSpeed = 5.0f;
    [SerializeField] private float maxSpeed = 15.0f;
    [SerializeField] private float moveAccel = 0.2f;
    private float curSpeed = 5.0f;

    [Header("Jump Variables")]
    [SerializeField] private float jumpHeight = 0.1f;
    [SerializeField] private float jumpTime = 0.7f;

    //values calculated using jump height and jump time
    private float timeToJumpApex; //JumpTime / 2
    private float initJumpVelocity;

    //Character Movement
    Vector2 direction;
    Vector3 velocity;

    //calculated based on our jump values - this is the Y velocity that we will apply
    private float gravity;

    private bool isJumpPressed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();

        timeToJumpApex = jumpTime / 2;
        gravity = (-2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        initJumpVelocity = -(gravity * timeToJumpApex);
    }

    void OnEnable()
    {
        input = new ProjectActions();
        input.Enable();
        input.Overworld.SetCallbacks(this);
    }

    void OnDisable()
    {
        input.Disable();
        input.Overworld.RemoveCallbacks(this);
    }

    #region Input Functions
    public void OnJump(InputAction.CallbackContext context) => isJumpPressed = context.ReadValueAsButton();

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed) direction = context.ReadValue<Vector2>();
        if (context.canceled) direction = Vector2.zero;
    }
    #endregion

    void FixedUpdate()
    {
        UpdateCharacterVelocity();

        cc.Move(velocity);
    }

    private void UpdateCharacterVelocity()
    {
        if (direction == Vector2.zero) curSpeed = initSpeed;

        velocity.x = direction.x * curSpeed;
        velocity.z = direction.y * curSpeed;

        curSpeed += moveAccel * Time.fixedDeltaTime;

        if (!cc.isGrounded) velocity.y += gravity * Time.fixedDeltaTime;
        else velocity.y = CheckJump();
    }

    private float CheckJump()
    {
        if (isJumpPressed) return initJumpVelocity;
        else return -cc.minMoveDistance;
    }
}
