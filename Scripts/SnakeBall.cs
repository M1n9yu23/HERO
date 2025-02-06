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
            Vector2 currentPosition = transform.position; // 현재 위치와 타겟 위치 계산
            Vector2 direction = targetPosition - currentPosition; // 타겟과 캐릭터 위치를 벡터로 계산


            // 거리 계산
            float distance = direction.magnitude;

            if (distance <= speed * Time.deltaTime) // 거리가 짧으면 그 위치에서 멈추는 버그가 발생하여 if문 넣음
            {
                // DestroyBall(); // 거리가 짧으면 투사체 파괴
                ReturnToPool();  // 아무것도 안넣으면 그 자리에서 멈춤 그래서 비활성화라도 시킴
            }
            else
            {
                // 이동할 새로운 위치 계산
                Vector2 normalizedDirection = direction.normalized; // 정규화된 방향 벡터 계산
                Vector2 newPosition = currentPosition + normalizedDirection * speed * Time.deltaTime; // 현재 위치에서 이동할 새로운 위치 계산
                transform.position = newPosition; // 새로운 위치로 이동
                // 타겟을 계속 쫓아가는 문제점이 있어서 이렇게 수정 해줬음
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