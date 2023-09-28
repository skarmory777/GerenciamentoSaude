using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
//using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposLeito;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto
{
    [AutoMap(typeof(FaturamentoConta))]
    public class FaturamentoContaDto : CamposPadraoCRUDDto
    {
        public string Matricula { get; set; }
        public string CodDependente { get; set; }
        public string NumeroGuia { get; set; }
        public string Titular { get; set; }
        
        public int OrigemTitular { get; set; }
        public string GuiaOperadora { get; set; }
        public string GuiaPrincipal { get; set; }
        public string Observacao { get; set; }
        public string SenhaAutorizacao { get; set; }
        public string IdentAcompanhante { get; set; }
        public long? PacienteId { get; set; }
        public PacienteDto Paciente { get; set; }
        public long? MedicoId { get; set; }
        public MedicoDto Medico { get; set; }
        public long? ConvenioId { get; set; }
        public ConvenioDto Convenio { get; set; }
        public long? PlanoId { get; set; }
        public PlanoDto Plano { get; set; }
        // Modelo antigo
        public long? GuiaId { get; set; }
        public GuiaDto Guia { get; set; }
        // Novo modelo
        public long? FatGuiaId { get; set; }
        public FaturamentoGuiaDto FatGuia { get; set; }
        public long? EmpresaId { get; set; }
        public EmpresaDto Empresa { get; set; }
        public long? AtendimentoId { get; set; }
        public AtendimentoDto Atendimento { get; set; }
        public long? UnidadeOrganizacionalId { get; set; }
        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }
        public long? TipoAcomodacaoId { get; set; }
        public TipoLeitoDto TipoLeito { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public DateTime? DataPagamento { get; set; }
        public DateTime? ValidadeCarteira { get; set; }
        public DateTime? DataAutorizacao { get; set; }
        public long? DiasAutorizacao { get; set; }
        
        public DateTime? DiaSerie1 { get; set; }
        public DateTime? DiaSerie2 { get; set; }
        public DateTime? DiaSerie3 { get; set; }
        public DateTime? DiaSerie4 { get; set; }
        public DateTime? DiaSerie5 { get; set; }
        public DateTime? DiaSerie6 { get; set; }
        public DateTime? DiaSerie7 { get; set; }
        public DateTime? DiaSerie8 { get; set; }
        public DateTime? DiaSerie9 { get; set; }
        public DateTime? DiaSerie10 { get; set; }
        public DateTime? DataEntrFolhaSala { get; set; }
        public DateTime? DataEntrDescCir { get; set; }
        public DateTime? DataEntrBolAnest { get; set; }
        public DateTime? DataEntrCDFilme { get; set; }
        public DateTime? DataValidadeSenha { get; set; }
        public bool IsAutorizador { get; set; }
        public int TipoAtendimento { get; set; }
        public long? StatusId { get; set; }
        public FaturamentoContaStatusDto Status { get; set; }
        public List<FaturamentoContaItemDto> Itens { get; set; }

        public string MotivoPendencia { get; set; }

        public DateTime? DataConferencia { get; set; }
        public long? UsuarioConferenciaId { get; set; }
        public User UsuarioConferencia { get; set; }

        public float ValorTotal { get; set; }
        
        public long? ContaMedicaId { get; set; }
        
        public FaturamentoContaDto ContaMedica { get; set; }
        
        public bool IsAtivo { get; set; }
        
        public long Versao { get; set; }

        public FaturamentoConta MapearParaBanco()
        {
            var fatConta = new FaturamentoConta
            {
                Id = Id,
                DataConferencia = DataConferencia,
                UsuarioConferenciaId = UsuarioConferenciaId,
                FatGuiaId = FatGuiaId,
                Matricula = Matricula,
                CodDependente = CodDependente,
                NumeroGuia = NumeroGuia,
                Titular = Titular,
                GuiaOperadora = GuiaOperadora,
                GuiaPrincipal = GuiaPrincipal,
                Observacao = Observacao,
                SenhaAutorizacao = SenhaAutorizacao,
                IdentAcompanhante = IdentAcompanhante,
                PacienteId = PacienteId == 0 ? null : PacienteId,
                AtendimentoId = AtendimentoId == 0 ? null : AtendimentoId,
                MedicoId = MedicoId == 0 ? null : MedicoId,
                ConvenioId = ConvenioId == 0 ? null : ConvenioId,
                PlanoId = PlanoId == 0 ? null : PlanoId,
                GuiaId = GuiaId == 0 ? null : GuiaId,
                EmpresaId = EmpresaId == 0 ? null : EmpresaId,
                UnidadeOrganizacionalId = UnidadeOrganizacionalId == 0 ? null : UnidadeOrganizacionalId,
                // fatConta.TipoLeitoId = this.TipoLeitoId == 0 ? null : this.TipoLeitoId;
                TipoAcomodacaoId = TipoAcomodacaoId == 0 ? null : TipoAcomodacaoId,
                DataInicio = DataInicio,
                DataFim = DataFim,
                DataPagamento = DataPagamento,
                ValidadeCarteira = ValidadeCarteira,
                DataAutorizacao = DataAutorizacao,
                DiaSerie1 = DiaSerie1,
                DiaSerie2 = DiaSerie2,
                DiaSerie3 = DiaSerie3,
                DiaSerie4 = DiaSerie4,
                DiaSerie5 = DiaSerie5,
                DiaSerie6 = DiaSerie6,
                DiaSerie7 = DiaSerie7,
                DiaSerie8 = DiaSerie8,
                DiaSerie9 = DiaSerie9,
                DiaSerie10 = DiaSerie10,
                DataEntrFolhaSala = DataEntrFolhaSala,
                DataEntrDescCir = DataEntrDescCir,
                DataEntrBolAnest = DataEntrBolAnest,
                DataEntrCDFilme = DataEntrCDFilme,
                DataValidadeSenha = DataValidadeSenha,
                IsAutorizador = IsAutorizador,
                TipoAtendimento = TipoAtendimento,
                StatusId = StatusId,
                IsAtivo = IsAtivo,
                ContaMedicaId = ContaMedicaId,
                Versao = Versao
            };

            return fatConta;
        }


        public static FaturamentoContaDto Mapear(FaturamentoConta faturamentoConta)
        {
            if(faturamentoConta == null)
            {
                return null;
            }
            FaturamentoContaDto fatConta = new FaturamentoContaDto
            {
                Id = faturamentoConta.Id,
                DataConferencia = faturamentoConta.DataConferencia,
                UsuarioConferenciaId = faturamentoConta.UsuarioConferenciaId,
                FatGuiaId = faturamentoConta.FatGuiaId,
                Matricula = faturamentoConta.Matricula,
                CodDependente = faturamentoConta.CodDependente,
                NumeroGuia = faturamentoConta.NumeroGuia,
                Titular = faturamentoConta.Titular,
                GuiaOperadora = faturamentoConta.GuiaOperadora,
                GuiaPrincipal = faturamentoConta.GuiaPrincipal,
                Observacao = faturamentoConta.Observacao,
                SenhaAutorizacao = faturamentoConta.SenhaAutorizacao,
                IdentAcompanhante = faturamentoConta.IdentAcompanhante,
                PacienteId = faturamentoConta.PacienteId == 0 ? null : faturamentoConta.PacienteId,
                AtendimentoId = faturamentoConta.AtendimentoId == 0 ? null : faturamentoConta.AtendimentoId,
                MedicoId = faturamentoConta.MedicoId == 0 ? null : faturamentoConta.MedicoId,
                ConvenioId = faturamentoConta.ConvenioId == 0 ? null : faturamentoConta.ConvenioId,
                PlanoId = faturamentoConta.PlanoId == 0 ? null : faturamentoConta.PlanoId,
                GuiaId = faturamentoConta.GuiaId == 0 ? null : faturamentoConta.GuiaId,
                EmpresaId = faturamentoConta.EmpresaId == 0 ? null : faturamentoConta.EmpresaId,
                UnidadeOrganizacionalId = faturamentoConta.UnidadeOrganizacionalId == 0 ? null : faturamentoConta.UnidadeOrganizacionalId,
                // fatConta.TipoLeitoId = faturamentoConta.TipoLeitoId == 0 ? null : faturamentoConta.TipoLeitoId;
                TipoAcomodacaoId = faturamentoConta.TipoAcomodacaoId == 0 ? null : faturamentoConta.TipoAcomodacaoId,
                DataInicio = faturamentoConta.DataInicio,
                DataFim = faturamentoConta.DataFim,
                DataPagamento = faturamentoConta.DataPagamento,
                ValidadeCarteira = faturamentoConta.ValidadeCarteira,
                DataAutorizacao = faturamentoConta.DataAutorizacao,
                DiaSerie1 = faturamentoConta.DiaSerie1,
                DiaSerie2 = faturamentoConta.DiaSerie2,
                DiaSerie3 = faturamentoConta.DiaSerie3,
                DiaSerie4 = faturamentoConta.DiaSerie4,
                DiaSerie5 = faturamentoConta.DiaSerie5,
                DiaSerie6 = faturamentoConta.DiaSerie6,
                DiaSerie7 = faturamentoConta.DiaSerie7,
                DiaSerie8 = faturamentoConta.DiaSerie8,
                DiaSerie9 = faturamentoConta.DiaSerie9,
                DiaSerie10 = faturamentoConta.DiaSerie10,
                DataEntrFolhaSala = faturamentoConta.DataEntrFolhaSala,
                DataEntrDescCir = faturamentoConta.DataEntrDescCir,
                DataEntrBolAnest = faturamentoConta.DataEntrBolAnest,
                DataEntrCDFilme = faturamentoConta.DataEntrCDFilme,
                DataValidadeSenha = faturamentoConta.DataValidadeSenha,
                IsAutorizador = faturamentoConta.IsAutorizador,
                TipoAtendimento = faturamentoConta.TipoAtendimento,
                StatusId = faturamentoConta.StatusId
            };

            if (faturamentoConta.Status != null)
            {
                fatConta.Status = FaturamentoContaStatusDto.Mapear(faturamentoConta.Status);
            }

            if (faturamentoConta.Atendimento != null)
            {
                fatConta.Atendimento = AtendimentoDto.Mapear(faturamentoConta.Atendimento);
            }

            if (faturamentoConta.Empresa != null)
            {
                fatConta.Empresa = EmpresaDto.Mapear(faturamentoConta.Empresa);
            }

            if (faturamentoConta.Convenio != null)
            {
                fatConta.Convenio = ConvenioDto.Mapear(faturamentoConta.Convenio);
            }
            
            if (faturamentoConta.Plano != null)
            {
                fatConta.Plano = PlanoDto.Mapear(faturamentoConta.Plano);
            }


            //fatConta.Itens = new List<FaturamentoContaItemDto>();

            //if (faturamentoConta.ContaItens != null)
            //{
            //    foreach (var item in faturamentoConta.ContaItens)
            //    {
            //        // item.FaturamentoConta = null;
            //        fatConta.Itens.Add(FaturamentoContaItemDto.MapearFromCore(item));
            //    }
            //}

            return fatConta;
        }
        
        public static FaturamentoConta Mapear(FaturamentoContaDto dto)
        {
            FaturamentoContaDto fatConta = new FaturamentoContaDto();

            var entity = MapearBase<FaturamentoConta>(dto);
            
            entity.DataConferencia = dto.DataConferencia;
            entity.UsuarioConferenciaId = dto.UsuarioConferenciaId;
            entity.FatGuiaId = dto.FatGuiaId;
            entity.Matricula = dto.Matricula;
            entity.CodDependente = dto.CodDependente;
            entity.NumeroGuia = dto.NumeroGuia;
            entity.Titular = dto.Titular;
            entity.GuiaOperadora = dto.GuiaOperadora;
            entity.GuiaPrincipal = dto.GuiaPrincipal;
            entity.Observacao = dto.Observacao;
            entity.SenhaAutorizacao = dto.SenhaAutorizacao;
            entity.IdentAcompanhante = dto.IdentAcompanhante;
            entity.PacienteId = dto.PacienteId == 0 ? null : dto.PacienteId;
            entity.AtendimentoId = dto.AtendimentoId == 0 ? null : dto.AtendimentoId;
            entity.MedicoId = dto.MedicoId == 0 ? null : dto.MedicoId;
            entity.ConvenioId = dto.ConvenioId == 0 ? null : dto.ConvenioId;
            entity.PlanoId = dto.PlanoId == 0 ? null : dto.PlanoId;
            entity.GuiaId = dto.GuiaId == 0 ? null : dto.GuiaId;
            entity.EmpresaId = dto.EmpresaId == 0 ? null : dto.EmpresaId;
            entity.UnidadeOrganizacionalId = dto.UnidadeOrganizacionalId == 0 ? null : dto.UnidadeOrganizacionalId;
            // fatConta.TipoLeitoId = faturamentoConta.TipoLeitoId == 0 ? null : faturamentoConta.TipoLeitoId;
            entity.TipoAcomodacaoId = dto.TipoAcomodacaoId == 0 ? null : dto.TipoAcomodacaoId;
            entity.DataInicio = dto.DataInicio;
            entity.DataFim = dto.DataFim;
            entity.DataPagamento = dto.DataPagamento;
            entity.ValidadeCarteira = dto.ValidadeCarteira;
            entity.DataAutorizacao = dto.DataAutorizacao;
            entity.DiaSerie1 = dto.DiaSerie1;
            entity.DiaSerie2 = dto.DiaSerie2;
            entity.DiaSerie3 = dto.DiaSerie3;
            entity.DiaSerie4 = dto.DiaSerie4;
            entity.DiaSerie5 = dto.DiaSerie5;
            entity.DiaSerie6 = dto.DiaSerie6;
            entity.DiaSerie7 = dto.DiaSerie7;
            entity.DiaSerie8 = dto.DiaSerie8;
            entity.DiaSerie9 = dto.DiaSerie9;
            entity.DiaSerie10 = dto.DiaSerie10;
            entity.DataEntrFolhaSala = dto.DataEntrFolhaSala;
            entity.DataEntrDescCir = dto.DataEntrDescCir;
            entity.DataEntrBolAnest = dto.DataEntrBolAnest;
            entity.DataEntrCDFilme = dto.DataEntrCDFilme;
            entity.DataValidadeSenha = dto.DataValidadeSenha;
            entity.IsAutorizador = dto.IsAutorizador;
            entity.TipoAtendimento = dto.TipoAtendimento;
            entity.StatusId = dto.StatusId;

            if (dto.Status != null)
            {
                entity.Status = FaturamentoContaStatusDto.Mapear(dto.Status);
            }

            if (dto.Atendimento != null)
            {
                entity.Atendimento = AtendimentoDto.Mapear(dto.Atendimento);
            }

            if (dto.Empresa != null)
            {
                entity.Empresa = EmpresaDto.Mapear(dto.Empresa);
            }

            if (dto.Convenio != null)
            {
                entity.Convenio = ConvenioDto.Mapear(dto.Convenio);
            }


            //fatConta.Itens = new List<FaturamentoContaItemDto>();

            //if (faturamentoConta.ContaItens != null)
            //{
            //    foreach (var item in faturamentoConta.ContaItens)
            //    {
            //        // item.FaturamentoConta = null;
            //        fatConta.Itens.Add(FaturamentoContaItemDto.MapearFromCore(item));
            //    }
            //}

            return entity;
        }



    }


    public class FaturamentoContaVersaoDto : CamposPadraoCRUDDto
    {
        public long? ContaMedicaId { get; set; }

        public bool IsAtivo { get; set; }
        
        public long Versao { get; set; }
    }
}
