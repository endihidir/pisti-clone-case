public class DiamondCard : NumberedCard
{
    public override int GetCardPoint() => CardNumber switch
    {
        1 => 1,
        10 => 3,
        _ => 0
    };
}