USE [AMERICAN]
GO

/****** Object:  UserDefinedFunction [dbo].[RptSolicitacaoSaidaDetalhamento]    Script Date: 4/6/2020 4:29:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [dbo].[RptSolicitacaoSaidaDetalhamento] (@solicitacaoId bigint)
RETURNS TABLE  
AS 
RETURN ( 

SELECT 
ROW_NUMBER() OVER(ORDER BY SolicitacaoItem.Id) rowIndex,
SolicitacaoItem.Id,
Produto.Codigo AS CodigoProduto,
UPPER(Produto.DescricaoResumida) AS DescricaoResumida,
UPPER(Unidade.Descricao) AS UnidadeDescricao,
SolicitacaoItem.Quantidade 

FROM EstSolicitacaoItem AS  SolicitacaoItem WITH (NOLOCK) 
INNER JOIN Est_Produto  AS Produto  WITH (NOLOCK) ON SolicitacaoItem.ProdutoId =  Produto.Id
INNER JOIN Est_ProdutoUnidade  AS ProdutoUnidade  WITH (NOLOCK) ON SolicitacaoItem.ProdutoUnidadeId =  ProdutoUnidade.Id
INNER JOIN Est_Unidade  AS Unidade  WITH (NOLOCK) ON ProdutoUnidade.UnidadeId = Unidade.Id
WHERE SolicitacaoId = @solicitacaoId
)
GO


USE [AMERICAN]
GO

/****** Object:  UserDefinedFunction [dbo].[RptSolicitacaoSaida]    Script Date: 4/6/2020 4:29:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [dbo].[RptSolicitacaoSaida] (@preMovimentoId bigint)
RETURNS TABLE  
AS 
RETURN ( 

SELECT 
 PreMovimento.Id,
 PreMovimento.Documento AS NumDocumento,
 FORMAT(PreMovimento.Emissao,'dd/MM/yyyy HH:mm:ss') AS DataEmissao,
 FORMAT(PreMovimento.Movimento,'dd/MM/yyyy HH:mm:ss') AS DataMovimento,
 TipoMovimento.Descricao AS TipoMovimento,
 (CASE 
 WHEN TipoMovimento.Id = 2 THEN
 'Baixa de Solicitação para Setor'
 WHEN TipoMovimento.Id = 3 THEN
 'Baixa de solicitação para Paciente'
 WHEN TipoMovimento.Id = 4 THEN
 'Baixa de Solicitação por perda'
 WHEN TipoMovimento.Id = 5 THEN
 'Baixa de Solicitação para Gasto de Sala'
 ELSE '' 
 END ) AS TituloBaixa,
 (CASE 
 WHEN TipoMovimento.Id = 2 THEN
 'Solicitação para Setor'
 WHEN TipoMovimento.Id = 3 THEN
 'Solicitação para Paciente'
 WHEN TipoMovimento.Id = 4 THEN
 'Solicitação por perda'
 WHEN TipoMovimento.Id = 5 THEN
 'Solicitação para Gasto de Sala'
 ELSE '' 
 END ) AS TituloSolicitacao,
 CASE WHEN Atendimento.IsAmbulatorioEmergencia = 1 THEN 'Emergência'
 WHEN Atendimento.IsInternacao = 1 THEN 'Internação'
 ELSE '' END AS TipoAtendimento,
 Leito.Descricao AS Leito,
 CentroCusto.Descricao AS CentroCusto,
 Estoque.Descricao AS EstoqueDescricao,
 PAC.CodigoPaciente, 
 dbo.InitCap(PPA.NomeCompleto) AS Paciente, 
 CONCAT(CONVERT(varchar, PPA.Nascimento, 103),' - ', [dbo].CalcIdade(PPA.Nascimento)) AS PacienteNascimento,
 dbo.InitCap(ISNULL(PCO.NomeFantasia, PCO.NomeCompleto)) AS Convenio, 
 PLA.Descricao AS Plano, 
 dbo.InitCap(MED.NomeCompleto) AS Medico,
 Med.NumeroConselho  AS NumeroConselho,
 PreMovimento.Observacao AS Observacao,
 CONCAT(UsuarioCriador.Name,' ',UsuarioCriador.Surname) AS UsuarioCriador
FROM 
EstoquePreMovimento As PreMovimento WITH (NOLOCK)
LEFT JOIN EstTipoMovimento AS TipoMovimento WITH (NOLOCK) ON PreMovimento.EstTipoMovimentoId = TipoMovimento.Id AND TipoMovimento.IsDeleted = 0
LEFT JOIN CentroCusto AS CentroCusto WITH (NOLOCK) ON PreMovimento.CentroCustoId = CentroCusto.Id AND CentroCusto.IsDeleted = 0
LEFT JOIN Est_Estoque AS Estoque WITH (NOLOCK) ON PreMovimento.EstoqueId = Estoque.Id AND Estoque.IsDeleted = 0
LEFT OUTER JOIN AteAtendimento AS Atendimento WITH (NOLOCK) ON PreMovimento.AtendimentoId = Atendimento.Id AND Atendimento.IsDeleted = 0
LEFT OUTER JOIN AteLeito AS Leito  WITH (NOLOCK) ON Atendimento.AteLeitoId = Leito.Id AND Leito.IsDeleted = 0
LEFT OUTER JOIN dbo.SisPaciente AS PAC WITH (NOLOCK) ON Atendimento.SisPacienteId = PAC.Id AND PAC.IsDeleted = 0
LEFT OUTER JOIN dbo.SisPessoa AS PPA WITH (NOLOCK) ON PPA.Id = PAC.SisPessoaId  AND PPA.IsDeleted = 0
LEFT OUTER JOIN dbo.SisMedico AS MED WITH (NOLOCK) ON MED.Id = PreMovimento.MedicoSolcitanteId  AND MED.IsDeleted = 0
LEFT OUTER JOIN dbo.SisPessoa AS PME WITH (NOLOCK) ON PME.Id = MED.SisPessoaId  AND PME.IsDeleted = 0
LEFT OUTER JOIN dbo.SisConvenio AS CON WITH (NOLOCK) ON CON.Id = Atendimento.SisConveniolId  AND CON.IsDeleted = 0
LEFT OUTER JOIN dbo.SisPessoa AS PCO WITH (NOLOCK) ON CON.SisPessoaId = PCO.Id  AND PCO.IsDeleted = 0
LEFT OUTER JOIN dbo.SisPlano AS PLA WITH (NOLOCK) ON PLA.Id = Atendimento.SisPlanoId AND PLA.IsDeleted = 0
LEFT OUTER JOIN AbpUsers AS UsuarioCriador WITH (NOLOCK) ON PreMovimento.CreatorUserId = UsuarioCriador.Id AND UsuarioCriador.IsDeleted = 0

where (PreMovimento.Id = @preMovimentoId)
)
GO


