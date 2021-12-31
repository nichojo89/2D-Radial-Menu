namespace RadialMenuSkia.Pickers
{
    public class MenuItem
    {
        public string Name { get; }
        public string Icon { get; }

        public MenuItem(string name, string icon)
        {
            Name = name;
            Icon = icon;
        }
    }
}