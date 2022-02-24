namespace MyApplication.Datas
{
    public interface IItem
    {
         string Name { get; set; }

         decimal Size { get; set; }

         string PathString { get; set; }

         Guid ID { get; set; }

        Guid ParentID { get; set; }

         void CalculateSize();
    }
}
