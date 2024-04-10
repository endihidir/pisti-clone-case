public class HeartCard : CardBehaviour
{
    public override int GetCardPoint() => CardNumber == 1 ? 1 : 0;
}