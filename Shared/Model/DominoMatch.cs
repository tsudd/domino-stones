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
    public int FirstDominoBaseIndex { get; set; }
    public int SecondDominoBaseIndex { get; set; }
    public DominoMatch(
        Domino firstStone,
        Domino secondStone,
        DominoHalfs firstStoneHalf,
        DominoHalfs secondStoneHalf,
        int firstStoneBaseIndex = -1,
        int secondDominoBaseIndex = -1)
    {
        Dominos = new Tuple<Domino, Domino>(firstStone, secondStone);
        FirstDominoHalf = firstStoneHalf;
        SecondDominoHalf = secondStoneHalf;
        FirstDominoBaseIndex = firstStoneBaseIndex;
        SecondDominoBaseIndex = secondDominoBaseIndex;
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

    public DominoHalfs GetDominoHalfByBaseInd(int index)
    {
        if (FirstDominoBaseIndex == index)
            return FirstDominoHalf;
        return SecondDominoHalf;
    }

    public int GetOtherDominoBaseInd(int index)
    {
        if (FirstDominoBaseIndex == index)
            return SecondDominoBaseIndex;
        return FirstDominoBaseIndex;
    }
}