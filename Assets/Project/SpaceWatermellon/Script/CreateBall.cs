using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBall : MonoBehaviour
{   
    [SerializeField] GameObject first;
    [SerializeField] GameObject second;
    [SerializeField] GameObject third;
    [SerializeField] GameObject fourth;
    [SerializeField] GameObject fifth;
    [SerializeField] GameObject sixth;

    [SerializeField] Transform target;
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            shoot(first);
        }
    }
    private void shoot(GameObject prefab)
    {
        float z = transform.rotation.eulerAngles.z;
        Vector2 direction = new Vector2(Mathf.Cos(z * Mathf.Deg2Rad), Mathf.Sin(z * Mathf.Deg2Rad));
        
        GameObject ball = Instantiate(prefab,target.position,target.rotation);
        Rigidbody2D rbball = ball.GetComponent<Rigidbody2D>();
        rbball.AddForce(direction * 5f, ForceMode2D.Impulse);

    }
}
