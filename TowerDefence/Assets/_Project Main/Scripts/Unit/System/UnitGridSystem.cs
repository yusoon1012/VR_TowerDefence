using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGridSystem : MonoBehaviour
{
    private int gridLayer = -1; // 겹치면 그리드를 구현할 레이어
    public Material unitGridMaterial; // 겹치는 영역에 적용할 Material

    private void Awake()
    {
        gridLayer = 1 << LayerMask.NameToLayer("TransparentFX");
    }

    private void Start()
    {
        Collider[] gridColliders = Physics.OverlapSphere(transform.position, 1.0f, gridLayer);

        // 바닥과 겹치는 Sphere 영역에 Material 할당
        foreach (Collider collider in gridColliders)
        {
            Renderer renderer = collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = unitGridMaterial;
            }
        }

        //foreach (Collider collider in gridColliders)
        //{
        //    Renderer renderer = collider.GetComponent<Renderer>();
        //    if (renderer != null)
        //    {
        //        renderer.material = unitGridMaterial;
        //    }
        //}
    }
}
