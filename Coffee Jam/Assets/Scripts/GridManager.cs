using UnityEngine;
using System.Collections.Generic;
public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [Header("Grid Settings")]
    public GameObject tilePrefab;
    public int columns = 5;
    public int rows = 5;
    public float tileSize = 1f;
    public float zPosition = 0f; 
    [Header("Tray Settings")]
    public TrayLayout trayLayout;
    public GameObject[] trayPrefabs;
    public Color[] trayColors;
    private Dictionary<Vector2Int, GameObject> gridTiles = new Dictionary<Vector2Int, GameObject>();
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GenerateGrid();
        if (trayLayout != null) SpawnTraysFromLayout();
    }
    void GenerateGrid()
    {
        foreach (var tile in gridTiles.Values)
        {
            if (tile != null) Destroy(tile);
        }
        gridTiles.Clear();
        Vector3 startPos = transform.position - new Vector3(
            (columns * tileSize) / 2f - tileSize/2f, 
            (rows * tileSize) / 2f - tileSize/2f,
            0);
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 pos = startPos + new Vector3(
                    x * tileSize, 
                    y * tileSize, 
                    zPosition);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                gridTiles.Add(new Vector2Int(x, y), tile);
            }
        }
    }
    void SpawnTraysFromLayout()
    {
        if (trayLayout == null || trayPrefabs == null || trayPrefabs.Length == 0)
            return;
        foreach (var trayData in trayLayout.trays)
        {
            if (trayData.trayType >= 0 && trayData.trayType < trayPrefabs.Length && 
                trayPrefabs[trayData.trayType] != null)
            {
                SpawnTray(trayData.gridPosition, trayData.trayType);
            }
        }
    }
    void SpawnTray(Vector2Int gridPos, int trayType)
    {
        Vector3 worldPos = GridToWorldPosition(gridPos);
        Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
        GameObject tray = Instantiate(trayPrefabs[trayType], worldPos, rotation);
        if (trayColors != null && trayType < trayColors.Length)
        {
            Color trayColor = trayColors[trayType];
            SetColorForAllChildren(tray, trayColor);
        }
        BoxCollider2D collider = tray.GetComponent<BoxCollider2D>();
        if (collider == null) 
        {
            collider = tray.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(tileSize, tileSize);
        }
        TrayController controller = tray.GetComponent<TrayController>();
        if (controller == null) controller = tray.AddComponent<TrayController>();
        controller.gridPosition = gridPos;
    }
    private void SetColorForAllChildren(GameObject parent, Color color)
    {
        Renderer[] renderers = parent.GetComponentsInChildren<Renderer>(true); 
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials; 
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = color;
            }
            renderer.materials = materials;
        }
    }
    public Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        Vector3 startPos = transform.position - new Vector3(
            (columns * tileSize) / 2f - tileSize/2f, 
            (rows * tileSize) / 2f - tileSize/2f,
            0);
        return startPos + new Vector3(
            gridPos.x * tileSize, 
            gridPos.y * tileSize, 
            zPosition);
    }
    public Vector2Int WorldToGridPosition(Vector3 worldPos)
    {
        Vector3 startPos = transform.position - new Vector3(
            (columns * tileSize) / 2f - tileSize/2f, 
            (rows * tileSize) / 2f - tileSize/2f,
            0);
        Vector3 relativePos = worldPos - startPos;
        int x = Mathf.RoundToInt(relativePos.x / tileSize);
        int y = Mathf.RoundToInt(relativePos.y / tileSize);
        return new Vector2Int(
            Mathf.Clamp(x, 0, columns - 1),
            Mathf.Clamp(y, 0, rows - 1));
    }
    public bool IsGridPositionValid(Vector2Int gridPos)
    {
        return gridPos.x >= 0 && gridPos.x < columns && 
               gridPos.y >= 0 && gridPos.y < rows;
    }
}