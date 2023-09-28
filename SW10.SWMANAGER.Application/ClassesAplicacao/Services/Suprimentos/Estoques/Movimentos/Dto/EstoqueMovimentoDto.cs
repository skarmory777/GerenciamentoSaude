using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cfops.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using System;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    public class EstoqueMovimentoDto : CamposPadraoCRUDDto
    {
        public string Documento { get; set; }
        public DateTimeOffset Movimento { get; set; }
        public long? EstoqueId { get; set; }
        public long TipoMovimentoId { get; set; }
        public long? FornecedorId { get; set; }
        public long? EmpresaId { get; set; }
        public decimal Quantidade { get; set; }
        public long PreMovimentoEstadoId { get; set; }
        public decimal TotalProduto { get; set; }
        public decimal Frete { get; set; }
        public decimal AcrescimoDecrescimo { get; set; }
        public decimal TotalDocumento { get; set; }
        public long? CentroCustoId { get; set; }
        public bool IsEntrada { get; set; }
        public long? MotivoPerdaProdutoId { get; set; }

        public bool PossuiLoteValidade { get; set; }

        public EstoqueDto Estoque { get; set; }
        public EstoqueTipoMovimentoDto TipoMovimento { get; set; }
        public FornecedorDto Fornecedor { get; set; }
        public FornecedorDto Frete_Fornecedor { get; set; }
        public EmpresaDto Empresa { get; set; }
        public EstoquePreMovimentoEstadoDto PreMovimentoEstado { get; set; }
        public CentroCustoDto CentroCusto { get; set; }

        //  public virtual ICollection<EstoquePreMovimentoItemDto> PreMovimentosItem { get; set; }



        public bool Contabiliza { get; set; }
        public bool Consiginado { get; set; }
        public bool AplicacaoDireta { get; set; }
        public bool EntragaProduto { get; set; }
        public string Serie { get; set; }
        public long? TipoDocumentoId { get; set; }
        public long? OrdemId { get; set; }

        public long? EstTipoMovimentoId { get; set; }
        public long? EstTipoOperacaoId { get; set; }


        // public TipoDocumentoDto TipoDocumento { get; set; }
        public OrdemCompraDto OrdemCompra { get; set; }


        public long? CFOPId { get; set; }
        public CfopDto CFOP { get; set; }
        public decimal? ICMSPer { get; set; }
        public decimal? ValorICMS { get; set; }
        public decimal? DescontoPer { get; set; }

        public decimal ValorDesconto { get; set; }
        public decimal ValorAcrescimo { get; set; }


        public long? TipoFreteId { get; set; }
        public TipoFreteDto TipoFrete { get; set; }

        public bool InclusoNota { get; set; }
        public decimal? FretePer { get; set; }
        public decimal? ValorFrete { get; set; }
        public long? Frete_FornecedorId { get; set; }
        public FornecedorDto Frete_Forncedor { get; set; }

        public DateTimeOffset Emissao { get; set; }

        public bool PermiteConfirmacaoEntrada { get; set; }

        public String NomUsuario { get; set; }


        public long? PacienteId { get; set; }

        public PacienteDto Paciente { get; set; }

        public long? MedicoSolcitanteId { get; set; }

        public MedicoDto MedicoSolicitante { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }
        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }


        public string Observacao { get; set; }

        public long? AtendimentoId { get; set; }

        public AtendimentoDto Atendimento { get; set; }

        public bool IsSaidaPaciente { get; set; }
        public bool IsSaidaSetor { get; set; }

        public int SaidaPorId { get; set; }
        public long EstoquePreMovimentoId { get; set; }
        
        public static EstoqueMovimentoDto MapMovimento(EstoqueMovimento movimento)
        {
            var movimentoDto = new EstoqueMovimentoDto
            {
                AcrescimoDecrescimo = movimento.AcrescimoDecrescimo,
                AplicacaoDireta = movimento.AplicacaoDireta,
                AtendimentoId = movimento.AtendimentoId,
                CentroCustoId = movimento.CentroCustoId,
                Consiginado = movimento.Consiginado,
                Contabiliza = movimento.Contabiliza,
                CreationTime = movimento.CreationTime,
                CreatorUserId = movimento.CreatorUserId,
                DeleterUserId = movimento.DeleterUserId,
                DeletionTime = movimento.DeletionTime,
                DescontoPer = movimento.DescontoPer,
                Documento = movimento.Documento,
                Emissao = movimento.Emissao,
                EmpresaId = movimento.EmpresaId,
                EntragaProduto = movimento.EntragaProduto,
                EstoqueId = movimento.EstoqueId,
                FornecedorId = movimento.SisFornecedorId,
                Frete = movimento.Frete,
                FretePer = movimento.FretePer,
                Frete_FornecedorId = movimento.Frete_SisFornecedorId,
                ICMSPer = movimento.ICMSPer,
                Id = movimento.Id,
                InclusoNota = movimento.InclusoNota,
                IsEntrada = movimento.IsEntrada,
                MedicoSolcitanteId = movimento.MedicoSolcitanteId,
                Movimento = movimento.Movimento,
                Observacao = movimento.Observacao,
                OrdemId = movimento.OrdemId,
                PacienteId = movimento.PacienteId,
                //  MovimentoDto.MovimentoEstadoId = Movimento.MovimentoEstadoId;
                Quantidade = movimento.Quantidade,
                Serie = movimento.Serie,
                EstTipoOperacaoId = movimento.EstTipoOperacaoId,
                TipoFreteId = movimento.TipoFreteId,
                EstTipoMovimentoId = movimento.EstTipoMovimentoId,
                TotalDocumento = movimento.TotalDocumento,
                TotalProduto = movimento.TotalProduto,
                UnidadeOrganizacionalId = movimento.UnidadeOrganizacionalId,
                ValorFrete = movimento.ValorFrete,
                ValorICMS = movimento.ValorICMS,
                //MovimentoDto.MotivoPerdaProdutoId = Movimento.MotivoPerdaProdutoId;
                CFOPId = movimento.CFOPId,
                EstoquePreMovimentoId = movimento.EstoquePreMovimentoId
            };
            return movimentoDto;
        }

        public static EstoqueMovimento MapMovimento(EstoqueMovimentoDto movimentoDto)
        {
            var movimento = new EstoqueMovimento
            {
                AcrescimoDecrescimo = movimentoDto.AcrescimoDecrescimo,
                AplicacaoDireta = movimentoDto.AplicacaoDireta,
                AtendimentoId = movimentoDto.AtendimentoId,
                CentroCustoId = movimentoDto.CentroCustoId,
                Consiginado = movimentoDto.Consiginado,
                Contabiliza = movimentoDto.Contabiliza,
                CreationTime = movimentoDto.CreationTime,
                CreatorUserId = movimentoDto.CreatorUserId,
                DeleterUserId = movimentoDto.DeleterUserId,
                DeletionTime = movimentoDto.DeletionTime,
                DescontoPer = movimentoDto.DescontoPer,
                Documento = movimentoDto.Documento,
                Emissao = movimentoDto.Emissao,
                EmpresaId = movimentoDto.EmpresaId,
                EntragaProduto = movimentoDto.EntragaProduto,
                EstoqueId = movimentoDto.EstoqueId,
                SisFornecedorId = movimentoDto.FornecedorId,
                Frete = movimentoDto.Frete,
                FretePer = movimentoDto.FretePer,
                Frete_SisFornecedorId = movimentoDto.Frete_FornecedorId,
                ICMSPer = movimentoDto.ICMSPer,
                Id = movimentoDto.Id,
                InclusoNota = movimentoDto.InclusoNota,
                IsEntrada = movimentoDto.IsEntrada,
                MedicoSolcitanteId = movimentoDto.MedicoSolcitanteId,
                Movimento = movimentoDto.Movimento,
                Observacao = movimentoDto.Observacao,
                OrdemId = movimentoDto.OrdemId,
                PacienteId = movimentoDto.PacienteId,
                //   Movimento.MovimentoEstadoId = MovimentoDto.MovimentoEstadoId;
                Quantidade = movimentoDto.Quantidade,
                Serie = movimentoDto.Serie,
                EstTipoOperacaoId = movimentoDto.EstTipoOperacaoId,
                TipoFreteId = movimentoDto.TipoFreteId,
                EstTipoMovimentoId = movimentoDto.EstTipoMovimentoId,
                TotalDocumento = movimentoDto.TotalDocumento,
                TotalProduto = movimentoDto.TotalProduto,
                UnidadeOrganizacionalId = movimentoDto.UnidadeOrganizacionalId,
                ValorFrete = movimentoDto.ValorFrete,
                ValorICMS = movimentoDto.ValorICMS,
                //  Movimento.MotivoPerdaProdutoId = MovimentoDto.MotivoPerdaProdutoId;
                CFOPId = movimentoDto.CFOPId,
                EstoquePreMovimentoId = movimentoDto.EstoquePreMovimentoId
            };

            return movimento;
        }
    }
}
