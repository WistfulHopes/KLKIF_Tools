namespace UnitDataEditor.UnitData;

public interface IUnitData
{
    public int Parse(byte[] buffer);
    public byte[] Rebuild();
}