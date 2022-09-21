namespace DominoStones.Shared.Model;

public enum DominoHalfs
{
    First,
    Second
}
public class DominoMatch
{
    public Tuple<Domino, Domino> Dominos { get; init; }
    private Tuple<DominoHalfs, DominoHalfs> _matchedHalfs;

    public DominoMatch(
        Domino firstStone,
        Domino secondStone,
        DominoHalfs firstStoneHalf,
        DominoHalfs secondStoneHalf)
    {
        Dominos = new Tuple<Domino, Domino>(firstStone, secondStone);
        _matchedHalfs = new Tuple<DominoHalfs, DominoHalfs>(firstStoneHalf, secondStoneHalf);
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
            $"{Dominos.Item2.ToString()}{_matchedHalfs.Item1}{_matchedHalfs.Item2}").GetHashCode();
    }
}