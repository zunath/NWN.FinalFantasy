namespace NWN.FinalFantasy.AI
{
    public class EnmityTarget
    {
        public NWGameObject Target { get; set; }

        private int _amount;
        public int Amount
        {
            get => _amount;
            set
            {
                _amount += value;
                if (_amount < 0)
                    _amount = 0;
            }
        }

        public EnmityTarget(NWGameObject target, int amount)
        {
            Target = target;
            Amount = amount;
        }
    }
}
