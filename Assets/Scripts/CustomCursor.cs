using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Texture2D mouseCursor;

    Vector2 hotSpot = new Vector2(16.5f, 16.5f);
    CursorMode cursorMode = CursorMode.Auto;

    private void Start()
    {
        Cursor.SetCursor(mouseCursor, hotSpot, cursorMode);
    }
}
