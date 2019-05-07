namespace Bas.D20FlashCards
{
    public abstract class Card
    {
        public string Name { get; set; }
        
        public override string ToString() => $": {Name}";
    }
}