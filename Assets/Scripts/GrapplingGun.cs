using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappable;
    public Transform gunTip, camera, player;
    public float maxDistance = 100f;
  
    public float grapplingSpeed;
    private SpringJoint joint;
    private Rigidbody rb;
   

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    void LateUpdate()
    {
        DrawRope();
        
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4f;
            joint.damper = 9f;
            joint.massScale = 5f;

            lr.positionCount = 2;

            //move towards grapple point
            
        }
    }

    void DrawRope()
    {
        if (!joint)
        {
            return;
        }

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

      
}
