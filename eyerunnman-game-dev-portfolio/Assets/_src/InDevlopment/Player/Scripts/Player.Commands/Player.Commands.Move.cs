using UnityEngine;
using com.portfolio.interfaces;

namespace com.portfolio.player
{
    public partial class Player
    {
        public partial class Commands
        {
            public class Move : ICommand<Player>
            {
                private readonly Vector2 moveInput;

                public Move(Vector2 moveInput)
                {
                    this.moveInput = moveInput;
                }

                public void Execute(Player player)
                {
                    player.MoveInput = moveInput.normalized;
                }
            }

        }
    }

}

