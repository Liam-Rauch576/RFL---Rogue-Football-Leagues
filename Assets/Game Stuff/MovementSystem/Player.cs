using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Player : MonoBehaviour
{
    protected Rigidbody rb;
    public Transform Hands;
    private Vector2 moveInput;
    private bool running = false;

    public string name;
    public float speed;
    public float strength;
    public float agility;

    public bool hasPossession = false;

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

    private void Update()
    {
        if (running)
        {
            rb.linearVelocity = new Vector3(moveInput.x * (speed / 10) * (7f/4f), rb.linearVelocity.y, moveInput.y * (speed / 10) * (3f/2f));
        }
        else
        {
            rb.linearVelocity = new Vector3(moveInput.x * (speed / 10), rb.linearVelocity.y, moveInput.y * (speed / 10));
        }
    }
}
