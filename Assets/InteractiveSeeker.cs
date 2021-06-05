using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InteractiveSeeker : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform childSpriteTransform;
    private Vector2 movementDirection;


    [Header("Standard Attributes")]
    public bool WASD;
    public float velocityMultiplier;

    [Header("Agent")]
    public GameObject agent;

  
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = 0;
        movementDirection = new Vector2(0, 0);
        childSpriteTransform = transform.GetChild(0);
    }

    private void Update()
    {
        movementDirection = Vector2.zero;

        if (WASD)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                movementDirection.y = 1;
                movementDirection.x = 0;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                movementDirection.y = -1;
                movementDirection.x = 0;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                movementDirection.x = -1;
                movementDirection.y = 0;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                movementDirection.x = 1;
                movementDirection.y = 0;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                movementDirection.y = 1;
                movementDirection.x = 0;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                movementDirection.y = -1;
                movementDirection.x = 0;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                movementDirection.x = -1;
                movementDirection.y = 0;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                movementDirection.x = 1;
                movementDirection.y = 0;
            }
        }


        if (movementDirection != Vector2.zero)
        {
            rb.velocity = movementDirection.normalized * velocityMultiplier * Time.fixedDeltaTime;
            childSpriteTransform.up = rb.velocity;
        }

        if (rb.velocity.y != 0 && rb.velocity.x != 0)
        {
            rb.velocity = movementDirection.normalized * velocityMultiplier * Time.fixedDeltaTime;
            childSpriteTransform.up = rb.velocity;
        }

    }

    private void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.angularVelocity = 0f;
        //rb.velocity = Vector2.zero;
        if (collision.collider.CompareTag("Player"))
        {
            if (agent.GetComponent<CustomAgentSeeker>() != null)
            {
                agent.GetComponent<CustomAgentSmart>().Seeked();
                agent.GetComponent<CustomAgentSmart>().successCount = 0;
            }

            else if (agent.GetComponent<CustomAgentSmart>() != null)
            {
                agent.GetComponent<CustomAgentSmart>().Seeked();
                agent.GetComponent<CustomAgentSmart>().successCount = 0;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        rb.angularVelocity = 0f;
        if (collision.collider.CompareTag("Player"))
        {
            if (agent.GetComponent<CustomAgentSeeker>() != null)
            {
                agent.GetComponent<CustomAgentSmart>().Seeked();
                agent.GetComponent<CustomAgentSmart>().successCount = 0;
            }
            else if (agent.GetComponent<CustomAgentSmart>() != null)
            {
                agent.GetComponent<CustomAgentSmart>().Seeked();
                agent.GetComponent<CustomAgentSmart>().successCount = 0;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        rb.angularVelocity = 0f;
    }

}
