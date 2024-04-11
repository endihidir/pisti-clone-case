public class HeartCard : NumberedCard
{
    public override int GetCardPoint() => CardNumber == 1 ? 1 : 0;
}