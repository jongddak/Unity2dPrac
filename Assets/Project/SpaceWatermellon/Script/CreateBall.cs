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
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            shoot(first);
        }
    }
    private void shoot(GameObject prefab)
    {
        Instantiate(prefab,transform.position,transform.rotation);
    }
}
