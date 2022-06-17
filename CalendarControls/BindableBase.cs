using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CalendarControls;

/// <summary>
/// ViewModel base class.
/// </summary>
public class BindableBase
    : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
