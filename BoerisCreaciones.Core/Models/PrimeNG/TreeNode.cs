namespace BoerisCreaciones.Core.Models.PrimeNG
{
    public class TreeNode<T>
    {
        public TreeNode(string key, string label, string icon, string type)
        {
            this.key = key;
            this.label = label;
            this.icon = icon;
            this.type = type;
            selectable = true;
            children = new List<TreeNode<T>>();
        }

        public TreeNode(string key, string label, string icon, string type, TreeNode<T> parent)
            : this(key, label, icon, type)
        {
            this.parent = parent;
        }

        public TreeNode(string key, string label, string icon, string type, T data, TreeNode<T> parent)
            : this(key, label, icon, type)
        {
            this.data = data;
            this.parent = parent;
            selectable = true;
        }

        public TreeNode(string key, string label, string icon, string type, bool selectable)
            : this(key, label, icon, type)
        {
            this.selectable = selectable;
            children = new List<TreeNode<T>>();
        }

        public string key {  get; set; }
        public string label { get; set; }
        public string icon { get; set; }
        public T data { get; set; }
        public string type { get; set; }
        public bool selectable { get; set; }
        public List<TreeNode<T>> children { get; set; }
        public TreeNode<T> parent { get; set; } = null;
    }
}
