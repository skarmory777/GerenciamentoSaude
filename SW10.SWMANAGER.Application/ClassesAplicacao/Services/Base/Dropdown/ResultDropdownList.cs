using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown
{
    public class ResultDropdownList : ResultDropdownList<long>, IResultDropdownList<long>
    {
        public List<DropdownItems> Items { get; set; }
    }

    public class ResultDropdownList<TType> : IResultDropdownList<TType>
    {
        public List<DropdownItems<TType>> Items { get; set; }

        public int TotalCount { get; set; }
    }

    public interface IResultDropdownList<TType>
    {
        List<DropdownItems<TType>> Items { get; set; }

        int TotalCount { get; set; }
    }
}
