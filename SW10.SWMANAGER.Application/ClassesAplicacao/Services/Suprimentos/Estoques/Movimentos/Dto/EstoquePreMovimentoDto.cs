using Abp.AutoMapper;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cfops.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Globais.HorasDias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    [AutoMap(typeof(EstoquePreMovimento))]
    public class EstoquePreMovimentoDto : CamposPadraoCRUDDto
    {
        public string Documento { get; set; }

        [JsonConverter(typeof(FixedIsoDateTimeOffsetConverter))]
        public DateTimeOffset Movimento { get; set; }
        public long? EstoqueId { get; set; }
        public long TipoMovimentoId { get; set; }
        public long? FornecedorId { get; set; }
        public long? EmpresaId { get; set; }
        public long? EmprestimoEmpresaId { get; set; }

        public decimal Quantidade { get; set; }
        public long PreMovimentoEstadoId { get; set; }
        public decimal TotalProduto { get; set; }
        public decimal Frete { get; set; }
        public decimal AcrescimoDecrescimo { get; set; }
        public decimal TotalDocumento { get; set; }
        public long? CentroCustoId { get; set; } = 0;
        public bool IsEntrada { get; set; }
        public long? MotivoPerdaProdutoId { get; set; }

        public MotivoPerdaProdutoDto MotivoPerdaProduto { get; set; }

        public bool PossuiLoteValidade { get; set; }

        public EstoqueDto Estoque { get; set; }
        public EstoqueTipoMovimentoDto TipoMovimento { get; set; }
        public SisFornecedorDto Fornecedor { get; set; }
        public SisFornecedorDto Frete_Fornecedor { get; set; }
        public EmpresaDto Empresa { get; set; }
        public SisPessoaDto EmprestimoEmpresa { get; set; }
        public EstoquePreMovimentoEstadoDto PreMovimentoEstado { get; set; }
        public CentroCustoDto CentroCusto { get; set; }
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
        public decimal? ValorIPI { get; set; }
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

        [JsonConverter(typeof(FixedIsoDateTimeOffsetConverter))]
        public DateTimeOffset Emissao { get; set; }

        public bool PermiteConfirmacaoEntrada { get; set; }

        public string NomUsuario { get; set; }
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

        public long? GrupoOperacaoId { get; set; }

        public string Itens { get; set; }

        public long ProdutoUnidadeId { get; set; }

        public long? TipoAtendimentoId { get; set; }
        public string TipoAtendimento { get; set; }

        public List<EstoqueImportacaoProdutoDto> ImportacaoProdutos { get; set; }
        public string CNPJNota { get; set; }
        public string NFeItens { get; set; }

        public string LancamentosJson { get; set; }
        public string RateioJson { get; set; }

        public DateTime? HoraPrescrita { get; set; }

        public long? HoraDiaId { get; set; }
        public HoraDiaDto HoraDiaDto { get; set; }

        public long? PrescricaoMedicaId { get; set; }
        public PrescricaoMedicaDto PrescricaoMedicaDto { get; set; }

        public long? PrescricaoItemRespostaId { get; set; }

        public PrescricaoItemRespostaDto PrescricaoItemResposta { get; set; }

        public string Chave { get; set; }

        public long? InventarioId { get; set; }
        
        
        public static EstoquePreMovimentoDto MapPreMovimento(EstoquePreMovimento preMovimento)
        {
            if (preMovimento == null) return null;

            var preMovimentoDto = new EstoquePreMovimentoDto
            {
                AcrescimoDecrescimo = preMovimento.AcrescimoDecrescimo,
                AplicacaoDireta = preMovimento.AplicacaoDireta,
                AtendimentoId = preMovimento.AtendimentoId,
                CentroCustoId = preMovimento.CentroCustoId,
                Consiginado = preMovimento.Consiginado,
                Contabiliza = preMovimento.Contabiliza,
                CreationTime = preMovimento.CreationTime,
                CreatorUserId = preMovimento.CreatorUserId,
                DeleterUserId = preMovimento.DeleterUserId,
                DeletionTime = preMovimento.DeletionTime,
                DescontoPer = preMovimento.DescontoPer,
                Documento = preMovimento.Documento,
                Emissao = preMovimento.Emissao,
                EmpresaId = preMovimento.EmpresaId,
                EmprestimoEmpresaId = preMovimento.EmprestimoEmpresaId,
                EntragaProduto = preMovimento.EntragaProduto,
                EstoqueId = preMovimento.EstoqueId,
                FornecedorId = preMovimento.SisFornecedorId,
                InventarioId = preMovimento.InventarioId,
                Paciente = preMovimento.Paciente.MapTo<PacienteDto>(),
                MotivoPerdaProdutoId = preMovimento.MotivoPerdaProdutoId,
                EstoqueEmprestimoId = preMovimento.EstoqueEmprestimoId
            };


            if (preMovimento.EstTipoMovimento != null)
            {
                preMovimentoDto.EstTipoMovimentoId = preMovimento.EstTipoMovimento.Id;
            }

            if (preMovimento.SisFornecedor != null)
            {
                preMovimentoDto.Fornecedor = new SisFornecedorDto
                {
                    Id = preMovimento.SisFornecedor.Id,
                    Codigo = preMovimento.SisFornecedor.SisPessoa.Codigo,
                    Descricao = preMovimento.SisFornecedor.SisPessoa.FisicaJuridica == "F"
                        ? preMovimento.SisFornecedor.SisPessoa.NomeCompleto
                        : preMovimento.SisFornecedor.SisPessoa.NomeFantasia,
                    SisPessoaId = preMovimento.SisFornecedor.SisPessoaId
                };
            }

            preMovimentoDto.Frete = preMovimento.Frete;
            preMovimentoDto.FretePer = preMovimento.FretePer;
            preMovimentoDto.Frete_FornecedorId = preMovimento.Frete_SisFornecedorId;

            if (preMovimento.Frete_SisForncedor != null)
            {
                preMovimentoDto.Frete_Fornecedor = new SisFornecedorDto
                {
                    Id = preMovimento.Frete_SisForncedor.Id,
                    Codigo = preMovimento.Frete_SisForncedor.SisPessoa.Codigo,
                    Descricao = preMovimento.Frete_SisForncedor.SisPessoa.FisicaJuridica == "F"
                        ? preMovimento.Frete_SisForncedor.SisPessoa.NomeCompleto
                        : preMovimento.Frete_SisForncedor.SisPessoa.NomeFantasia
                };
            }

            preMovimentoDto.ICMSPer = preMovimento.ICMSPer;
            preMovimentoDto.Id = preMovimento.Id;
            preMovimentoDto.InclusoNota = preMovimento.InclusoNota;
            preMovimentoDto.IsEntrada = preMovimento.IsEntrada;
            preMovimentoDto.MedicoSolcitanteId = preMovimento.MedicoSolcitanteId;
            preMovimentoDto.Movimento = preMovimento.Movimento;
            preMovimentoDto.Observacao = preMovimento.Observacao;
            preMovimentoDto.OrdemId = preMovimento.OrdemId;
            preMovimentoDto.PacienteId = preMovimento.PacienteId;
            preMovimentoDto.PreMovimentoEstadoId = preMovimento.PreMovimentoEstadoId;
            preMovimentoDto.Quantidade = preMovimento.Quantidade;
            preMovimentoDto.Serie = preMovimento.Serie;
            preMovimentoDto.EstTipoOperacaoId = preMovimento.EstTipoOperacaoId;
            preMovimentoDto.TipoFreteId = preMovimento.TipoFreteId;
            preMovimentoDto.EstTipoMovimentoId = preMovimento.EstTipoMovimentoId;
            preMovimentoDto.TotalDocumento = preMovimento.TotalDocumento;
            preMovimentoDto.TotalProduto = preMovimento.TotalProduto;
            preMovimentoDto.UnidadeOrganizacionalId = preMovimento.UnidadeOrganizacionalId;
            preMovimentoDto.ValorFrete = preMovimento.ValorFrete;
            preMovimentoDto.ValorICMS = preMovimento.ValorICMS;
            preMovimentoDto.MotivoPerdaProdutoId = preMovimento.MotivoPerdaProdutoId;
            preMovimentoDto.CFOPId = preMovimento.CFOPId;
            preMovimentoDto.GrupoOperacaoId = preMovimento.GrupoOperacaoId;
            preMovimentoDto.DescontoPer = preMovimento.DescontoPer;
            preMovimentoDto.HoraDiaId = preMovimento.HoraDiaId;
            preMovimentoDto.HoraPrescrita = preMovimento.HoraPrescrita;
            preMovimentoDto.PrescricaoMedicaId = preMovimento.PrescricaoMedicaId;
            preMovimentoDto.Chave = preMovimento.Chave;

            if (preMovimento.EstTipoMovimento != null)
            {
                preMovimentoDto.TipoMovimento = new EstoqueTipoMovimentoDto
                { Id = preMovimento.EstTipoMovimento.Id, Descricao = preMovimento.EstTipoMovimento.Descricao };
            }

            if (preMovimento.CentroCusto != null)
            {
                preMovimentoDto.CentroCusto = new CentroCustoDto
                { Id = preMovimento.CentroCusto.Id, Descricao = preMovimento.CentroCusto.Descricao };
            }


            if (preMovimento.Empresa != null)
            {
                preMovimentoDto.Empresa = preMovimento.Empresa.MapTo<EmpresaDto>();
            }

            if (preMovimento.EmprestimoEmpresa != null)
            {
                preMovimentoDto.EmprestimoEmpresa = preMovimento.EmprestimoEmpresa.MapTo<SisPessoaDto>();
            }

            if (preMovimento.Atendimento != null)
            {
                preMovimentoDto.Atendimento = AtendimentoDto.Mapear(preMovimento.Atendimento);
            }

            if (preMovimento.MedicoSolicitante != null)
            {
                preMovimentoDto.MedicoSolicitante = MedicoDto.Mapear(preMovimento.MedicoSolicitante);
            }

            if (preMovimento.Estoque != null)
            {
                preMovimentoDto.Estoque = preMovimento.Estoque.MapTo<EstoqueDto>();
            }

            if (preMovimento.TipoFrete != null)
            {
                preMovimentoDto.TipoFrete = TipoFreteDto.Maprear(preMovimento.TipoFrete);
            }

            if (preMovimento.CFOP != null)
            {
                preMovimentoDto.CFOP = CfopDto.Mapear(preMovimento.CFOP);
            }
            
            preMovimentoDto.UnidadeOrganizacional = UnidadeOrganizacionalDto.Mapear(preMovimento.UnidadeOrganizacional);
            preMovimentoDto.MotivoPerdaProduto = MotivoPerdaProdutoDto.Mapear(preMovimento.MotivoPerdaProduto);
            preMovimentoDto.Atendimento = AtendimentoDto.Mapear(preMovimento.Atendimento);            
            preMovimentoDto.EstoqueEmprestimo = EstoqueEmprestimoDto.Mapear(preMovimento.EstoqueEmprestimo);

            return preMovimentoDto;
        }

        public static EstoquePreMovimento MapPreMovimento(EstoquePreMovimentoDto preMovimentoDto)
        {
            var preMovimento = new EstoquePreMovimento
            {
                AcrescimoDecrescimo = preMovimentoDto.AcrescimoDecrescimo,
                AplicacaoDireta = preMovimentoDto.AplicacaoDireta,
                AtendimentoId = preMovimentoDto.AtendimentoId,
                CentroCustoId = preMovimentoDto.CentroCustoId,
                Consiginado = preMovimentoDto.Consiginado,
                Contabiliza = preMovimentoDto.Contabiliza,
                CreationTime = preMovimentoDto.CreationTime,
                CreatorUserId = preMovimentoDto.CreatorUserId,
                DeleterUserId = preMovimentoDto.DeleterUserId,
                DeletionTime = preMovimentoDto.DeletionTime,
                DescontoPer = preMovimentoDto.DescontoPer,
                Documento = preMovimentoDto.Documento,
                Emissao = preMovimentoDto.Emissao,
                EmpresaId = preMovimentoDto.EmpresaId,
                EntragaProduto = preMovimentoDto.EntragaProduto,
                EstoqueId = preMovimentoDto.EstoqueId,
                SisFornecedorId = preMovimentoDto.FornecedorId,
                Frete = preMovimentoDto.Frete,
                FretePer = preMovimentoDto.FretePer,
                Frete_SisFornecedorId = preMovimentoDto.Frete_FornecedorId,
                ICMSPer = preMovimentoDto.ICMSPer,
                Id = preMovimentoDto.Id,
                InclusoNota = preMovimentoDto.InclusoNota,
                IsEntrada = preMovimentoDto.IsEntrada,
                MedicoSolcitanteId = preMovimentoDto.MedicoSolcitanteId,
                Movimento = preMovimentoDto.Movimento,
                Observacao = preMovimentoDto.Observacao,
                OrdemId = preMovimentoDto.OrdemId,
                PacienteId = preMovimentoDto.PacienteId,
                PreMovimentoEstadoId = preMovimentoDto.PreMovimentoEstadoId,
                Quantidade = preMovimentoDto.Quantidade,
                Serie = preMovimentoDto.Serie,
                EstTipoOperacaoId = preMovimentoDto.EstTipoOperacaoId,
                TipoFreteId = preMovimentoDto.TipoFreteId,
                EstTipoMovimentoId = preMovimentoDto.EstTipoMovimentoId,
                TotalDocumento = preMovimentoDto.TotalDocumento,
                TotalProduto = preMovimentoDto.TotalProduto,
                UnidadeOrganizacionalId = preMovimentoDto.UnidadeOrganizacionalId,
                ValorFrete = preMovimentoDto.ValorFrete,
                ValorICMS = preMovimentoDto.ValorICMS,
                MotivoPerdaProdutoId = preMovimentoDto.MotivoPerdaProdutoId,
                CFOPId = preMovimentoDto.CFOPId,
                GrupoOperacaoId = preMovimentoDto.GrupoOperacaoId,
                InventarioId = preMovimentoDto.InventarioId
            };

            preMovimento.DescontoPer = preMovimentoDto.DescontoPer;

            preMovimento.HoraDiaId = preMovimentoDto.HoraDiaId;
            preMovimento.HoraPrescrita = preMovimentoDto.HoraPrescrita;
            preMovimento.PrescricaoMedicaId = preMovimentoDto.PrescricaoMedicaId;
            preMovimento.Chave = preMovimentoDto.Chave;

            return preMovimento;
        }
        
        public static List<EstoquePreMovimentoDto> MapPreMovimento(List<EstoquePreMovimento> preMovimentos)
        {
            var preMovimentosDto = new List<EstoquePreMovimentoDto>();
            foreach (var item in preMovimentos)
            {
                preMovimentosDto.Add(MapPreMovimento(item));
            }

            return preMovimentosDto;
        }

        public static List<EstoquePreMovimento> MapPreMovimento(List<EstoquePreMovimentoDto> preMovimentosDto)
        {
            var preMovimentos = new List<EstoquePreMovimento>();
            foreach (var item in preMovimentosDto)
            {
                preMovimentos.Add(MapPreMovimento(item));
            }

            return preMovimentos;
        }

        public long? EstoqueEmprestimoId { get; set; }
        public EstoqueEmprestimoDto EstoqueEmprestimo { get; set; }

        public static EstoquePreMovimento Mapear(EstoquePreMovimentoDto preMovimentoDto)
        {
            if (preMovimentoDto == null) return null;

            var preMovimento = new EstoquePreMovimento
            {
                AcrescimoDecrescimo = preMovimentoDto.AcrescimoDecrescimo,
                AplicacaoDireta = preMovimentoDto.AplicacaoDireta,
                AtendimentoId = preMovimentoDto.AtendimentoId,
                CentroCustoId = preMovimentoDto.CentroCustoId,
                Consiginado = preMovimentoDto.Consiginado,
                Contabiliza = preMovimentoDto.Contabiliza,
                CreationTime = preMovimentoDto.CreationTime,
                CreatorUserId = preMovimentoDto.CreatorUserId,
                DeleterUserId = preMovimentoDto.DeleterUserId,
                DeletionTime = preMovimentoDto.DeletionTime,
                DescontoPer = preMovimentoDto.DescontoPer,
                Documento = preMovimentoDto.Documento,
                Emissao = preMovimentoDto.Emissao,
                EmpresaId = preMovimentoDto.EmpresaId,
                EntragaProduto = preMovimentoDto.EntragaProduto,
                EstoqueId = preMovimentoDto.EstoqueId,
                SisFornecedorId = preMovimentoDto.FornecedorId,
                Frete = preMovimentoDto.Frete,
                FretePer = preMovimentoDto.FretePer,
                Frete_SisFornecedorId = preMovimentoDto.Frete_FornecedorId,
                ICMSPer = preMovimentoDto.ICMSPer,
                Id = preMovimentoDto.Id,
                InclusoNota = preMovimentoDto.InclusoNota,
                IsEntrada = preMovimentoDto.IsEntrada,
                MedicoSolcitanteId = preMovimentoDto.MedicoSolcitanteId,
                Movimento = preMovimentoDto.Movimento,
                Observacao = preMovimentoDto.Observacao,
                OrdemId = preMovimentoDto.OrdemId,
                PacienteId = preMovimentoDto.PacienteId,
                PreMovimentoEstadoId = preMovimentoDto.PreMovimentoEstadoId,
                Quantidade = preMovimentoDto.Quantidade,
                Serie = preMovimentoDto.Serie,
                EstTipoOperacaoId = preMovimentoDto.EstTipoOperacaoId,
                TipoFreteId = preMovimentoDto.TipoFreteId,
                EstTipoMovimentoId = preMovimentoDto.EstTipoMovimentoId,
                TotalDocumento = preMovimentoDto.TotalDocumento,
                TotalProduto = preMovimentoDto.TotalProduto,
                UnidadeOrganizacionalId = preMovimentoDto.UnidadeOrganizacionalId,
                ValorFrete = preMovimentoDto.ValorFrete,
                ValorICMS = preMovimentoDto.ValorICMS,
                MotivoPerdaProdutoId = preMovimentoDto.MotivoPerdaProdutoId,
                CFOPId = preMovimentoDto.CFOPId,
                GrupoOperacaoId = preMovimentoDto.GrupoOperacaoId,
                InventarioId = preMovimentoDto.InventarioId,
                EstoqueEmprestimoId = preMovimentoDto.EstoqueEmprestimoId
            };

            preMovimento.DescontoPer = preMovimentoDto.DescontoPer;
            preMovimento.HoraDiaId = preMovimentoDto.HoraDiaId;
            preMovimento.HoraPrescrita = preMovimentoDto.HoraPrescrita;
            preMovimento.PrescricaoMedicaId = preMovimentoDto.PrescricaoMedicaId;
            preMovimento.Chave = preMovimentoDto.Chave;

            return preMovimento;
        }

        public static EstoquePreMovimentoDto Mapear(EstoquePreMovimento preMovimento)
        {
            if (preMovimento == null) return null;

            var preMovimentoDto = new EstoquePreMovimentoDto
            {
                AcrescimoDecrescimo = preMovimento.AcrescimoDecrescimo,
                AplicacaoDireta = preMovimento.AplicacaoDireta,
                AtendimentoId = preMovimento.AtendimentoId,
                CentroCustoId = preMovimento.CentroCustoId,
                Consiginado = preMovimento.Consiginado,
                Contabiliza = preMovimento.Contabiliza,
                CreationTime = preMovimento.CreationTime,
                CreatorUserId = preMovimento.CreatorUserId,
                DeleterUserId = preMovimento.DeleterUserId,
                DeletionTime = preMovimento.DeletionTime,
                DescontoPer = preMovimento.DescontoPer,
                Documento = preMovimento.Documento,
                Emissao = preMovimento.Emissao,
                EmpresaId = preMovimento.EmpresaId,
                EmprestimoEmpresaId = preMovimento.EmprestimoEmpresaId,
                EntragaProduto = preMovimento.EntragaProduto,
                EstoqueId = preMovimento.EstoqueId,
                FornecedorId = preMovimento.SisFornecedorId,
                InventarioId = preMovimento.InventarioId,
                Paciente = preMovimento.Paciente.MapTo<PacienteDto>(),
                MotivoPerdaProdutoId = preMovimento.MotivoPerdaProdutoId,
                EstoqueEmprestimoId = preMovimento.EstoqueEmprestimoId
            };


            if (preMovimento.EstTipoMovimento != null)
            {
                preMovimentoDto.EstTipoMovimentoId = preMovimento.EstTipoMovimento.Id;
            }

            if (preMovimento.SisFornecedor != null)
            {
                preMovimentoDto.Fornecedor = new SisFornecedorDto
                {
                    Id = preMovimento.SisFornecedor.Id,
                    Codigo = preMovimento.SisFornecedor.SisPessoa.Codigo,
                    Descricao = preMovimento.SisFornecedor.SisPessoa.FisicaJuridica == "F"
                        ? preMovimento.SisFornecedor.SisPessoa.NomeCompleto
                        : preMovimento.SisFornecedor.SisPessoa.NomeFantasia,
                    SisPessoaId = preMovimento.SisFornecedor.SisPessoaId
                };
            }

            preMovimentoDto.Frete = preMovimento.Frete;
            preMovimentoDto.FretePer = preMovimento.FretePer;
            preMovimentoDto.Frete_FornecedorId = preMovimento.Frete_SisFornecedorId;

            if (preMovimento.Frete_SisForncedor != null)
            {
                preMovimentoDto.Frete_Fornecedor = new SisFornecedorDto
                {
                    Id = preMovimento.Frete_SisForncedor.Id,
                    Codigo = preMovimento.Frete_SisForncedor.SisPessoa.Codigo,
                    Descricao = preMovimento.Frete_SisForncedor.SisPessoa.FisicaJuridica == "F"
                        ? preMovimento.Frete_SisForncedor.SisPessoa.NomeCompleto
                        : preMovimento.Frete_SisForncedor.SisPessoa.NomeFantasia
                };
            }

            preMovimentoDto.ICMSPer = preMovimento.ICMSPer;
            preMovimentoDto.Id = preMovimento.Id;
            preMovimentoDto.InclusoNota = preMovimento.InclusoNota;
            preMovimentoDto.IsEntrada = preMovimento.IsEntrada;
            preMovimentoDto.MedicoSolcitanteId = preMovimento.MedicoSolcitanteId;
            preMovimentoDto.Movimento = preMovimento.Movimento;
            preMovimentoDto.Observacao = preMovimento.Observacao;
            preMovimentoDto.OrdemId = preMovimento.OrdemId;
            preMovimentoDto.PacienteId = preMovimento.PacienteId;
            preMovimentoDto.PreMovimentoEstadoId = preMovimento.PreMovimentoEstadoId;
            preMovimentoDto.Quantidade = preMovimento.Quantidade;
            preMovimentoDto.Serie = preMovimento.Serie;
            preMovimentoDto.EstTipoOperacaoId = preMovimento.EstTipoOperacaoId;
            preMovimentoDto.TipoFreteId = preMovimento.TipoFreteId;
            preMovimentoDto.EstTipoMovimentoId = preMovimento.EstTipoMovimentoId;
            preMovimentoDto.TotalDocumento = preMovimento.TotalDocumento;
            preMovimentoDto.TotalProduto = preMovimento.TotalProduto;
            preMovimentoDto.UnidadeOrganizacionalId = preMovimento.UnidadeOrganizacionalId;
            preMovimentoDto.ValorFrete = preMovimento.ValorFrete;
            preMovimentoDto.ValorICMS = preMovimento.ValorICMS;
            preMovimentoDto.MotivoPerdaProdutoId = preMovimento.MotivoPerdaProdutoId;
            preMovimentoDto.CFOPId = preMovimento.CFOPId;
            preMovimentoDto.GrupoOperacaoId = preMovimento.GrupoOperacaoId;
            preMovimentoDto.DescontoPer = preMovimento.DescontoPer;
            preMovimentoDto.HoraDiaId = preMovimento.HoraDiaId;
            preMovimentoDto.HoraPrescrita = preMovimento.HoraPrescrita;
            preMovimentoDto.PrescricaoMedicaId = preMovimento.PrescricaoMedicaId;
            preMovimentoDto.Chave = preMovimento.Chave;

            if (preMovimento.EstTipoMovimento != null)
            {
                preMovimentoDto.TipoMovimento = new EstoqueTipoMovimentoDto
                { Id = preMovimento.EstTipoMovimento.Id, Descricao = preMovimento.EstTipoMovimento.Descricao };
            }

            if (preMovimento.CentroCusto != null)
            {
                preMovimentoDto.CentroCusto = new CentroCustoDto
                { Id = preMovimento.CentroCusto.Id, Descricao = preMovimento.CentroCusto.Descricao };
            }

            if (preMovimento.Empresa != null)
            {
                preMovimentoDto.Empresa = preMovimento.Empresa.MapTo<EmpresaDto>();
            }

            if (preMovimento.EmprestimoEmpresa != null)
            {
                preMovimentoDto.EmprestimoEmpresa = preMovimento.EmprestimoEmpresa.MapTo<SisPessoaDto>();
            }

            preMovimentoDto.Atendimento = AtendimentoDto.Mapear(preMovimento.Atendimento);
            preMovimentoDto.MedicoSolicitante = MedicoDto.Mapear(preMovimento.MedicoSolicitante);

            if (preMovimento.Estoque != null)
            {
                preMovimentoDto.Estoque = preMovimento.Estoque.MapTo<EstoqueDto>();
            }

            if (preMovimento.TipoFrete != null)
            {
                preMovimentoDto.TipoFrete = TipoFreteDto.Maprear(preMovimento.TipoFrete);
            }

            if (preMovimento.CFOP != null)
            {
                preMovimentoDto.CFOP = CfopDto.Mapear(preMovimento.CFOP);
            }

            preMovimentoDto.UnidadeOrganizacional = UnidadeOrganizacionalDto.Mapear(preMovimento.UnidadeOrganizacional);
            preMovimentoDto.EstoqueEmprestimo = EstoqueEmprestimoDto.Mapear(preMovimento.EstoqueEmprestimo);
            preMovimentoDto.MotivoPerdaProduto = MotivoPerdaProdutoDto.Mapear(preMovimento.MotivoPerdaProduto);

            return preMovimentoDto;
        }

    }
}
