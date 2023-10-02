using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    public Transform GetNearest()
    {
        Transform result = null;
        float diff = 100f;

        foreach(var target in targets)
        {
            float dis = Vector3.Distance(transform.position, target.transform.position);
            if (dis < diff)
            {
                diff = dis;
                result = target.transform;
            }
        }

        return result;
        
    }

}
