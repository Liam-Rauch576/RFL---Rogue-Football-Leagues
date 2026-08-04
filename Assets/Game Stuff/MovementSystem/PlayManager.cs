using UnityEngine;
using System.Collections.Generic;

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance { get; private set; }
    public PlayType CurrentPlay { get; private set; } = PlayType.None;
    private readonly List<GameObject> spawnedPlayers = new List<GameObject>();

    [Header("Prefabs")]
    [SerializeField] private GameObject centerPrefab;
    [SerializeField] private GameObject QuarterBackPrefab;
    [SerializeField] private GameObject RunningBackPrefab;
    [SerializeField] private GameObject WideReceiverPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform centerSpawnPoint;
    [SerializeField] private Transform QuarterBackSpawnPoint;
    [SerializeField] private Transform RunningBackSpawnPoint;
    [SerializeField] private Transform[] WideReceiverSpawnPoints;

    [System.Serializable]
    public struct RouteAssignment
    {
        public int offenseIndex; // index into PossessionManager.instance.offense
        public RouteType route;
    }

    [Header("Route Assignments")]
    [SerializeField] private RouteAssignment[] passPlayRoutes;

    private void Awake()
    {
        instance = this;
    }

    private Player SpawnAt(GameObject prefab, Transform spawnPoint, string label)
    {
        if(prefab == null)
        {
            Debug.LogError($"PlayManager: Missing prefab reference for {label}.");
            return null;
        }
        if(spawnPoint == null)
        {
            Debug.LogError($"PlayManager: Missing spawn point for {label}.");
            return null;
        }

        GameObject instance = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        instance.name = label;
        spawnedPlayers.Add(instance);

        Player player = instance.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError($"PlayManager: Spawned {label} prefab has no Player component.");
        }

        return player;
    }

    public void ClearSpawnedPlayers()
    {
        foreach( var player in spawnedPlayers)
        {
            if(player != null)
            {
                Destroy(player);
            }
            spawnedPlayers.Clear();
            CurrentPlay = PlayType.None;
        }

        IndicatorManager.instance.ClearIndicators();
    }

    public void StartPassPlay()
    {
        ClearSpawnedPlayers();
        CurrentPlay = PlayType.Pass;

        PossessionManager.instance.offense.Clear();

        Player Center = SpawnAt(centerPrefab, centerSpawnPoint, "Center");
        Player QuarterBack = SpawnAt(QuarterBackPrefab, QuarterBackSpawnPoint, "QuarterBack");
        Player RunningBack = SpawnAt(RunningBackPrefab, RunningBackSpawnPoint, "RunningBack");

        PossessionManager.instance.offense.Add(Center);
        PossessionManager.instance.offense.Add(QuarterBack);
        PossessionManager.instance.offense.Add(RunningBack);

        for( int i = 0; i < WideReceiverSpawnPoints.Length; i++)
        {
            Player wideReceiver = SpawnAt(WideReceiverPrefab, WideReceiverSpawnPoints[i], "Wide Receiver_" + (i + 1));
            PossessionManager.instance.offense.Add(wideReceiver);
        }

        FootballLogic.instance.startPosition = Center.Hands;
        FootballLogic.instance.qbPosition = QuarterBack.Hands;

        IndicatorManager.instance.SetupIndicators(PossessionManager.instance.offense);

        PossessionManager.instance.GivePossession(Center);
        FootballLogic.instance.StateChange("Center");
    }

    public void StartRunPlay()
    {
        ClearSpawnedPlayers();
        CurrentPlay = PlayType.Rush;

        PossessionManager.instance.offense.Clear();

        Player Center = SpawnAt(centerPrefab, centerSpawnPoint, "Center");
        Player QuarterBack = SpawnAt(QuarterBackPrefab, QuarterBackSpawnPoint, "QuarterBack");
        Player RunningBack = SpawnAt(RunningBackPrefab, RunningBackSpawnPoint, "Running Back");

        PossessionManager.instance.offense.Add(Center);
        PossessionManager.instance.offense.Add(QuarterBack);
        PossessionManager.instance.offense.Add(RunningBack);

        for (int i = 0; i < WideReceiverSpawnPoints.Length; i++)
        {
            Player wideReceiver = SpawnAt(WideReceiverPrefab, WideReceiverSpawnPoints[i], "Wide Receiver_" + (i + 1));
            PossessionManager.instance.offense.Add(wideReceiver);
        }

        FootballLogic.instance.startPosition = Center.Hands;
        FootballLogic.instance.qbPosition = QuarterBack.Hands;

        PossessionManager.instance.GivePossession(Center);
        FootballLogic.instance.StateChange("Center");

    }

    public void TriggerRoutes()
    {
        if (CurrentPlay != PlayType.Pass) return;

        foreach (var assignment in passPlayRoutes)
        {
            if (assignment.offenseIndex < 0 || assignment.offenseIndex >= PossessionManager.instance.offense.Count)
            {
                Debug.LogWarning($"PlayManager: RouteAssignment index {assignment.offenseIndex} is out of range for offense list.");
                continue;
            }

            Player player = PossessionManager.instance.offense[assignment.offenseIndex];
            if (player != null)
            {
                player.RunAssignedRoute(assignment.route);
            }
        }
    }


}
