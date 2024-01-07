using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vaulting : MonoBehaviour
{
    public float vaultHeight = 1.5f; // Adjust this based on your game's requirements
    public LayerMask vaultableLayers; // Define layers that can be vaulted over
    public float vaultForce = 5f; // Adjust this based on your game's feel

    private bool isVaulting = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isVaulting)
        {
            TryVault();
        }
    }

    void TryVault()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.0f, vaultableLayers))
        {
            StartCoroutine(VaultSequence(hit.point));
        }
    }

    IEnumerator VaultSequence(Vector3 targetPosition)
    {
        isVaulting = true;

        // Calculate the target position for the player to vault to
        Vector3 targetVaultPosition = new Vector3(targetPosition.x, targetPosition.y + vaultHeight, targetPosition.z);

        // Calculate the vault direction
        Vector3 vaultDirection = (targetVaultPosition - transform.position).normalized;

        // Apply force to the Rigidbody for a physics-based vault
        GetComponent<Rigidbody>().AddForce(vaultDirection * vaultForce, ForceMode.Impulse);

        // Wait for the player to complete the vault
        yield return new WaitForSeconds(0.5f); // Adjust this based on your game's feel

        // Reset the flag after completing the vault
        isVaulting = false;
    }
}
