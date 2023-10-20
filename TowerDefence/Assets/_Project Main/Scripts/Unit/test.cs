using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject prefab = default;


    private void Start()
    {
        Quaternion rotation = Quaternion.Euler(0, 130, 0);
        GameObject obj = Instantiate(prefab, new Vector3(0, 0, 0), rotation);
        
    }
}
