using UnityEngine;
using System.Collections.Generic;


public class IndicatorManager : MonoBehaviour
{
    public static IndicatorManager instance { get; private set; }

    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private Canvas targetCanvas; // Screen Space - Overlay
    [SerializeField] private float heightOffset = 2.2f; // above head

    [Header("Button Glyphs")]
    [SerializeField] private Sprite buttonSprite1;
    [SerializeField] private Sprite buttonSprite2;
    [SerializeField] private Sprite buttonSprite3;
    [SerializeField] private Sprite buttonSprite4;

    private readonly Dictionary<Player, GameObject> Indicators = new Dictionary<Player, GameObject>();

    private static Dictionary<int, (string label, Sprite sprite)> offenseIndexLabels;

    private void Awake()
    {
        instance = this;

        offenseIndexLabels = new Dictionary<int, (string label, Sprite sprite)>
        {
            { 3, ("1", buttonSprite1) }, // ThrowToReceiver1 -> offense[3]
            { 4, ("2", buttonSprite2) }, // ThrowToReceiver2 -> offense[4]
            { 5, ("3", buttonSprite3) }, // ThrowToReceiver3 -> offense[5]
            { 2, ("4", buttonSprite4) }, // ThrowToReceiver4 -> offense[2] (RB)
        };
    }

    public void SetupIndicators(List<Player> offense)
    {
        ClearIndicators();

        foreach (var kvp in offenseIndexLabels)
        {
            int index = kvp.Key;
            var (label, sprite) = kvp.Value;

            if (index < 0 || index >= offense.Count || offense[index] == null)
                continue;

            Player receiver = offense[index];
            GameObject indicatorObj = Instantiate(indicatorPrefab, targetCanvas.transform);

            var indicator = indicatorObj.GetComponent<ReceiverIndicator>();
            indicator.Initialize(receiver, label, sprite, heightOffset);

            Indicators[receiver] = indicatorObj;
        }
    }

    public void ClearIndicators()
    {
        foreach (var obj in Indicators.Values)
        {
            if (obj != null) Destroy(obj);
        }
        Indicators.Clear();
    }

    public void SetIndicatorsVisible(bool visible)
    {
        foreach (var obj in Indicators.Values)
        {
            if (obj != null) obj.SetActive(visible);
        }
    }
}

