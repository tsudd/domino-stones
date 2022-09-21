﻿using DominoStones.Shared.Model;
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
        _matches = new List<DominoMatch>[rawStones.Count()];
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
        MatchDominos();
    }

    protected void MatchDominos()
    {
        for (var i = 0; i < _stones.Count; i++)
        {
            for (var j = i + 1; j < _stones.Count; j++)
            {
                var stonesMatches = new List<DominoMatch>();
                if (_stones[i].TryMatchTo(_stones[j], out stonesMatches))
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

    // public string FindCycle()
    // {

    // }
}
