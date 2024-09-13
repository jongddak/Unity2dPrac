using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Targeting : MonoBehaviour
{   
   
    private void Update()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //ȭ��� ���콺 ��ġ 
       
        Vector2 direction = mousePos - transform.position;   // ���콺 ��ġ�� ȸ���� ������Ʈ ������ �Ÿ� 

        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;  // �ƹ�ư ���а������� ���� ���ϰ� ��ȯ 

        
        transform.rotation = Quaternion.Euler(0, 0, angle); // 2d �� z �ุ�� ȸ����Ű�� �� 
    }
}
