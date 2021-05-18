using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Animator)), RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 input;
    private float angle;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    private void OnValidate()
    {
        gameObject.name = "Player";
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        //   rb.bodyType = RigidbodyType2D.Kinematic;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 rawInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = rawInput.normalized * speed;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * Time.fixedDeltaTime);
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, angle, Time.fixedDeltaTime * 8));

        if (input.magnitude == 0)
            animator.SetBool("IsWalking", false);
        else
        {
            animator.SetBool("IsWalking", true);
            angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        }
    }
}
