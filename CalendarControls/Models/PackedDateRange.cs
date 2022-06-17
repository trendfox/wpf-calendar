namespace CalendarControls;

public class PackedDateRange<TPackedItem>
{
    public int PackIndex { get; private set; }
    public int ColorIndex { get; private set; }
    public TPackedItem PackedItem { get; private set; }

    public PackedDateRange(int packNumber, int colorIndex, TPackedItem packedItem)
    {
        PackIndex = packNumber;
        ColorIndex = colorIndex;
        PackedItem = packedItem;
    }
}
