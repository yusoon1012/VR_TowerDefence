using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Boss_Hp_Test : MonoBehaviour
{
    public float currentHp = 0;
    public float maxHp = 100;
    public Slider hpSlider;
    // Start is called before the first frame update
    void Start()
    {
        currentHp=maxHp;
        hpSlider.value = currentHp/maxHp;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoseHp(int damage)
    {
        currentHp -= damage;
        hpSlider.value = currentHp/maxHp;
    }
}
