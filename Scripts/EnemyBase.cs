using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //�������� ���� �� type ����
    [SerializeField] public EnemyType enemyType;

    public enum EnemyType
    {
        FlyEnemy,
        Knight,
        Snake,
        VultureEnemy
    }

    [Header("# Enemy Info")]
    public float EnemyHp;  //���� �� hp
    public float MaxEnemyHp;   //�� �ִ� hp
    public float EnemyAtk; //�� ���ݷ�
    public bool isLive = true; //���� ���� ����
    public float walkStop = 0.25f;
    Animator animator;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;


    private void Awake()
    {
        //�ʱ�ȭ
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
            //EnemyType�� FlyEnemy
            case EnemyType.FlyEnemy:
                // ���� �ǰ� ���ϸ��̼� ����
                StopCoroutine(MonsterHit());
                StartCoroutine(MonsterHit());
                //ü���� 0 ���ϰ� �� ��� ���
                if (EnemyHp <= 0)
                {
                    //Enemy ��� ���� �ۼ�
                    Debug.Log("��ѱⰡ �׾����ϴ�.");
                    Monsterdie();
                }
                break;
            //EnemyType�� Knight
            case EnemyType.Knight:
                StopCoroutine(MonsterHit());
                StartCoroutine(MonsterHit());

                //ü���� 0 ���ϰ� �� ��� ���
                if (EnemyHp <= 0)
                {
                    Debug.Log("Knight�� �׾����ϴ�.");
                    Monsterdie();
                }
                break;
            //EnemyType�� Snake
            case EnemyType.Snake:
                StopCoroutine(MonsterHit());
                StartCoroutine(MonsterHit());
                //ü���� 0 ���ϰ� �� ��� ���
                if (EnemyHp <= 0)
                {
                    Debug.Log("Snake�� �׾����ϴ�.");
                    Monsterdie();
                }
                break;
            //EnemyType�� VultureEnemy
            case EnemyType.VultureEnemy:
                StopCoroutine(MonsterHit());
                StartCoroutine(MonsterHit());
                //ü���� 0 ���ϰ� �� ��� ���
                if (EnemyHp <= 0)
                {
                    Debug.Log("VultureEnemy�� �׾����ϴ�.");
                    Monsterdie();
                }
                break;
        }

    }

    // ��� �� �ǰ� ����� ���� ������ �ش� SpriteRendere�� Ȱ���� ���� ��ȭ�� ��.
    private IEnumerator MonsterHit()
    {
        Debug.Log("Monster�� ���ظ� �Ծ����ϴ�.");
        //�ǰ� �� ���� ����
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        //���� ����
        spriteRenderer.color = Color.white;
    }

    //���� ���
    private void Monsterdie()
    {
        isLive = false;
        rigid.velocity = new Vector2(Mathf.Lerp(rigid.velocity.x, 0, walkStop), rigid.velocity.y);
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        Invoke("EnemyDestroy", 1f);
    }
    //Enemy ��� �� SetActive False�� ���� ������Ʈ Ǯ���� ����ϱ� ����.
    private void EnemyDestroy()
    {
        gameObject.SetActive(false);
    }
}
