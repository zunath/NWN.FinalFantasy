using NWN.FinalFantasy.Quest.API.Contracts;
using static NWN._;

namespace NWN.FinalFantasy.Quest.API.Reward
{
    public class ItemReward : IQuestReward
    {
        public bool IsSelectable { get; }
        public string MenuName { get; }
        private readonly string _resref;
        private readonly int _quantity;

        public ItemReward(string resref, int quantity, bool isSelectable)
        {
            _resref = resref;
            _quantity = quantity;
            IsSelectable = isSelectable;

            var tempStorage = GetObjectByTag("TEMP_QUEST_ITEM_STORAGE");
            var tempItem = CreateItemOnObject(resref, tempStorage, quantity);
            var name = GetName(tempItem);
            DestroyObject(tempItem, 0.1f);

            if (_quantity > 1)
                MenuName = _quantity + "x " + name;
            else
                MenuName = name;
        }


        public void GiveReward(NWGameObject player)
        {
            CreateItemOnObject(_resref, player, _quantity);
        }
    }
}