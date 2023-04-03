using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVSystem : MonoBehaviour
{
    public float viewRadius;            //시야 거리
    public float viewAngle;             //시야 각

    public LayerMask targetMask;        //Target 레이어에서 가져옴
    public LayerMask obstacleMask;      //oibstacleMask 레이어에서 가져옴

    public List<Transform> visibleTargets = new List<Transform>();                  //보이는 타겟 리스트

    // Start is called before the first frame update
    public virtual void Start()
    {
        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTartgetColor(Color.white);
        visibleTargets.Clear();

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for(int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward , dirToTarget) < viewAngle/2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, dirToTarget , dstToTarget , obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }

        visibleTartgetColor(Color.green);
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    void visibleTartgetColor(Color color)
    {
        for(int i = 0; i < visibleTargets.Count; i++)
        {
            visibleTargets[i].GetComponent<Renderer>().material.SetColor("_Color", color);
        }

    }
}
