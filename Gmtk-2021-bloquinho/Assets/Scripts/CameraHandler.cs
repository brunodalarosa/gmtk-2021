using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
   
    private Transform Target;
    [SerializeField] private float SmoothSpeed = 1.3f;

    [SerializeField]
    private float LeftLimit = 0;
    [SerializeField]
    private float RightLimit = 0;
    [SerializeField]
    private float BottomLimit = 0;
    [SerializeField]
    private float UpperLimit = 0;
    
    void Start()
    {
        SetPlayerAsTarget();
    }

    void LateUpdate()
    {
        if(Target)
        {
            Vector3 desiredPosition = new Vector3(
                Mathf.Clamp(Target.transform.position.x, LeftLimit, RightLimit),
                Mathf.Clamp(Target.transform.position.y, BottomLimit, UpperLimit),
                -10);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed * Time.deltaTime);

            transform.position = smoothedPosition;

        }
    }

    public void SetTarget(Transform target)
    {
        this.Target = target;
    }

    public Transform GetTarget()
    {
        return this.Target; 
    }

    public void SetPlayerAsTarget()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

}
