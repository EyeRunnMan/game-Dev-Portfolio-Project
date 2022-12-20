namespace com.portfolio.player
{
    public partial class PlayerInvoker
    {
        private class CachedComponents
        {
            public Player Player { get; set; }
            public CachedComponents(PlayerInvoker playerInvoker)
            {
                Player = playerInvoker.GetComponent<Player>();
            }

        }
    }
}

