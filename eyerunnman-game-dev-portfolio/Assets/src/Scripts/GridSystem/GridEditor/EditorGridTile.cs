using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.portfolio.gridSystem
{
    [ExecuteInEditMode]
    public class EditorGridTile : MonoBehaviour
    {
        GridTileObject tile;

        [SerializeField] TMP_Text tileNumber;
        [SerializeField] TMP_Text coordinates;
        [SerializeField] TMP_Text height;
        [SerializeField] TMP_Text slantDirection;
        [SerializeField] TMP_Text slantAngle;
        [SerializeField] TMP_Text tileType;

        private void Update()
        {
            UpdateDebugData();
        }

        public void UpdateDebugData()
        {
            if (tile == null)
            {
                tile = GetComponent<GridTileObject>();
            }

            if (tile == null)
            {
                return;
            }
            GridTileData tileData = tile.Data;

            tileNumber.text = tileData.TileNumber.ToString();
            coordinates.text = tileData.Coordinates.ToString();
            height.text = tileData.Height.ToString();
            slantDirection.text = tileData.SlantDirection.ToString();
            slantAngle.text = tileData.SlantAngle.ToString();
            tileType.text = tileData.Type.ToString();

            tile.SetUpTile(tile.Data);
        }


        private void OnDrawGizmos()
        {
            //Gizmos.color = Color.red;
            //Gizmos.DrawLine(tile.transform.position, tile.transform.position + tile.Data.ForwardVector);
            //Gizmos.color = Color.green;
            //Gizmos.DrawLine(tile.transform.position, tile.transform.position + tile.Data.RighVector);
            //Gizmos.color = Color.blue;
            //Gizmos.DrawLine(tile.transform.position, tile.transform.position + tile.Data.UpVector);
            //
            Gizmos.color = Color.cyan;
            
            Gizmos.DrawLine(tile.transform.position + tile.Data.TopLeftVertex, tile.transform.position + tile.Data.TopRightVertex);
            Gizmos.DrawLine(tile.transform.position + tile.Data.TopRightVertex, tile.transform.position + tile.Data.BottomRightVertex);
            Gizmos.DrawLine(tile.transform.position + tile.Data.BottomRightVertex, tile.transform.position + tile.Data.BottomLeftVertex);
            Gizmos.DrawLine(tile.transform.position + tile.Data.BottomLeftVertex, tile.transform.position + tile.Data.TopLeftVertex);

            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(tile.transform.position+tile.Data.TopLeftVertex, 0.01f);
            Gizmos.DrawSphere(tile.transform.position+tile.Data.TopRightVertex, 0.01f);
            Gizmos.DrawSphere(tile.transform.position+tile.Data.BottomRightVertex, 0.01f);
            Gizmos.DrawSphere(tile.transform.position+tile.Data.BottomLeftVertex, 0.01f);

            //Gizmos.color = Color.gray;
            //Gizmos.DrawSphere(tile.Data.SlantEdgeHeight*Vector3.up+Vector3.forward/2,0.01f);

        }

    }
}


