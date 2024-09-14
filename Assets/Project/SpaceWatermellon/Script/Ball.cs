using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] GameObject[] prepabs;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "sixth")
        {
            
            if (collision.gameObject.CompareTag(gameObject.tag))
            {

                if (gameObject.GetInstanceID() < collision.gameObject.GetInstanceID())
                {
                    
                    Vector3 collisionPosition = collision.transform.position;

                   
                    Destroy(collision.gameObject);
                    Destroy(gameObject);

                    Instantiate(prepabs[0], collisionPosition, Quaternion.identity);
                }
            }
        }
        else
        {
            Debug.Log("¿Ï¼º");
        }
    }

}
