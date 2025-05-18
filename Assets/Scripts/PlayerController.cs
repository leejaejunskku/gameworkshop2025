using System.Numerics;
using UnityEngine;
using System.Collections;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    public Animator animator; // 애니메이터 컴포넌트
    
    private Rigidbody2D rb; // 리지드바디 컴포넌트
    
    public float speed = 4f; // 기본 이동 속도
    public float accelerationRate = 2f; // 가속도 증가율
    public float decelerationRate = 3f; // 감속도 감소율
    public float maxSpeed = 16f; // 최대 속도
    public float vulnerableDuration = 3f; // 취약 상태 지속 시간
    public float slowedSpeedMultiplier = 0.5f; // 피격 시 속도 감소 비율
    private float currentSpeed; // 현재 속도
    private bool isVulnerable = false; // 취약 상태 여부
    private float vulnerableTimer = 0f; // 취약 상태 타이머
    private Vector2 targetPosition; // 마우스로 클릭한 목표 위치
    private bool isMovingToTarget = false; // 목표 지점으로 이동 중인지 여부

    public float jumpHeight = 2.5f; // 점프 높이
    public float jumpDuration = 0.1f; // 점프 전체 소요 시간
    private bool isJumping = false; // 점프 중인지 확인
    private float jumpTimer = 0f; // 점프 타이머
    private float initialY; // 점프 시작 위치

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        currentSpeed = speed;
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 초기화
    }
    
    void Update()
    {
        bool isMoving = Input.GetKey(KeyCode.UpArrow) || 
                        Input.GetKey(KeyCode.DownArrow) || 
                        Input.GetKey(KeyCode.LeftArrow) || 
                        Input.GetKey(KeyCode.RightArrow);
    
        animator.SetBool("run", isMoving);

        // 점프 중이 아닐 때만 새로운 점프 가능
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartJump();
            animator.SetTrigger("jump");
        }

        // 점프 처리
        if (isJumping)
        {
            ProcessJump();
        }
        
        // 마우스 입력 처리
        if (Input.GetMouseButtonUp(0)) // 마우스 왼쪽 버튼을 뗄 때
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
            isMovingToTarget = true;
        }

        float moveVertical = 0f;
        float moveHorizontal = 0f;

        // 키보드 입력이 있으면 마우스 이동을 취소
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            isMovingToTarget = false;
        }

        // 취약 상태 타이머 관리
        if (isVulnerable)
        {
            vulnerableTimer -= Time.deltaTime;
            if (vulnerableTimer <= 0f)
            {
                isVulnerable = false;
            }
        }

        // 점프 중이 아닐 때만 수직 이동 허용
        if (!isJumping)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                moveVertical = 1f;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                moveVertical = -1f;
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
            moveHorizontal = -1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
            moveHorizontal = 1f;
        }

        // 마우스 클릭 위치로 이동
        if (isMovingToTarget)
        {
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            moveHorizontal = direction.x;
            moveVertical = direction.y;

            // 목표 지점에 거의 도달했으면 이동 종료
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMovingToTarget = false;
            }

            // 스프라이트 방향 설정
            if (moveHorizontal != 0)
            {
                this.GetComponent<SpriteRenderer>().flipX = (moveHorizontal < 0);
            }
        }

        // 이동 입력이 있을 때 가속
        if (moveHorizontal != 0f || moveVertical != 0f)
        {
            currentSpeed = Mathf.Min(currentSpeed + accelerationRate * Time.deltaTime, maxSpeed);
        }
        // 이동 입력이 없을 때 감속
        else
        {
            currentSpeed = Mathf.Max(currentSpeed - decelerationRate * Time.deltaTime, speed);
        }

        // 취약 상태일 때 속도 감소 적용
        float appliedSpeed = isVulnerable ? currentSpeed * slowedSpeedMultiplier : currentSpeed;
        
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;

        
        Debug.Log("Movement" + movement.x + ", " + movement.y);
        Debug.Log("appliedsppeed" + appliedSpeed);
        Debug.Log("time.deltatime" + Time.deltaTime);
        //3D 이동
        //Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        transform.Translate(movement * (appliedSpeed * Time.deltaTime));
        //rb.MovePosition(rb.position + movement * (appliedSpeed * Time.fixedDeltaTime));
        //transform.Translate();
        
    }

    private void StartJump()
    {
        isJumping = true;
        jumpTimer = 0f;
        initialY = transform.position.y;
    }

    private void ProcessJump()
    {
        jumpTimer += Time.deltaTime;
        float progress = jumpTimer / jumpDuration;
        
        if (progress >= 1f)
        {
            isJumping = false;
            return;
        }

        // 포물선 형태의 점프 곡선 (위로 올라갔다가 아래로 내려옴)
        float heightMultiplier = Mathf.Sin(progress * Mathf.PI);
        float newY = initialY + (jumpHeight * heightMultiplier);
        
        Vector3 currentPos = transform.position;
        transform.position = new Vector3(currentPos.x, newY, currentPos.z);
    }

    public void Die()
    {
        gameObject.SetActive(false); // Deactivate the player object

        GameManager gameManager = FindFirstObjectByType<GameManager>();
        gameManager.EndGame();
    }
    
    public void OnHit(float fDamage)
    {
        Debug.Log("이만큼 아프다:" + fDamage);
        if (isVulnerable)
        {
            // 취약 상태에서 다시 맞으면 사망
            gameObject.SetActive(false);
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            gameManager.EndGame();
        }
        else
        {
            StartCoroutine(ShakePlayer());
            // 첫 피격 시 취약 상태 진입
            isVulnerable = true;
            vulnerableTimer = vulnerableDuration;
        }
    }
    
    public void OnReward(float fReward)
    {
        Debug.Log("이만큼 보상:" + fReward);
        // 보상 처리 로직 추가
        gameObject.transform.localScale *= fReward;
    }
    
    
    private IEnumerator ShakePlayer()
    {
        Vector2 originalPosition = transform.position;
        float elapsed = 0f;
        float duration = 0.2f;
        float magnitude = 0.3f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = originalPosition + new Vector2(x, y);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }
}
