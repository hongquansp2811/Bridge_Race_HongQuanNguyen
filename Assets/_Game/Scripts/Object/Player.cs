using System.Collections.Generic;
using UnityEngine;

public class Player : Charater
{
    public Joystick joystick;
    
    public float moveSpeed = 5.0f;
    public float rotSpeed = 10.0f;
    public float slopeForce = 1.0f;
    public float gravity = -9.81f;
    public CharacterController charController;
    private Vector3 velocity;

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (!isMoveUp && vertical > 0)
        {
            return;
        }

        Vector3 move = direction * moveSpeed * Time.deltaTime;

        if (charController.isGrounded)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        move += velocity * Time.deltaTime;

        // Kiểm tra hướng di chuyển trước khi quay hướng
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }

        // Kiểm tra và xử lý di chuyển trên dốc
        if ((vertical != 0 || horizontal != 0) && OnSlope())
        {
            move += Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime;
        }

        charController.Move(move);

        anim.SetBool("Running", (vertical != 0 || horizontal != 0));
    }

    protected bool OnSlope()
    {
        if (charController.isGrounded)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * 1.5f))
            {
                if (hit.normal != Vector3.up)
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected override void HandleFinnishCollision()
    {
        // Hiển thị FinnishMenu khi Player va chạm với Finnish
        GameManager.Instance.FinishGame();
    }
}
