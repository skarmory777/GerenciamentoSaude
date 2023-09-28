namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown
{
    public class DropdownItems : DropdownItems<long>, IDropdownItems<long>
    {
    }

    public class DropdownItems<TIdType> : IDropdownItems<TIdType>
    {
        public TIdType id { get; set; }
        public string text { get; set; }
    }
    public interface IDropdownItems<TIdType>
    {
        TIdType id { get; set; }
        string text { get; set; }
    }
}
