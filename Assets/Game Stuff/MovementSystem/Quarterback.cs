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

    public void ThrowToReceiver(Player player)
    {
        if (!hasPossession || ThrowToReceiver == null) return;

        Vector3 qbPos = Hands.transform.position;
        Vector3 receiverPosition = player.transform.position;
        Vector3 receiverVel = player.velocity;
        Vector3 target;
        float flightTime;

        bool foundIntercept = ThrowingStuff.TryGetInterceptPoint(qbPos, receiverPosition, receiverVel, throwPower, out target, out flightTime);

        if (!foundIntercept)
        {
            target = receiverPosition;
            flightTime = Vector3.Distance(qbPos, receiverPosition) / throwPower;
        }

        float throwDistanceYards = Vector3.Distance(qbPos, target);
        target += ThrowingStuff.GetAccuracyOffset(accuracy, throwDistanceYards);

        FootballLogic.instance.CurrentBallCarrier = player;
        FootballLogic.instance.Throwto(targetpoint, flightTime);
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

    public void ThrowToReceiver4(InputAction.CallbackContext cntxt)
    {
        if (!hasPossession)
        {
            return;
        }

        if (cntxt.performed)
        {
            Player receiver = PossessionManager.instance.offense[2];
            Transform fieldPosition = receiver.transform;
            FootballLogic.instance.receiverPosition = fieldPosition;
            FootballLogic.instance.currentBallCarrier = receiver;
            FootballLogic.instance.StateChange("Thrown");
        }
    }
}
