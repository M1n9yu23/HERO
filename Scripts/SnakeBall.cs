using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBall : MonoBehaviour
{
    public float speed =  5f;

    TouchingDirections touchingDirections;
    // private Rigidbody2D rb;
    // Player target;
    Vector2 targetPosition;

    bool isActive = false;
    float timer = 0;

    public void SetTarget(Player target)
    {
        if (target != null)
        {
            targetPosition = target.transform.position;
            isActive = true;
           
        }
        else
        {
            isActive = false;
        }
    }

    private void Awake()
    {
        touchingDirections = GetComponent<TouchingDirections>();
       // rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        isActive = false;
       
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        //if (targetPosition != null)
        if(isActive)
        {
            Vector2 currentPosition = transform.position; // ���� ��ġ�� Ÿ�� ��ġ ���
            Vector2 direction = targetPosition - currentPosition; // Ÿ�ٰ� ĳ���� ��ġ�� ���ͷ� ���


            // �Ÿ� ���
            float distance = direction.magnitude;

            if (distance <= speed * Time.deltaTime) // �Ÿ��� ª���� �� ��ġ���� ���ߴ� ���װ� �߻��Ͽ� if�� ����
            {
                // DestroyBall(); // �Ÿ��� ª���� ����ü �ı�
                ReturnToPool();  // �ƹ��͵� �ȳ����� �� �ڸ����� ���� �׷��� ��Ȱ��ȭ�� ��Ŵ
            }
            else
            {
                // �̵��� ���ο� ��ġ ���
                Vector2 normalizedDirection = direction.normalized; // ����ȭ�� ���� ���� ���
                Vector2 newPosition = currentPosition + normalizedDirection * speed * Time.deltaTime; // ���� ��ġ���� �̵��� ���ο� ��ġ ���
                transform.position = newPosition; // ���ο� ��ġ�� �̵�
                // Ÿ���� ��� �Ѿư��� �������� �־ �̷��� ���� ������
            }
        }

        if (touchingDirections.IsGrounded || touchingDirections.IsOnWall || touchingDirections.IsOnCeiling || timer > 10f)
        {
            timer = 0;
            ReturnToPool();
        }

    }
   /* 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            ReturnToPool();
        }
    }
   */
    private void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}