using com.portfolio.interfaces;

namespace com.portfolio.player
{
    public partial class Player
    {
        public partial class Commands
        {
            public class SetConfig : ICommand<Player>
            {
                private readonly PlayerConfig config;
                public SetConfig(PlayerConfig config)
                {
                    this.config = config;
                }

                public void Execute(Player player)
                {
                    player.PlayerConfig = config;
                }
            }
        }
    }
}

