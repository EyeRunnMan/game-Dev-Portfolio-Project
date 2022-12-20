using UnityEngine;

namespace com.portfolio.player
{
    public partial class Player
    {
        private partial class State
        {
            public class Walking : State
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
                    if (player.JumpInput)
                    {
                        SwitchState(player, player.JumpState);
                    }
                    if (player.RunInput)
                    {
                        SwitchState(player, player.RunningState);
                    }
                    if (player.MoveInput == Vector2.zero)
                    {
                        SwitchState(player, player.IdleState);
                    }
                    
                    player.Move(player.PlayerConfig.walkSpeed);
                    

                }
            }
        }
    }
}

