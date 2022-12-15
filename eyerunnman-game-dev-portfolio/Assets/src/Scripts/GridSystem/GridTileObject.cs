using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace com.portfolio.gridSystem
{
    public class GridTileObject : MonoBehaviour
    {
        public GridTileData Data { get { return _data; } }
        public bool IsInitalized { get { return IsInitalized; } }

        private GridTileData _data;

        public void SetUpTile(GridTileData tileData)
        {
            _data = tileData;

            transform.localPosition = new Vector3(tileData.Coordinates.X, tileData.Height, tileData.Coordinates.Y);

            transform.up = tileData.UpVector;

            gameObject.name = "Grid Tile Object : " + tileData.TileNumber;

        }

    }
}

