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
            // 태그가 같은지 확인
            if (collision.gameObject.CompareTag(gameObject.tag))
            {

                if (gameObject.GetInstanceID() < collision.gameObject.GetInstanceID())
                {
                    // 충돌한 위치에서 새로운 오브젝트 생성
                    Vector3 collisionPosition = collision.transform.position;

                    // 충돌한 두 오브젝트 제거
                    Destroy(collision.gameObject);
                    Destroy(gameObject);

                    // 새로운 오브젝트 생성
                    Instantiate(prepabs[0], collisionPosition, Quaternion.identity);
                }
            }
        }
        else
        {
            Debug.Log("완성");
        }
    }

}
