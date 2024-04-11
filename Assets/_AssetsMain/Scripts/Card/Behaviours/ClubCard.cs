public class ClubCard : NumberedCard
{
    public override int GetCardPoint() => CardNumber == 1 ? 1 : 0;
}