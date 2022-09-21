namespace DominoStones.Test;

public class DominoCollectionTest
{
    [Fact]
    public void TestCollectionCreation()
    {
        //given
        var testSequence = "[2|1] [2|3] [1|3]";

        //when
        var dominoCollection = new DominoCollection(testSequence);
        var ans = dominoCollection.ToString();

        //then
        Assert.Equal(testSequence, ans);
    }

    [Fact]
    public void TestBadCollectionCreation()
    {
        //given
        var testSequence = "[2|1 ]a[22|3] [1|333]";

        //when
        Assert.Throws<ArgumentException>(() =>
        {
            var dominoCollection = new DominoCollection(testSequence);
        });

        //given
        testSequence = "[2|122] [22|3] [1|333]";

        //when
        Assert.Throws<ArgumentException>(() =>
        {
            var dominoCollection = new DominoCollection(testSequence);
        });

        //given
        testSequence = "bad string and stones: [1|2] [2|3]";

        //when
        Assert.Throws<ArgumentException>(() =>
        {
            var dominoCollection = new DominoCollection(testSequence);
        });
    }

    [Fact]
    public void TestDominoMatching()
    {
        //given
        var testSequence = "[2|1] [2|3] [1|3]";

        //when
        var dominoCollection = new DominoCollection(testSequence);

        var stones = dominoCollection.Stones.ToArray<Domino>();
        var matches = dominoCollection.Matches.ToArray<DominoMatch>();

        var expectedMatches = new[]{
            new DominoMatch(stones[0], stones[1], DominoHalfs.First, DominoHalfs.First),
            new DominoMatch(stones[0], stones[2], DominoHalfs.Second, DominoHalfs.First),
            new DominoMatch(stones[1], stones[2], DominoHalfs.Second, DominoHalfs.Second),
        };

        //then
        Assert.Equal<int>(expectedMatches.Count(), matches.Count());
        for (var i = 0; i < expectedMatches.Count(); i++)
        {
            Assert.True(matches[i].Equals(expectedMatches[i]));
        }
    }
}