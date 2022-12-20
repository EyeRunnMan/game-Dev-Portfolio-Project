using System.Collections.Generic;
using UnityEngine;

namespace com.package.gridSystem
{
    public class GameGrid : MonoBehaviour
    {

        private GridData gridData;

        private Dictionary<int, GridTileObject> tileObjectDictionary = new();

        public List<GridTileData> GridTilesData
        {
            get => new(gridData.GridTilesDataDictionary.Values);
        }

        public Vector2Int GridDimension { get => gridData.GridDimension; }

        private int Cols => gridData.Cols;
        private int Rows => gridData.Rows;

        public void GenerateGrid(GridData data,GridTileObject gridTileObjectPrefab)
        {
            ResetGrid();
            this.gridData = data;

            foreach (int tileNumber in gridData.GridTilesDataDictionary.Keys)
            {
                GridTileObject gridTileObject = Instantiate(gridTileObjectPrefab, transform);
                gridTileObject.SetUpTile(gridData.GridTilesDataDictionary[tileNumber]);
                
                tileObjectDictionary.Add(tileNumber, gridTileObject);
            }


        }

        private void ResetGrid()
        {
            foreach (GridTileObject tile in tileObjectDictionary.Values)
            {
                Destroy(tile);
            }

            tileObjectDictionary = new();
        }

        public void SetupTile(GridTileData data)
        {
            if (tileObjectDictionary.ContainsKey(data.TileId))
            {
                tileObjectDictionary[data.TileId].SetUpTile(data);
            }
        }

        public GridTileData GetTileInDirection(int tileId , GridEnums.Direction direction)
        {
            return gridData.GetTileInDirection(tileId,direction);
        }
    }
}
