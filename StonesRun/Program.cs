
Console.WriteLine("Input random sequence of domino stones in the specified format (\'[1|2] [2|5] ...\')");
var stonesSequence = Console.ReadLine() ?? "";

try
{
    var dominoCollection = new DominoCollection(stonesSequence);

    var answer = dominoCollection.FindCircle();
    Console.WriteLine(answer);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Wrong sequence format: {ex.Message}");
}
catch (AggregateException ex)
{
    Console.WriteLine($"Couldn't find possible circle in the sequence: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Something went wrong: {ex.Message}");
}