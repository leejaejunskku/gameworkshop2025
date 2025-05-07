using UnityEngine;

public class BulletSpawner : MonoBehaviour
{

    public GameObject bulletPrefab; // Reference to the bullet prefab
    public float spawnRateMin = 0.5f; // Rate at which bullets are spawned
    public float spawnRateMax = 3.0f; // Maximum spawn rate

    private Transform target; // Reference to the target transform
    private float spawnRate; // Current spawn rate MIN/MAX ������ ���� ����
    private float timeAfterSpawn; // Time since the last bullet was spawned

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        timeAfterSpawn = 0.0f; // Initialize time since last spawn

        spawnRate = Random.Range(spawnRateMin, spawnRateMax); // Set a random spawn rate between min and max

        target = FindFirstObjectByType<PlayerController>().transform; // Find the player controller in the scene and get its transform



    }

    // Update is called once per frame
    void Update()
    {

        timeAfterSpawn += Time.deltaTime; // Increment time since last spawn

        if (timeAfterSpawn >= spawnRate) // Check if enough time has passed to spawn a new bullet
        {
            
            // Instantiate a new bullet at the spawner's position with no rotation
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity); // Create a new bullet instance
            
            // Initialize the bullet's properties
            bullet bulletSCript = bullet.GetComponent<bullet>();
            bulletSCript.Initialize();

            // 랜덤한 오프셋 생성 (반지름 1 구체 내의 랜덤한 점)
            Vector2 randomOffset = Random.insideUnitCircle * 2.0f;
            randomOffset.y = 0f; // Y축은 0으로 고정
            
            // 타겟 위치에 랜덤 오프셋을 더한 위치를 바라보도록 설정
            Vector2 randomTargetPos = (Vector2)target.position + randomOffset;
            //bullet.transform.LookAt(randomTargetPos);
            
            Vector2 direction = ((Vector2)target.position + randomOffset - (Vector2)bullet.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

            spawnRate = Random.Range(spawnRateMin, spawnRateMax); // Set a new random spawn rate

            timeAfterSpawn = 0.0f; // Reset time since last spawn

        }

        
    }
}
