using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
            slantDirection.text = tileData.SlantAngle.ToString();
            slantAngle.text = tileData.SlantAngle.ToString();
            tileType.text = tileData.Type.ToString();

            tile.SetUpTile(tile.Data);
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;

            Vector3 firstVertex = tile.Data.TopLeftVertex;
            Vector3 secondVertex = tile.Data.TopRightVertex;
            Vector3 thirdVertex = tile.Data.BottomRightVertex;
            Vector3 fourthVertex = tile.Data.BottomLeftVertex;

            Vector3 currentPositionVector = tile.transform.position;

            firstVertex += currentPositionVector;
            secondVertex += currentPositionVector;
            thirdVertex += currentPositionVector;
            fourthVertex += currentPositionVector;


            Gizmos.DrawLine(firstVertex, secondVertex);
            Gizmos.DrawLine(secondVertex, thirdVertex);
            Gizmos.DrawLine(thirdVertex, fourthVertex);
            Gizmos.DrawLine(fourthVertex, firstVertex);


            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(tile.transform.position+tile.Data.TopLeftVertex, 0.01f);
            Gizmos.DrawSphere(tile.transform.position+tile.Data.TopRightVertex, 0.01f);
            Gizmos.DrawSphere(tile.transform.position+tile.Data.BottomRightVertex, 0.01f);
            Gizmos.DrawSphere(tile.transform.position+tile.Data.BottomLeftVertex, 0.01f);

        }

    }
}


