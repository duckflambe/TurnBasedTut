using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    private static MouseWorld instance;

    private void Awake()
    {
        instance = this;
    }
    
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, float.MaxValue, layerMask))
        {
            transform.position = hit.point;
        }
    }

    public static Vector3 GetPosition()
    {
        return instance.transform.position;
    }
}
