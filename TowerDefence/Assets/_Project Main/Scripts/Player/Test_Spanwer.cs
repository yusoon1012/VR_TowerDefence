using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Spanwer : MonoBehaviour
{
    public GameObject meteorPrefab;
    float spawnTimer = 0;
    float spawnRate = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if(spawnTimer>=spawnRate) 
        {
            GameObject meteor=Instantiate(meteorPrefab,transform.position,transform.rotation);
            spawnTimer = 0;
        }
    }
}
