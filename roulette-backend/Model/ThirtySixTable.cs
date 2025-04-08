namespace Roulette.App.Model
{
    public class ThirtySixTable : ITableLayout
    {
        public int[] Numbers { get; } = Enumerable.Range(0, 37).ToArray();
        public int[] redNumbers { get; } = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
        public int[] blackNumbers { get; } = { 2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35 };
        public string? GetColor(int number)
        {
            if (!Numbers.Contains(number)) { return null; }
            if (number == 0) { return "Green"; }
            return redNumbers.Contains(number) ? "Red" : "Black";
        }

        public int[] GetNumbers()
        {
            return Numbers;
        }
    }
}
