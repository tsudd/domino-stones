using DominoStones.Shared.Model;
using System.Text;
using System.Text.RegularExpressions;

namespace DominoStones.Shared;
public class DominoCollection
{
    public readonly static Regex _dominoRandomSequence = new Regex(
        @"^(\[([1-6]\|[1-6])\]\s?)+$", RegexOptions.Compiled);
    public IEnumerable<Domino> Stones
    {
        get
        {
            return _stones.ToArray();
        }
    }
    protected List<Domino> _stones = new List<Domino>();
    protected List<DominoMatch>[] _matches;
    public DominoCollection(string rawSequence)
    {
        var match = _dominoRandomSequence.Match(rawSequence);
        if (match.Length != rawSequence.Length)
            throw new ArgumentException("Incorrect format of dominos stones sequence");

        var rawStones = match.Groups[2].Captures;

        var stonesAmount = rawStones.Count();
        _matches = new List<DominoMatch>[stonesAmount];
        var i = 0;
        foreach (Capture rawStone in rawStones)
        {
            _stones.Add(
                new Domino(
                    byte.Parse(rawStone.Value[0].ToString()),
                    byte.Parse(rawStone.Value[^1].ToString())));

            _matches[i] = new List<DominoMatch>();
            i++;
        }
        _usedDominos = new DominoStates[stonesAmount];
        _dominoIndexSequence = Enumerable.Repeat(-1, stonesAmount).ToArray();
        MatchDominos();
    }

    protected void MatchDominos()
    {
        for (var i = 0; i < _stones.Count; i++)
        {
            for (var j = i + 1; j < _stones.Count; j++)
            {
                var stonesMatches = new List<DominoMatch>();
                if (_stones[i].TryMatchTo(_stones[j], out stonesMatches, i, j))
                {
                    _matches[i].AddRange(stonesMatches);
                    _matches[j].AddRange(stonesMatches);
                }
            }
        }
    }

    public override string ToString()
    {
        var ans = new StringBuilder();
        ans.AppendJoin(' ', _stones);
        return ans.ToString();
    }

    public IEnumerable<DominoMatch> GetDominoMatches(int index)
    {
        return _matches[index].ToArray();
    }

    private enum DominoStates
    {
        NotChecked,
        Checking,
        Checked
    }
    private DominoStates[] _usedDominos;
    private int[] _dominoIndexSequence;
    private int _startDominoInCircle;
    private int _endDominoInCircle;
    private DominoHalfs _endDominoHalf;

    public string FindCircle()
    {
        var dominoAmount = _stones.Count();
        _usedDominos = new DominoStates[dominoAmount];
        _dominoIndexSequence = new int[dominoAmount];
        _startDominoInCircle = -1;
        for (var i = 0; i < dominoAmount; i++)
        {
            if (DominoDFC(i, DominoHalfs.First))
                break;
        }
        if (_startDominoInCircle < 0)
            throw new AggregateException("Couldn't find any circles for dominos");
        if (AreAllDominosChecked() == false)
            throw new AggregateException("It's impossible to build a circle with all dominos.");
        var dominoSequence = new List<Tuple<Domino, bool>>()
        {
            _stones[_startDominoInCircle].RotateIfNeeded(
                new Tuple<Domino, bool>(_stones[_endDominoInCircle],
                (_stones[_startDominoInCircle].GetHalfValue(DominoHalfs.First)
                ==
                _stones[_endDominoInCircle].GetHalfValue(_endDominoHalf))
                ?
                false
                :
                true))
        };
        for (var i = _endDominoInCircle; i != _startDominoInCircle; i = _dominoIndexSequence[i])
        {
            dominoSequence.Add(_stones[i].RotateIfNeeded(dominoSequence[^1]));
        }
        var circle = new StringBuilder();
        foreach (var domino in dominoSequence)
        {
            circle.Append(domino.Item1.ToString(domino.Item2));
            circle.Append(" ");
        }
        return circle.ToString().Trim();
    }

    private bool AreAllDominosChecked()
    {
        foreach (var use in _usedDominos)
        {
            if (use == DominoStates.NotChecked)
            {
                return false;
            }
        }
        return true;
    }

    private bool DominoDFC(int enteredDominoIndex, DominoHalfs enteredHalf)
    {
        _usedDominos[enteredDominoIndex] = DominoStates.Checking;

        foreach (var halfMatch in _matches[enteredDominoIndex])
        {
            if (enteredHalf != halfMatch.GetDominoHalfByBaseInd(enteredDominoIndex))
            {
                int otherDominoBaseIndex = halfMatch.GetOtherDominoBaseInd(enteredDominoIndex);
                if (_usedDominos[otherDominoBaseIndex] == DominoStates.NotChecked)
                {
                    _dominoIndexSequence[otherDominoBaseIndex] = enteredDominoIndex;
                    if (DominoDFC(
                        otherDominoBaseIndex,
                        halfMatch.GetDominoHalfByBaseInd(otherDominoBaseIndex)))
                        return true;
                }
                else if (_usedDominos[otherDominoBaseIndex] == DominoStates.Checking && AreAllDominosChecked())
                {
                    _endDominoInCircle = enteredDominoIndex;
                    _startDominoInCircle = otherDominoBaseIndex;
                    _endDominoHalf = enteredHalf == DominoHalfs.First ? DominoHalfs.Second : DominoHalfs.First;
                    return true;
                }
            }
        }
        _usedDominos[enteredDominoIndex] = DominoStates.Checked;
        return false;
    }
}
