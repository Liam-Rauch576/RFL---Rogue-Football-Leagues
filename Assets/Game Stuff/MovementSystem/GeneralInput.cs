using UnityEngine;
using UnityEngine.InputSystem;

public class GeneralInput : MonoBehaviour
{

    [SerializeField] private InputActionReference endPlayAction;
    [SerializeField] private PlayManager playManager;

    private void OnEnable()
    {
        endPlayAction.action.Enable();
        endPlayAction.action.performed += OnEndPlay;
    }

    private void OnDisable()
    {
        endPlayAction.action.performed -= OnEndPlay;
        endPlayAction.action.Disable();
    }

    private void OnEndPlay(InputAction.CallbackContext context)
    {
        playManager.EndPlay();
    }
}
