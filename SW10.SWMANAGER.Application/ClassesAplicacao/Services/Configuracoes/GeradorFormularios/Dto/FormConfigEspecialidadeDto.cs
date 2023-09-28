using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto
{
    [AutoMap(typeof(FormConfigEspecialidade))]
    public class FormConfigEspecialidadeDto : CamposPadraoCRUDDto
    {
        public long? FormConfigId { get; set; }

        public long? EspecialidadeId { get; set; }

        public FormConfigDto FormConfig { get; set; }

        public EspecialidadeDto Especialidade { get; set; }

        public string EspecialidadesIncluidas { get; set; }

        public string EspecialidadesRemovidas { get; set; }

        public ICollection<EspecialidadeDto> ListaDisponiveis { get; set; }

        public ICollection<EspecialidadeDto> ListaAssociadas { get; set; }
    }
}
