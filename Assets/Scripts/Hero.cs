using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine;

public class Hero : MonoBehaviour
{
    public float speed = 3f;

    public Rigidbody2D rb;
    public Animator animator;
    public VectorValue pos;

    Vector2 movement;

    private void Start()
    {
        transform.position = pos.initialValue;
    }

    private void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.x > 0.1 || movement.x < -0.1 || movement.y > 0.1 || movement.y < -0.1)
        {
            animator.SetFloat("LastMoveX", movement.x);
            animator.SetFloat("LastMoveY", movement.y);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }
}

