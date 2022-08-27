namespace Raylab.Modules.Knucklebones.Mechanics;

internal readonly struct ChainTurn<T>
{
    private readonly int _index;
    private readonly T[] _values;

    public ChainTurn(params T[] values)
        : this(0, values)
    {
    }

    private ChainTurn(int index, params T[] values)
    {
        _index = index;
        _values = values;
    }

    public T Current => _values[_index];
    
    public ChainTurn<T> Next => new((_index + 1) % _values.Length, _values);
}
