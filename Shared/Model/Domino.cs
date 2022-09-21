namespace DominoStones.Shared.Model;
public class Domino
{
    private Tuple<byte, byte> _halfs;

    public Domino(byte firstHalf, byte secondHalf)
    {
        _halfs = new Tuple<byte, byte>(firstHalf, secondHalf);
    }

    public string ToString(bool rotate = false)
    {
        return rotate ? $"[{_halfs.Item2}|{_halfs.Item1}]" : $"[{_halfs.Item1}|{_halfs.Item2}]";
    }

    public override string ToString()
    {
        return this.ToString();
    }

    public bool IsFirstHalf(byte value)
    {
        if (value != _halfs.Item1 && value != _halfs.Item2)
            throw new ArgumentException("No such half in the stone!");
        if (value == _halfs.Item1)
            return true;
        return false;
    }

    public bool TryMatchTo(Domino otherStone, out List<DominoMatch> matches)
    {
        matches = new List<DominoMatch>();
        if (_halfs.Item1 == otherStone.GetHalfValue(DominoHalfs.First))
            matches.Add(new DominoMatch(this, otherStone, DominoHalfs.First, DominoHalfs.First));
        if (_halfs.Item2 == otherStone.GetHalfValue(DominoHalfs.First))
            matches.Add(new DominoMatch(this, otherStone, DominoHalfs.Second, DominoHalfs.First));
        if (_halfs.Item1 == otherStone.GetHalfValue(DominoHalfs.Second))
            matches.Add(new DominoMatch(this, otherStone, DominoHalfs.First, DominoHalfs.Second));
        if (_halfs.Item2 == otherStone.GetHalfValue(DominoHalfs.Second))
            matches.Add(new DominoMatch(this, otherStone, DominoHalfs.Second, DominoHalfs.Second));

        if (matches.Count == 0)
            return false;
        return true;
    }

    public byte GetHalfValue(DominoHalfs half)
    {
        if (DominoHalfs.First == half)
            return _halfs.Item1;
        return _halfs.Item2;
    }
}