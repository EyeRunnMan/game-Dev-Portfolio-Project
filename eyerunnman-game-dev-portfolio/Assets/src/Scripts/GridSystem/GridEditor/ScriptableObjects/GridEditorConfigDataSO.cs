using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.portfolio.gridSystem;

namespace com.portfolio.scriptableObjects
{
    [CreateAssetMenu(fileName = "GridDebugData", menuName = "GridSystem/GridDebugData", order = 1)]
    public class GridEditorConfigDataSO : ScriptableObject
    {
        public EditorGridTile DebugTileObject;
        [Range(0,1)]
        public float TileHeightSnap = 1f;
    }
}

