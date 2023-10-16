using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour
{
    public static HitManager instance = null;

    #region ╫л╠шео
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }       // Awake()
    #endregion

    private void Hit(GameObject _target)
    { 
    
    }
}
