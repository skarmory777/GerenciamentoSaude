using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.MovimentosAutomaticos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Terceirizados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;

using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.MovimentosAutomaticos.Dto
{
    public class MovimentoAutomaticoDto : CamposPadraoCRUDDto
    {
        public long EmpresaId { get; set; }
        public long? UnidadeOrganizacionalId { get; set; }
        public long? CentroCustoId { get; set; }
        public long? TerceirizadoId { get; set; }
        public long? TurnoId { get; set; }
        public long? TipoAcomodacaoId { get; set; }
        public float Quantidade { get; set; }
        public bool IsAmbulatorio { get; set; }
        public bool IsInternacao { get; set; }
        public bool IsNovoAtendimento { get; set; }
        public bool IsDiaria { get; set; }
        public bool IsCobraPernoite { get; set; }
        public bool IsCobraRefeicao { get; set; }
        public bool IsCobraFralda { get; set; }

        public EmpresaDto Empresa { get; set; }
        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }
        public CentroCustoDto CentroCusto { get; set; }
        public TerceirizadoDto Terceirizado { get; set; }
        public TurnoDto Turno { get; set; }
        public TipoAcomodacaoDto TipoAcomodacao { get; set; }
        public FaturamentoItemDto FaturamentoItem { get; set; }
        public long FaturamentoItemId { get; set; }

        public IList<MovimentoAutomaticoConvenioPlanoDto> MovimentosAutomaticosConveiosPlanos { get; set; }
        public IList<MovimentoAutomaticoEspecialidadeDto> MovimentosAutomaticosEspecialidades { get; set; }
        public IList<MovimentoAutomaticoFaturamentoItemDto> MovimentosAutomaticosFaturamentosItens { get; set; }
        public IList<MovimentoAutomaticoTipoGuiaDto> MovimentosAutomaticosTiposGuias { get; set; }

        public string TiposLeitos { get; set; }
        public string TiposGuias { get; set; }
        public string Especialidades { get; set; }
        public string ConveniosPlanos { get; set; }


        public static MovimentoAutomaticoDto Mapear(MovimentoAutomatico movimentoAutomatico)
        {
            MovimentoAutomaticoDto movimentoAutomaticoDto = new Dto.MovimentoAutomaticoDto();

            movimentoAutomaticoDto.Id = movimentoAutomatico.Id;
            movimentoAutomaticoDto.Codigo = movimentoAutomatico.Codigo;
            movimentoAutomaticoDto.Descricao = movimentoAutomatico.Descricao;
            movimentoAutomaticoDto.EmpresaId = movimentoAutomatico.EmpresaId;
            movimentoAutomaticoDto.UnidadeOrganizacionalId = movimentoAutomatico.UnidadeOrganizacionalId;
            movimentoAutomaticoDto.CentroCustoId = movimentoAutomatico.CentroCustoId;
            movimentoAutomaticoDto.TerceirizadoId = movimentoAutomatico.TerceirizadoId;
            movimentoAutomaticoDto.TurnoId = movimentoAutomatico.TurnoId;
            movimentoAutomaticoDto.TipoAcomodacaoId = movimentoAutomatico.TipoAcomodacaoId;
            movimentoAutomaticoDto.Quantidade = movimentoAutomatico.Quantidade;
            movimentoAutomaticoDto.IsAmbulatorio = movimentoAutomatico.IsAmbulatorio;
            movimentoAutomaticoDto.IsInternacao = movimentoAutomatico.IsInternacao;
            movimentoAutomaticoDto.IsNovoAtendimento = movimentoAutomatico.IsNovoAtendimento;
            movimentoAutomaticoDto.IsDiaria = movimentoAutomatico.IsDiaria;
            movimentoAutomaticoDto.IsCobraPernoite = movimentoAutomatico.IsCobraPernoite;
            movimentoAutomaticoDto.IsCobraRefeicao = movimentoAutomatico.IsCobraRefeicao;
            movimentoAutomaticoDto.IsCobraFralda = movimentoAutomatico.IsCobraFralda;

            if (movimentoAutomatico.Empresa != null)
            {
                movimentoAutomaticoDto.Empresa = EmpresaDto.Mapear(movimentoAutomatico.Empresa);
            }

            if (movimentoAutomatico.UnidadeOrganizacional != null)
            {
                movimentoAutomaticoDto.UnidadeOrganizacional = UnidadeOrganizacionalDto.Mapear(movimentoAutomatico.UnidadeOrganizacional);
            }

            if (movimentoAutomatico.CentroCusto != null)
            {
                movimentoAutomaticoDto.CentroCusto = CentroCustoDto.Mapear(movimentoAutomatico.CentroCusto);
            }

            if (movimentoAutomatico.Turno != null)
            {
                movimentoAutomaticoDto.Turno = TurnoDto.Mapear(movimentoAutomatico.Turno);
            }

            if (movimentoAutomatico.TipoAcomodacao != null)
            {
                movimentoAutomaticoDto.TipoAcomodacao = TipoAcomodacaoDto.Mapear(movimentoAutomatico.TipoAcomodacao);
            }

            if (movimentoAutomatico.Terceirizado != null)
            {
                movimentoAutomaticoDto.Terceirizado = TerceirizadoDto.Mapear(movimentoAutomatico.Terceirizado);
            }

            movimentoAutomaticoDto.MovimentosAutomaticosConveiosPlanos = MovimentoAutomaticoConvenioPlanoDto.Mapear(movimentoAutomatico.MovimentosAutomaticosConveiosPlanos.ToList());
            movimentoAutomaticoDto.MovimentosAutomaticosEspecialidades = MovimentoAutomaticoEspecialidadeDto.Mapear(movimentoAutomatico.MovimentosAutomaticosEspecialidades.ToList());
            movimentoAutomaticoDto.MovimentosAutomaticosFaturamentosItens = MovimentoAutomaticoFaturamentoItemDto.Mapear(movimentoAutomatico.MovimentosAutomaticosFaturamentosItens.ToList());
            movimentoAutomaticoDto.MovimentosAutomaticosTiposGuias = MovimentoAutomaticoTipoGuiaDto.Mapear(movimentoAutomatico.MovimentosAutomaticosTiposGuias.ToList());

            return movimentoAutomaticoDto;
        }
    }
}
