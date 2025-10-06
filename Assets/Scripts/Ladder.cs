using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private float inputVertical;    // ������ ������������ ���� (W/S)
    public float speed;            // �������� ������� (������������� � ����������)
    Rigidbody2D rb;                // ��������� ������
    public float distance;         // ��������� �������� ��������
    public LayerMask whatisLadder;  // ����, �� ������� ��������� ��������
    private bool Climbing;         // ��������� �������
    private bool wasGrounded; // ���� ��� ������������ ��������� �� �����

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wasGrounded = true;
    }

    private void FixedUpdate()
    {
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatisLadder);

        if (hitinfo.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Climbing = true;
            }
        }
        else
        {
            Climbing = false;
        }

        // ���������, ��������� �� ����� �� �����
        bool isGrounded = Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Ground"));

        if (Climbing == true && hitinfo.collider != null)
        {
            inputVertical = Input.GetAxisRaw("Vertical");
            rb.linearVelocity = new Vector2(rb.position.x, inputVertical * speed);
            rb.gravityScale = 0;  // ��������� ���������� ��� �������
        }
        else
        {
            if (!isGrounded)
            {
                rb.gravityScale = 5;
            }
            else
            {
                rb.gravityScale = 1;  // �������� ���������� ����������, ���� �� �����
            }
        }
    }
}

