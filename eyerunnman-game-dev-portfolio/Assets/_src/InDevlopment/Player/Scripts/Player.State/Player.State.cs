using com.portfolio.interfaces;

namespace com.portfolio.player
{
    public partial class Player
    {
        private abstract partial class State : IState<Player>
        {
            public virtual void OnEnterState(Player player)
            {
                player.CurrentState = this;
            }

            public abstract void OnExitState(Player player);

            public abstract void OnUpdateState(Player player);

            public virtual void SwitchState(Player player, IState<Player> newState)
            {
                OnExitState(player);
                newState.OnEnterState(player);
            }
        }
    }
}

