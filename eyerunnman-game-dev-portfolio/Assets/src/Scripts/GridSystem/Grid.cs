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

        private Dictionary<int, GridTileObject> tileObjectDictionary = new();

        public List<GridTileObject> GridTileObjects
        {
            get => new(tileObjectDictionary.Values);
        }
        public List<GridTileData> GridTilesData
        {
            get => new(gridData.GridTilesDataDictionary.Values);
        }

        public Vector2Int GridDimension { get => gridData.GridDimension; }

        private int cols => GridDimension.x;
        private int rows => GridDimension.y;


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

        public void SetupTile(GridTileData data)
        {
            if (tileObjectDictionary.ContainsKey(data.TileNumber))
            {
                tileObjectDictionary[data.TileNumber].SetUpTile(data);
            }
        }

        private GridTileData GetTileByTileNumber(int tileNumber)
        {
            if (tileObjectDictionary.ContainsKey(tileNumber))
            {
                return tileObjectDictionary[tileNumber].Data;
            }
            else
            {
                return default;
            }
        }

        public GridTileData GetTileInDirection(GridTileData refrenceTileData, GridEnums.Direction direction)
        {

            Vector2Int resultTileCoordinates;
            switch (direction)
            {
                case GridEnums.Direction.North:
                    resultTileCoordinates = refrenceTileData.Coordinates + Vector2Int.up;
                    break;
                case GridEnums.Direction.South:
                    resultTileCoordinates = refrenceTileData.Coordinates + Vector2Int.down;
                    break;
                case GridEnums.Direction.East:
                    resultTileCoordinates = refrenceTileData.Coordinates + Vector2Int.right;
                    break;
                case GridEnums.Direction.West:
                    resultTileCoordinates = refrenceTileData.Coordinates + Vector2Int.left;
                    break;
                default:
                    return default;
            }

            if (resultTileCoordinates.x < 0 || resultTileCoordinates.x >= GridDimension.x || resultTileCoordinates.y < 0 || resultTileCoordinates.y >= GridDimension.y)
            {
                return default;
            }

            int resultTileNumber = resultTileCoordinates.x + resultTileCoordinates.y * rows;

            GridTileData resultTileData = GetTileByTileNumber(resultTileNumber);

            return resultTileData;
        }
    }
}
