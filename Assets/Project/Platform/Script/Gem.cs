using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gem : MonoBehaviour
{
    public UnityEvent Onget;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Áª È¹µæ ");
            Onget?.Invoke();
            Destroy(gameObject);
        }
        
    }
}
