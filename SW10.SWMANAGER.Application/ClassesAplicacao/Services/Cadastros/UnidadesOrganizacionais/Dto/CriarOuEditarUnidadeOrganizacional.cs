using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto
{
    [AutoMap(typeof(UnidadeOrganizacional))]
    public class CriarOuEditarUnidadeOrganizacional : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public string Localizacao { get; set; }

        public bool IsAtivo { get; set; }

        public bool ControlaAlta { get; set; }

        public bool IsInternacao { get; set; }

        public bool IsAmbulatorioEmergencia { get; set; }

        public bool IsHospitalDia { get; set; }

        public bool IsSetorExames { get; set; }

        public bool IsLocalUtilizacao { get; set; }

        public bool IsEstoque { get; set; }

        public long? UnidadeInternacaoTipoId { get; set; }

        public long OrganizationUnitId { get; set; }
    }
}
