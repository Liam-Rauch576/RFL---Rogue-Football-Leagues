using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReceiverIndicator : MonoBehaviour
{
    private Player target;
    private float heightOffset;
    private Camera cam;

    [SerializeField] private Image glyphImage;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private RectTransform rect;

    //Scaling stuff
    [SerializeField] private float baseScale = 1f;
    [SerializeField] private float referenceDistance = 10f;

    public void Initialize(Player targetPlayer, string buttonLabel, Sprite buttonSprite, float offset)
    {
        target = targetPlayer;
        heightOffset = offset;
        label.text = buttonLabel; 
        glyphImage.sprite = buttonSprite;
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        Vector3 worldPos = target.transform.position + Vector3.up * heightOffset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        if (screenPos.z < 0)
        {
            rect.gameObject.SetActive(false);
            return;
        }

        rect.gameObject.SetActive(true);
        rect.position = screenPos;

        float distance = Vector3.Distance(cam.transform.position, worldPos);
        float scale = baseScale * (referenceDistance / distance);
        rect.localScale = Vector3.one * scale * .75f;
    }
}