namespace Mimo.Framework.WebControls
{
    public interface IMappableField
    {
        string EntityPropertyName { get; set; }
        bool EmptyIsNull { get; set; }
        void Map(object entity);
        void Unmap(object entity);
    }
}
