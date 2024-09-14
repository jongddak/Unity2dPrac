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
            // �±װ� ������ Ȯ��
            if (collision.gameObject.CompareTag(gameObject.tag))
            {

                if (gameObject.GetInstanceID() < collision.gameObject.GetInstanceID())
                {
                    // �浹�� ��ġ���� ���ο� ������Ʈ ����
                    Vector3 collisionPosition = collision.transform.position;

                    // �浹�� �� ������Ʈ ����
                    Destroy(collision.gameObject);
                    Destroy(gameObject);

                    // ���ο� ������Ʈ ����
                    Instantiate(prepabs[0], collisionPosition, Quaternion.identity);
                }
            }
        }
        else
        {
            Debug.Log("�ϼ�");
        }
    }

}
