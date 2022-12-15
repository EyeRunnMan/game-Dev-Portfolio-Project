using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.portfolio.gridSystem;
using System;
using UnityEditor;

namespace com.portfolio.scriptableObjects{

    [CreateAssetMenu(fileName = "GridData", menuName = "GridSystem/GridData", order = 100)]
    public class GridDataSO : ScriptableObject
    {
        public GridData GridData=new();
        public Mesh GridMesh;

        public void EditorUpdateGridData (gridSystem.Grid editorGrid)
        {

            Dictionary<int, GridTileData> updatedTileDataDictionary = new();

            foreach (GridTileObject tile in editorGrid.GridTileObjects)
            {
                GridTileData tileData = tile.Data;

                tileData.Height = tile.transform.position.y;

                updatedTileDataDictionary.Add(tileData.TileNumber, tileData);

            }

            GridData.GridTilesDataDictionary = updatedTileDataDictionary;
        }

        public void EditorSetTileData(GridTileObject gridTileObject , GridTileData data)
        {
            gridTileObject.SetUpTile(data);

            Dictionary<int, GridTileData> updatedGridTileObjectData = GridData.GridTilesDataDictionary;

            updatedGridTileObjectData[data.TileNumber] = data;

            GridData.GridTilesDataDictionary = updatedGridTileObjectData;
        }

        public GridTileData EditorGetTileData(int tileNumber)
        {
            return GridData.GridTilesDataDictionary[tileNumber];
        }

        [ContextMenu("test")]
        public Mesh EditorGenerateMesh()
        {
            List<GridTileData> gridTilesData = new(GridData.GridTilesDataDictionary.Values);
            
            Mesh mesh = new();

            List<Vector3> vertices = new();
            List<int> Triangles = new();
            List<Vector2> uv = new();

            int counter = 0;

            foreach (GridTileData tileData in gridTilesData)
            {
                counter += 4;

                Vector3 firstVertex = tileData.TopLeftVertex;
                Vector3 secondVertex = tileData.TopRightVertex;
                Vector3 thirdVertex = tileData.BottomRightVertex;
                Vector3 fourthVertex = tileData.BottomLeftVertex;

                Vector3 currentPositionVector = new Vector3(tileData.Coordinates.X, tileData.Height, tileData.Coordinates.Y);

                firstVertex += currentPositionVector;
                secondVertex += currentPositionVector;
                thirdVertex += currentPositionVector;
                fourthVertex += currentPositionVector;

                vertices.Add(firstVertex);
                uv.Add(firstVertex);
                vertices.Add(secondVertex);
                uv.Add(secondVertex);
                vertices.Add(thirdVertex);
                uv.Add(thirdVertex);
                vertices.Add(fourthVertex);
                uv.Add(fourthVertex);

                Triangles.Add(counter - 4);
                Triangles.Add(counter - 3);
                Triangles.Add(counter - 2);

                Triangles.Add(counter - 4);
                Triangles.Add(counter - 2);
                Triangles.Add(counter - 1);

            }

            mesh.Clear();

            mesh.vertices = vertices.ToArray();
            mesh.triangles = Triangles.ToArray();
            //mesh.uv = uv.ToArray();

            GridMesh = mesh;
            //get all vertices 
            //make a list of triangles from those vertices
            //make triangles from those vertices

            return mesh;
        }
    }
}

namespace com.portfolio.gridSystem
{

    [Serializable]
    public struct GridData
    {
        [SerializeField]
        private Vector2Int gridDimension;

        [SerializeField]
        private List<GridTileData> GridTilesData;

        public Vector2Int GridDimension { get => gridDimension; }
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

                    GridTileData tileData = new()
                    {
                        Coordinates = new() { X = xCoordinate, Y = yCoordinate },
                        TileNumber = tileNumber
                    };

                    keyValuePairs.Add(tileNumber, tileData);
                }

                foreach (GridTileData gridTileData in GridTilesData)
                {
                    if (keyValuePairs.ContainsKey(gridTileData.TileNumber))
                    {
                        gridTileData.SoftClone(keyValuePairs[gridTileData.TileNumber]);

                        keyValuePairs[gridTileData.TileNumber] = gridTileData;
                    }
                    else
                    {
                        throw new Exception("Illegal Values in list"); 
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

        public GridData(Vector2Int dimension, List<GridTileData> gridTilesData)
        {
            gridDimension = dimension;
            GridTilesData = gridTilesData;
        }
        public GridData(Vector2Int dimension)
        {
            gridDimension = dimension;
            GridTilesData = new();
        }


    }

    [Serializable]
    public struct GridTileData
    {
        public int TileNumber;
        public GridTileCoordinate Coordinates;
        public float Height;
        public GridEnums.Navigation SlantDirection;
        public int SlantAngle;
        public GridEnums.Tile.Type Type;

        public void SoftClone(GridTileData data)
        {
            TileNumber = data.TileNumber;
            Coordinates = data.Coordinates;
        }

        public Vector3 UpVector
        {
            get
            {
                Vector3 slantDirectionVector = new();

                switch (SlantDirection)
                {
                    case GridEnums.Navigation.North:
                        slantDirectionVector = Vector3.forward;
                        break;
                    case GridEnums.Navigation.South:
                        slantDirectionVector = Vector3.back;
                        break;
                    case GridEnums.Navigation.East:
                        slantDirectionVector = Vector3.right;
                        break;
                    case GridEnums.Navigation.West:
                        slantDirectionVector = Vector3.left;
                        break;
                    default:
                        break;
                }

                Vector3 aboutAxis = Vector3.Cross(slantDirectionVector, Vector3.up);

                Vector3 upwardVector = Quaternion.AngleAxis(SlantAngle, aboutAxis) * Vector3.up;

                return upwardVector.normalized;
            }

        }
        public Vector3 ForwardVector
        {
            get
            {

                Vector3 aboutAxis = Vector3.Cross(UpVector, Vector3.forward);

                Vector3 forwardVector = Quaternion.AngleAxis(90, aboutAxis) * UpVector;

                return forwardVector.normalized;
            }
        }
        public Vector3 RighVector
        {
            get
            {

                Vector3 aboutAxis = Vector3.Cross(UpVector, Vector3.right);

                Vector3 rightVector = Quaternion.AngleAxis(90, aboutAxis) * UpVector;

                return rightVector.normalized;
            }
        }
        public Vector3 DownVector
        {
            get => -UpVector.normalized;
        }
        public Vector3 BackVector
        {
            get => -ForwardVector.normalized;
        }
        public Vector3 LeftVector
        {
            get => -RighVector.normalized;
        }

        public Vector3 TopLeftVertex
        {
            get
            {
                return SlantDirection switch
                {
                    GridEnums.Navigation.North or GridEnums.Navigation.South => (ForwardVector * (1 + SlantGap) + LeftVector) / 2,
                    GridEnums.Navigation.East or GridEnums.Navigation.West => (LeftVector * (1 + SlantGap) + ForwardVector) / 2,
                    _ => Vector3.zero,
                };
            }
        }
        public Vector3 TopRightVertex
        {
            get
            {
                return SlantDirection switch
                {
                    GridEnums.Navigation.North or GridEnums.Navigation.South => (ForwardVector * (1 + SlantGap) + RighVector) / 2,
                    GridEnums.Navigation.East or GridEnums.Navigation.West => (RighVector * (1 + SlantGap) + ForwardVector) / 2,
                    _ => Vector3.zero,
                };
            }
        }
        public Vector3 BottomRightVertex
        {
            get
            {
                return SlantDirection switch
                {
                    GridEnums.Navigation.North or GridEnums.Navigation.South => (BackVector * (1 + SlantGap) + RighVector) / 2,
                    GridEnums.Navigation.East or GridEnums.Navigation.West => (RighVector * (1 + SlantGap) + BackVector) / 2,
                    _ => Vector3.zero,
                };
            }
        }
        public Vector3 BottomLeftVertex
        {
            get
            {
                return SlantDirection switch
                {
                    GridEnums.Navigation.North or GridEnums.Navigation.South => (BackVector * (1 + SlantGap) + LeftVector) / 2,
                    GridEnums.Navigation.East or GridEnums.Navigation.West => (LeftVector * (1 + SlantGap) + BackVector) / 2,
                    _ => Vector3.zero,
                };
            }
        }

        private float SlantGap
        {
            get
            {
                float hypotenuseLength = 1 / Mathf.Cos(SlantAngle * Mathf.Deg2Rad);

                return (hypotenuseLength-1);
            }
        }
    }

    [Serializable]
    public struct GridTileCoordinate
    {
        public int X;
        public int Y;

        public override string ToString()
        {
            return "X : "+X+" Y : "+Y;
        }
    }
}

