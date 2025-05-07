using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    
    public float rotationSpeed = 10f; // Speed of rotation
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //업데이트가 호출되는 속도가 컴퓨터마다 다르다. 
        //좋은 컴퓨터는 업데이트가 빨리 되고, 
        //나쁜 컴퓨터는 업데이트가 느리게 된다.
        //좋은 컴퓨터의 update 0.5초에 1회 호출되고 == deltaTime 0.5초
        //나쁜 컴퓨터는 2초에 1회 호출된다. == deltaTime 2초
        //실제 물리세계에서의 시간이 10초 지났다고 했을 때 좋은 컴퓨터는 20회, 나쁜 컴퓨터에서는 5회
        //10, 0.5 * 20 = 100
        //10, 5 * 2 = 100
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        
    }
}
