public class Bonus
{
    public BonusType Type { get; }
    public int Magnitude { get; }

    public Bonus(CardBase cardBase)
    {
        Type = cardBase.BonusType;
        Magnitude = cardBase.Magnitude;
    }
}
