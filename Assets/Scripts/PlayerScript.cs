using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    public float speed;
    public float jumpForce;
    Rigidbody2D _rbody;
    Vector2 moveDirection = Vector2.zero;
    bool isGrounded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        _rbody.linearVelocity = new Vector2(moveDirection.x * speed, _rbody.linearVelocity.y);
    }

    public void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            _rbody.linearVelocity = new Vector2(
                _rbody.linearVelocity.x,
                jumpForce
            );
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
       if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
