public class SpadeCard : CardBehaviour
{
    public override int GetCardPoint() => CardNumber == 1 ? 1 : 0;
}