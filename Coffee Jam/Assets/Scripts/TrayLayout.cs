using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewTrayLayout", menuName = "Puzzle/Tray Layout")]
public class TrayLayout : ScriptableObject
{
    [Serializable]
    public struct TrayData
    {
        public Vector2Int gridPosition;
        public int trayType;
    }

    public TrayData[] trays;
}