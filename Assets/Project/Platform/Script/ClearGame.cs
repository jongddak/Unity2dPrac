using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClearGame : MonoBehaviour
{
    public UnityEvent Onclear;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Å¬¸®¾î");
        Onclear?.Invoke();
    }

}
