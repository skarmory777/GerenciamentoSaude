ALTER FUNCTION [dbo].[RptPrescricaoMedica] (@prescricaoMedica bigint)
RETURNS TABLE  
AS 
RETURN ( 
SELECT 
	ATE.Codigo AS CodAtendimento, 
	UPPER(ISNULL(EMP.NomeFantasia, EMP.RazaoSocial)) AS Empresa, 
	ATE.DataRegistro, 
	ATE.DataAlta,
	LEITO.Descricao AS Leito,
	PAC.CodigoPaciente, 
	dbo.InitCap(PAC.NomeCompleto) AS Paciente, 
	CONCAT(CONVERT(varchar, PAC.Nascimento, 103),' - ', [dbo].CalcIdade(PAC.Nascimento)) AS PacienteNascimento,
	dbo.InitCap(ISNULL(PCO.NomeFantasia, PCO.NomeCompleto)) AS Convenio, 
	PLA.Descricao AS Plano, 
	ATE.Matricula, 
	ATE.Titular,  
	dbo.InitCap(MED.NomeCompleto) AS Medico,
	Med.NumeroConselho  AS NumeroConselho,
	dbo.InitCap(ESP.Descricao) AS Especialidade, 
	dbo.InitCap(ORG.Descricao) AS UnidadeOrganizacional, 
	PRM.Id AS CodigoPrescricao,
	PRM.DataPrescricao,
	PacienteAlergias.PacienteAlergia
FROM            
	dbo.AteAtendimento AS ATE WITH (NOLOCK) INNER JOIN
    dbo.SisPaciente AS PAC WITH (NOLOCK) ON PAC.Id = ATE.SisPacienteId LEFT JOIN
	(
	SELECT PacienteAlergias.PacienteId,  STUFF((SELECT ', '  + Alergia
						FROM PacienteAlergias STUFFPacienteAlergias
						WHERE STUFFPacienteAlergias.PacienteId = PacienteAlergias.PacienteId
						FOR XML PATH('')), 1, 2, '') AS PacienteAlergia 
	FROM PacienteAlergias
	GROUP BY PacienteAlergias.PacienteId) AS PacienteAlergias ON PacienteAlergias.PacienteId = PAC.ID INNER  JOIN
		dbo.AssPrescricaoMedica AS PRM WITH (NOLOCK) ON PRM.AteAtendimentoId = ATE.Id LEFT OUTER JOIN
        dbo.SisPessoa AS PPA WITH (NOLOCK) ON PPA.Id = PAC.SisPessoaId INNER JOIN
        dbo.SisMedico AS MED WITH (NOLOCK) ON MED.Id = PRM.SisMedicoId INNER JOIN
        dbo.SisPessoa AS PME WITH (NOLOCK) ON PME.Id = MED.SisPessoaId INNER JOIN
        dbo.SisConvenio AS CON WITH (NOLOCK) ON CON.Id = ATE.SisConveniolId INNER JOIN
        dbo.SisPessoa AS PCO WITH (NOLOCK) ON CON.SisPessoaId = PCO.Id INNER JOIN
        dbo.SisPlano AS PLA WITH (NOLOCK) ON PLA.Id = ATE.SisPlanoId LEFT OUTER JOIN
        dbo.SisEmpresa AS EMP WITH (NOLOCK) ON EMP.Id = ATE.SisEmpresaId LEFT OUTER JOIN
        dbo.FatGuia AS GUI WITH (NOLOCK) ON GUI.Id = ATE.FatGuiaId LEFT OUTER JOIN
        dbo.SisUnidadeOrganizacional AS ORG WITH (NOLOCK) ON ORG.Id = ATE.SisUnidadeOrganizacionalId LEFT OUTER JOIN
        dbo.SisEspecialidade AS ESP WITH (NOLOCK) ON ESP.Id = ATE.SisEspecialidadeId LEFT OUTER JOIN
        
		dbo.AteLeito AS LEITO WITH (NOLOCK) ON LEITO.Id = ATE.AteLeitoId
WHERE        
		(PRM.Id = @prescricaoMedica)
)
GO


CREATE FUNCTION [dbo].[RptPrescricaoMedicaDetalhamento] (@prescricaoMedica bigint, @imprimirResumido bit)
RETURNS TABLE  
AS 
RETURN ( 
SELECT 
DISTINCT
Divisao.Descricao AS Divisao,
PrescricaoItem.Descricao AS PrescricaoItem,
PrescricaoItemResposta.Quantidade As Quantidade,
UnidadeItemResposta.Sigla AS Unidade,
Concat(FormaAplicacaoItemResposta.Descricao,
CASE 
WHEN VelocidadeInfusao.Descricao IS NULL THEN ''
ELSE 
CONCAT(' - ',VelocidadeInfusao.Descricao)
END) AS Aplicacao,
Concat(DiluenteItemResposta.Descricao,
CASE 
WHEN PrescricaoItemResposta.VolumeDiluente IS NULL THEN ''
ELSE 
CONCAT(' - ',PrescricaoItemResposta.VolumeDiluente, 'ml')
END) AS Diluente,
PrescricaoItemResposta.Observacao,
PrescricaoItemHora.Horas AS Aprazamento,
PrescricaoItemStatusItemResposta.Descricao AS Status,
PrescricaoItemResposta.IsAcrescimo AS IsAcrescimo,
(CASE 
WHEN PrescricaoItemResposta.IsSuspenso = 1 THEN 1
WHEN PrescricaoItemResposta.AssPrescricaoItemStatusId = 5 THEN 1
ELSE 0
END) AS IsSuspenso,
(
CASE
WHEN PrescricaoItem.IsNegrito = 1 THEN 1
WHEN Produto.IsNegrito = 1 THEN 1
WHEN Grupo.IsNegrito = 1 THEN 1
WHEN GrupoClasse.IsNegrito = 1 THEN 1
WHEN GrupoSubClasse.IsNegrito = 1 THEN 1
ELSE 0
END) AS IsNegrito,
(
CASE
WHEN PrescricaoItem.IsItalico = 1 THEN 1
WHEN Produto.IsItalico = 1 THEN 1
WHEN Grupo.IsItalico = 1 THEN 1
WHEN GrupoClasse.IsItalico = 1 THEN 1
WHEN GrupoSubClasse.IsItalico = 1 THEN 1
ELSE 0
END) AS IsItalico,
PrescricaoItemResposta.CreationTime,
FrequenciaItemResposta.Descricao AS Frequencia,
SisPessoaMedicoItemResposta.NomeCompleto AS MedicoItemRespostaNomeCompleto,
MedicoItemResposta.NumeroConselho AS MedicoItemRespostaNumeroConselho
FROM            
dbo.AteAtendimento AS ATE WITH (NOLOCK) INNER JOIN
dbo.AssPrescricaoMedica AS PRM WITH (NOLOCK) ON PRM.AteAtendimentoId = ATE.Id  INNER JOIN
dbo.AssPrescricaoItemResposta AS PrescricaoItemResposta WITH (NOLOCK) ON PrescricaoItemResposta.AssPrescricaoMedicaId = PRM.Id LEFT OUTER JOIN
dbo.AssPrescricaoItem AS DiluenteItemResposta ON PrescricaoItemResposta.DiluenteId = DiluenteItemResposta.Id LEFT OUTER JOIN
dbo.AssDivisao AS DivisaoItemResposta ON PrescricaoItemResposta.AssDivisaoId = DivisaoItemResposta.Id LEFT OUTER JOIN
dbo.AssDivisao AS DivisaoPrincipalItemResposta ON DivisaoPrincipalItemResposta.AssDivisaoId = DivisaoItemResposta.Id LEFT OUTER JOIN
dbo.AssTipoPrescricao AS TipoPrescricaoItemResposta ON TipoPrescricaoItemResposta.Id = DivisaoItemResposta.AssTipoPrescricaoId LEFT OUTER JOIN
dbo.AssFormaAplicacao AS FormaAplicacaoItemResposta ON FormaAplicacaoItemResposta.Id = PrescricaoItemResposta.AssFormaAplicacaoId LEFT OUTER JOIN
dbo.AssFrequencia AS FrequenciaItemResposta ON PrescricaoItemResposta.AssFrequenciaId = FrequenciaItemResposta.Id LEFT OUTER JOIN
dbo.SisMedico AS MedicoItemResposta ON PrescricaoItemResposta.SisMedicoId = MedicoItemResposta.Id LEFT OUTER JOIN
dbo.SisPessoa AS SisPessoaMedicoItemResposta ON MedicoItemResposta.SisPessoaId = SisPessoaMedicoItemResposta.Id LEFT OUTER JOIN
dbo.AssPrescricaoItem AS PrescricaoItem ON PrescricaoItemResposta.AssPrescricaoItemId = PrescricaoItem.Id LEFT OUTER JOIN
dbo.AssTipoPrescricao AS TipoPrescricao ON PrescricaoItem.AssTipoPrescricaoId = TipoPrescricao.Id LEFT OUTER JOIN
dbo.AssDivisao AS Divisao ON PrescricaoItem.AssDivisaoId = Divisao.Id LEFT OUTER JOIN
dbo.AssFormaAplicacao AS FormaAplicacao ON PrescricaoItem.AssFormaAplicacaoId = FormaAplicacao.Id LEFT OUTER JOIN
dbo.AssFrequencia AS Frequencia ON PrescricaoItem.AssFrequenciaId = Frequencia.Id LEFT OUTER JOIN
dbo.AssTipoControle AS TipoControle ON PrescricaoItem.AssTipoControleId = TipoControle.Id LEFT OUTER JOIN
dbo.Est_Unidade AS Unidade ON PrescricaoItem.EstUnidadeId = Unidade.Id LEFT OUTER JOIN
dbo.Est_Unidade AS UnidadeRequisicao ON PrescricaoItem.EstUnidadeRequisicaoId = UnidadeRequisicao.Id LEFT OUTER JOIN
dbo.AssVelocidadeInfusao AS VelocidadeInfusao ON PrescricaoItem.AssVelocidadeInfusaoId = VelocidadeInfusao.Id LEFT OUTER JOIN
dbo.Est_Produto AS Produto ON PrescricaoItem.EstProdutoId = Produto.Id LEFT OUTER JOIN
dbo.Est_Grupo AS Grupo On Produto.GrupoId = Grupo.ID LEFT OUTER JOIN
dbo.Est_GrupoClasse AS GrupoClasse On Produto.GrupoClasseId = GrupoClasse.ID LEFT OUTER JOIN
dbo.Est_GrupoSubClasse AS GrupoSubClasse On Produto.GrupoSubClasseId = GrupoSubClasse.ID LEFT OUTER JOIN
dbo.FatItem AS FaturamentoItem ON PrescricaoItem.FatItemId = FaturamentoItem.Id LEFT OUTER JOIN
dbo.AssPrescricaoStatus AS PrescricaoItemStatusItemResposta ON PrescricaoItemResposta.AssPrescricaoItemStatusId = PrescricaoItemStatusItemResposta.Id LEFT OUTER JOIN
dbo.Est_Unidade AS UnidadeItemResposta ON PrescricaoItemResposta.EstUnidadeId = UnidadeItemResposta.Id LEFT OUTER JOIN
dbo.SisUnidadeOrganizacional AS UnidadeOrganizacionalItemResposta ON PrescricaoItemResposta.SisUnidadeOrganizacionalId = UnidadeOrganizacionalItemResposta.Id LEFT OUTER JOIN
dbo.AbpOrganizationUnits AS OrganizationUnitItemResposta ON UnidadeOrganizacionalItemResposta.SisOrganizationUnitId = OrganizationUnitItemResposta.Id LEFT OUTER JOIN
dbo.AteUnidadeInternacaoTipo AS UnidadeInternacaoTipoItemResposta ON UnidadeOrganizacionalItemResposta.AteUnidadeInternacaoTipoId = UnidadeInternacaoTipoItemResposta.Id LEFT OUTER JOIN
dbo.AssVelocidadeInfusao AS VelocidadeInfusaoItemResposta ON PrescricaoItemResposta.AssVelocidadeInfusaoId = VelocidadeInfusaoItemResposta.Id LEFT OUTER JOIN
(
SELECT PrescricaoItemHora.AssPrescricaoItemRespostaId, STUFF((SELECT DISTINCT(', ' + CONVERT(varchar(5),DataMedicamento,108))
FROM AssPrescricaoItemHora AS SubPrescricaoItemHora 
WHERE SubPrescricaoItemHora.AssPrescricaoItemRespostaId = PrescricaoItemHora.AssPrescricaoItemRespostaId FOR XML PATH('')
                     ), 1, 1, '') AS Horas
FROM AssPrescricaoItemHora AS PrescricaoItemHora) AS PrescricaoItemHora ON PrescricaoItemHora.AssPrescricaoItemRespostaId = PrescricaoItemResposta.Id

WHERE        (PRM.Id = @prescricaoMedica)
AND PrescricaoItemResposta.IsDeleted = 0

AND 
(@imprimirResumido = 1 AND (PrescricaoItemResposta.IsAcrescimo = 1 OR PrescricaoItemResposta.IsSuspenso = 1) 
OR @imprimirResumido = 0 AND (PrescricaoItemResposta.IsAcrescimo IN(0,1) OR PrescricaoItemResposta.IsSuspenso IN(0,1)))
)


GO