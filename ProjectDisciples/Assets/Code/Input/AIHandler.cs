using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AIHandler : MonoBehaviour
{
    // Interfaces
    ICharacterMovement[] iMovement;
    ICharacterElement iAttack;
    ICharacterAim[] iAim;

    void Start()
    {
        // Destroy all player related components
        if (TryGetComponent(out PlayerHandler playerHandler))
        {
            Destroy(playerHandler);
        }
        if (TryGetComponent(out UnityEngine.InputSystem.PlayerInput input))
        {
            Destroy(input);
        }

        //Assign interfaces
        iMovement = GetComponents<ICharacterMovement>();
        iAttack = GetComponent<ICharacterElement>();
        iAim = GetComponents<ICharacterAim>();
    }

    void Update()
    {
        
    }
}