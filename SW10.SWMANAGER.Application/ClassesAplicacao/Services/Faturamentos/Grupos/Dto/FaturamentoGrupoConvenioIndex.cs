using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto
{
    public class FaturamentoGrupoConvenioIndex : CamposPadraoCRUDDto
    {
        public long? ConvenioId { get; set; }
        public ConvenioDto Convenio { get; set; }
        public long? GrupoId { get; set; }
        public FaturamentoGrupoDto Grupo { get; set; }
        public string GrupoDescricao { get; set; }
        public bool IsOutraDespesa { get; set; }
        public override string Codigo { get; set; }
        public string IsCobrancaDia { get; set; }
        public long IdGrid { get; set; }
    }
}
