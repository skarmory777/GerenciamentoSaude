using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto
{
    [AutoMap(typeof(FormConfigUnidadeOrganizacional))]
    public class FormConfigUnidadeOrganizacionalDto : CamposPadraoCRUDDto
    {
        public long? FormConfigId { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }

        public FormConfigDto FormConfig { get; set; }

        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }

        public string UnidadesIncluidas { get; set; }

        public string UnidadesRemovidas { get; set; }

        public ICollection<UnidadeOrganizacionalDto> ListaDisponiveis { get; set; }

        public ICollection<UnidadeOrganizacionalDto> ListaAssociadas { get; set; }
    }
}
