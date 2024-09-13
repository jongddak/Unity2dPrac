using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Targeting : MonoBehaviour
{   
   
    private void Update()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //화면상 마우스 위치 
       
        Vector2 direction = mousePos - transform.position;   // 마우스 위치와 회전할 오브젝트 사이의 거리 

        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;  // 아무튼 수학공식으로 각도 구하고 변환 

        
        transform.rotation = Quaternion.Euler(0, 0, angle); // 2d 는 z 축만ㅁ 회전시키면 됨 
    }
}
