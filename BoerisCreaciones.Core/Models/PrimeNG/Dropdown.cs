namespace BoerisCreaciones.Core.Models.PrimeNG.Dropdown
{
    public class SelectItem<T>
    {
        public SelectItem(string label, T value, string? styleClass, string? icon, string? title, bool? disabled)
        {
            this.label = label;
            this.value = value;
            this.styleClass = styleClass;
            this.icon = icon;
            this.title = title;
            this.disabled = disabled;
        }

        public SelectItem(string label, T value)
        {
            this.label = label;
            this.value = value;
        }

        public string label { get; set; }
        public T value { get; set; }
        public string? styleClass { get; set; }
        public string? icon { get; set; }
        public string? title { get; set; }
        public bool? disabled { get; set; }
    }

    public class SelectItemGroup<Father, Children>
    {
        public SelectItemGroup(string label, Father? value, List<SelectItem<Children>> items)
        {
            this.label = label;
            this.value = value;
            this.items = items;
        }

        public SelectItemGroup(string label, Father? value)
        {
            this.label = label;
            this.value = value;
            this.items = new List<SelectItem<Children>>();
        }

        public string label { get; set; }
        public Father? value { get; set; }
        public List<SelectItem<Children>> items { get; set; }
    }
}
