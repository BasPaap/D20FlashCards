namespace Bas.D20FlashCards
{
    public abstract class Card
    {
        public string Title { get; set; }
        public CardType CardType { get; set; }

        public override string ToString() => $"{CardType}: {Title}";
    }
}