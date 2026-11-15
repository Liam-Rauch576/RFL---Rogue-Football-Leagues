using UnityEngine;

[CreateAssetMenu(fileName = "NewPlay", menuName = "Football/Play")]
public class Play : ScriptableObject
{
    public string playName;
    public Sprite icon;
    public PlayType playType;
    public Formation formation;
    public RouteType[] routes;

    private void OnValidate()
    {
        if (formation == null) return;
        if (routes == null || routes.Length != formation.playerPositions.Length)
            System.Array.Resize(ref routes, formation.playerPositions.Length);

    }
}