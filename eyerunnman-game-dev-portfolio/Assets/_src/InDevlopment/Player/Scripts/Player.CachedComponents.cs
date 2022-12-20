using UnityEngine;

namespace com.portfolio.player
{
    public partial class Player
    {
        private class CachedComponents
        {
            public CharacterController CharacterController { get; set; }
            public CachedComponents(Player player)
            {
                CharacterController = player.GetComponent<CharacterController>();
            }

        }

    }

}

