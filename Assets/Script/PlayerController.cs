using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Tốc độ di chuyển
    public float jumpForce = 4f; // Lực nhảy
    private bool canDoubleJump = true;

    private Rigidbody2D rb; // Rigidbody2D của nhân vật
    private SpriteRenderer spriteRenderer; // SpriteRenderer để lật hình ảnh
    private Animator animator; // Animator để điều khiển animation

    private bool isRunning;
    private bool onGround;
    private bool isJump;
    private bool isFalling;
    private bool doubleJumping;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moving();
        jump();

        // Kiểm tra trạng thái rơi
        if (rb.velocity.y < 0 && !onGround)
        {
            isFalling = true;
            isJump = false;
            doubleJumping = false; // Reset double jump khi đang rơi
        }
        else if (onGround)
        {
            isFalling = false;
            isJump = false;
            doubleJumping = false; // Reset trạng thái double jump khi tiếp đất
        }

        // Cập nhật trạng thái Animator
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJump", isJump);
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("doubleJumping", doubleJumping);
    }

    void moving()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Chỉ di chuyển và bật isRunning nếu nhân vật trên mặt đất và không nhảy
        if (onGround && !isJump && !doubleJumping)
        {
            isRunning = (moveInput != 0);
        }
        else
        {
            isRunning = false; // Tắt chạy khi nhảy hoặc double jump
        }

        // Cập nhật vận tốc di chuyển
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Lật hình ảnh nhân vật theo hướng di chuyển
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void jump()
    {
        // Kiểm tra nhảy lần đầu
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJump = true;
            onGround = false;
            canDoubleJump = true; // Cho phép double jump
        }
        // Kiểm tra double jump
        else if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump && !onGround)
        {

            doubleJumping = true; // Kích hoạt double jump
            rb.velocity = new Vector2(rb.velocity.x, jumpForce );
            canDoubleJump = false; // Vô hiệu hóa double jump sau khi sử dụng
            isJump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            onGround = true;
            isJump = false;
            doubleJumping = false; // Reset double jump khi tiếp đất
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            onGround = false;
        }
    }



}
