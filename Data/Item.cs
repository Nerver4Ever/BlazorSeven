namespace BlazorSeven.Data
{
    public abstract class Item
    {
        public Guid ID { get; set; }

        public Guid ParentID { get; set; }

        public string Name { get; set; }

        public decimal Size { get; set; }

        public abstract ItemType ItemType { get; }


    }
}
