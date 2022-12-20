using System.Collections.Generic;
using UnityEngine;
using System;

namespace com.package.gridSystem
{
    [Serializable]
    public struct GridData
    {
        [SerializeField]
        private UnityEngine.Vector2Int gridDimension;

        [SerializeField]
        private List<GridTileData> GridTilesData;

        public UnityEngine.Vector2Int GridDimension { get => gridDimension; }
        public Dictionary<int, GridTileData> GridTilesDataDictionary
        {
            get
            {
                Dictionary<int, GridTileData> keyValuePairs = new();

                int totalTiles = gridDimension.x * gridDimension.y;

                //set default data for tiles
                for (int tileNumber = 0; tileNumber < totalTiles; tileNumber++)
                {
                    int yCoordinate = (int)Mathf.Floor(tileNumber / gridDimension.x);
                    int xCoordinate = (tileNumber % gridDimension.x);

                    GridTileData tileData = new(tileNumber, new Vector2Int(xCoordinate, yCoordinate));

                    keyValuePairs.Add(tileNumber, tileData);
                }

                foreach (GridTileData gridTileData in GridTilesData)
                {
                    if (keyValuePairs.ContainsKey(gridTileData.TileId))
                    {
                        keyValuePairs[gridTileData.TileId] = gridTileData;
                    }
                    else if(!gridTileData.IsDefault)
                    {
                        throw new Exception("Illegal Values in list TileNumber : "+ gridTileData.TileId); 
                    }
                }

                return keyValuePairs;

            }
            set
            {
                GridTilesData = new();
                foreach (GridTileData tileData in value.Values)
                {
                    GridTilesData.Add(tileData);
                }
            }
        }

        public int Cols => GridDimension.x;
        public int Rows => GridDimension.y;

        public GridData(UnityEngine.Vector2Int dimension, List<GridTileData> gridTilesData)
        {
            gridDimension = dimension;
            GridTilesData = gridTilesData;
        }
        public GridData(UnityEngine.Vector2Int dimension)
        {
            gridDimension = dimension;
            GridTilesData = new();
        }

        private GridTileData GetTileByTileId(int tileId)
        {
            if (GridTilesDataDictionary.ContainsKey(tileId))
            {
                return GridTilesDataDictionary[tileId];
            }
            else
            {
                return GridTileData.Default;
            }
        }

        public GridTileData GetTileInDirection(int tileId, GridEnums.Direction direction)
        {

            Vector2Int resultTileCoordinates;
            GridTileData refrenceTileData = GetTileByTileId(tileId);

            if (refrenceTileData.IsDefault) return refrenceTileData;

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
                    return GridTileData.Default;
            }

            if (resultTileCoordinates.x < 0 || resultTileCoordinates.x >= GridDimension.x || resultTileCoordinates.y < 0 || resultTileCoordinates.y >= GridDimension.y)
            {
                return GridTileData.Default;
            }

            int resultTileNumber = resultTileCoordinates.x + resultTileCoordinates.y * Cols;

            GridTileData resultTileData = GetTileByTileId(resultTileNumber);

            return resultTileData;
        }

    }


}

