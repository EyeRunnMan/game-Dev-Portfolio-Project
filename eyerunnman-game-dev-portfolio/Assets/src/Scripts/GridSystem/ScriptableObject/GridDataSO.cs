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

        public void EditorSetTileData(gridSystem.Grid editorGrid , GridTileData data)
        {
            if (data.Equals(default))
            {
                return;
            }

            editorGrid.SetupTile(data);

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

                Vector3 currentPositionVector = new Vector3(tileData.Coordinates.x, tileData.Height, tileData.Coordinates.y);

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
                    if (keyValuePairs.ContainsKey(gridTileData.TileNumber))
                    {
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


    }

    [Serializable]
    public struct GridTileData
    {
        [SerializeField]
        private int tileNumber;
        [SerializeField]
        private Vector2Int coordinates;
        [SerializeField]
        private float height;
        [SerializeField]
        private GridEnums.Direction slantDirection;
        [SerializeField]
        private float slantAngle;
        [SerializeField]
        private GridEnums.Tile.Type type;

        public GridTileData(int tileNumber, Vector2Int coordinates, float height=0, GridEnums.Direction slantDirection=GridEnums.Direction.North, float slantAngle=0, GridEnums.Tile.Type type=GridEnums.Tile.Type.Undefined)
        {
            this.tileNumber = tileNumber;
            this.coordinates = coordinates;
            this.height = height;
            this.slantDirection = slantDirection;
            this.slantAngle = slantAngle;
            this.type = type;
        }

        public GridTileData(GridTileData refrenceData,float leadingEdgeHeight,GridEnums.Direction slantDirection)
        {
            this.tileNumber = refrenceData.TileNumber;
            this.coordinates = refrenceData.coordinates;
            this.height = refrenceData.height;
            this.slantDirection = slantDirection;
            this.type = refrenceData.type;

            slantAngle = Vector2.Angle(Vector2.up * leadingEdgeHeight+Vector2.right, Vector2.right);
        }

        public int TileNumber => tileNumber;
        public Vector2Int Coordinates => coordinates;
        public GridEnums.Direction SlantDirection => slantDirection;
        public float Height=> height;
        public float SlantAngle => slantAngle;
        public GridEnums.Tile.Type Type => type;

        public Vector3 UpVector
        {
            get
            {
                Vector3 slantDirectionVector = new();

                switch (slantDirection)
                {
                    case GridEnums.Direction.North:
                        slantDirectionVector = Vector3.forward;
                        if (slantAngle == 90)
                        {
                            return Vector3.back;
                        }
                        break;
                    case GridEnums.Direction.South:
                        slantDirectionVector = Vector3.back;
                        if (slantAngle == 90)
                        {
                            return Vector3.forward;
                        }
                        break;
                    case GridEnums.Direction.East:
                        slantDirectionVector = Vector3.right;
                        if (slantAngle == 90)
                        {
                            return Vector3.left;
                        }
                        break;
                    case GridEnums.Direction.West:
                        slantDirectionVector = Vector3.left;
                        if (slantAngle == 90)
                        {
                            return Vector3.right;
                        }
                        break;
                    default:
                        break;
                }

                Vector3 aboutAxis = Vector3.Cross(slantDirectionVector, Vector3.up);

                Vector3 upwardVector = Quaternion.AngleAxis(slantAngle, aboutAxis) * Vector3.up;

                return upwardVector.normalized;
            }

        }
        public Vector3 ForwardVector
        {
            get
            {
                switch (slantDirection)
                {
                    case GridEnums.Direction.North:
                        if (slantAngle == 90)
                        {
                            return Vector3.up;
                        }
                        break;
                    case GridEnums.Direction.South:
                        if (slantAngle == 90)
                        {
                            return Vector3.down;
                        }
                        break;
                    case GridEnums.Direction.East:
                        if (slantAngle == 90)
                        {
                            return Vector3.forward;
                        }
                        break;
                    case GridEnums.Direction.West:
                        if (slantAngle == 90)
                        {
                            return Vector3.forward;
                        }
                        break;
                    default:
                        break;
                }


                Vector3 aboutAxis = Vector3.Cross(UpVector, Vector3.forward);

                Vector3 forwardVector = Quaternion.AngleAxis(90, aboutAxis) * UpVector;

                return forwardVector.normalized;
            }
        }
        public Vector3 RighVector
        {
            get
            {
                switch (slantDirection)
                {
                    case GridEnums.Direction.North:
                        if (slantAngle == 90)
                        {
                            return Vector3.right;
                        }
                        break;
                    case GridEnums.Direction.South:
                        if (slantAngle == 90)
                        {
                            return Vector3.right;
                        }
                        break;
                    case GridEnums.Direction.East:
                        if (slantAngle == 90)
                        {
                            return Vector3.up;
                        }
                        break;
                    case GridEnums.Direction.West:
                        if (slantAngle == 90)
                        {
                            return Vector3.down;
                        }
                        break;
                    default:
                        break;
                }


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
                Vector3 Vertex = slantDirection switch
                {
                    GridEnums.Direction.North or GridEnums.Direction.South => (ForwardVector * (1 + SlantGap) + LeftVector) / 2,
                    GridEnums.Direction.East or GridEnums.Direction.West => (LeftVector * (1 + SlantGap) + ForwardVector) / 2,
                    _ => Vector3.zero,
                };
                return Vertex + Vector3.up * SlantHeightOffset;
            }
        }
        public Vector3 TopRightVertex
        {
            get
            {
                Vector3 Vertex = slantDirection switch
                {
                    GridEnums.Direction.North or GridEnums.Direction.South => (ForwardVector * (1 + SlantGap) + RighVector) / 2,
                    GridEnums.Direction.East or GridEnums.Direction.West => (RighVector * (1 + SlantGap) + ForwardVector) / 2,
                    _ => Vector3.zero,
                };
                return Vertex + Vector3.up * SlantHeightOffset;
            }
        }
        public Vector3 BottomRightVertex
        {
            get
            {
                Vector3 Vertex = slantDirection switch
                {
                    GridEnums.Direction.North or GridEnums.Direction.South => (BackVector * (1 + SlantGap) + RighVector) / 2,
                    GridEnums.Direction.East or GridEnums.Direction.West => (RighVector * (1 + SlantGap) + BackVector) / 2,
                    _ => Vector3.zero,
                };
                return Vertex + Vector3.up * SlantHeightOffset;
            }
        }
        public Vector3 BottomLeftVertex
        {
            get
            {
                Vector3 Vertex =  slantDirection switch
                {
                    GridEnums.Direction.North or GridEnums.Direction.South => (BackVector * (1 + SlantGap) + LeftVector) / 2,
                    GridEnums.Direction.East or GridEnums.Direction.West => (LeftVector * (1 + SlantGap) + BackVector) / 2,
                    _ => Vector3.zero,
                };

                return Vertex + Vector3.up * SlantHeightOffset;
            }
        }

        public float LeadingEdgeHeight
        {
            get
            {
                float angle = slantAngle;
                float hypotenuse = 1 + SlantGap;

                float sinvalue = Mathf.Sin(angle * Mathf.Deg2Rad);

                return sinvalue * hypotenuse;
            }
        }

        private float SlantHeightOffset
        {
            get
            {
                float angle = 90 - slantAngle;
                float hypotenuseLenght = 1 + SlantGap;

                float cosVal = Mathf.Cos(angle*Mathf.Deg2Rad);

                return cosVal * hypotenuseLenght/2;
            }
        }
   
        private float SlantGap
        {
            get
            {
                float hypotenuseLength = 1 / Mathf.Cos(slantAngle * Mathf.Deg2Rad);

                return (hypotenuseLength-1);
            }
        }
    }


}

