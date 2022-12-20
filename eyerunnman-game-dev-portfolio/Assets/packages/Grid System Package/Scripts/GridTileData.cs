using UnityEngine;
using System;

namespace com.package.gridSystem
{
    [Serializable]
    public struct GridTileData : IEquatable<GridTileData>
    {
        [SerializeField]
        private int tileId;
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

        public static GridTileData Default => new(-1,new Vector2Int(-1,-1));

        public bool IsDefault => this == Default;

        #region Constructors
        public GridTileData(int tileId, Vector2Int coordinates, float height = 0, GridEnums.Direction slantDirection = GridEnums.Direction.North, float slantAngle = 0, GridEnums.Tile.Type type = GridEnums.Tile.Type.Undefined)
        {
            this.tileId = tileId;
            this.coordinates = coordinates;
            this.height = height;
            this.slantDirection = slantDirection;
            this.slantAngle = slantAngle;
            this.type = type;
        }

        public GridTileData(GridTileData refrenceData, float leadingEdgeHeight, GridEnums.Direction slantDirection)
        {

            if (refrenceData.IsDefault)
            {
                this = Default;
                return;
            }

            this.tileId = refrenceData.TileId;
            this.coordinates = refrenceData.coordinates;
            this.height = refrenceData.height;
            this.slantDirection = slantDirection;
            this.type = refrenceData.type;

            slantAngle = Vector2.Angle(Vector2.up * leadingEdgeHeight + Vector2.right, Vector2.right);

            if (leadingEdgeHeight < 0)
            {
                this.height = refrenceData.height + leadingEdgeHeight;

                this.slantDirection = slantDirection switch
                {
                    GridEnums.Direction.North => GridEnums.Direction.South,
                    GridEnums.Direction.South => GridEnums.Direction.North,
                    GridEnums.Direction.East => GridEnums.Direction.West,
                    GridEnums.Direction.West => GridEnums.Direction.East,
                    _ => slantDirection,
                };
            }
        }

        public GridTileData(GridTileData currentData, GridTileData refrenceData)
        {
            this.tileId = currentData.tileId;
            this.coordinates = currentData.coordinates;
            this.height = refrenceData.height;
            this.slantDirection = refrenceData.SlantDirection;
            this.slantAngle = refrenceData.slantAngle;
            this.type = refrenceData.type;
        }
        #endregion

        #region Public Properties


        public int TileId => tileId;

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
                return Vertex + Vector3.up * SlantHeightOffset + TileCenter;
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
                return Vertex + Vector3.up * SlantHeightOffset + TileCenter;
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
                return Vertex + Vector3.up * SlantHeightOffset + TileCenter;
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

                return Vertex + Vector3.up * SlantHeightOffset + TileCenter;
            }
        }

        public Vector3 Center => new(Coordinates.x, Height + SlantHeightOffset, Coordinates.y);

        private Vector3 TileCenter => new(Coordinates.x, Height, Coordinates.y);

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


        #endregion


        public override bool Equals(object obj)
        {
            return obj is GridTileData data && Equals(data);
        }
        public bool Equals(GridTileData other)
        {
            if (other.GetHashCode() != GetHashCode())
            {
                return false;
            }

            return tileId == other.tileId &&
                   coordinates.Equals(other.coordinates) &&
                   height == other.height &&
                   slantDirection == other.slantDirection &&
                   slantAngle == other.slantAngle &&
                   type == other.type &&
                   TopLeftVertex.Equals(other.TopLeftVertex) &&
                   TopRightVertex.Equals(other.TopRightVertex) &&
                   BottomRightVertex.Equals(other.BottomRightVertex) &&
                   BottomLeftVertex.Equals(other.BottomLeftVertex) &&
                   Center.Equals(other.Center) &&
                   TileCenter.Equals(other.TileCenter) &&
                   LeadingEdgeHeight == other.LeadingEdgeHeight &&
                   SlantHeightOffset == other.SlantHeightOffset &&
                   SlantGap == other.SlantGap;
        }
        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(tileId);
            hash.Add(coordinates);
            hash.Add(height);
            hash.Add(slantDirection);
            hash.Add(slantAngle);
            hash.Add(type);
            hash.Add(TopLeftVertex);
            hash.Add(TopRightVertex);
            hash.Add(BottomRightVertex);
            hash.Add(BottomLeftVertex);
            hash.Add(Center);
            hash.Add(TileCenter);
            hash.Add(LeadingEdgeHeight);
            hash.Add(SlantHeightOffset);
            hash.Add(SlantGap);
            return hash.ToHashCode();
        }
        public static bool operator ==(GridTileData left, GridTileData right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(GridTileData left, GridTileData right)
        {
            return !(left == right);
        }
    }


}

