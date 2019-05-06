namespace Bas.D20FlashCards
{
    public abstract class Card
    {
        public string Name { get; set; }
        public CardType CardType { get; protected set; }

        public override string ToString() => $"{CardType}: {Name}";
    }
}