using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] float movePower;
    [SerializeField] float x;
    [SerializeField] SpriteRenderer spriteRenderer;
    private void Update()
    {
        x = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Jump();
        }
    }
    private void FixedUpdate()
    {
        Move();

    }
    private void Move() 
    {
        rb2d.AddForce(Vector2.right * x * movePower, ForceMode2D.Force);
        if (x < 0) 
        {   
           
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX= false;
        }

    }
    private void Jump() 
    {
        rb2d.AddForce(Vector2.up * movePower, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Ãæµ¹");
    }
}
