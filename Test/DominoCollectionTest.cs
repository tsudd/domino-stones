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
        var allMatches = new[]{
            new DominoMatch(stones[0], stones[1], DominoHalfs.First, DominoHalfs.First),
            new DominoMatch(stones[0], stones[2], DominoHalfs.Second, DominoHalfs.First),
            new DominoMatch(stones[1], stones[2], DominoHalfs.Second, DominoHalfs.Second),
        };

        //then
        var matches = dominoCollection.GetDominoMatches(0).ToArray<DominoMatch>();

        var expectedMatches = new DominoMatch[] {
            allMatches[0],
            allMatches[1]
        };
        Assert.Equal<int>(expectedMatches.Count(), matches.Count());
        for (var i = 0; i < expectedMatches.Count(); i++)
        {
            Assert.True(matches[i].Equals(expectedMatches[i]));
        }

        matches = dominoCollection.GetDominoMatches(1).ToArray<DominoMatch>();
        expectedMatches = new DominoMatch[] {
            allMatches[0],
            allMatches[2],
        };
        Assert.Equal<int>(expectedMatches.Count(), matches.Count());
        for (var i = 0; i < expectedMatches.Count(); i++)
        {
            Assert.True(matches[i].Equals(expectedMatches[i]));
        }

        matches = dominoCollection.GetDominoMatches(2).ToArray<DominoMatch>();
        expectedMatches = new DominoMatch[] {
            allMatches[1],
            allMatches[2],
        };
        Assert.Equal<int>(expectedMatches.Count(), matches.Count());
        for (var i = 0; i < expectedMatches.Count(); i++)
        {
            Assert.True(matches[i].Equals(expectedMatches[i]));
        }
    }

    [Fact]
    public void TestCycleCheck()
    {
        // Given
        var dominoCollection = new DominoCollection("[2|1] [2|3] [1|3]");

        // When
        var ans = dominoCollection.FindCircle();

        // Then
        Assert.Equal("[1|2] [2|3] [3|1]", ans);

        // Given
        dominoCollection = new DominoCollection("[1|1] [1|1] [1|1]");

        // When
        ans = dominoCollection.FindCircle();

        // Then
        Assert.Equal("[1|1] [1|1] [1|1]", ans);

        // Given
        dominoCollection = new DominoCollection("[1|1] [1|1] [1|1]");

        // When
        ans = dominoCollection.FindCircle();

        // Then
        Assert.Equal("[1|1] [1|1] [1|1]", ans);

        // Given
        dominoCollection = new DominoCollection("[1|1] [1|2] [1|2] [1|1]");

        // When
        ans = dominoCollection.FindCircle();

        // Then
        Assert.Equal("[1|1] [1|1] [1|2] [2|1]", ans);

    }

    [Fact]
    public void TestInvalidCycle()
    {
        // Given
        var dominoCollection = new DominoCollection("[1|2] [4|1] [2|3]");

        // When
        Assert.Throws<AggregateException>(() =>
        {
            var ans = dominoCollection.FindCircle();
        });

        // Given
        dominoCollection = new DominoCollection("[1|1] [1|2] [1|2] [3|1]");

        // When
        Assert.Throws<AggregateException>(() =>
        {
            var ans = dominoCollection.FindCircle();
        });

        // Given
        dominoCollection = new DominoCollection("[1|3] [3|1] [6|2] [2|6]");

        // When
        Assert.Throws<AggregateException>(() =>
        {
            var ans = dominoCollection.FindCircle();
        });

        // Given
        dominoCollection = new DominoCollection("[1|2] [3|3] [3|5] [5|2]");

        // When
        Assert.Throws<AggregateException>(() =>
        {
            var ans = dominoCollection.FindCircle();
        });
    }
}