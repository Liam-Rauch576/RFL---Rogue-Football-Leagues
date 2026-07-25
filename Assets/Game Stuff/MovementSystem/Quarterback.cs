using UnityEngine;
using UnityEngine.InputSystem;


public class Quarterback : Player
{

    public float throwPower;
    public float accuracy;

    private PlayerInput playerInput;

    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions.FindActionMap("Player").Enable();
    }

    public void ThrowToReceiver1(InputAction.CallbackContext cntxt)
    {
        if (!hasPossession)
        {
            return;
        }

        if (cntxt.performed)
        {
            Player receiver = PossessionManager.instance.offense[3];
            Transform fieldPosition = receiver.transform;
            FootballLogic.instance.receiverPosition = fieldPosition;
            FootballLogic.instance.currentBallCarrier = receiver;
            FootballLogic.instance.StateChange("Thrown");
        }
    }

    public void ThrowToReceiver2(InputAction.CallbackContext cntxt)
    {
        if (!hasPossession)
        {
            return;
        }

        if (cntxt.performed)
        {
            Player receiver = PossessionManager.instance.offense[4];
            Transform fieldPosition = receiver.transform;
            FootballLogic.instance.receiverPosition = fieldPosition;
            FootballLogic.instance.currentBallCarrier = receiver;
            FootballLogic.instance.StateChange("Thrown");
        }
    }

    public void ThrowToReceiver3(InputAction.CallbackContext cntxt)
    {
        if (!hasPossession)
        {
            return;
        }

        if (cntxt.performed)
        {
            Player receiver = PossessionManager.instance.offense[5];
            Transform fieldPosition = receiver.transform;
            FootballLogic.instance.receiverPosition = fieldPosition;
            FootballLogic.instance.currentBallCarrier = receiver;
            FootballLogic.instance.StateChange("Thrown");
        }
    }
}
