using UnityEngine;


public enum BallState
{
    Center,
    Snapping,
    QuarterBack,
}

public class FootballLogic : MonoBehaviour
{
    public static FootballLogic instance { get; private set;  }

    public BallState state { get; private set; } = BallState.Center;

    private Rigidbody rb;
    public Transform startPosition;
    public Transform qbPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        instance = this;
    }

    public void SnapMovement()
    {
        state = BallState.QuarterBack;
    }

    void Start()
    {
        state = BallState.Center;
        transform.position = startPosition.position;
        transform.rotation = startPosition.rotation;
    }

    private void Update()
    {
        switch (state)
        {
            case BallState.QuarterBack:
                transform.position = qbPosition.position;
                transform.rotation = qbPosition.rotation;
                break;
        }
    }
}
