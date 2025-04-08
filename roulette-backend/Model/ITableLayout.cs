namespace Roulette.App.Model
{
    public interface ITableLayout
    {
        public string? GetColor(int number);
        public int[] GetNumbers();
    }
}
