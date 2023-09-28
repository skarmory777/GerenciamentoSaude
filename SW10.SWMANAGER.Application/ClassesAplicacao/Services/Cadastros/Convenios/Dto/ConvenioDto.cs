using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CEP;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposLogradouro;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.CodigosCredenciados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto
{
    [AutoMap(typeof(Convenio))]
    public class ConvenioDto : PessoaJuridicaDto
    {
        public long? SisPessoaId { get; set; }
        public SisPessoaDto SisPessoa { get; set; }
        public string Nome { get; set; }
        public bool IsAtivo { get; set; }
        public byte[] Logotipo { get; set; }
        public string LogotipoMimeType { get; set; }
        public bool IsFilotranpica { get; set; }
        public string LogradouroCobranca { get; set; }
        public long? CepCobrancaId { get; set; }
        public virtual Cep CepCobranca { get; set; }
        public long? TipoLogradouroCobrancaId { get; set; }
        public virtual TipoLogradouro TipoLogradouroCobranca { get; set; }
        public string NumeroCobranca { get; set; }
        public string ComplementoCobranca { get; set; }
        public string BairroCobranca { get; set; }
        public long? CidadeCobrancaId { get; set; }
        public virtual Cidade CidadeCobranca { get; set; }
        public long? EstadoCobrancaId { get; set; }
        public virtual Estado EstadoCobranca { get; set; }
        public string Cargo { get; set; }
        public DateTime DataInicioContrato { get; set; }
        public int Vigencia { get; set; }
        public DateTime DataProximaRenovacaoContratual { get; set; }
        public DateTime DataInicialContrato { get; set; }
        public DateTime DataUltimaRenovacaoContrato { get; set; }
        public string RegistroANS { get; set; }
        //public virtual ICollection<PlanoDto> Planos { get; set; }
        public long? VersaoTissId { get; set; }
        public VersaoTissDto VersaoTiss { get; set; }

        //Campos de configuração do TISS
        public bool IsPreencheCodigoCredenciadoCodigoOperadora { get; set; }
        public bool IsImprimeTratamento { get; set; }
        public bool IsImprimeObsConta { get; set; }
        public bool IsAgrupaGuiaXml { get; set; }
        public bool Is09e10 { get; set; }
        public bool IsFatorMultiplicador { get; set; }
        public bool IsEquipeMedicaBranco { get; set; }
        public bool IsObrigaEspecialidade { get; set; }
        public bool Is41a45BrancoPJ { get; set; }
        public bool IsSomarFilmeTaxa { get; set; }
        public bool IsImprimeObsContaGuiaConsulta { get; set; }
        public bool IsImportaAgudaCronica { get; set; }
        public bool Is38e39GuiaConsulta { get; set; }
        public bool Is86e89GuiaSPSADT { get; set; }
        public bool IsFilmeGuiaOutrasDespesas { get; set; }
        public bool Is22GuiaSPSADT { get; set; }
        public int XmlUltimosDigitosMatricula { get; set; }
        public int XmlPrimeirosDigitosMatricula { get; set; }

        public List<IdentificacaoPrestadorNaOperadoraDto> IdentificacoesPrestadoresNaOperadoraDto { get; set; }

        //public ICollection<ConvenioURLServicoDto> UrlServicos { get; set; }

        public string VerificaElegibilidadeHomologacao { get; set; }
        public string ComunicacaoBeneficiarioHomologacao { get; set; }
        public string CancelaGuiaHomologacao { get; set; }
        public string SolicitacaoProcedimentoHomologacao { get; set; }
        public string SolicitaStatusAutorizacaoHomologacao { get; set; }
        public string LoteAnexoHomologacao { get; set; }
        public string LoteGuiasHomologacao { get; set; }
        public string SolicitaStatusProtocoloHomologacao { get; set; }
        public string SolicitacaoDemonstrativoRetornoHomologacao { get; set; }
        public string RecursoGlosaHomologacao { get; set; }

        public string VerificaElegibilidade { get; set; }
        public string ComunicacaoBeneficiario { get; set; }
        public string CancelaGuia { get; set; }
        public string SolicitacaoProcedimento { get; set; }
        public string SolicitastatusAutorizacao { get; set; }
        public string LoteAnexo { get; set; }
        public string LoteGuias { get; set; }
        public string SolicitaStatusProtocolo { get; set; }
        public string SolicitacaoDemonstrativoRetorno { get; set; }
        public string RecursoGlosa { get; set; }

        public string WebService { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string Homologacao { get; set; }
        public string Certificado { get; set; }
        public string CaCerfts { get; set; }
        public string SenhaCertificado { get; set; }
        public string SenhaCacerts { get; set; }

        public ConvenioURLServicoDto ConvenioURLServico { get; set; }
        public string URLServicos { get; set; }

        public bool IsEletivo { get; set; }
        public bool IsUrgencia { get; set; }

        public long? FormaAutorizacaoId { get; set; }

        public FormaAutorizacaoDto FormaAutorizacao { get; set; }

        public string DadosContato { get; set; }

        public bool IsPreencheGuiaAutomaticamente { get; set; }

        public long? EmpresaPadraoEmergenciaId { get; set; }

        public EmpresaDto EmpresaPadraoEmergencia { get; set; }

        public long? MedicoPadraoEmergenciaId { get; set; }

        public MedicoDto MedicoPadraoEmergencia { get; set; }

        public long? EspecialidadePadraoEmergenciaId { get; set; }

        public EspecialidadeDto EspecialidadePadraoEmergencia { get; set; }

        public long? EmpresaPadraoInternacaoId { get; set; }

        public EmpresaDto EmpresaPadraoInternacao { get; set; }

        public long? MedicoPadraoInternacaoId { get; set; }

        public MedicoDto MedicoPadraoInternacao { get; set; }

        public long? EspecialidadePadraoInternacaoId { get; set; }

        public EspecialidadeDto EspecialidadePadraoInternacao { get; set; }

        public List<IntervaloGuiasConvenioIndex> IntervaloGuiasConveniosIndex { get; set; }

        public List<CodigoCredenciadoIndex> CodigosCredenciadoIndex { get; set; }

        public List<FaturamentoGrupoConvenioIndex> FatGrupoConvenioIndex { get; set; }
        
        public string IntervaloGuiasConveniosIndexJson { get; set; }

        public string CodigoCredenciadoConveniosIndexJson { get; set; }

        public string FatGrupoConvenioIndexJson { get; set; }

        public bool IsParticular { get; set; }

        public bool HabilitaAuditoriaInterna { get; set; }

        public bool HabilitaAuditoriaExterna { get; set; }

        public bool HabilitaEntrega { get; set; }

        public long? ConfiguracaoResumoContaInternacaoId { get; set; }

        public ConvenioConfiguracaoResumoContaDto ConfiguracaoResumoContaInternacao { get; set; }

        public long? ConfiguracaoResumoContaEmergenciaId { get; set; }
 
        public ConvenioConfiguracaoResumoContaDto ConfiguracaoResumoContaEmergencia { get; set; }

        #region Mapeamento
        public static ConvenioDto Mapear(Convenio input)
        {
            if (input == null)
            {
                return null;
            }

            ConvenioDto result = MapearBase<ConvenioDto>(input);

            result.Id = input.Id;
            result.Codigo = input.Codigo;
            result.Descricao = input.Descricao;
            result.Nome = input.Nome;
            result.NomeFantasia = input.NomeFantasia;
            result.RazaoSocial = input.RazaoSocial;
            result.RegistroANS = input.RegistroANS;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.DataInicialContrato = input.DataInicialContrato;
            result.DataInicioContrato = input.DataInicioContrato;
            result.DataProximaRenovacaoContratual = input.DataProximaRenovacaoContratual;
            result.DataUltimaRenovacaoContrato = input.DataUltimaRenovacaoContrato;
            result.Is09e10 = input.Is09e10;
            result.Is22GuiaSPSADT = input.Is22GuiaSPSADT;
            result.Is38e39GuiaConsulta = input.Is38e39GuiaConsulta;
            result.Is41a45BrancoPJ = input.Is41a45BrancoPJ;
            result.Is86e89GuiaSPSADT = input.Is86e89GuiaSPSADT;
            result.IsAgrupaGuiaXml = input.IsAgrupaGuiaXml;
            result.IsAtivo = input.IsAtivo;
            result.IsEquipeMedicaBranco = input.IsEquipeMedicaBranco;
            result.IsFatorMultiplicador = input.IsFatorMultiplicador;
            result.IsFilmeGuiaOutrasDespesas = input.IsFilmeGuiaOutrasDespesas;
            result.IsFilotranpica = input.IsFilotranpica;
            result.IsImportaAgudaCronica = input.IsImportaAgudaCronica;
            result.IsImprimeObsConta = input.IsImprimeObsConta;
            result.IsImprimeObsContaGuiaConsulta = input.IsImprimeObsContaGuiaConsulta;
            result.IsImprimeTratamento = input.IsImprimeTratamento;
            result.IsObrigaEspecialidade = input.IsObrigaEspecialidade;
            result.IsPreencheCodigoCredenciadoCodigoOperadora = input.IsPreencheCodigoCredenciadoCodigoOperadora;
            result.IsSistema = input.IsSistema;
            result.IsSomarFilmeTaxa = input.IsSomarFilmeTaxa;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.SisPessoaId = input.SisPessoaId;
            result.VersaoTissId = input.VersaoTissId;
            result.XmlPrimeirosDigitosMatricula = input.XmlPrimeirosDigitosMatricula;
            result.XmlUltimosDigitosMatricula = input.XmlUltimosDigitosMatricula;

            result.FormaAutorizacaoId = input.FormaAutorizacaoId;
            result.DadosContato = input.DadosContato;
            result.HabilitaEntrega = input.HabilitaEntrega;
            result.HabilitaAuditoriaExterna = input.HabilitaAuditoriaExterna;
            result.HabilitaAuditoriaInterna = input.HabilitaAuditoriaInterna;
            result.ConfiguracaoResumoContaEmergenciaId = input.ConfiguracaoResumoContaEmergenciaId;
            result.ConfiguracaoResumoContaInternacaoId = input.ConfiguracaoResumoContaInternacaoId;

            if (input.FormaAutorizacao != null)
            {
                result.FormaAutorizacao = new FormaAutorizacaoDto
                {
                    Id = input.FormaAutorizacao.Id,
                    Codigo = input.FormaAutorizacao.Codigo,
                    Descricao = input.FormaAutorizacao.Descricao
                };
            }

            result.Logotipo = input.Logotipo;
            result.IdentificacoesPrestadoresNaOperadoraDto = new List<IdentificacaoPrestadorNaOperadoraDto>();

            if (input.IdentificacoesPrestadoresNaOperadora != null)
            {
                foreach (var item in input.IdentificacoesPrestadoresNaOperadora)
                {
                    // item.Convenio = null;
                    result.IdentificacoesPrestadoresNaOperadoraDto.Add(IdentificacaoPrestadorNaOperadoraDto.Mapear(item));
                }
            }

            if (input.SisPessoa != null)
            {
                result.SisPessoa = SisPessoaDto.Mapear(input.SisPessoa);
            }

            if (input.VersaoTiss != null)
            {
                result.VersaoTiss = input.VersaoTiss.MapTo<VersaoTissDto>();
            }

            result.IsPreencheGuiaAutomaticamente = input.IsPreencheGuiaAutomaticamente;
            result.EmpresaPadraoEmergenciaId = input.EmpresaPadraoEmergenciaId;
            result.MedicoPadraoEmergenciaId = input.MedicoPadraoEmergenciaId;
            result.EspecialidadePadraoEmergenciaId = input.EspecialidadePadraoEmergenciaId;
            result.EmpresaPadraoInternacaoId = input.EmpresaPadraoInternacaoId;
            result.MedicoPadraoInternacaoId = input.MedicoPadraoInternacaoId;
            result.EspecialidadePadraoInternacaoId = input.EspecialidadePadraoInternacaoId;

            if (input.EmpresaPadraoEmergencia != null)
            {
                result.EmpresaPadraoEmergencia = EmpresaDto.Mapear(input.EmpresaPadraoEmergencia);
            }

            if (input.MedicoPadraoEmergencia != null)
            {
                result.MedicoPadraoEmergencia = MedicoDto.Mapear(input.MedicoPadraoEmergencia);
            }

            if (input.EspecialidadePadraoEmergencia != null)
            {
                result.EspecialidadePadraoEmergencia = EspecialidadeDto.Mapear(input.EspecialidadePadraoEmergencia);
            }

            if (input.EmpresaPadraoInternacao != null)
            {
                result.EmpresaPadraoInternacao = EmpresaDto.Mapear(input.EmpresaPadraoInternacao);
            }

            if (input.MedicoPadraoInternacao != null)
            {
                result.MedicoPadraoInternacao = MedicoDto.Mapear(input.MedicoPadraoInternacao);
            }

            if (input.EspecialidadePadraoInternacao != null)
            {
                result.EspecialidadePadraoInternacao = EspecialidadeDto.Mapear(input.EspecialidadePadraoInternacao);
            }

            if (input.IntervalosGuiasConvenios != null)
            {
                result.IntervaloGuiasConveniosIndex = new List<IntervaloGuiasConvenioIndex>();

                long idGrid = 0;

                foreach (var item in input.IntervalosGuiasConvenios)
                {
                    var index = new IntervaloGuiasConvenioIndex
                    {
                        Id = item.Id,
                        ConvenioId = item.ConvenioId,
                        ConvenioDescricao = input.NomeFantasia,
                        EmpresaId = item.EmpresaId,
                        EmpresaDescricao = item.Empresa?.RazaoSocial,
                        FaturamentoGuiaId = item.FaturamentoGuiaId,
                        FaturamentoGuiaDescricao = item.FaturamentoGuia?.Descricao,
                        Inicio = item.Inicio,
                        Final = item.Final,
                        NumeroGuiasParaAviso = item.NumeroGuiasParaAviso,
                        IdGrid = idGrid++
                    };

                    result.IntervaloGuiasConveniosIndex.Add(index);
                }
            }

            if (input.CodigosCredenciado != null)
            {
                result.CodigosCredenciadoIndex = new List<CodigoCredenciadoIndex>();

                long idGrid = 0;

                foreach (var item in input.CodigosCredenciado)
                {
                    var codigoCredenciadoDto = new CodigoCredenciadoIndex
                    {
                        Id = item.Id,
                        ConvenioId = item.ConvenioId,
                        Codigo = item.Codigo,
                        EmpresaId = item.EmpresaId,
                        EmpresaDescricao = item.Empresa?.RazaoSocial,
                        Descricao = item.Descricao,
                        IdGrid = idGrid++
                    };

                    result.CodigosCredenciadoIndex.Add(codigoCredenciadoDto);
                }
            }

            if (input.FatGrupoConvenio != null)
            {
                result.FatGrupoConvenioIndex = new List<FaturamentoGrupoConvenioIndex>();

                long idGrid = 0;

                foreach (var item in input.FatGrupoConvenio)
                {
                    var faturamentoGrupoDto = new FaturamentoGrupoConvenioIndex
                    {
                        Id = item.Id,
                        ConvenioId = item.ConvenioId,
                        Codigo = item.Codigo,
                        GrupoId = item.GrupoId,
                        GrupoDescricao = item.Grupo?.Descricao,
                        IsCobrancaDia = item.IsCobrancaDia == true ? "Sim" : "Não",
                        IdGrid = idGrid++
                    };

                    result.FatGrupoConvenioIndex.Add(faturamentoGrupoDto);
                }
            }

            if (input.ConfiguracaoResumoContaInternacao  != null)
            {
                result.ConfiguracaoResumoContaInternacao = ConvenioConfiguracaoResumoContaDto.Mapear(input.ConfiguracaoResumoContaInternacao);
            }

            if (input.ConfiguracaoResumoContaEmergencia != null)
            {
                result.ConfiguracaoResumoContaEmergencia = ConvenioConfiguracaoResumoContaDto.Mapear(input.ConfiguracaoResumoContaEmergencia);
            }

            result.IsParticular = input.IsParticular;

            return result;

        }

        public static Convenio Mapear(ConvenioDto input)
        {
            if (input == null)
            {
                return null;
            }

            Convenio result = new Convenio
            {
                Id = input.Id,
                Codigo = input.Codigo,
                Descricao = input.Descricao,
                Nome = input.Nome,
                NomeFantasia = input.NomeFantasia,

                CreationTime = input.CreationTime,
                CreatorUserId = input.CreatorUserId,
                DataInicialContrato = input.DataInicialContrato,
                DataInicioContrato = input.DataInicioContrato,
                DataProximaRenovacaoContratual = input.DataProximaRenovacaoContratual,
                DataUltimaRenovacaoContrato = input.DataUltimaRenovacaoContrato,
                Is09e10 = input.Is09e10,
                Is22GuiaSPSADT = input.Is22GuiaSPSADT,
                Is38e39GuiaConsulta = input.Is38e39GuiaConsulta,
                Is41a45BrancoPJ = input.Is41a45BrancoPJ,
                Is86e89GuiaSPSADT = input.Is86e89GuiaSPSADT,
                IsAgrupaGuiaXml = input.IsAgrupaGuiaXml,
                IsAtivo = input.IsAtivo,
                IsEquipeMedicaBranco = input.IsEquipeMedicaBranco,
                IsFatorMultiplicador = input.IsFatorMultiplicador,
                IsFilmeGuiaOutrasDespesas = input.IsFilmeGuiaOutrasDespesas,
                IsFilotranpica = input.IsFilotranpica,
                IsImportaAgudaCronica = input.IsImportaAgudaCronica,
                IsImprimeObsConta = input.IsImprimeObsConta,
                IsImprimeObsContaGuiaConsulta = input.IsImprimeObsContaGuiaConsulta,
                IsImprimeTratamento = input.IsImprimeTratamento,
                IsObrigaEspecialidade = input.IsObrigaEspecialidade,
                IsPreencheCodigoCredenciadoCodigoOperadora = input.IsPreencheCodigoCredenciadoCodigoOperadora,
                IsSistema = input.IsSistema,
                IsSomarFilmeTaxa = input.IsSomarFilmeTaxa,
                LastModificationTime = input.LastModificationTime,
                LastModifierUserId = input.LastModifierUserId,
                SisPessoaId = input.SisPessoaId,
                VersaoTissId = input.VersaoTissId,
                XmlPrimeirosDigitosMatricula = input.XmlPrimeirosDigitosMatricula,
                XmlUltimosDigitosMatricula = input.XmlUltimosDigitosMatricula,
                FormaAutorizacaoId = input.FormaAutorizacaoId,
                DadosContato = input.DadosContato,

                IsPreencheGuiaAutomaticamente = input.IsPreencheGuiaAutomaticamente,
                EmpresaPadraoEmergenciaId = input.EmpresaPadraoEmergenciaId,
                MedicoPadraoEmergenciaId = input.MedicoPadraoEmergenciaId,
                EspecialidadePadraoEmergenciaId = input.EspecialidadePadraoEmergenciaId,
                EmpresaPadraoInternacaoId = input.EmpresaPadraoInternacaoId,
                MedicoPadraoInternacaoId = input.MedicoPadraoInternacaoId,
                EspecialidadePadraoInternacaoId = input.EspecialidadePadraoInternacaoId,
                HabilitaEntrega = input.HabilitaEntrega,
                HabilitaAuditoriaExterna = input.HabilitaAuditoriaExterna,
                HabilitaAuditoriaInterna = input.HabilitaAuditoriaInterna,
                ConfiguracaoResumoContaEmergenciaId = input.ConfiguracaoResumoContaEmergenciaId,
                ConfiguracaoResumoContaInternacaoId = input.ConfiguracaoResumoContaInternacaoId,

                IsParticular = input.IsParticular
            };

            if (input.ConfiguracaoResumoContaInternacao != null)
            {
                result.ConfiguracaoResumoContaInternacao = ConvenioConfiguracaoResumoContaDto.Mapear(input.ConfiguracaoResumoContaInternacao);
            }

            if (input.ConfiguracaoResumoContaEmergencia != null)
            {
                result.ConfiguracaoResumoContaEmergencia = ConvenioConfiguracaoResumoContaDto.Mapear(input.ConfiguracaoResumoContaEmergencia);
            }

            return result;
        }

        public static IEnumerable<ConvenioDto> Mapear(List<Convenio> list)
        {
            foreach (var input in list)
            {
                yield return Mapear(input);
            }
        }

        public static IEnumerable<Convenio> Mapear(List<ConvenioDto> list)
        {
            foreach (var input in list)
            {
                yield return Mapear(input);
            }
        }
        #endregion
    }


    public class ConvenioConfiguracaoResumoContaDto: CamposPadraoCRUDDto
    {
        public bool IsAgrupaItens { get; set; }
        public bool IsAgrupaUnidadeOrganizacional { get; set; }
        public bool IsImprimeCID { get; set; }
        public bool IsImprimeCPFMedico { get; set; }
        public bool IsImprimeObservacaoItens { get; set; }

        public bool IsImprimeTratamento { get; set; }
        public bool IsImprimeTratamentos { get; set; }

        public bool IsImprimeDataHoraImpressao { get; set; }

        public bool IsExibeDescontoDoCaixa { get; set; }


        public static ConvenioConfiguracaoResumoContaDto Mapear(ConvenioConfiguracaoResumoConta entity)
        {
            var dto = MapearBase<ConvenioConfiguracaoResumoContaDto>(entity);
            dto.IsAgrupaItens = entity.IsAgrupaItens;
            dto.IsAgrupaUnidadeOrganizacional = entity.IsAgrupaUnidadeOrganizacional;
            dto.IsImprimeCID = entity.IsImprimeCID;
            dto.IsImprimeCPFMedico = entity.IsImprimeCPFMedico;
            dto.IsImprimeObservacaoItens = entity.IsImprimeObservacaoItens;
            dto.IsImprimeTratamento = entity.IsImprimeTratamento;
            dto.IsImprimeTratamentos = entity.IsImprimeTratamentos;
            dto.IsImprimeDataHoraImpressao = entity.IsImprimeDataHoraImpressao;
            dto.IsExibeDescontoDoCaixa = entity.IsExibeDescontoDoCaixa;
            return dto;
        }

        public static ConvenioConfiguracaoResumoConta Mapear(ConvenioConfiguracaoResumoContaDto dto)
        {
            var entity = MapearBase<ConvenioConfiguracaoResumoConta>(dto);
            entity.IsAgrupaItens = dto.IsAgrupaItens;
            entity.IsAgrupaUnidadeOrganizacional = dto.IsAgrupaUnidadeOrganizacional;
            entity.IsImprimeCID = dto.IsImprimeCID;
            entity.IsImprimeCPFMedico = dto.IsImprimeCPFMedico;
            entity.IsImprimeObservacaoItens = dto.IsImprimeObservacaoItens;
            entity.IsImprimeTratamento = dto.IsImprimeTratamento;
            entity.IsImprimeTratamentos = dto.IsImprimeTratamentos;
            entity.IsImprimeDataHoraImpressao = dto.IsImprimeDataHoraImpressao;
            entity.IsExibeDescontoDoCaixa = dto.IsExibeDescontoDoCaixa;
            return entity;
        }
    }


    public class ListarConveniosTabelaPrecoDto: CamposPadraoCRUDDto
    {
        public string NomeFantasia { get; set; }
        
        public string RazaoSocial { get; set; }
        
        public string RegistroANS { get; set; }
        
        public bool IsAtivo { get; set; }
    }
}