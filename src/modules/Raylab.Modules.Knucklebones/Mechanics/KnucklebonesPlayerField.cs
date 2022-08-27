namespace Raylab.Modules.Knucklebones.Mechanics;

internal class KnucklebonesPlayerField
{
    private const byte FieldSize = 3;

    private readonly int[][] _field = new int[FieldSize][];

    public KnucklebonesPlayerField()
    {
        for (var column = 0; column < FieldSize; column++)
        {
            _field[column] = new int[FieldSize];
        }
    }

    public int Score => CalculateScore();

    public bool Filled => _field.All(column => column.All(row => row != 0));

    public int GetDice(int column, int row)
    {
        return _field[column][row];
    }

    public bool AddDice(int column, int value)
    {
        for (var row = 0; row < FieldSize; row++)
        {
            if (_field[column][row] == 0)
            {
                _field[column][row] = value;
                return true;
            }
        }

        return false;
    }

    public void RemoveDice(int column, int value)
    {
        for (var row = 0; row < FieldSize; row++)
        {
            if (_field[column][row] == value)
            {
                _field[column][row] = 0;
            }
        }
    }

    private int CalculateScore()
    {
        return (int)_field.Select(row => row
                .GroupBy(x => x)
                .Select(x => x.Key * Math.Pow(x.Count(), 2))
                .Sum())
            .Sum();
    }
}
