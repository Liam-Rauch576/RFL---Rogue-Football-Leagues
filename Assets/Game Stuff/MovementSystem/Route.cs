// Route.cs — new file
using UnityEngine;

[CreateAssetMenu(fileName = "NewRoute", menuName = "Football/Route")]
public class Route : ScriptableObject
{
    [System.Serializable]
    public struct Segment
    {
        public Vector3 direction; // e.g. (0,0,1) = downfield, (1,0,0) = toward sideline
        public float distance;
    }

    public Segment[] segments;
}
