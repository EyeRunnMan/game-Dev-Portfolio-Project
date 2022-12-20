using com.portfolio.interfaces;

namespace com.portfolio.player
{
    public partial class Player
    {
        public partial class Commands
        {
            public class Run : ICommand<Player>
            {
                private readonly bool run;
                public Run(bool run)
                {
                    this.run = run;
                }

                public void Execute(Player player)
                {
                    player.RunInput = run;
                }
            }
        }
    }

}

