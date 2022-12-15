using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.portfolio.patterns;
using com.portfolio.interfaces;
using System.Threading.Tasks;
using System;

namespace com.portfolio.gridSystem
{
    public class Grid : MonoBehaviour
    {
        GridData gridData;

        Dictionary<int, GridTileObject> tileObjectDictionary = new();

        public List<GridTileObject> GridTileObjects
        {
            get => new(tileObjectDictionary.Values);
        }
        public List<GridTileData> GridTilesData
        {
            get => new(gridData.GridTilesDataDictionary.Values);
        }

        public Vector2Int GridDimension { get => gridData.GridDimension; }

        public async Task GenerateGridAsync(GridData data,GridTileObject gridTileObjectPrefab)
        {
            ResetGrid();
            this.gridData = data;

            foreach (int tileNumber in gridData.GridTilesDataDictionary.Keys)
            {
                GridTileObject gridTileObject = Instantiate(gridTileObjectPrefab, transform);

                gridTileObject.SetUpTile(gridData.GridTilesDataDictionary[tileNumber]);

                tileObjectDictionary.Add(tileNumber, gridTileObject);
            }

            await Task.Yield();
        }

        public void GenerateGrid(GridData data, GridTileObject gridTileObjectPrefab)
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
    }
}
