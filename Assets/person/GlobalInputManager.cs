using UnityEngine;

public class GlobalInputManager : MonoBehaviour
{
    public static GlobalInputManager Instance { get; private set; }

    [Header("通用输入键位")]
    public KeyCode interactKey = KeyCode.Space;
    public KeyCode cancelKey = KeyCode.Escape;
    public KeyCode toggleFullscreenKey = KeyCode.F11;

    public Vector2 MoveDirection { get; private set; }
    public bool IsInteractPressed { get; private set; }
    public bool IsCancelPressed { get; private set; }
    public bool IsToggleFullscreenPressed { get; private set; }

    public bool IsAnyMovement => MoveDirection.sqrMagnitude > 0.001f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        MoveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (MoveDirection.sqrMagnitude > 1f)
        {
            MoveDirection.Normalize();
        }

        IsInteractPressed = Input.GetKeyDown(interactKey);
        IsCancelPressed = Input.GetKeyDown(cancelKey);
        IsToggleFullscreenPressed = Input.GetKeyDown(toggleFullscreenKey);
    }
}
