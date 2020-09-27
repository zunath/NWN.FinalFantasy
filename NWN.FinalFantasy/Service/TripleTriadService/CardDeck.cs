namespace NWN.FinalFantasy.Service.TripleTriadService
{
    public class CardDeck
    {
        public CardDeck()
        {
            Name = string.Empty;
            Card1 = CardType.Invalid;
            Card2 = CardType.Invalid;
            Card3 = CardType.Invalid;
            Card4 = CardType.Invalid;
            Card5 = CardType.Invalid;
        }

        public string Name { get; set; }
        public CardType Card1 { get; set; }
        public CardType Card2 { get; set; }
        public CardType Card3 { get; set; }
        public CardType Card4 { get; set; }
        public CardType Card5 { get; set; }
    }
}
