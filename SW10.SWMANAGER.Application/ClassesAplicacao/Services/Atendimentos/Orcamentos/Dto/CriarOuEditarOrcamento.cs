using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Orcamentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Orcamentos.Dto
{
    [AutoMap(typeof(Orcamento))]
    public class CriarOuEditarOrcamento : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Data { get; set; }

        public long? ConvenioId { get; set; }

        public long PlanoId { get; set; }

        public long PrestadorId { get; set; }

        public long EmpresaId { get; set; }

        public long CentroCustoId { get; set; }

        public long UnidadeOrganizacionalId { get; set; }

        public long PreAtendimentoId { get; set; }

        public long PacienteId { get; set; }

        [ForeignKey("ConvenioId")]
        public virtual ConvenioDto Convenio { get; set; }

        [ForeignKey("PlanoId")]
        public virtual PlanoDto Plano { get; set; }

        //[ForeignKey("PrestadorId")]
        //public virtual PrestadorDto Prestador { get; set; }

        [ForeignKey("EmpresaId")]
        public virtual EmpresaDto Empresa { get; set; }

        //[ForeignKey("CentroCustoId")]
        //public virtual CentroCustoDto CentroCusto { get; set; }

        [ForeignKey("UnidadeOrganizacionalId")]
        public virtual UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }

        [ForeignKey("PreAtendimentoId")]
        public virtual PreAtendimentoDto PreAtendimento { get; set; }

        [ForeignKey("PacienteId")]
        public virtual PacienteDto Paciente { get; set; }
    }
}
