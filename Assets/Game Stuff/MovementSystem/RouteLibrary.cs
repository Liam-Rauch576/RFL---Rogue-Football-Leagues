using UnityEngine;
using System.Collections.Generic;

public class RouteLibrary : MonoBehaviour
{
    public static RouteLibrary instance { get; private set; }

    [System.Serializable]
    public struct RouteEntry
    {
        public RouteType type;
        public Route route;
    }

    [SerializeField] private List<RouteEntry> routeEntries;

    private Dictionary<RouteType, Route> routes;

    private void Awake()
    {
        instance = this;

        routes = new Dictionary<RouteType, Route>();
        foreach (var entry in routeEntries)
        {
            if (entry.route == null)
            {
                Debug.LogWarning($"RouteLibrary: No Route asset assigned for {entry.type}");
                continue;
            }
            if (!routes.ContainsKey(entry.type))
            {
                routes.Add(entry.type, entry.route);
            }
            else
            {
                Debug.LogWarning($"RouteLibrary: Duplicate entry for {entry.type}, ignoring.");
            }
        }
    }

    public Route Get(RouteType type)
    {
        if (type == RouteType.None) return null;

        if (routes.TryGetValue(type, out Route route))
        {
            return route;
        }

        Debug.LogError($"RouteLibrary: No route registered for {type}");
        return null;
    }
}