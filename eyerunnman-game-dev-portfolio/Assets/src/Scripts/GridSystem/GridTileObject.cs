using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace com.portfolio.gridSystem
{
    public class GridTileObject : MonoBehaviour
    {
        public GridTileData Data { get { return data; } }
        private GridTileData data;

        public void SetUpTile(GridTileData tileData)
        {
            data = tileData;

            transform.localPosition = new Vector3(tileData.Coordinates.x, tileData.Height, tileData.Coordinates.y);

            transform.up = tileData.UpVector;

            gameObject.name = "Grid Tile Object : " + tileData.TileNumber;
        }

    }
}

