using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vaulting : MonoBehaviour
{
    public float climbDistance = 1f;
    public float climbSpeed = 5f; // Adjust this value to control the climbing speed

    private bool isClimbing = false;
    private Vector3 targetPosition;
    public LayerMask vaultableLayer;
    //private Animator anim;

    void Start()
    {
        //anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isClimbing)
        {
            TryClimb();
        }

        if (isClimbing)
        {
            MoveToTargetPosition();
        }
    }

    void TryClimb()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, climbDistance, vaultableLayer))
        {
            targetPosition = hit.transform.position + Vector3.up * hit.transform.localScale.y / 2f;
            isClimbing = true;
            //anim.SetBool("isVaulting", true);
        }
    }

    void MoveToTargetPosition()
    {
        // Gradually move the player to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, climbSpeed * Time.deltaTime);

        // Check if the player has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isClimbing = false;
            //anim.SetBool("isVaulting", false);
        }
    }
}
