
using System;

namespace com.portfolio.gridSystem
{
    public static class GridEnums{

        [Serializable]
        public enum Direction
        {
            North,
            South,
            East,
            West,
            Undefined
        }

        public static class Tile{

            [Serializable]
            public enum Type
            {
               Undefined,Stone
            }
        }
        
    }
}
