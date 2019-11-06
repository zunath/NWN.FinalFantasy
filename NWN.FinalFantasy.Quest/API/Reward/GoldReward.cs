using NWN.FinalFantasy.Quest.API.Contracts;
using static NWN._;

namespace NWN.FinalFantasy.Quest.API.Reward
{
    internal class GoldReward: IQuestReward
    {
        public int Amount { get; }
        public bool IsSelectable { get; }
        public string MenuName => Amount + " Gil";

        public GoldReward(int amount, bool isSelectable)
        {
            Amount = amount;
            IsSelectable = isSelectable;
        }

        public void GiveReward(NWGameObject player)
        {
            GiveGoldToCreature(player, Amount);
        }
    }
}
