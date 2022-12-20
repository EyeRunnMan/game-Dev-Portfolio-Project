using UnityEngine;

namespace com.portfolio.player
{
    public partial class Player
    {
        private partial class State
        {
            public class Jump : State
            {
                public override void OnEnterState(Player player)
                {
                    base.OnEnterState(player);
                    //fire jump
                }

                public override void OnExitState(Player player)
                {

                }

                public override void OnUpdateState(Player player)
                {
                    //on grounded
                    if (true)
                    {
                        if (player.JumpInput)
                        {
                            SwitchState(player, player.JumpState);
                        }

                        if (player.MoveInput == Vector2.zero)
                        {
                            SwitchState(player, player.IdleState);
                        }

                        if (!player.RunInput)
                        {
                            SwitchState(player, player.WalkingState);
                        }
                        else
                        {
                            SwitchState(player, player.RunningState);
                        }
                    }

                }
            }

        }

    }

}

