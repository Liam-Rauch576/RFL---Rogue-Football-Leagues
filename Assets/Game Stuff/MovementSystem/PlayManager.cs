using UnityEngine;
using System.Collections.Generic;

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance { get; private set; }
    public PlayType CurrentPlay { get; private set; } = PlayType.None;
    private readonly List<GameObject> spawnedPlayers = new List<GameObject>();
   
    public Play PlayData { get; private set; }
    private Player[] players;

    [Header("Prefabs")]
    [SerializeField] private GameObject centerPrefab;
    [SerializeField] private GameObject QuarterBackPrefab;
    [SerializeField] private GameObject RunningBackPrefab;
    [SerializeField] private GameObject WideReceiverPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform formation;
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
        }
        spawnedPlayers.Clear();
        CurrentPlay = PlayType.None;

        IndicatorManager.instance.ClearIndicators();
    }

    private GameObject PrefabFor(PlayerRole role)
    {
        switch (role)
        {
            case PlayerRole.Center: 
                return centerPrefab;
            case PlayerRole.QuarterBack:
                return QuarterBackPrefab;
            case PlayerRole.RunningBack:
                return RunningBackPrefab;
            case PlayerRole.WideReceiver:
                return WideReceiverPrefab;
            default:
                Debug.LogError($"PlayManager: No prefab defined for role {role}");
                return null;
        }
    }

    private Player FirstOfRole(PlayerRole role, Formation.PlayerPosition[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Role == role)
            {
                return players[i];
            }
            Debug.LogError($"PlayManager: No player found for role {role}");
            return null;
        }
    }

    public void StartPlay (Play play)
    {
        ClearSpawnedPlayers();

        CurrentPlay = play.playType;
        PlayData = play;
        PossessionManager.instance.offense.Clear();

        var positions = play.formation.playerPositions;
        players = new Player[positions.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            players[i] = SpawnAt(PrefabFor(positions[i].role), formation.position + positions[i].offset, formation.rotation, positions[i].label);

        }
        var offense = PossessionManager.instance.offense;
        offense.Add(FirstOfRole(PlayerRole.Center));
        offense.Add(FirstOfRole(PlayerRole.QuarterBack));
        offense.Add(FirstOfRole(PlayerRole.RunningBack));

        if (play.PlayType == PlayType.Pass)
        {
            IndicatorManager.instance.SetupIndicators(offense);
        }
    }

    public void TriggerRoutes()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (CurrentPlay.routes[i] != RouteType.None)
            {
                players[i].RunAssignedRoute(PlayData.routes[i]);
            }
        }
    }
    public event System.Action PlayEnded;

    public void EndPlay()
    {
        if (CurrentPlay == PlayType.None) return;

        Vector3 formP = formation.position;
        formation.position = new Vector3 (formP.x, formP.y, FootballLogic.instance.currentBallCarrier.transform.position.z + 1.89f);
        FootballLogic.instance.ResetBall(centerSpawnPoint.position + Vector3.up);
        ClearSpawnedPlayers();
        PossessionManager.instance.offense.Clear();

        PlayEnded?.Invoke();

    }
}
