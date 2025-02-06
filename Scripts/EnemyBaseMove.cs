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
    public GameObject snakeBallPrefab; // 스네이크 프리팹 변수
    public Transform pos; // 투사체 생성 위치
    public float ballspawn = 1.5f;
    float delta = 0;

    // 오브젝트 풀 관련 변수
    public int poolSize = 10;
    private List<GameObject> objectPool;
    private int currentIndex = 0;
    private List<Vector3> objectPoolPositions; // 오브젝트 풀의 생성 위치 저장
    #endregion Snake

    #region VultureEnemy
    public float flySpeed = 4f;
    public float flyHeight = 3f;
    #endregion VultureEnemy

    Vector2 walkDirectionVector = Vector2.right; // 시작 시 이동 방향

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
            // 오브젝트 풀 초기화
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
            pos = transform; // 투사체 생성 위치를 현재 스크립트가 부착된 오브젝트로 설정
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
            // pos 위치에서 프리팹 생성
            GameObject ball = Instantiate(snakeBallPrefab, pos.position, Quaternion.identity);
            ball.SetActive(false);
            objectPool.Add(ball);
            objectPoolPositions.Add(pos.position);
        }
    }

    private void SpawnSnakeBall()
    {
        // 오브젝트 풀에서 사용 가능한 오브젝트 가져오기
        GameObject ball = GetAvailableObjectFromPool();
        // pos 위치에서 프리팹 생성
        // GameObject ball = Instantiate(snakeBallPrefab, pos.position, Quaternion.identity);

        if (ball != null)
        {
            // 타겟 선정
            Player target = FindObjectOfType<Player>();

            if (target != null)
            {
                HasTarget = true; // 타겟이 있을 경우 HasTarget을 true로 설정
                ball.GetComponent<SnakeBall>().SetTarget(target);

                // Snake의 방향 전환
                if (target.transform.position.x > transform.position.x) // 타겟의 x 좌표가 Snake의 x 좌표보다 큰 경우 true, 이것이 참이면 타겟이 오른쪽에 위치했다는 것을 의미
                {
                    // 플레이어가 오른쪽에 있는 경우
                    transform.localScale = new Vector3(-1f, 1f, 1f); // 스프라이트의 x 스케일을 음수로 설정하여 오른쪽 방향을 향하도록 함
                }
                else
                {
                    // 플레이어가 왼쪽에 있는 경우
                    transform.localScale = new Vector3(1f, 1f, 1f); // 스프라이트의 x 스케일을 양수로 설정하여 왼쪽 방향을 향하도록 함
                }
            }
            else
            {
                HasTarget = false; // 타겟이 없을 경우 HasTarget을 false로 설정
                ball.GetComponent<SnakeBall>().SetTarget(null);
            }
        }
    }

    private GameObject GetAvailableObjectFromPool()
    {
        // 오브젝트 풀에서 사용 가능한 오브젝트 찾기
        for (int i = 0; i < objectPool.Count; i++)
        {
            int index = (currentIndex + i) % objectPool.Count; // 현재 인덱스를 기준으로 순환하며 오브젝트를 가져오기 위해 인덱스 계산

            if (!objectPool[index].activeInHierarchy)
            {
                currentIndex = (index + 1) % objectPool.Count; // 다음에 가져올 오브젝트의 인덱스를 저장
                objectPool[index].transform.position = objectPoolPositions[index]; // 비활성화된 오브젝트의 위치를 생성 위치로 이동
                objectPool[index].SetActive(true);
                return objectPool[index];
            }
        }
        // 풀에 더 이상 사용 가능한 오브젝트가 없는 경우
        // 새로운 오브젝트 생성해서 풀에 추가
        GameObject newObj = Instantiate(snakeBallPrefab, pos.position, Quaternion.identity);
        objectPool.Add(newObj);
        objectPoolPositions.Add(pos.position);
        newObj.SetActive(true);
        currentIndex = objectPool.Count - 1; // 새로 생성된 오브젝트의 인덱스를 저장
        return newObj;
    }

    void FlipSpriteBasedOnDirection()
    {
        // 스프라이트 방향을 벡터의 x값에 따라 변경
        if (walkDirectionVector.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            // Scale로 좌우반전이 가능
        }
        else if (walkDirectionVector.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            // Scale로 좌우반전이 가능 - 음수가 붙으면 좌우반전
        }
    }
    void FlipDirection()
    {

        walkDirectionVector *= -1; // 방향 벡터를 반대로 전환
    }

    void Flight()
    {
        // 다음 포인트로 비행
        Vector2 directionToWaypoint = (nextwaypoint.position - transform.position).normalized;

        // 포인트에 도달했는지 확인
        float distance = Vector2.Distance(nextwaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;

        UpdateDirection();

        // 포인트 전환해야하는지 확인
        if (distance <= waypointReachedDistance)
        {
            // 다음 포인트로 전환
            waypointNum++;
            if (waypointNum >= waypoints.Count)
            {
                // 원래 포인트로 돌아가기
                waypointNum = 0;
            }
            nextwaypoint = waypoints[waypointNum];
        }
    }

    private void MoveAttack()
    {

        // 가장 가까운 적 찾기
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
            // 적 위치를 바라보도록 회전
            Vector3 diff = nearestCollider.transform.position - transform.position;

            // 몬스터 방향으로 이동
            Vector2 moveDirection = diff.normalized;
            rb.velocity = moveDirection * attackSpeed;

            if (nearestDistance <= waypointReachedDistance)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    // 바라보는 방향에 맞게 오브젝트 전환
    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale; // Scale에 -1을 하면 좌우반전
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
