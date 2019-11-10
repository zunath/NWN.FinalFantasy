namespace NWN.FinalFantasy.Core.Message
{
    public class JobXPGained
    {
        public NWGameObject Creature { get; set; }
        public int Amount { get; set; }

        public JobXPGained(NWGameObject creature, int amount)
        {
            Creature = creature;
            Amount = amount;
        }
    }
}
