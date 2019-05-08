namespace Bas.D20FlashCards
{
    public sealed class Feat : Card
    {
        public string Description { get; set; }
        public string Benefit { get; set; }
        public string Prerequisites { get; set; }
        public string Normal { get; set; }
        public string Special { get; set; }

        public override string ToString() => $"{nameof(Feat)}{(string.IsNullOrWhiteSpace(Name) ? string.Empty : $" \"{Name}\"")}";
    }
}