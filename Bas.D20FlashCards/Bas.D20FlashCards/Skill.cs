namespace Bas.D20FlashCards
{
    public sealed class Skill : Card
    {
        public string Description { get; set; }
        public string Check { get; set; }
        public string Action { get; set; }
        public string TryAgain { get; set; }
        public string Special { get; set; }
        public string Untrained { get; set; }

        public override string ToString() => $"{nameof(Skill)}{(string.IsNullOrWhiteSpace(Name) ? string.Empty : $"\"{Name}\"")}";
    }
}