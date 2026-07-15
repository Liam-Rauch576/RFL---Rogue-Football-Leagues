using UnityEngine;
using UnityEngine.InputSystem;

public class Center : Player
{
    
    public float blocking;
    public float catching;

    private void Start()
    {
        PossessionManager.instance.GivePossession(this);
    }

    public void OnSnap(InputAction.CallbackContext cntxt)
    {
        if (!hasPossession) return;
        if (cntxt.performed)
        {
            FootballLogic.instance.SnapMovement();
            PossessionManager.instance.GivePossession(PossessionManager.instance.offense[1]);
            
        }
    }


}
