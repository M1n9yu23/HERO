using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //열거형을 통한 적 type 설정
    [SerializeField] public EnemyType enemyType;

    public enum EnemyType
    {
        FlyEnemy,
        Knight,
        Snake,
        VultureEnemy
    }

    [Header("# Enemy Info")]
    public float EnemyHp;  //현재 적 hp
    public float MaxEnemyHp;   //적 최대 hp
    public float EnemyAtk; //적 공격력
    public bool isLive = true; //적의 생존 여부
    public float walkStop = 0.25f;
    Animator animator;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;


    private void Awake()
    {
        //초기화
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        EnemyHp = MaxEnemyHp;
    }

    public void takeDamage(float AtkDamage)
    {
        EnemyHp = EnemyHp - AtkDamage;
        switch (enemyType)
        {
            //EnemyType이 FlyEnemy
            case EnemyType.FlyEnemy:
                // 몬스터 피격 에니메이션 실행
                StopCoroutine(MonsterHit());
                StartCoroutine(MonsterHit());
                //체력이 0 이하가 될 경우 사망
                if (EnemyHp <= 0)
                {
                    //Enemy 사망 로직 작성
                    Debug.Log("비둘기가 죽었습니다.");
                    Monsterdie();
                }
                break;
            //EnemyType이 Knight
            case EnemyType.Knight:
                StopCoroutine(MonsterHit());
                StartCoroutine(MonsterHit());

                //체력이 0 이하가 될 경우 사망
                if (EnemyHp <= 0)
                {
                    Debug.Log("Knight가 죽었습니다.");
                    Monsterdie();
                }
                break;
            //EnemyType이 Snake
            case EnemyType.Snake:
                StopCoroutine(MonsterHit());
                StartCoroutine(MonsterHit());
                //체력이 0 이하가 될 경우 사망
                if (EnemyHp <= 0)
                {
                    Debug.Log("Snake가 죽었습니다.");
                    Monsterdie();
                }
                break;
            //EnemyType이 VultureEnemy
            case EnemyType.VultureEnemy:
                StopCoroutine(MonsterHit());
                StartCoroutine(MonsterHit());
                //체력이 0 이하가 될 경우 사망
                if (EnemyHp <= 0)
                {
                    Debug.Log("VultureEnemy가 죽었습니다.");
                    Monsterdie();
                }
                break;
        }

    }

    // 사망 및 피격 모션이 없기 때문에 해당 SpriteRendere를 활용해 색의 변화를 줌.
    private IEnumerator MonsterHit()
    {
        Debug.Log("Monster가 피해를 입었습니다.");
        //피격 시 색상 변경
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        //색상 복구
        spriteRenderer.color = Color.white;
    }

    //몬스터 사망
    private void Monsterdie()
    {
        isLive = false;
        rigid.velocity = new Vector2(Mathf.Lerp(rigid.velocity.x, 0, walkStop), rigid.velocity.y);
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        Invoke("EnemyDestroy", 1f);
    }
    //Enemy 사망 시 SetActive False를 통해 오브젝트 풀링을 사용하기 위함.
    private void EnemyDestroy()
    {
        gameObject.SetActive(false);
    }
}
