using UnityEngine;

namespace com.portfolio.player
{
        [CreateAssetMenu(fileName = "PlayerConfigData", menuName = "Player/GridDebugData", order = 2)]
        public class PlayerConfig : ScriptableObject {
            [Range(0,10)]
            public float walkSpeed = 1f;
            [Range(0, 20)]
            public float runSpeed = 1f;
        }
}

