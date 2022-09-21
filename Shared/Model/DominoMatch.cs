namespace DominoStones.Shared.Model;

public enum DominoHalfs
{
    First,
    Second
}
public class DominoMatch
{
    public Tuple<Domino, Domino> Dominos { get; init; }
    public DominoHalfs FirstDominoHalf { get; init; }
    public DominoHalfs SecondDominoHalf { get; init; }

    public DominoMatch(
        Domino firstStone,
        Domino secondStone,
        DominoHalfs firstStoneHalf,
        DominoHalfs secondStoneHalf)
    {
        Dominos = new Tuple<Domino, Domino>(firstStone, secondStone);
        FirstDominoHalf = firstStoneHalf;
        SecondDominoHalf = secondStoneHalf;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj is DominoMatch)
            return obj.GetHashCode() == GetHashCode();
        return false;
    }

    public override int GetHashCode()
    {
        return ($"{Dominos.Item1.ToString()}" +
            $"{Dominos.Item2.ToString()}{FirstDominoHalf}{SecondDominoHalf}").GetHashCode();
    }
}