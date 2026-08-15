using UnityEngine;

public enum BallState
{
    Idle,
    Center,
    Snapping,
    QuarterBack,
    Handoff,
    Thrown,
    BallCarrier,
}

public class FootballLogic : MonoBehaviour
{
    public static FootballLogic instance { get; private set;  }

    public BallState state { get; private set; } = BallState.Idle;

    private Rigidbody rb;

    //data used to snap the ball
    public Transform startPosition;
    public Transform qbPosition;
    private float snapTime = 0f;
    private float snapDuration = .4f;

    //data used to figure out throwing positions
    public Vector3 Target;
    private float ThrowTime = 0f;
    private float throwDuration;
    private float catchRadius = 1.5f;
    private Transform qbHands => PossessionManager.instance.offense[1].Hands;
    [SerializeField] private float arcHeightStuff = 0.15f;
    private float ThrowArcHeight = 0f;

    //data used to handoff the ball
    private float handoffTime = 0f;
    private float handoffDuration = .3f;

    //data used to figure out who the ballcarrier is (will change after throws hopefully)
    public Player currentBallCarrier;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        instance = this;
    }

    public void ThrowTo(Vector3 target, float duration)
    {
        Debug.Log($"ThrowTo called - duration in: {duration}, will clamp to: {Mathf.Max(duration, 0.15f)}");

        Target = target;
        throwDuration = Mathf.Max(duration, .015f);

        float throwDistance = Vector3.Distance(qbHands.position, target);
        ThrowArcHeight = throwDistance * arcHeightStuff;

        ThrowTime = 0f;
        StateChange("Thrown");
    }

    public void CatchingMath()
    {
        float distanceToTarget = Vector3.Distance(currentBallCarrier.transform.position, Target);

        if(distanceToTarget <= catchRadius)
        {
            StateChange("BallCarrier");
            PossessionManager.instance.GivePossession(currentBallCarrier);
        }
    }

    public void StateChange(string indicator)
    {
        if(indicator == "Idle")
        {
            state = BallState.Idle;
        }
        else if (indicator == "Snapping")
        {
            snapTime = 0f;
            state = BallState.Snapping;
            PlayManager.instance.TriggerRoutes();
        } 
        else if (indicator == "Center")
        {
            state = BallState.Center;
        } 
        else if (indicator == "QuarterBack")
        {
            state = BallState.QuarterBack;
            if(PlayManager.instance.CurrentPlay == PlayType.Pass)
            {
                IndicatorManager.instance.SetIndicatorsVisible(true);
            }
        } 
        else if (indicator == "Handoff")
        {
            state = BallState.Handoff;
        }
        else if(indicator == "Thrown")
        {
            ThrowTime = 0f;
            state = BallState.Thrown;
            IndicatorManager.instance.SetIndicatorsVisible(false);
        }
        else
        {
            state = BallState.BallCarrier;
        }
    }

    void Start()
    {
        StateChange("Idle");
    }

    private void Update()
    {
        switch (state)
        {
            case BallState.Idle:
                break;

            case BallState.Center:
                transform.position = startPosition.position;
                transform.rotation = startPosition.rotation;
                break;

            case BallState.Snapping:
                snapTime += Time.deltaTime;
                float t = Mathf.Clamp01(snapTime / snapDuration);
                transform.position = Vector3.Lerp(startPosition.position, qbPosition.position, t);
                if(t >= 1f)
                {
                    StateChange("QuarterBack");
                    PossessionManager.instance.GivePossession(PossessionManager.instance.offense[1]);
                    if (PlayManager.instance.CurrentPlay == PlayType.Rush){
                        currentBallCarrier = PossessionManager.instance.offense[2];
                        StateChange("Handoff");
                    }
                }
                break;

            case BallState.QuarterBack:
                transform.position = qbPosition.position;
                transform.rotation = qbPosition.rotation;
                break;

            case BallState.Handoff:
                handoffTime += Time.deltaTime;
                float time1 = Mathf.Clamp01(handoffTime / handoffDuration);
                transform.position = Vector3.Lerp(qbPosition.position, currentBallCarrier.Hands.transform.position, time1);
                if (time1 >= 1f)
                {
                    StateChange("BallCarrier");
                    PossessionManager.instance.GivePossession(currentBallCarrier);
                }
                break;

            case BallState.Thrown:
                ThrowTime += Time.deltaTime;
                float time = Mathf.Clamp01(ThrowTime / throwDuration);

                Vector3 straightLinePos = Vector3.Lerp(qbHands.position, Target, (time));
                float arc = Mathf.Sin(time * Mathf.PI) * ThrowArcHeight;
                transform.position = straightLinePos + Vector3.up * arc;

                if(time >= 1f)
                {
                    CatchingMath();
                }
                break;

            case BallState.BallCarrier:
                transform.position = currentBallCarrier.Hands.transform.position;
                transform.rotation = currentBallCarrier.Hands.transform.rotation;
                break;
        }
    }
}
