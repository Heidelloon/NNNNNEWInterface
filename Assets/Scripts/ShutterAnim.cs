using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutterAnim : MonoBehaviour
{
    private Animator myAnimator;
    private myInventoryClass inventory; // Reference to the player's inventory script

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        inventory = FindObjectOfType<myInventoryClass>(); // Find the player's inventory script
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider has the "Player" tag and if the player's inventory contains a card
        if (other.CompareTag("Player") && inventory != null && inventory.IsCardInInventory())
        {
            // Activate the Animator component if it exists
            if (myAnimator != null)
            {
                myAnimator.enabled = true;
            }
        }
    }
}
