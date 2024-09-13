using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    private void Update()
    {
        rb.AddForce(Vector2.right * 0.2f, ForceMode2D.Impulse);
    }
}
