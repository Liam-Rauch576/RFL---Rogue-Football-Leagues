using UnityEngine;
using UnityEngine.InputSystem;


public class Quarterback : Player
{

    public float throwPower;
    public float accuracy;

    private float ballSpeed = 25f;

    private PlayerInput playerInput;

    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions.FindActionMap("Player").Enable();
    }

    public void ThrowToReceiver(Player player)
    {
        if (!hasPossession || (player == null)) return;

        Vector3 qbPos = Hands.transform.position;
        Vector3 receiverPosition = player.transform.position;
        Vector3 receiverVel = player.Velocity;
        Vector3 target;
        float flightTime;

        bool foundIntercept = ThrowingStuff.TryGetInterceptPoint(qbPos, receiverPosition, receiverVel, ballSpeed, out target, out flightTime);


        if (!foundIntercept)
        {
            target = receiverPosition;
            flightTime = (Vector3.Distance(qbPos, receiverPosition)/ballSpeed);
        }

        float throwDistanceYards = Vector3.Distance(qbPos, target);
        //target += ThrowingStuff.GetAccuracyOffset(accuracy, throwDistanceYards);

        FootballLogic.instance.currentBallCarrier = player;
        FootballLogic.instance.ThrowTo(target, flightTime);
    }

    public void ThrowToReceiver1(InputAction.CallbackContext cntxt)
    {
        if (!hasPossession)
        {
            return;
        }

        if (cntxt.performed)
        {
            ThrowToReceiver(PossessionManager.instance.offense[3]);
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
            ThrowToReceiver(PossessionManager.instance.offense[4]);
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
            ThrowToReceiver(PossessionManager.instance.offense[5]);
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
            ThrowToReceiver(PossessionManager.instance.offense[2]);

        }
    }
}
