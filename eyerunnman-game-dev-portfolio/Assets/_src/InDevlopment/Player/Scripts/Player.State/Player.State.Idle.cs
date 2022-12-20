using com.portfolio.interfaces;
using UnityEngine;

namespace com.portfolio.player
{
    public partial class Player
    {
        private partial class State
        {
            public class Idle : State
            {
                public override void OnEnterState(Player player)
                {
                    base.OnEnterState(player);
                }

                public override void OnExitState(Player player)
                {

                }

                public override void OnUpdateState(Player player)
                {
                    if (player.MoveInput != Vector2.zero)
                    {
                        SwitchState(player, player.WalkingState);
                    }

                }
            }

        }
    }
}

