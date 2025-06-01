using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [System.Serializable]
    public class ItemUI
    {
        public Image icon;
        public Image cooltime;
        public Text text;
    }

    public ItemUI[] itemUIs = new ItemUI[5];
    private Dictionary<ItemType, float> cooldowns = new Dictionary<ItemType, float>();
    private Dictionary<ItemType, float> durations = new Dictionary<ItemType, float>();
    private PlayerController playerController;
    private List<(GameObject bullet, Vector3 originalVelocity)> stoppedBullets = new List<(GameObject bullet, Vector3 originalVelocity)>();

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        
        // 모든 아이템 초기화
        foreach (ItemType type in System.Enum.GetValues(typeof(ItemType)))
        {
            cooldowns[type] = 0f;
            durations[type] = 0f;
        }
    }

    void Update()
    {
        // 키 입력 처리
        if (Input.GetKeyDown(KeyCode.Alpha1)) TryUseItem(ItemType.HP);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TryUseItem(ItemType.MP);
        if (Input.GetKeyDown(KeyCode.Alpha3)) TryUseItem(ItemType.TimeStop);
        if (Input.GetKeyDown(KeyCode.Alpha4)) TryUseItem(ItemType.Shield);
        if (Input.GetKeyDown(KeyCode.Alpha5)) TryUseItem(ItemType.SpeedUp);

        // 쿨다운 및 지속시간 업데이트
        foreach (ItemType type in System.Enum.GetValues(typeof(ItemType)))
        {
            UpdateItemStatus(type);
        }
    }

    private void TryUseItem(ItemType type)
    {
        if (cooldowns[type] <= 0)
        {
            UseItem(type);
            cooldowns[type] = type.GetCooldown();
            if (type.GetDuration() > 0)
            {
                durations[type] = type.GetDuration();
            }
        }
    }

    private void UseItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.HP:
                // HP 회복 로직
                break;
            case ItemType.MP:
                // MP 회복 로직
                break;
            case ItemType.TimeStop:
                GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
                stoppedBullets.Clear();
                
                foreach (GameObject bullet in bullets)
                {
                    Debug.Log(bullet.transform.gameObject.name+ "을 처리합니다");
                    
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        stoppedBullets.Add((bullet, rb.linearVelocity));
                        rb.linearVelocity = Vector2.zero;
                        Debug.Log(rb.gameObject.name + "의 속도를 멈췄습니다.");
                    }
                }
                break;
            case ItemType.Shield:
                // 무적 상태 로직
                break;
            case ItemType.SpeedUp:
                // 속도 증가 로직
                break;
        }
    }

    private void UpdateItemStatus(ItemType type)
    {
        int index = (int)type;
        
        // 쿨다운 업데이트
        if (cooldowns[type] > 0)
        {
            cooldowns[type] -= Time.deltaTime;
            itemUIs[index].cooltime.fillAmount = cooldowns[type] / type.GetCooldown();
        }

        // 지속시간 업데이트
        if (durations[type] > 0)
        {
            durations[type] -= Time.deltaTime;
            if (durations[type] <= 0)
            {
                // 효과 종료 처리
                DeactivateItemEffect(type);
            }
        }
    }

    private void DeactivateItemEffect(ItemType type)
    {
        switch (type)
        {
            case ItemType.TimeStop:
                foreach (var bulletInfo in stoppedBullets)
                {
                    if (bulletInfo.bullet != null)
                    {
                        Rigidbody2D rb = bulletInfo.bullet.GetComponent<Rigidbody2D>();
                        if (rb != null)
                        {
                            rb.linearVelocity = bulletInfo.originalVelocity;
                        }
                    }
                }
                stoppedBullets.Clear();
                break;
            case ItemType.Shield:
                // 무적 해제
                break;
            case ItemType.SpeedUp:
                // 속도 원복
                break;
        }
    }
}
