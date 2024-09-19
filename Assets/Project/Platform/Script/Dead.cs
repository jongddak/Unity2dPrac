using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dead : MonoBehaviour
{
    public UnityEvent OnDead;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Á×À½");
        OnDead?.Invoke();
    }
}
