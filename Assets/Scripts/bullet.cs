using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class bullet : MonoBehaviour
{

    public float speed = 1.0f;
    
    public float damage;
    public float reward;
    public BulletType bulletType;
    
    private Rigidbody2D bulletRigidbody;

    
    // 생성자 역할을 하는 초기화 메서드
    public void Initialize(float speed = 0, float damage = 0, BulletType? type = null, float reward = 0)
    {
        // speed, damage, reward가 0이면 랜덤 값 설정
        // speed = speed == 0 ? Random.Range(5f, 10f) : speed;
        // damage = damage == 0 ? Random.Range(10f, 30f) : damage;
        // reward = reward == 0 ? Random.Range(1f, 2f) : reward;
        
        this.gameObject.tag = "Bullet";
        
        this.speed = speed == 0 ? Random.Range(5f, 10f) : speed;
        this.damage = damage == 0 ? Random.Range(10f, 30f) : damage;
        this.reward = reward == 0 ? Random.Range(1f, 2f) : reward;
            
        this.bulletType = type ?? (BulletType)Random.Range(0, 5);
        // 총알 타입에 따른 이미지/색상 변경
        SetBulletAppearance();
    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        // 기본 초기화
        if (speed == 0) Initialize();
        
        bulletRigidbody = GetComponent<Rigidbody2D>();
        bulletRigidbody.linearVelocity = transform.right * speed;
        
        Destroy(gameObject, 10.0f);
        


    }

    // Update is called once per frame
    void Update()
    {


        
    }
    
    private void SetBulletAppearance()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            switch (bulletType)
            {
                case BulletType.Normal:
                    spriteRenderer.color = Color.white;
                    spriteRenderer.sprite = Resources.Load<Sprite>("Bullets/NormalBullet");
                    AdjustSpriteToCollider(spriteRenderer);
                    break;
                case BulletType.Fire:
                    spriteRenderer.color = Color.red;
                    spriteRenderer.sprite = Resources.Load<Sprite>("Bullets/FireBullet");
                    AdjustSpriteToCollider(spriteRenderer);
                    break;
                case BulletType.Ice:
                    spriteRenderer.color = Color.blue;
                    spriteRenderer.sprite = Resources.Load<Sprite>("Bullets/IceBullet");
                    AdjustSpriteToCollider(spriteRenderer);
                    break;
                case BulletType.Lightning:
                    spriteRenderer.color = Color.yellow;
                    spriteRenderer.sprite = Resources.Load<Sprite>("Bullets/LightningBullet");
                    AdjustSpriteToCollider(spriteRenderer);
                    break;
                case BulletType.Poison:
                    spriteRenderer.color = Color.green;
                    spriteRenderer.sprite = Resources.Load<Sprite>("Bullets/PoisonBullet");
                    AdjustSpriteToCollider(spriteRenderer);
                    break;
            }
        }
    }
    
    private void AdjustSpriteToCollider(SpriteRenderer spriteRenderer)
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null && spriteRenderer.sprite != null)
        {
            Vector2 colliderSize = collider.bounds.size;
            Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        
            transform.localScale = new Vector3(
                colliderSize.x / spriteSize.x,
                colliderSize.y / spriteSize.y,
                1f
            );
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        //파랑 빨강은 먹으면 아프다.
        if (other.CompareTag("Player") && (bulletType == BulletType.Ice || bulletType == BulletType.Fire ))
        {
            
            
            
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.OnHit(damage);
            }
            Destroy(gameObject);
        }
        else// if (other.CompareTag("Player")&& (!(bulletType == BulletType.Ice) || !(bulletType == BulletType.Fire) ))
        {
            
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.OnReward(reward);
            }
            Destroy(gameObject);
            
        }
            
    }



}
