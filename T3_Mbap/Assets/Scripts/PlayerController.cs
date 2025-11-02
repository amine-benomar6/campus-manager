using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody2D rb;
    Vector2 dir;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.y = Input.GetAxisRaw("Vertical");
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);

        SetParam();
    }

    void SetParam()
    {
        if (dir == Vector2.zero)
        {
            anim.SetInteger("dir", 0);
        }
        else
        {
            if (dir.y < 0)
            {
                anim.SetInteger("dir", 1); // Down
            }
            else if (dir.x > 0)
            {
                anim.SetInteger("dir", 2); // Right
            }
            else if (dir.x < 0)
            {
                anim.SetInteger("dir", 3); // Left
            }
            else if (dir.y > 0)
            {
                anim.SetInteger("dir", 4); // Up
            }
        }
    }
}
