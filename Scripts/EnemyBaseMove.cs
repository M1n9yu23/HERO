using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseMove : MonoBehaviour
{
    [SerializeField] private EnemyBase.EnemyType enemyType;


    EnemyBase enemyBase;
    Animator animator;
    TouchingDirections touchingDirections;
    Rigidbody2D rb;
    public DetectionZone attackZone;


    #region Snake
    public GameObject snakeBallPrefab; // ������ũ ������ ����
    public Transform pos; // ����ü ���� ��ġ
    public float ballspawn = 1.5f;
    float delta = 0;

    // ������Ʈ Ǯ ���� ����
    public int poolSize = 10;
    private List<GameObject> objectPool;
    private int currentIndex = 0;
    private List<Vector3> objectPoolPositions; // ������Ʈ Ǯ�� ���� ��ġ ����
    #endregion Snake

    #region VultureEnemy
    public float flySpeed = 4f;
    public float flyHeight = 3f;
    #endregion VultureEnemy

    Vector2 walkDirectionVector = Vector2.right; // ���� �� �̵� ����

    #region Knight
    public float walkSpeed = 4f;
    public float walkStopRate = 0.25f;
    #endregion Knight

    #region FlyEnemy
    public float flightSpeed = 5f;
    public float waypointReachedDistance = 0.1f;
    public DetectionZone detectionZone;
    public List<Transform> waypoints;
    public float attackSpeed = 5f;
    Transform nextwaypoint;
    int waypointNum = 0;
# endregion FlyEnemy


    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool _hasTarget = false;
    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }

        private
                set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        enemyBase = GetComponent<EnemyBase>();

        if (enemyType == EnemyBase.EnemyType.Snake)
        {
            // ������Ʈ Ǯ �ʱ�ȭ
            InitializeObjectPool();
        }
    }
    private void Start()
    {
        if (enemyType == EnemyBase.EnemyType.FlyEnemy)
        {
            nextwaypoint = waypoints[waypointNum];
        }
            

        if (enemyType == EnemyBase.EnemyType.Snake)
        {
            pos = transform; // ����ü ���� ��ġ�� ���� ��ũ��Ʈ�� ������ ������Ʈ�� ����
        }
    }

    private void Update()
    {
        if (enemyType == EnemyBase.EnemyType.FlyEnemy)
        {
            HasTarget = detectionZone.detectedColliders.Count > 0;
        }
        if (enemyType == EnemyBase.EnemyType.Knight)
        {
            HasTarget = attackZone.detectedColliders.Count > 0;
        }
    }

    private void FixedUpdate()
    {
        delta += Time.deltaTime;

        if (enemyBase.isLive == true)
        {
            if (enemyType == EnemyBase.EnemyType.FlyEnemy)
            {
                if (CanMove)
                {
                    Flight();
                }
                else if (HasTarget)
                {
                    MoveAttack();
                }
                else
                {
                    rb.velocity = Vector3.zero;
                }
            }
            if (enemyType == EnemyBase.EnemyType.Knight)
            {
                if (touchingDirections.IsGrounded && touchingDirections.IsOnWall)
                {
                    FlipDirection();
                }

                if (CanMove)
                {
                    rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
                }

                FlipSpriteBasedOnDirection();
            }
            if (enemyType == EnemyBase.EnemyType.Snake)
            {
                if (delta > ballspawn)
                {
                    delta = 0;

                    SpawnSnakeBall();
                }
            }
            if (enemyType == EnemyBase.EnemyType.VultureEnemy)
            {
                if (touchingDirections.IsOnWall)
                {
                    FlipDirection();
                }

                if (CanMove)
                {
                    rb.velocity = new Vector2(flySpeed * walkDirectionVector.x, flyHeight * walkDirectionVector.y);
                }
                else
                {
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, 0), flyHeight * walkDirectionVector.y);
                }

                FlipSpriteBasedOnDirection();
            }
        }
    }
    private void InitializeObjectPool()
    {
        objectPool = new List<GameObject>();
        objectPoolPositions = new List<Vector3>();
        for (int i = 0; i < poolSize; i++)
        {
            // pos ��ġ���� ������ ����
            GameObject ball = Instantiate(snakeBallPrefab, pos.position, Quaternion.identity);
            ball.SetActive(false);
            objectPool.Add(ball);
            objectPoolPositions.Add(pos.position);
        }
    }

    private void SpawnSnakeBall()
    {
        // ������Ʈ Ǯ���� ��� ������ ������Ʈ ��������
        GameObject ball = GetAvailableObjectFromPool();
        // pos ��ġ���� ������ ����
        // GameObject ball = Instantiate(snakeBallPrefab, pos.position, Quaternion.identity);

        if (ball != null)
        {
            // Ÿ�� ����
            Player target = FindObjectOfType<Player>();

            if (target != null)
            {
                HasTarget = true; // Ÿ���� ���� ��� HasTarget�� true�� ����
                ball.GetComponent<SnakeBall>().SetTarget(target);

                // Snake�� ���� ��ȯ
                if (target.transform.position.x > transform.position.x) // Ÿ���� x ��ǥ�� Snake�� x ��ǥ���� ū ��� true, �̰��� ���̸� Ÿ���� �����ʿ� ��ġ�ߴٴ� ���� �ǹ�
                {
                    // �÷��̾ �����ʿ� �ִ� ���
                    transform.localScale = new Vector3(-1f, 1f, 1f); // ��������Ʈ�� x �������� ������ �����Ͽ� ������ ������ ���ϵ��� ��
                }
                else
                {
                    // �÷��̾ ���ʿ� �ִ� ���
                    transform.localScale = new Vector3(1f, 1f, 1f); // ��������Ʈ�� x �������� ����� �����Ͽ� ���� ������ ���ϵ��� ��
                }
            }
            else
            {
                HasTarget = false; // Ÿ���� ���� ��� HasTarget�� false�� ����
                ball.GetComponent<SnakeBall>().SetTarget(null);
            }
        }
    }

    private GameObject GetAvailableObjectFromPool()
    {
        // ������Ʈ Ǯ���� ��� ������ ������Ʈ ã��
        for (int i = 0; i < objectPool.Count; i++)
        {
            int index = (currentIndex + i) % objectPool.Count; // ���� �ε����� �������� ��ȯ�ϸ� ������Ʈ�� �������� ���� �ε��� ���

            if (!objectPool[index].activeInHierarchy)
            {
                currentIndex = (index + 1) % objectPool.Count; // ������ ������ ������Ʈ�� �ε����� ����
                objectPool[index].transform.position = objectPoolPositions[index]; // ��Ȱ��ȭ�� ������Ʈ�� ��ġ�� ���� ��ġ�� �̵�
                objectPool[index].SetActive(true);
                return objectPool[index];
            }
        }
        // Ǯ�� �� �̻� ��� ������ ������Ʈ�� ���� ���
        // ���ο� ������Ʈ �����ؼ� Ǯ�� �߰�
        GameObject newObj = Instantiate(snakeBallPrefab, pos.position, Quaternion.identity);
        objectPool.Add(newObj);
        objectPoolPositions.Add(pos.position);
        newObj.SetActive(true);
        currentIndex = objectPool.Count - 1; // ���� ������ ������Ʈ�� �ε����� ����
        return newObj;
    }

    void FlipSpriteBasedOnDirection()
    {
        // ��������Ʈ ������ ������ x���� ���� ����
        if (walkDirectionVector.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            // Scale�� �¿������ ����
        }
        else if (walkDirectionVector.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            // Scale�� �¿������ ���� - ������ ������ �¿����
        }
    }
    void FlipDirection()
    {

        walkDirectionVector *= -1; // ���� ���͸� �ݴ�� ��ȯ
    }

    void Flight()
    {
        // ���� ����Ʈ�� ����
        Vector2 directionToWaypoint = (nextwaypoint.position - transform.position).normalized;

        // ����Ʈ�� �����ߴ��� Ȯ��
        float distance = Vector2.Distance(nextwaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;

        UpdateDirection();

        // ����Ʈ ��ȯ�ؾ��ϴ��� Ȯ��
        if (distance <= waypointReachedDistance)
        {
            // ���� ����Ʈ�� ��ȯ
            waypointNum++;
            if (waypointNum >= waypoints.Count)
            {
                // ���� ����Ʈ�� ���ư���
                waypointNum = 0;
            }
            nextwaypoint = waypoints[waypointNum];
        }
    }

    private void MoveAttack()
    {

        // ���� ����� �� ã��
        Collider2D nearestCollider = null;
        float nearestDistance = float.MaxValue;
        foreach (Collider2D collider in detectionZone.detectedColliders)
        {
            Vector2 diff = collider.transform.position - transform.position;
            float distance = diff.magnitude;
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestCollider = collider;
            }
        }

        if (nearestCollider != null)
        {
            // �� ��ġ�� �ٶ󺸵��� ȸ��
            Vector3 diff = nearestCollider.transform.position - transform.position;

            // ���� �������� �̵�
            Vector2 moveDirection = diff.normalized;
            rb.velocity = moveDirection * attackSpeed;

            if (nearestDistance <= waypointReachedDistance)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    // �ٶ󺸴� ���⿡ �°� ������Ʈ ��ȯ
    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale; // Scale�� -1�� �ϸ� �¿����
        if (transform.localScale.x < 0)
        {
            //  Facing the right
            if (rb.velocity.x < 0)
            {
                // Flip
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else
        {
            // Facing the Left
            if (rb.velocity.x > 0)
            {
                // Flip
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }
}
