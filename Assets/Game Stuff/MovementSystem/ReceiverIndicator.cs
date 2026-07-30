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

    public void Initialize(Player targetPlayer, string buttonLabel, Sprite buttonSprite, float offset)
    {
        target = targetPlayer;
        heightOffset = offset;
        label.text = buttonLabel; // optional now, since the sprite itself shows the button
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
    }
}