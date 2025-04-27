using UnityEngine;
using UnityEngine.EventSystems;
public class TrayController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Vector2Int gridPosition;
    private bool isDragging;
    private Vector3 offset;
    private void Start()
    {
        transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            isDragging = true;
            offset = transform.position - Camera.main.ScreenToWorldPoint(
                new Vector3(eventData.position.x, eventData.position.y, 10f));
        }
        Vector3 cursorPoint = new Vector3(eventData.position.x, eventData.position.y, 10f);
        Vector3 cursorWorldPos = Camera.main.ScreenToWorldPoint(cursorPoint);
        transform.position = cursorWorldPos + offset;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        SnapToGrid();
    }
    private void SnapToGrid()
    {
        Vector2Int newGridPos = GridManager.Instance.WorldToGridPosition(transform.position);
        if (GridManager.Instance.IsGridPositionValid(newGridPos))
        {
            gridPosition = newGridPos;
            transform.position = GridManager.Instance.GridToWorldPosition(gridPosition);
        }
        else
        {
            transform.position = GridManager.Instance.GridToWorldPosition(gridPosition);
        }
        transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tray") && !isDragging)
        {
            Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
            collision.gameObject.transform.Translate(pushDirection * 0.1f);
        }
    }
}