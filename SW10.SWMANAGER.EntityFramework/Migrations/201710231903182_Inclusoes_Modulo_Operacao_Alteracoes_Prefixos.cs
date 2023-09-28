namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;

    public partial class Inclusoes_Modulo_Operacao_Alteracoes_Prefixos : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AdmissaoMedica", newName: "AssAdmissaoMedica");
            RenameTable(name: "dbo.TipoAtendimento", newName: "AteTipoAtendimento");
            RenameTable(name: "dbo.Convenio", newName: "SisConvenio");
            RenameTable(name: "dbo.Cep", newName: "SisCep");
            RenameTable(name: "dbo.Cidade", newName: "SisCidade");
            RenameTable(name: "dbo.Estado", newName: "SisEstado");
            RenameTable(name: "dbo.Pais", newName: "SisPais");
            RenameTable(name: "dbo.TipoLogradouro", newName: "SisTipoLogradouro");
            RenameTable(name: "dbo.Nacionalidade", newName: "SisNacionalidade");
            RenameTable(name: "dbo.Naturalidade", newName: "SisNaturalidade");
            RenameTable(name: "dbo.Profissao", newName: "SisProfissao");
            RenameTable(name: "dbo.Empresa", newName: "SisEmpresa");
            RenameTable(name: "dbo.UnidadeOrganizacional", newName: "SisUnidadeOrganizacional");
            RenameTable(name: "dbo.UnidadeInternacaoTipo", newName: "AteUnidadeInternacaoTipo");
            RenameTable(name: "dbo.Plano", newName: "SisPlano");
            RenameTable(name: "dbo.Especialidade", newName: "SisEspecialidade");
            RenameTable(name: "dbo.MedicoEspecialidade", newName: "SisMedicoEspecialidade");
            RenameTable(name: "dbo.Medico", newName: "SisMedico");
            RenameTable(name: "dbo.Guia", newName: "SisGuia");
            RenameTable(name: "dbo.Leito", newName: "SisLeito");
            RenameTable(name: "dbo.TabelaDominio", newName: "SisTabelaDominio");
            RenameTable(name: "dbo.GrupoTipoTabelaDominio", newName: "SisGrupoTipoTabelaDominio");
            RenameTable(name: "dbo.TipoTabelaDominio", newName: "SisTipoTabelaDominio");
            RenameTable(name: "dbo.TabelaDominioVersaoTiss", newName: "SisTabelaDominioVersaoTiss");
            RenameTable(name: "dbo.VersaoTiss", newName: "SisVersaoTiss");
            RenameTable(name: "dbo.TipoAcomodacao", newName: "SisTipoAcomodacao");
            RenameTable(name: "dbo.MotivoAlta", newName: "AssMotivoAlta");
            RenameTable(name: "dbo.MotivoAltaTipoAlta", newName: "AssMotivoAltaTipoAlta");
            RenameTable(name: "dbo.Origem", newName: "SisOrigem");
            RenameTable(name: "dbo.Paciente", newName: "SisPaciente");
            RenameTable(name: "dbo.TipoSanguineo", newName: "SisTipoSanguineo");
            RenameTable(name: "dbo.ParecerEspecialista", newName: "AssParecerEspecialista");
            RenameTable(name: "dbo.ParecerEspecialistaResposta", newName: "AssParecerEspecialistaResposta");
            RenameTable(name: "dbo.FormResposta", newName: "SisFormResposta");
            RenameTable(name: "dbo.FormData", newName: "SisFormData");
            RenameTable(name: "dbo.ColConfig", newName: "SisFormColConfig");
            RenameTable(name: "dbo.ColMultiOption", newName: "SisFormColMultiOption");
            RenameTable(name: "dbo.FormConfig", newName: "SisFormConfig");
            RenameTable(name: "dbo.RowConfig", newName: "SisFormRowConfig");
            RenameTable(name: "dbo.Prestador", newName: "SisPrestador");
            RenameTable(name: "dbo.Conselho", newName: "SisConselho");
            RenameTable(name: "dbo.TipoParticipacao", newName: "SisTipoParticipacao");
            RenameTable(name: "dbo.TipoPrestador", newName: "SisTipoPrestador");
            RenameTable(name: "dbo.Atestado", newName: "AssAtestado");
            RenameTable(name: "dbo.TipoAtestado", newName: "AssTipoAtestado");
            RenameTable(name: "dbo.Escolaridade", newName: "SisEscolaridade");
            RenameTable(name: "dbo.EstadoCivil", newName: "SisEstadoCivil");
            RenameTable(name: "dbo.EvolucaoMedica", newName: "AssEvolucaoMedica");
            RenameTable(name: "dbo.Turno", newName: "SisTurno");
            RenameTable(name: "dbo.Favorito", newName: "SisFavorito");
            RenameTable(name: "dbo.ItemPrescricao", newName: "AssPrescricaoItem");
            RenameTable(name: "dbo.TipoControle", newName: "AssTipoControle");
            RenameTable(name: "dbo.GuiaTipo", newName: "SisGuiaTipo");
            RenameTable(name: "dbo.ModeloAtestado", newName: "AssModeloAtestado");
            RenameTable(name: "dbo.Religiao", newName: "SisReligiao");
            RenameTable(name: "dbo.Sexo", newName: "SisSexo");
            RenameTable(name: "dbo.TipoTelefone", newName: "SisTipoTelefone");
            RenameTable(name: "dbo.UltimoId", newName: "SisUltimoId");
            DropForeignKey("dbo.SisPrestador", "TipoLogradouroId", "dbo.TipoLogradouro");
            RenameColumn(table: "dbo.AssAdmissaoMedica", name: "UnidadeOrganizacionalId", newName: "SisUnidadeOrganizacionalId");
            RenameColumn(table: "dbo.Atendimento", name: "PacienteId", newName: "SisPacienteId");
            RenameColumn(table: "dbo.Atendimento", name: "OrigemId", newName: "SisOrigemId");
            RenameColumn(table: "dbo.Atendimento", name: "MedicoId", newName: "SisMedicoId");
            RenameColumn(table: "dbo.Atendimento", name: "EspecialidadeId", newName: "SisEspecialidadeId");
            RenameColumn(table: "dbo.Atendimento", name: "EmpresaId", newName: "SisEmpresaId");
            RenameColumn(table: "dbo.Atendimento", name: "ConvenioId", newName: "SisConveniolId");
            RenameColumn(table: "dbo.Atendimento", name: "PlanoId", newName: "SisPlanoId");
            RenameColumn(table: "dbo.Atendimento", name: "UnidadeOrganizacionalId", newName: "SisUnidadeOrganizacionalId");
            RenameColumn(table: "dbo.Atendimento", name: "AtendimentoTipoId", newName: "SisAtendimentoTipoId");
            RenameColumn(table: "dbo.Atendimento", name: "GuiaId", newName: "SisGuiaId");
            RenameColumn(table: "dbo.Atendimento", name: "LeitoId", newName: "SisLeitoId");
            RenameColumn(table: "dbo.Atendimento", name: "MotivoAltaId", newName: "AssMotivoAltaId");
            RenameColumn(table: "dbo.Atendimento", name: "ServicoMedicoPrestadoId", newName: "SisServicoMedicoPrestadoId");
            RenameColumn(table: "dbo.Atendimento", name: "NacionalidadeResponsavelId", newName: "SisNacionalidadeResponsavelId");
            RenameColumn(table: "dbo.SisConvenio", name: "CepCobrancaId", newName: "SisCepCobrancaId");
            RenameColumn(table: "dbo.SisConvenio", name: "TipoLogradouroCobrancaId", newName: "SisTipoLogradouroCobrancaId");
            RenameColumn(table: "dbo.SisConvenio", name: "CidadeCobrancaId", newName: "SisCidadeCobrancaId");
            RenameColumn(table: "dbo.SisConvenio", name: "EstadoCobrancaId", newName: "SisEstadoCobrancaId");
            RenameColumn(table: "dbo.SisCep", name: "CidadeId", newName: "SisCidadeId");
            RenameColumn(table: "dbo.SisCep", name: "EstadoId", newName: "SisEstadoId");
            RenameColumn(table: "dbo.SisCep", name: "PaisId", newName: "SisPaisId");
            RenameColumn(table: "dbo.SisCep", name: "TipoLogradouroId", newName: "SisTipoLogradouroId");
            RenameColumn(table: "dbo.SisCidade", name: "EstadoId", newName: "SisEstadoId");
            RenameColumn(table: "dbo.SisEstado", name: "PaisId", newName: "SisPaisId");
            RenameColumn(table: "dbo.SisEmpresa", name: "ConvenioId", newName: "SisConvenioId");
            RenameColumn(table: "dbo.SisEmpresa", name: "PlanoId", newName: "SisPlanoId");
            RenameColumn(table: "dbo.SisUnidadeOrganizacional", name: "UnidadeInternacaoTipoId", newName: "AteUnidadeInternacaoTipoId");
            RenameColumn(table: "dbo.SisUnidadeOrganizacional", name: "OrganizationUnitId", newName: "SisOrganizationUnitId");
            RenameColumn(table: "dbo.SisPlano", name: "ConvenioId", newName: "SisConvenioId");
            RenameColumn(table: "dbo.SisMedicoEspecialidade", name: "MedicoId", newName: "SisMedicoId");
            RenameColumn(table: "dbo.SisMedicoEspecialidade", name: "EspecialidadeId", newName: "SisEspecialidadeId");
            RenameColumn(table: "dbo.SisGuia", name: "OriginariaId", newName: "SisOriginariaId");
            RenameColumn(table: "dbo.SisLeito", name: "UnidadeOrganizacionalId", newName: "SisUnidadeOrganizacionalId");
            RenameColumn(table: "dbo.SisLeito", name: "TipoAcomodacaoId", newName: "SisTipoAcomodacaoId");
            RenameColumn(table: "dbo.SisLeito", name: "TabelaItemTissId", newName: "SisTabelaItemTissId");
            RenameColumn(table: "dbo.SisLeito", name: "LeitoStatusId", newName: "SisLeitoStatusId");
            RenameColumn(table: "dbo.AssMotivoAlta", name: "MotivoAltaTipoAltaId", newName: "AssMotivoAltaTipoAltaId");
            RenameColumn(table: "dbo.SisOrigem", name: "UnidadeOrganizacionalId", newName: "SisUnidadeOrganizacionalId");
            RenameColumn(table: "dbo.AssParecerEspecialista", name: "MedicoSolicitanteId", newName: "SisMedicoSolicitanteId");
            RenameColumn(table: "dbo.AssParecerEspecialista", name: "MedicoSolicitadoId", newName: "SisMedicoSolicitadoId");
            RenameColumn(table: "dbo.AssParecerEspecialista", name: "EspecialidadeSolicitadaId", newName: "SisEspecialidadeSolicitadaId");
            RenameColumn(table: "dbo.AssParecerEspecialistaResposta", name: "ParecerEspecialistaSolicitacaoId", newName: "AssParecerEspecialistaSolicitacaoId");
            RenameColumn(table: "dbo.AssParecerEspecialistaResposta", name: "MedicoRespondenteId", newName: "SisMedicoRespondenteId");
            RenameColumn(table: "dbo.ServicoMedicoPrestado", name: "EspecialidadeId", newName: "SisEspecialidadeId");
            RenameColumn(table: "dbo.SisPrestador", name: "TipoVinculoEmpregaticioId", newName: "SisTipoVinculoEmpregaticioId");
            RenameColumn(table: "dbo.SisPrestador", name: "TipoParticipacaoId", newName: "SisParticipacaoId");
            RenameColumn(table: "dbo.SisPrestador", name: "CepComercialId", newName: "SisCepComercialId");
            RenameColumn(table: "dbo.SisPrestador", name: "TipoLogradouroComercialId", newName: "SisLogradouroComercialId");
            RenameColumn(table: "dbo.SisPrestador", name: "TipoPrestadorId", newName: "SisTipoPrestadorId");
            RenameColumn(table: "dbo.SisPrestador", name: "ConselhoId", newName: "SisConselhoId");
            RenameColumn(table: "dbo.SisPrestador", name: "UserId", newName: "SisUserId");
            RenameColumn(table: "dbo.AssistencialAtendimento", name: "PacienteId", newName: "SisPacienteId");
            RenameColumn(table: "dbo.AssistencialAtendimento", name: "MedicoId", newName: "SisMedicoId");
            RenameColumn(table: "dbo.AssistencialAtendimento", name: "ConvenioId", newName: "SisConvenioId");
            RenameColumn(table: "dbo.AssistencialAtendimento", name: "EmpresaId", newName: "SisEmpresaId");
            RenameColumn(table: "dbo.AssistencialAtendimento", name: "UnidadeOrganizacionalId", newName: "SisUnidadeOrganizacionalId");
            RenameColumn(table: "dbo.AssAtestado", name: "MedicoId", newName: "SisMedicoId");
            RenameColumn(table: "dbo.AssAtestado", name: "PacienteId", newName: "SisPacienteId");
            RenameColumn(table: "dbo.AssAtestado", name: "TipoAtestadoId", newName: "AssTipoAtestadoId");
            RenameColumn(table: "dbo.AssAtestado", name: "ModeloAtestadoId", newName: "AssModeloAtestadoId");
            RenameColumn(table: "dbo.AssEvolucaoMedica", name: "FormRespostaId", newName: "SisFormRespostaId");
            RenameColumn(table: "dbo.AssFormTipoResposta", name: "UnidadeId", newName: "EstUnidadeId");
            RenameColumn(table: "dbo.AssSolicitacaoExameItem", name: "SolicitacaoExameId", newName: "AssSolicitacaoExameId");
            RenameColumn(table: "dbo.AssSolicitacaoExameItem", name: "FaturamentoItemId", newName: "FatItemId");
            RenameColumn(table: "dbo.AssSolicitacaoExameItem", name: "MaterialId", newName: "LabMaterialId");
            RenameColumn(table: "dbo.AssSolicitacaoExameItem", name: "KitExameId", newName: "LabKitExameId");
            RenameColumn(table: "dbo.AssSolicitacaoExame", name: "OrigemId", newName: "SisOrigemId");
            RenameColumn(table: "dbo.AssSolicitacaoExame", name: "LeitoId", newName: "SisLeitoId");
            RenameColumn(table: "dbo.AssSolicitacaoExame", name: "UnidadeOrganizacionalId", newName: "SisUnidadeOrganizacionalId");
            RenameColumn(table: "dbo.AssSolicitacaoExame", name: "MedicoSolicitanteId", newName: "SisMedicoSolicitanteId");
            RenameColumn(table: "dbo.AssSolicitacaoExame", name: "PrescricaoId", newName: "AssPrescricaoId");
            RenameColumn(table: "dbo.SisPaciente", name: "TipoSanguineoId", newName: "SisTipoSanguineoId");
            RenameIndex(table: "dbo.AssAdmissaoMedica", name: "IX_UnidadeOrganizacionalId", newName: "IX_SisUnidadeOrganizacionalId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_PacienteId", newName: "IX_SisPacienteId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_OrigemId", newName: "IX_SisOrigemId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_MedicoId", newName: "IX_SisMedicoId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_EspecialidadeId", newName: "IX_SisEspecialidadeId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_EmpresaId", newName: "IX_SisEmpresaId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_ConvenioId", newName: "IX_SisConveniolId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_PlanoId", newName: "IX_SisPlanoId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_UnidadeOrganizacionalId", newName: "IX_SisUnidadeOrganizacionalId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_AtendimentoTipoId", newName: "IX_SisAtendimentoTipoId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_GuiaId", newName: "IX_SisGuiaId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_LeitoId", newName: "IX_SisLeitoId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_MotivoAltaId", newName: "IX_AssMotivoAltaId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_ServicoMedicoPrestadoId", newName: "IX_SisServicoMedicoPrestadoId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_NacionalidadeResponsavelId", newName: "IX_SisNacionalidadeResponsavelId");
            RenameIndex(table: "dbo.SisConvenio", name: "IX_CepCobrancaId", newName: "IX_SisCepCobrancaId");
            RenameIndex(table: "dbo.SisConvenio", name: "IX_TipoLogradouroCobrancaId", newName: "IX_SisTipoLogradouroCobrancaId");
            RenameIndex(table: "dbo.SisConvenio", name: "IX_CidadeCobrancaId", newName: "IX_SisCidadeCobrancaId");
            RenameIndex(table: "dbo.SisConvenio", name: "IX_EstadoCobrancaId", newName: "IX_SisEstadoCobrancaId");
            RenameIndex(table: "dbo.SisCep", name: "IX_CidadeId", newName: "IX_SisCidadeId");
            RenameIndex(table: "dbo.SisCep", name: "IX_EstadoId", newName: "IX_SisEstadoId");
            RenameIndex(table: "dbo.SisCep", name: "IX_PaisId", newName: "IX_SisPaisId");
            RenameIndex(table: "dbo.SisCep", name: "IX_TipoLogradouroId", newName: "IX_SisTipoLogradouroId");
            RenameIndex(table: "dbo.SisCidade", name: "IX_EstadoId", newName: "IX_SisEstadoId");
            RenameIndex(table: "dbo.SisEstado", name: "IX_PaisId", newName: "IX_SisPaisId");
            RenameIndex(table: "dbo.SisEmpresa", name: "IX_ConvenioId", newName: "IX_SisConvenioId");
            RenameIndex(table: "dbo.SisEmpresa", name: "IX_PlanoId", newName: "IX_SisPlanoId");
            RenameIndex(table: "dbo.SisUnidadeOrganizacional", name: "IX_UnidadeInternacaoTipoId", newName: "IX_AteUnidadeInternacaoTipoId");
            RenameIndex(table: "dbo.SisUnidadeOrganizacional", name: "IX_OrganizationUnitId", newName: "IX_SisOrganizationUnitId");
            RenameIndex(table: "dbo.SisPlano", name: "IX_ConvenioId", newName: "IX_SisConvenioId");
            RenameIndex(table: "dbo.SisMedicoEspecialidade", name: "IX_MedicoId", newName: "IX_SisMedicoId");
            RenameIndex(table: "dbo.SisMedicoEspecialidade", name: "IX_EspecialidadeId", newName: "IX_SisEspecialidadeId");
            RenameIndex(table: "dbo.SisGuia", name: "IX_OriginariaId", newName: "IX_SisOriginariaId");
            RenameIndex(table: "dbo.SisLeito", name: "IX_UnidadeOrganizacionalId", newName: "IX_SisUnidadeOrganizacionalId");
            RenameIndex(table: "dbo.SisLeito", name: "IX_TipoAcomodacaoId", newName: "IX_SisTipoAcomodacaoId");
            RenameIndex(table: "dbo.SisLeito", name: "IX_TabelaItemTissId", newName: "IX_SisTabelaItemTissId");
            RenameIndex(table: "dbo.SisLeito", name: "IX_LeitoStatusId", newName: "IX_SisLeitoStatusId");
            RenameIndex(table: "dbo.AssMotivoAlta", name: "IX_MotivoAltaTipoAltaId", newName: "IX_AssMotivoAltaTipoAltaId");
            RenameIndex(table: "dbo.SisOrigem", name: "IX_UnidadeOrganizacionalId", newName: "IX_SisUnidadeOrganizacionalId");
            RenameIndex(table: "dbo.SisPaciente", name: "IX_TipoSanguineoId", newName: "IX_SisTipoSanguineoId");
            RenameIndex(table: "dbo.AssParecerEspecialista", name: "IX_MedicoSolicitanteId", newName: "IX_SisMedicoSolicitanteId");
            RenameIndex(table: "dbo.AssParecerEspecialista", name: "IX_MedicoSolicitadoId", newName: "IX_SisMedicoSolicitadoId");
            RenameIndex(table: "dbo.AssParecerEspecialista", name: "IX_EspecialidadeSolicitadaId", newName: "IX_SisEspecialidadeSolicitadaId");
            RenameIndex(table: "dbo.AssParecerEspecialistaResposta", name: "IX_ParecerEspecialistaSolicitacaoId", newName: "IX_AssParecerEspecialistaSolicitacaoId");
            RenameIndex(table: "dbo.AssParecerEspecialistaResposta", name: "IX_MedicoRespondenteId", newName: "IX_SisMedicoRespondenteId");
            RenameIndex(table: "dbo.ServicoMedicoPrestado", name: "IX_EspecialidadeId", newName: "IX_SisEspecialidadeId");
            RenameIndex(table: "dbo.SisPrestador", name: "IX_TipoVinculoEmpregaticioId", newName: "IX_SisTipoVinculoEmpregaticioId");
            RenameIndex(table: "dbo.SisPrestador", name: "IX_TipoParticipacaoId", newName: "IX_SisParticipacaoId");
            RenameIndex(table: "dbo.SisPrestador", name: "IX_CepComercialId", newName: "IX_SisCepComercialId");
            RenameIndex(table: "dbo.SisPrestador", name: "IX_TipoPrestadorId", newName: "IX_SisTipoPrestadorId");
            RenameIndex(table: "dbo.SisPrestador", name: "IX_ConselhoId", newName: "IX_SisConselhoId");
            RenameIndex(table: "dbo.SisPrestador", name: "IX_UserId", newName: "IX_SisUserId");
            RenameIndex(table: "dbo.AssistencialAtendimento", name: "IX_PacienteId", newName: "IX_SisPacienteId");
            RenameIndex(table: "dbo.AssistencialAtendimento", name: "IX_MedicoId", newName: "IX_SisMedicoId");
            RenameIndex(table: "dbo.AssistencialAtendimento", name: "IX_ConvenioId", newName: "IX_SisConvenioId");
            RenameIndex(table: "dbo.AssistencialAtendimento", name: "IX_EmpresaId", newName: "IX_SisEmpresaId");
            RenameIndex(table: "dbo.AssistencialAtendimento", name: "IX_UnidadeOrganizacionalId", newName: "IX_SisUnidadeOrganizacionalId");
            RenameIndex(table: "dbo.AssAtestado", name: "IX_MedicoId", newName: "IX_SisMedicoId");
            RenameIndex(table: "dbo.AssAtestado", name: "IX_PacienteId", newName: "IX_SisPacienteId");
            RenameIndex(table: "dbo.AssAtestado", name: "IX_TipoAtestadoId", newName: "IX_AssTipoAtestadoId");
            RenameIndex(table: "dbo.AssAtestado", name: "IX_ModeloAtestadoId", newName: "IX_AssModeloAtestadoId");
            RenameIndex(table: "dbo.AssEvolucaoMedica", name: "IX_FormRespostaId", newName: "IX_SisFormRespostaId");
            RenameIndex(table: "dbo.AssSolicitacaoExameItem", name: "IX_SolicitacaoExameId", newName: "IX_AssSolicitacaoExameId");
            RenameIndex(table: "dbo.AssSolicitacaoExameItem", name: "IX_FaturamentoItemId", newName: "IX_FatItemId");
            RenameIndex(table: "dbo.AssSolicitacaoExameItem", name: "IX_MaterialId", newName: "IX_LabMaterialId");
            RenameIndex(table: "dbo.AssSolicitacaoExameItem", name: "IX_KitExameId", newName: "IX_LabKitExameId");
            RenameIndex(table: "dbo.AssSolicitacaoExame", name: "IX_LeitoId", newName: "IX_SisLeitoId");
            RenameIndex(table: "dbo.AssSolicitacaoExame", name: "IX_UnidadeOrganizacionalId", newName: "IX_SisUnidadeOrganizacionalId");
            RenameIndex(table: "dbo.AssSolicitacaoExame", name: "IX_MedicoSolicitanteId", newName: "IX_SisMedicoSolicitanteId");
            RenameIndex(table: "dbo.AssSolicitacaoExame", name: "IX_PrescricaoId", newName: "IX_AssPrescricaoId");
            CreateTable(
                "dbo.SisModulo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Modulo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.SisOperacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    SisModuloId = c.Long(),
                    Label = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Operacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisModulo", t => t.SisModuloId)
                .Index(t => t.SisModuloId);

            CreateTable(
                "dbo.AssProntuario",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    DataAdmissao = c.DateTime(nullable: false),
                    SisUnidadeOrganizacionalId = c.Long(nullable: false),
                    AtendimentoId = c.Long(nullable: false),
                    SisPrestadorId = c.Long(nullable: false),
                    Observacao = c.String(),
                    SisFormRespostaId = c.Long(),
                    OperacaoModuloId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Prontuario_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Atendimento", t => t.AtendimentoId, cascadeDelete: false)
                .ForeignKey("dbo.SisFormResposta", t => t.SisFormRespostaId)
                .ForeignKey("dbo.SisPrestador", t => t.SisPrestadorId, cascadeDelete: false)
                .ForeignKey("dbo.SisUnidadeOrganizacional", t => t.SisUnidadeOrganizacionalId, cascadeDelete: false)
                .Index(t => t.SisUnidadeOrganizacionalId)
                .Index(t => t.AtendimentoId)
                .Index(t => t.SisPrestadorId)
                .Index(t => t.SisFormRespostaId);

            AlterTableAnnotations(
                "dbo.SisTipoSanguineo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_TipoSanguineo_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    //{ 
                    //    "DynamicFilter_TipoSanguineo_SoftDelete",
                    //    new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    //},
                });

            AlterColumn("dbo.AssFormTipoResposta", "EstUnidadeId", c => c.Long());
            CreateIndex("dbo.SisPrestador", "SisLogradouroComercialId");
            CreateIndex("dbo.AssFormTipoResposta", "EstUnidadeId");
            CreateIndex("dbo.AssSolicitacaoExame", "SisOrigemId");
            AddForeignKey("dbo.AssFormTipoResposta", "EstUnidadeId", "dbo.Est_Unidade", "Id");
            AddForeignKey("dbo.AssSolicitacaoExame", "SisOrigemId", "dbo.SisOrigem", "Id");
            AddForeignKey("dbo.SisPrestador", "SisLogradouroComercialId", "dbo.SisTipoLogradouro", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisPrestador", "SisLogradouroComercialId", "dbo.SisTipoLogradouro");
            DropForeignKey("dbo.AssProntuario", "SisUnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional");
            DropForeignKey("dbo.AssProntuario", "SisPrestadorId", "dbo.SisPrestador");
            DropForeignKey("dbo.AssProntuario", "SisFormRespostaId", "dbo.SisFormResposta");
            DropForeignKey("dbo.AssProntuario", "AtendimentoId", "dbo.Atendimento");
            DropForeignKey("dbo.SisOperacao", "SisModuloId", "dbo.SisModulo");
            DropForeignKey("dbo.AssSolicitacaoExame", "SisOrigemId", "dbo.SisOrigem");
            DropForeignKey("dbo.AssFormTipoResposta", "EstUnidadeId", "dbo.Est_Unidade");
            DropIndex("dbo.AssProntuario", new[] { "SisFormRespostaId" });
            DropIndex("dbo.AssProntuario", new[] { "SisPrestadorId" });
            DropIndex("dbo.AssProntuario", new[] { "AtendimentoId" });
            DropIndex("dbo.AssProntuario", new[] { "SisUnidadeOrganizacionalId" });
            DropIndex("dbo.SisOperacao", new[] { "SisModuloId" });
            DropIndex("dbo.AssSolicitacaoExame", new[] { "SisOrigemId" });
            DropIndex("dbo.AssFormTipoResposta", new[] { "EstUnidadeId" });
            DropIndex("dbo.SisPrestador", new[] { "SisLogradouroComercialId" });
            AlterColumn("dbo.AssFormTipoResposta", "EstUnidadeId", c => c.Long(nullable: false));
            AlterTableAnnotations(
                "dbo.SisTipoSanguineo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "DynamicFilter_TipoSanguineo_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    //{ 
                    //    "DynamicFilter_TipoSanguineo_SoftDelete",
                    //    new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    //},
                });

            DropTable("dbo.AssProntuario",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Prontuario_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisOperacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Operacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisModulo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Modulo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            RenameIndex(table: "dbo.AssSolicitacaoExame", name: "IX_AssPrescricaoId", newName: "IX_PrescricaoId");
            RenameIndex(table: "dbo.AssSolicitacaoExame", name: "IX_SisMedicoSolicitanteId", newName: "IX_MedicoSolicitanteId");
            RenameIndex(table: "dbo.AssSolicitacaoExame", name: "IX_SisUnidadeOrganizacionalId", newName: "IX_UnidadeOrganizacionalId");
            RenameIndex(table: "dbo.AssSolicitacaoExame", name: "IX_SisLeitoId", newName: "IX_LeitoId");
            RenameIndex(table: "dbo.AssSolicitacaoExameItem", name: "IX_LabKitExameId", newName: "IX_KitExameId");
            RenameIndex(table: "dbo.AssSolicitacaoExameItem", name: "IX_LabMaterialId", newName: "IX_MaterialId");
            RenameIndex(table: "dbo.AssSolicitacaoExameItem", name: "IX_FatItemId", newName: "IX_FaturamentoItemId");
            RenameIndex(table: "dbo.AssSolicitacaoExameItem", name: "IX_AssSolicitacaoExameId", newName: "IX_SolicitacaoExameId");
            RenameIndex(table: "dbo.AssEvolucaoMedica", name: "IX_SisFormRespostaId", newName: "IX_FormRespostaId");
            RenameIndex(table: "dbo.AssAtestado", name: "IX_AssModeloAtestadoId", newName: "IX_ModeloAtestadoId");
            RenameIndex(table: "dbo.AssAtestado", name: "IX_AssTipoAtestadoId", newName: "IX_TipoAtestadoId");
            RenameIndex(table: "dbo.AssAtestado", name: "IX_SisPacienteId", newName: "IX_PacienteId");
            RenameIndex(table: "dbo.AssAtestado", name: "IX_SisMedicoId", newName: "IX_MedicoId");
            RenameIndex(table: "dbo.AssistencialAtendimento", name: "IX_SisUnidadeOrganizacionalId", newName: "IX_UnidadeOrganizacionalId");
            RenameIndex(table: "dbo.AssistencialAtendimento", name: "IX_SisEmpresaId", newName: "IX_EmpresaId");
            RenameIndex(table: "dbo.AssistencialAtendimento", name: "IX_SisConvenioId", newName: "IX_ConvenioId");
            RenameIndex(table: "dbo.AssistencialAtendimento", name: "IX_SisMedicoId", newName: "IX_MedicoId");
            RenameIndex(table: "dbo.AssistencialAtendimento", name: "IX_SisPacienteId", newName: "IX_PacienteId");
            RenameIndex(table: "dbo.SisPrestador", name: "IX_SisUserId", newName: "IX_UserId");
            RenameIndex(table: "dbo.SisPrestador", name: "IX_SisConselhoId", newName: "IX_ConselhoId");
            RenameIndex(table: "dbo.SisPrestador", name: "IX_SisTipoPrestadorId", newName: "IX_TipoPrestadorId");
            RenameIndex(table: "dbo.SisPrestador", name: "IX_SisCepComercialId", newName: "IX_CepComercialId");
            RenameIndex(table: "dbo.SisPrestador", name: "IX_SisParticipacaoId", newName: "IX_TipoParticipacaoId");
            RenameIndex(table: "dbo.SisPrestador", name: "IX_SisTipoVinculoEmpregaticioId", newName: "IX_TipoVinculoEmpregaticioId");
            RenameIndex(table: "dbo.ServicoMedicoPrestado", name: "IX_SisEspecialidadeId", newName: "IX_EspecialidadeId");
            RenameIndex(table: "dbo.AssParecerEspecialistaResposta", name: "IX_SisMedicoRespondenteId", newName: "IX_MedicoRespondenteId");
            RenameIndex(table: "dbo.AssParecerEspecialistaResposta", name: "IX_AssParecerEspecialistaSolicitacaoId", newName: "IX_ParecerEspecialistaSolicitacaoId");
            RenameIndex(table: "dbo.AssParecerEspecialista", name: "IX_SisEspecialidadeSolicitadaId", newName: "IX_EspecialidadeSolicitadaId");
            RenameIndex(table: "dbo.AssParecerEspecialista", name: "IX_SisMedicoSolicitadoId", newName: "IX_MedicoSolicitadoId");
            RenameIndex(table: "dbo.AssParecerEspecialista", name: "IX_SisMedicoSolicitanteId", newName: "IX_MedicoSolicitanteId");
            RenameIndex(table: "dbo.SisPaciente", name: "IX_SisTipoSanguineoId", newName: "IX_TipoSanguineoId");
            RenameIndex(table: "dbo.SisOrigem", name: "IX_SisUnidadeOrganizacionalId", newName: "IX_UnidadeOrganizacionalId");
            RenameIndex(table: "dbo.AssMotivoAlta", name: "IX_AssMotivoAltaTipoAltaId", newName: "IX_MotivoAltaTipoAltaId");
            RenameIndex(table: "dbo.SisLeito", name: "IX_SisLeitoStatusId", newName: "IX_LeitoStatusId");
            RenameIndex(table: "dbo.SisLeito", name: "IX_SisTabelaItemTissId", newName: "IX_TabelaItemTissId");
            RenameIndex(table: "dbo.SisLeito", name: "IX_SisTipoAcomodacaoId", newName: "IX_TipoAcomodacaoId");
            RenameIndex(table: "dbo.SisLeito", name: "IX_SisUnidadeOrganizacionalId", newName: "IX_UnidadeOrganizacionalId");
            RenameIndex(table: "dbo.SisGuia", name: "IX_SisOriginariaId", newName: "IX_OriginariaId");
            RenameIndex(table: "dbo.SisMedicoEspecialidade", name: "IX_SisEspecialidadeId", newName: "IX_EspecialidadeId");
            RenameIndex(table: "dbo.SisMedicoEspecialidade", name: "IX_SisMedicoId", newName: "IX_MedicoId");
            RenameIndex(table: "dbo.SisPlano", name: "IX_SisConvenioId", newName: "IX_ConvenioId");
            RenameIndex(table: "dbo.SisUnidadeOrganizacional", name: "IX_SisOrganizationUnitId", newName: "IX_OrganizationUnitId");
            RenameIndex(table: "dbo.SisUnidadeOrganizacional", name: "IX_AteUnidadeInternacaoTipoId", newName: "IX_UnidadeInternacaoTipoId");
            RenameIndex(table: "dbo.SisEmpresa", name: "IX_SisPlanoId", newName: "IX_PlanoId");
            RenameIndex(table: "dbo.SisEmpresa", name: "IX_SisConvenioId", newName: "IX_ConvenioId");
            RenameIndex(table: "dbo.SisEstado", name: "IX_SisPaisId", newName: "IX_PaisId");
            RenameIndex(table: "dbo.SisCidade", name: "IX_SisEstadoId", newName: "IX_EstadoId");
            RenameIndex(table: "dbo.SisCep", name: "IX_SisTipoLogradouroId", newName: "IX_TipoLogradouroId");
            RenameIndex(table: "dbo.SisCep", name: "IX_SisPaisId", newName: "IX_PaisId");
            RenameIndex(table: "dbo.SisCep", name: "IX_SisEstadoId", newName: "IX_EstadoId");
            RenameIndex(table: "dbo.SisCep", name: "IX_SisCidadeId", newName: "IX_CidadeId");
            RenameIndex(table: "dbo.SisConvenio", name: "IX_SisEstadoCobrancaId", newName: "IX_EstadoCobrancaId");
            RenameIndex(table: "dbo.SisConvenio", name: "IX_SisCidadeCobrancaId", newName: "IX_CidadeCobrancaId");
            RenameIndex(table: "dbo.SisConvenio", name: "IX_SisTipoLogradouroCobrancaId", newName: "IX_TipoLogradouroCobrancaId");
            RenameIndex(table: "dbo.SisConvenio", name: "IX_SisCepCobrancaId", newName: "IX_CepCobrancaId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_SisNacionalidadeResponsavelId", newName: "IX_NacionalidadeResponsavelId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_SisServicoMedicoPrestadoId", newName: "IX_ServicoMedicoPrestadoId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_AssMotivoAltaId", newName: "IX_MotivoAltaId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_SisLeitoId", newName: "IX_LeitoId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_SisGuiaId", newName: "IX_GuiaId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_SisAtendimentoTipoId", newName: "IX_AtendimentoTipoId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_SisUnidadeOrganizacionalId", newName: "IX_UnidadeOrganizacionalId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_SisPlanoId", newName: "IX_PlanoId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_SisConveniolId", newName: "IX_ConvenioId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_SisEmpresaId", newName: "IX_EmpresaId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_SisEspecialidadeId", newName: "IX_EspecialidadeId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_SisMedicoId", newName: "IX_MedicoId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_SisOrigemId", newName: "IX_OrigemId");
            RenameIndex(table: "dbo.Atendimento", name: "IX_SisPacienteId", newName: "IX_PacienteId");
            RenameIndex(table: "dbo.AssAdmissaoMedica", name: "IX_SisUnidadeOrganizacionalId", newName: "IX_UnidadeOrganizacionalId");
            RenameColumn(table: "dbo.SisPaciente", name: "SisTipoSanguineoId", newName: "TipoSanguineoId");
            RenameColumn(table: "dbo.AssSolicitacaoExame", name: "AssPrescricaoId", newName: "PrescricaoId");
            RenameColumn(table: "dbo.AssSolicitacaoExame", name: "SisMedicoSolicitanteId", newName: "MedicoSolicitanteId");
            RenameColumn(table: "dbo.AssSolicitacaoExame", name: "SisUnidadeOrganizacionalId", newName: "UnidadeOrganizacionalId");
            RenameColumn(table: "dbo.AssSolicitacaoExame", name: "SisLeitoId", newName: "LeitoId");
            RenameColumn(table: "dbo.AssSolicitacaoExame", name: "SisOrigemId", newName: "OrigemId");
            RenameColumn(table: "dbo.AssSolicitacaoExameItem", name: "LabKitExameId", newName: "KitExameId");
            RenameColumn(table: "dbo.AssSolicitacaoExameItem", name: "LabMaterialId", newName: "MaterialId");
            RenameColumn(table: "dbo.AssSolicitacaoExameItem", name: "FatItemId", newName: "FaturamentoItemId");
            RenameColumn(table: "dbo.AssSolicitacaoExameItem", name: "AssSolicitacaoExameId", newName: "SolicitacaoExameId");
            RenameColumn(table: "dbo.AssFormTipoResposta", name: "EstUnidadeId", newName: "UnidadeId");
            RenameColumn(table: "dbo.AssEvolucaoMedica", name: "SisFormRespostaId", newName: "FormRespostaId");
            RenameColumn(table: "dbo.AssAtestado", name: "AssModeloAtestadoId", newName: "ModeloAtestadoId");
            RenameColumn(table: "dbo.AssAtestado", name: "AssTipoAtestadoId", newName: "TipoAtestadoId");
            RenameColumn(table: "dbo.AssAtestado", name: "SisPacienteId", newName: "PacienteId");
            RenameColumn(table: "dbo.AssAtestado", name: "SisMedicoId", newName: "MedicoId");
            RenameColumn(table: "dbo.AssistencialAtendimento", name: "SisUnidadeOrganizacionalId", newName: "UnidadeOrganizacionalId");
            RenameColumn(table: "dbo.AssistencialAtendimento", name: "SisEmpresaId", newName: "EmpresaId");
            RenameColumn(table: "dbo.AssistencialAtendimento", name: "SisConvenioId", newName: "ConvenioId");
            RenameColumn(table: "dbo.AssistencialAtendimento", name: "SisMedicoId", newName: "MedicoId");
            RenameColumn(table: "dbo.AssistencialAtendimento", name: "SisPacienteId", newName: "PacienteId");
            RenameColumn(table: "dbo.SisPrestador", name: "SisUserId", newName: "UserId");
            RenameColumn(table: "dbo.SisPrestador", name: "SisConselhoId", newName: "ConselhoId");
            RenameColumn(table: "dbo.SisPrestador", name: "SisTipoPrestadorId", newName: "TipoPrestadorId");
            RenameColumn(table: "dbo.SisPrestador", name: "SisLogradouroComercialId", newName: "TipoLogradouroComercialId");
            RenameColumn(table: "dbo.SisPrestador", name: "SisCepComercialId", newName: "CepComercialId");
            RenameColumn(table: "dbo.SisPrestador", name: "SisParticipacaoId", newName: "TipoParticipacaoId");
            RenameColumn(table: "dbo.SisPrestador", name: "SisTipoVinculoEmpregaticioId", newName: "TipoVinculoEmpregaticioId");
            RenameColumn(table: "dbo.ServicoMedicoPrestado", name: "SisEspecialidadeId", newName: "EspecialidadeId");
            RenameColumn(table: "dbo.AssParecerEspecialistaResposta", name: "SisMedicoRespondenteId", newName: "MedicoRespondenteId");
            RenameColumn(table: "dbo.AssParecerEspecialistaResposta", name: "AssParecerEspecialistaSolicitacaoId", newName: "ParecerEspecialistaSolicitacaoId");
            RenameColumn(table: "dbo.AssParecerEspecialista", name: "SisEspecialidadeSolicitadaId", newName: "EspecialidadeSolicitadaId");
            RenameColumn(table: "dbo.AssParecerEspecialista", name: "SisMedicoSolicitadoId", newName: "MedicoSolicitadoId");
            RenameColumn(table: "dbo.AssParecerEspecialista", name: "SisMedicoSolicitanteId", newName: "MedicoSolicitanteId");
            RenameColumn(table: "dbo.SisOrigem", name: "SisUnidadeOrganizacionalId", newName: "UnidadeOrganizacionalId");
            RenameColumn(table: "dbo.AssMotivoAlta", name: "AssMotivoAltaTipoAltaId", newName: "MotivoAltaTipoAltaId");
            RenameColumn(table: "dbo.SisLeito", name: "SisLeitoStatusId", newName: "LeitoStatusId");
            RenameColumn(table: "dbo.SisLeito", name: "SisTabelaItemTissId", newName: "TabelaItemTissId");
            RenameColumn(table: "dbo.SisLeito", name: "SisTipoAcomodacaoId", newName: "TipoAcomodacaoId");
            RenameColumn(table: "dbo.SisLeito", name: "SisUnidadeOrganizacionalId", newName: "UnidadeOrganizacionalId");
            RenameColumn(table: "dbo.SisGuia", name: "SisOriginariaId", newName: "OriginariaId");
            RenameColumn(table: "dbo.SisMedicoEspecialidade", name: "SisEspecialidadeId", newName: "EspecialidadeId");
            RenameColumn(table: "dbo.SisMedicoEspecialidade", name: "SisMedicoId", newName: "MedicoId");
            RenameColumn(table: "dbo.SisPlano", name: "SisConvenioId", newName: "ConvenioId");
            RenameColumn(table: "dbo.SisUnidadeOrganizacional", name: "SisOrganizationUnitId", newName: "OrganizationUnitId");
            RenameColumn(table: "dbo.SisUnidadeOrganizacional", name: "AteUnidadeInternacaoTipoId", newName: "UnidadeInternacaoTipoId");
            RenameColumn(table: "dbo.SisEmpresa", name: "SisPlanoId", newName: "PlanoId");
            RenameColumn(table: "dbo.SisEmpresa", name: "SisConvenioId", newName: "ConvenioId");
            RenameColumn(table: "dbo.SisEstado", name: "SisPaisId", newName: "PaisId");
            RenameColumn(table: "dbo.SisCidade", name: "SisEstadoId", newName: "EstadoId");
            RenameColumn(table: "dbo.SisCep", name: "SisTipoLogradouroId", newName: "TipoLogradouroId");
            RenameColumn(table: "dbo.SisCep", name: "SisPaisId", newName: "PaisId");
            RenameColumn(table: "dbo.SisCep", name: "SisEstadoId", newName: "EstadoId");
            RenameColumn(table: "dbo.SisCep", name: "SisCidadeId", newName: "CidadeId");
            RenameColumn(table: "dbo.SisConvenio", name: "SisEstadoCobrancaId", newName: "EstadoCobrancaId");
            RenameColumn(table: "dbo.SisConvenio", name: "SisCidadeCobrancaId", newName: "CidadeCobrancaId");
            RenameColumn(table: "dbo.SisConvenio", name: "SisTipoLogradouroCobrancaId", newName: "TipoLogradouroCobrancaId");
            RenameColumn(table: "dbo.SisConvenio", name: "SisCepCobrancaId", newName: "CepCobrancaId");
            RenameColumn(table: "dbo.Atendimento", name: "SisNacionalidadeResponsavelId", newName: "NacionalidadeResponsavelId");
            RenameColumn(table: "dbo.Atendimento", name: "SisServicoMedicoPrestadoId", newName: "ServicoMedicoPrestadoId");
            RenameColumn(table: "dbo.Atendimento", name: "AssMotivoAltaId", newName: "MotivoAltaId");
            RenameColumn(table: "dbo.Atendimento", name: "SisLeitoId", newName: "LeitoId");
            RenameColumn(table: "dbo.Atendimento", name: "SisGuiaId", newName: "GuiaId");
            RenameColumn(table: "dbo.Atendimento", name: "SisAtendimentoTipoId", newName: "AtendimentoTipoId");
            RenameColumn(table: "dbo.Atendimento", name: "SisUnidadeOrganizacionalId", newName: "UnidadeOrganizacionalId");
            RenameColumn(table: "dbo.Atendimento", name: "SisPlanoId", newName: "PlanoId");
            RenameColumn(table: "dbo.Atendimento", name: "SisConveniolId", newName: "ConvenioId");
            RenameColumn(table: "dbo.Atendimento", name: "SisEmpresaId", newName: "EmpresaId");
            RenameColumn(table: "dbo.Atendimento", name: "SisEspecialidadeId", newName: "EspecialidadeId");
            RenameColumn(table: "dbo.Atendimento", name: "SisMedicoId", newName: "MedicoId");
            RenameColumn(table: "dbo.Atendimento", name: "SisOrigemId", newName: "OrigemId");
            RenameColumn(table: "dbo.Atendimento", name: "SisPacienteId", newName: "PacienteId");
            RenameColumn(table: "dbo.AssAdmissaoMedica", name: "SisUnidadeOrganizacionalId", newName: "UnidadeOrganizacionalId");
            AddForeignKey("dbo.SisPrestador", "TipoLogradouroId", "dbo.TipoLogradouro", "Id");
            RenameTable(name: "dbo.SisUltimoId", newName: "UltimoId");
            RenameTable(name: "dbo.SisTipoTelefone", newName: "TipoTelefone");
            RenameTable(name: "dbo.SisSexo", newName: "Sexo");
            RenameTable(name: "dbo.SisReligiao", newName: "Religiao");
            RenameTable(name: "dbo.AssModeloAtestado", newName: "ModeloAtestado");
            RenameTable(name: "dbo.SisGuiaTipo", newName: "GuiaTipo");
            RenameTable(name: "dbo.AssTipoControle", newName: "TipoControle");
            RenameTable(name: "dbo.AssPrescricaoItem", newName: "ItemPrescricao");
            RenameTable(name: "dbo.SisFavorito", newName: "Favorito");
            RenameTable(name: "dbo.SisTurno", newName: "Turno");
            RenameTable(name: "dbo.AssEvolucaoMedica", newName: "EvolucaoMedica");
            RenameTable(name: "dbo.SisEstadoCivil", newName: "EstadoCivil");
            RenameTable(name: "dbo.SisEscolaridade", newName: "Escolaridade");
            RenameTable(name: "dbo.AssTipoAtestado", newName: "TipoAtestado");
            RenameTable(name: "dbo.AssAtestado", newName: "Atestado");
            RenameTable(name: "dbo.SisTipoPrestador", newName: "TipoPrestador");
            RenameTable(name: "dbo.SisTipoParticipacao", newName: "TipoParticipacao");
            RenameTable(name: "dbo.SisConselho", newName: "Conselho");
            RenameTable(name: "dbo.SisPrestador", newName: "Prestador");
            RenameTable(name: "dbo.SisFormRowConfig", newName: "RowConfig");
            RenameTable(name: "dbo.SisFormConfig", newName: "FormConfig");
            RenameTable(name: "dbo.SisFormColMultiOption", newName: "ColMultiOption");
            RenameTable(name: "dbo.SisFormColConfig", newName: "ColConfig");
            RenameTable(name: "dbo.SisFormData", newName: "FormData");
            RenameTable(name: "dbo.SisFormResposta", newName: "FormResposta");
            RenameTable(name: "dbo.AssParecerEspecialistaResposta", newName: "ParecerEspecialistaResposta");
            RenameTable(name: "dbo.AssParecerEspecialista", newName: "ParecerEspecialista");
            RenameTable(name: "dbo.SisTipoSanguineo", newName: "TipoSanguineo");
            RenameTable(name: "dbo.SisPaciente", newName: "Paciente");
            RenameTable(name: "dbo.SisOrigem", newName: "Origem");
            RenameTable(name: "dbo.AssMotivoAltaTipoAlta", newName: "MotivoAltaTipoAlta");
            RenameTable(name: "dbo.AssMotivoAlta", newName: "MotivoAlta");
            RenameTable(name: "dbo.SisTipoAcomodacao", newName: "TipoAcomodacao");
            RenameTable(name: "dbo.SisVersaoTiss", newName: "VersaoTiss");
            RenameTable(name: "dbo.SisTabelaDominioVersaoTiss", newName: "TabelaDominioVersaoTiss");
            RenameTable(name: "dbo.SisTipoTabelaDominio", newName: "TipoTabelaDominio");
            RenameTable(name: "dbo.SisGrupoTipoTabelaDominio", newName: "GrupoTipoTabelaDominio");
            RenameTable(name: "dbo.SisTabelaDominio", newName: "TabelaDominio");
            RenameTable(name: "dbo.SisLeito", newName: "Leito");
            RenameTable(name: "dbo.SisGuia", newName: "Guia");
            RenameTable(name: "dbo.SisMedico", newName: "Medico");
            RenameTable(name: "dbo.SisMedicoEspecialidade", newName: "MedicoEspecialidade");
            RenameTable(name: "dbo.SisEspecialidade", newName: "Especialidade");
            RenameTable(name: "dbo.SisPlano", newName: "Plano");
            RenameTable(name: "dbo.AteUnidadeInternacaoTipo", newName: "UnidadeInternacaoTipo");
            RenameTable(name: "dbo.SisUnidadeOrganizacional", newName: "UnidadeOrganizacional");
            RenameTable(name: "dbo.SisEmpresa", newName: "Empresa");
            RenameTable(name: "dbo.SisProfissao", newName: "Profissao");
            RenameTable(name: "dbo.SisNaturalidade", newName: "Naturalidade");
            RenameTable(name: "dbo.SisNacionalidade", newName: "Nacionalidade");
            RenameTable(name: "dbo.SisTipoLogradouro", newName: "TipoLogradouro");
            RenameTable(name: "dbo.SisPais", newName: "Pais");
            RenameTable(name: "dbo.SisEstado", newName: "Estado");
            RenameTable(name: "dbo.SisCidade", newName: "Cidade");
            RenameTable(name: "dbo.SisCep", newName: "Cep");
            RenameTable(name: "dbo.SisConvenio", newName: "Convenio");
            RenameTable(name: "dbo.AteTipoAtendimento", newName: "TipoAtendimento");
            RenameTable(name: "dbo.AssAdmissaoMedica", newName: "AdmissaoMedica");
        }
    }
}
