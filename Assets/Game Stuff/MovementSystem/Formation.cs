using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (fileName = "NewFormation", menuName = "Football/Formation")]
public class Formation : ScriptableObject
{
    [System.Serializable]
    public struct PlayerPosition
    {
        public string label;
        public PlayerRole role;
        [FormerlySerializedAs("position")]
        public Vector3 offset; // relative to the center of the formation
    }
    public PlayerPosition[] playerPositions;
}

