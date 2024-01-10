using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    public float grapplingHookRange = 10f;
    public float grapplingHookSpeed = 20f;
    public float maxSwingForce = 100f;
    public float swingAcceleration = 20f;
    public LayerMask grapplingLayer;
    public Transform ropeOrigin;

    private bool isGrappling = false;
    private Vector3 grapplingPoint;
    private Rigidbody playerRigidbody;
    private SpringJoint grapplingJoint;
    private LineRenderer ropeRenderer;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        grapplingJoint = gameObject.AddComponent<SpringJoint>();
        grapplingJoint.maxDistance = 0.1f;
        grapplingJoint.autoConfigureConnectedAnchor = false;

        ropeRenderer = gameObject.AddComponent<LineRenderer>();
        ropeRenderer.positionCount = 2;
        ropeRenderer.startWidth = 0.1f;
        ropeRenderer.endWidth = 0.1f;
        ropeRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isGrappling)
            {
                TryStartGrappling();
            }
            else
            {
                StopGrappling();
            }
        }

        if (isGrappling)
        {
            UpdateRopeRenderer();
            UpdateSwingForce();
        }
    }

    void FixedUpdate()
    {
        if (isGrappling)
        {
            ApplySwingForce();
        }
    }

    void TryStartGrappling()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grapplingHookRange, grapplingLayer))
        {
            StartGrappling(hit.point);
        }
    }

    void StartGrappling(Vector3 targetPoint)
    {
        isGrappling = true;
        grapplingPoint = targetPoint;

        grapplingJoint.connectedBody = null;
        grapplingJoint.connectedAnchor = grapplingPoint;
        grapplingJoint.spring = 0f;

        ropeRenderer.enabled = true;
        ropeRenderer.SetPosition(0, ropeOrigin.position);
        ropeRenderer.SetPosition(1, grapplingPoint);
    }

    void StopGrappling()
    {
        isGrappling = false;
        grapplingJoint.connectedBody = null;
        ropeRenderer.enabled = false;
    }

    void UpdateRopeRenderer()
    {
        ropeRenderer.SetPosition(0, ropeOrigin.position);
        ropeRenderer.SetPosition(1, grapplingPoint);
    }

    void UpdateSwingForce()
    {
        float distanceToGrapplingPoint = Vector3.Distance(transform.position, grapplingPoint);

        if (Input.GetKey(KeyCode.Space) && distanceToGrapplingPoint > 1f)
        {
            grapplingJoint.spring = maxSwingForce;
        }
        else
        {
            grapplingJoint.spring = 0f;
        }
    }

    void ApplySwingForce()
    {
        Vector3 playerToGrapplingPoint = grapplingPoint - transform.position;
        Vector3 perpendicular = Vector3.Cross(playerToGrapplingPoint, Vector3.up).normalized;
        Vector3 swingForce = perpendicular * swingAcceleration;

        playerRigidbody.AddForce(swingForce, ForceMode.Acceleration);
    }
}
