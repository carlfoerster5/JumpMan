using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public float speed = 5f;
    public float chargeSpeed = 0.2f;
    public float maxJumpSpeed = 10f;
    public float minJumpSpeed = 5f;
    public float horizontalJumpSpeed = 4f;

    Rigidbody2D _rbody;
    InputAction _jumpAction;

    Vector2 moveDirection = Vector2.zero;

    bool isGrounded = false;
    bool isCharging = false;
    float jumpSpeed = 5f;

    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        //used to check jump release
        _jumpAction = GetComponent<PlayerInput>().actions["Jump"];
    }


    // used this instead of OnJump since was having lots of trouble detecting release of charge that way
    void Update()
    {
        if (_jumpAction.WasPressedThisFrame() && isGrounded)
        {
            isCharging = true;
            jumpSpeed = minJumpSpeed;
        }

        if (_jumpAction.WasReleasedThisFrame() && isCharging && isGrounded)
        {
            isCharging = false;
            float xDir = 0f;
            // if movement left or right, then set fixed speed left or right for jump
            if (moveDirection.x > 0.0001f || moveDirection.x < -0.0001f)
            {
                xDir = Mathf.Sign(moveDirection.x);
            } 
            _rbody.linearVelocity = new Vector2(xDir * horizontalJumpSpeed, jumpSpeed);
            isGrounded = false;
        }

        if (_rbody.linearVelocity.y >= 0.1f)
        {
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        // if in air, should not be able to change physics at all
        if (!isGrounded) return;

        // charge jump in fixed update so that charge rate is set, dictated by physics engine speed
        if (isCharging)
        {
            _rbody.linearVelocity = new Vector2(0f, _rbody.linearVelocity.y);
            jumpSpeed = Mathf.Min(jumpSpeed + chargeSpeed, maxJumpSpeed);
        }
        else
        {
            _rbody.linearVelocity = new Vector2(moveDirection.x * speed, _rbody.linearVelocity.y);
        }
    }

    public void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && Mathf.Abs(_rbody.linearVelocity.y) <= 0.01f)
            isGrounded = true;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && Mathf.Abs(_rbody.linearVelocity.y) <= 0.01f)
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}