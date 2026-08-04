using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public enum ReceiverState
{
    PreSnap,
    RunningRoute,
    Scrambling,
    Catching,
    Blocking,

}

public class WideReceiver : Player
{
    public ReceiverState ReceiverState { get; private set; } = ReceiverState.PreSnap;

    private float catching;
    private float blocking;
    private float maxSpeed = 5f;

    [SerializeField] private float waypointTolerance = 0.15f;

    private Vector3[] worldWaypoints;
    private int currentIndex;

    public bool isRightOfCenter;

    // Call this once, at the moment the route should start.
    public void StartRoute(Vector2[] offsets, Vector3 snapSpot, Vector3 fieldRight, Vector3 fieldForward)
    {
        worldWaypoints = new Vector3[offsets.Length];
        for (int i = 0; i < offsets.Length; i++)
        {
            float lateral = isRightOfCenter ? -offsets[i].x : offsets[i].x;
            worldWaypoints[i] = snapSpot + fieldRight * lateral + fieldForward * offsets[i].y;
        }

        currentIndex = 0;
        ReceiverState = ReceiverState.RunningRoute;
    }

    private void Update()
    {
        if (ReceiverState == ReceiverState.RunningRoute)
        {
            RunTowardCurrentWaypoint();
        }
    }

    private void RunTowardCurrentWaypoint()
    {
        Vector3 target = worldWaypoints[currentIndex];
        float trueSpeed = (speed / 100f) * maxSpeed;
        transform.position = Vector3.MoveTowards(transform.position, target, trueSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) <= waypointTolerance)
        {
            currentIndex++;
            if (currentIndex >= worldWaypoints.Length)
            {
                ReceiverState = ReceiverState.Scrambling; // route's over, nothing left to run
            }
        }
    }
}
