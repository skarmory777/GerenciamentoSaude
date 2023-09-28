using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace SW10.SWMANAGER.ClassesAplicacao.Sistemas
{
    public class AgGridUserPreference : FullAuditedEntity<long>
    {
        public string GridIdentifier { get; set; }
        public string ColumnState { get; set; }
        
        public string ColumnGroupState { get; set; }

        public string SortModel { get; set; }

        public string FilterModel { get; set; }
    }
}