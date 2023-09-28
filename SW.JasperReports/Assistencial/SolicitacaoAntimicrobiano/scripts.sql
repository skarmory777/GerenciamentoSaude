USE [AMERICAN]
GO

/****** Object:  UserDefinedFunction [dbo].[RptSolicitacaoAntiMicrobiano]    Script Date: 12/9/2020 3:48:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[RptSolicitacaoAntiMicrobiano] (@solicitacaoAntiMicrobianoId bigint)
RETURNS TABLE  
AS 
RETURN ( SELECT 
	ATE.Codigo AS CodAtendimento,
	CONVERT(varchar, SA.DataSolicitacao, 103) AS DataSolicitacao,
	CONVERT(varchar, SA.DataMaximaTempoProvavel, 103) AS DataMaximaTempoProvavel,
	SA.TempoProvavelUso,
	SA.TipoInfeccao,
	SA.OutrasIndicacoes,
	SA.OutrosResultados,
	SA.Descricao,
	LEITO.Descricao AS Leito,
	PAC.CodigoPaciente, 
	dbo.InitCap(PAC.NomeCompleto) AS Paciente, 
	CONCAT(CONVERT(varchar, PPA.Nascimento, 103),' - ', [dbo].CalcIdade(PPA.Nascimento)) AS PacienteNascimento,
	dbo.InitCap(MED.NomeCompleto) AS Medico,
	Med.NumeroConselho  AS NumeroConselho,
	dbo.InitCap(ORG.Descricao) AS UnidadeOrganizacional
FROM     
	dbo.AssSolicitacaoAntimicrobianos AS SA WITH (NOLOCK)
	INNER JOIN dbo.AteAtendimento AS ATE WITH (NOLOCK) ON SA.AteAtendimentoId = ATE.Id LEFT JOIN
    dbo.SisPaciente AS PAC WITH (NOLOCK) ON PAC.Id = ATE.SisPacienteId LEFT JOIN
    dbo.SisPessoa AS PPA WITH (NOLOCK) ON PPA.Id = PAC.SisPessoaId LEFT JOIN
    dbo.SisMedico AS MED WITH (NOLOCK) ON MED.Id = SA.SisMedicoId LEFT JOIN
    dbo.SisPessoa AS PME WITH (NOLOCK) ON PME.Id = MED.SisPessoaId LEFT JOIN
    dbo.SisUnidadeOrganizacional AS ORG WITH (NOLOCK) ON ORG.Id = ATE.SisUnidadeOrganizacionalId LEFT  JOIN
    dbo.AteLeito AS LEITO WITH (NOLOCK) ON LEITO.Id = ATE.AteLeitoId
WHERE (Sa.Id = @solicitacaoAntiMicrobianoId))
GO

USE [AMERICAN]
GO

/****** Object:  UserDefinedFunction [dbo].[RptSolicitacaoAntiMicrobianoDetalhamentoCulturaResultado]    Script Date: 12/9/2020 3:48:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[RptSolicitacaoAntiMicrobianoDetalhamentoCulturaResultado] (@solicitacaoAntiMicrobianoId bigint)
RETURNS TABLE  
AS 
RETURN ( SELECT DISTINCT SAC.Id AS CulturaId, SAC.DataCultura,CROSSAPPLY.* 

FROM  AssSolicitacaoAntimicrobianosCulturas SAC INNER JOIN AssSolicitacaoAntimicrobianosResultados SAR ON SAR.CulturaId = SAC.Id

CROSS APPLY (
SELECT TSARCROSS.Id AS TipoId, TSARCROSS.Descricao, (CASE WHEN SARCROSS.Id IS NULL THEN 0 ELSE 1 END )Checked
FROM AssTipoSolicitacaoAntimicrobianosResultados AS TSARCROSS
LEFT JOIN AssSolicitacaoAntimicrobianosResultados SARCROSS 
ON SARCROSS.TipoSolicitacaoAntimicrobianosResultadoId = TSARCROSS.Id AND SARCROSS.CulturaId = SAR.CulturaId

) AS CROSSAPPLY
WHERE SAC.SolicitacaoAntimicrobianoId = @solicitacaoAntiMicrobianoId
)
GO

USE [AMERICAN]
GO

/****** Object:  UserDefinedFunction [dbo].[RptSolicitacaoAntiMicrobianoDetalhamentoIndicacao]    Script Date: 12/9/2020 3:48:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[RptSolicitacaoAntiMicrobianoDetalhamentoIndicacao] (@solicitacaoAntiMicrobianoId bigint)
RETURNS TABLE  
AS 
RETURN ( SELECT TSAI.Id, TSAI.Descricao, (CASE WHEN SAI.Id IS NULL THEN 0 ELSE 1 END )Checked
FROM AssTipoSolicitacaoAntimicrobianosIndicacoes AS TSAI
LEFT JOIN AssSolicitacaoAntimicrobianosIndicacoes SAI ON SAI.TipoSolicitacaoAntimicrobianosIndicacaoId = TSAI.Id AND SAI.SolicitacaoAntimicrobianoId = @solicitacaoAntiMicrobianoId )
GO






