using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    protected Rigidbody rb;
    public Transform Hands;
    private Vector2 moveInput;
    private bool running = false;
    public bool hasPossession = false;

    [Header("Stats")]
    public string name;
    public float speed;
    public float strength;
    public float agility;

    [Header("Route Running")]
    public float waypointArrivalthreshold = .5f;
    private readonly List<Vector3> routeWaypoints = new List<Vector3>();
    private int currentWayPointIndex = 0;
    private bool isRunningRoute = false;
    public Vector3 velocity => rb.linearVelocity;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void SetPossession(bool value)
    {
        hasPossession = value;
        if (!value)
        {
            moveInput = Vector2.zero;
            running = false;
        }
        else
        {
            StopRoute();
        }
    }

    public void OnJuke(InputAction.CallbackContext cntxt)
    {
        if (!hasPossession) return;
        if (cntxt.performed)
        {
            Vector2 jukeDirection = cntxt.ReadValue<Vector2>();
            if (Math.Abs(jukeDirection.y) > Math.Abs(jukeDirection.x) )
            {
                if(jukeDirection.y < 0)
                {
                    rb.AddForce(new Vector3(0, 0, (jukeDirection.y * agility) * 75));
                }
            }
            else
            {
                rb.AddForce(new Vector3((jukeDirection.x * agility) * 75, 0, 0));
            }
        }
        
    }

    public void OnSprint(InputAction.CallbackContext cntxt)
    {
        if (!hasPossession) return;

        if (cntxt.performed)
        {
            running = true;
        }
        else
        {
            running = false;
        }
    }

    public void OnMovement(InputAction.CallbackContext cntxt)
    {
        if (!hasPossession) return;
        moveInput = cntxt.ReadValue<Vector2>();
    }

    public void RunAssignedRoute(RouteType type)
    {
        Route route = RouteLibrary.instance.Get(type);
        if (route == null) return;
        StartRoute(route);
    }

    public void StartRoute(Route route)
    {
        routeWaypoints.Clear();
        currentWayPointIndex = 0;

        Vector3 cursor = transform.position;
        foreach(var segment in route.segments)
        {
            cursor += segment.direction.normalized * segment.distance;
            routeWaypoints.Add(cursor);
        }

        isRunningRoute = routeWaypoints.Count > 0;
    }

    public void StopRoute()
    {
        isRunningRoute = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void Update()
    {
        if (!hasPossession) return;
        if (running)
        {
            rb.linearVelocity = new Vector3(moveInput.x * (speed / 10) * (7f/4f), rb.linearVelocity.y, moveInput.y * (speed / 10) * (3f/2f));
        }
        else
        {
            rb.linearVelocity = new Vector3(moveInput.x * (speed / 10), rb.linearVelocity.y, moveInput.y * (speed / 10));
        }
    }

    private void FixedUpdate()
    {
        if (!isRunningRoute) return;

        Vector3 target = routeWaypoints[currentWayPointIndex];
        Vector3 toTarget = target - transform.position;
        toTarget.y = 0f;

        if(toTarget.magnitude <= waypointArrivalthreshold)
        {
            currentWayPointIndex++;
            if(currentWayPointIndex >= routeWaypoints.Count)
            {
                StopRoute();
                return;
            }
            target = routeWaypoints[currentWayPointIndex];
            toTarget.y = 0f;
        }

        Vector3 direction = toTarget.normalized;
        rb.linearVelocity = new Vector3(direction.x * (speed / 10f), rb.linearVelocity.y, direction.z * (speed / 10f));
    }
}
