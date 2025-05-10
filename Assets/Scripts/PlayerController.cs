using System.Numerics;
using UnityEngine;
using System.Collections;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    public float speed = 8f; // 기본 이동 속도
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

    void Update()
    {
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

        // 입력 감지
        if(Input.GetKey(KeyCode.UpArrow))
        {
            moveVertical = 1f;
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            moveVertical = -1f;
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
            moveHorizontal = -1f;
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
            //GetComponent<spriterenderer>().flipX = true;
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

        //3D 이동
        //Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        transform.Translate(movement * (appliedSpeed * Time.deltaTime));
        //transform.Translate();
        
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
