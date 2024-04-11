public class SpadeCard : NumberedCard
{
    public override int GetCardPoint() => CardNumber == 1 ? 1 : 0;
}