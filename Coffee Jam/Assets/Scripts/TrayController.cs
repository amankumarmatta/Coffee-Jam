using UnityEngine;


public class TrayController : MonoBehaviour
{
    public Vector2Int gridPosition;
    private Vector3 offset;
    private bool isDragging;
    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }
    private void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPosition();
    }
    private void OnMouseDrag()
    {
        if (!isDragging) return;
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        Vector3 targetPos = mouseWorldPos + offset;
        targetPos.z = -1f;  
        transform.position = targetPos;
    }
    private void OnMouseUp()
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
    }
    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float trayY = transform.position.y;
        Plane dragPlane = new Plane(Vector3.up, new Vector3(0, trayY, 0));
        if (dragPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}
