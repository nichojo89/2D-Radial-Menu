using System.Collections.Generic;
using RadialMenuSkia.Pickers;

namespace RadialMenuSkia.ViewModels
{
    public class MainPageViewModel
    {
        public MainPageViewModel()
        {
            ItemSource = new List<MenuItem>
        {
            new MenuItem("Apple", "fa-apple"),
            new MenuItem("Android", "fa-android"),
            new MenuItem("Windows", "fa-windows"),
            new MenuItem("Chip", "fa-microchip"),
            new MenuItem("Pizza", "fa-code"),
            new MenuItem("Terminal", "fa-terminal"),
            new MenuItem("Fire", "fa-fire-extinguisher"),
            new MenuItem("Bug", "fa-bug"),
            new MenuItem("Coffee", "fa-coffee"),
            new MenuItem("Developer", "fa-user-secret"),
        };
        }

        public IReadOnlyList<MenuItem> ItemSource { get; }
    }
}