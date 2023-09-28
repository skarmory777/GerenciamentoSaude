USE [AMERICAN]
GO

/****** Object:  UserDefinedFunction [dbo].[FnRptDevolucaoEstoque]    Script Date: 06/07/2021 16:38:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[FnRptDevolucaoEstoque](@estoqueId int, @DATAINICIAL DATETIME2, @DATAFINAL DATETIME2)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ate.Id									  AS IdAtendimento,
		   ate.codigo                                 AS CodigoAtendimento,
		   CONCAT(
			CONVERT(varchar,ate.dataregistro,103),
			' ',
			CONVERT(varchar,ate.dataregistro,108)
			)										  AS DataAtendimento,		   							
           CONCAT(
			CONVERT(varchar,ate.DataAlta,103),
			' ',
			CONVERT(varchar,ate.DataAlta,108)
			)										  AS DataAlta,
		   Estoque.Descricao						  AS Estoque,
		   ( CASE
			   WHEN isinternacao = 1 THEN 'Interna��o'
			   ELSE 'Emerg�ncia'
			 END )                                    AS Emergencia_Internacao,
           pac.Id                                     AS IdPaciente,   
		   pac.nomecompleto                           AS Paciente,
		   CONCAT(
			CONVERT(varchar,movitem.creationtime,103),
			' ',
			CONVERT(varchar,movitem.creationtime,108)
			)										  AS DataMovimento,
		   movitem.Id                                 AS IdItemMov,
		   prod.descricao                             AS Produto,
		   Lote.lote                                  AS Lote,
		   lote.Validade							  AS Validade,
		   MovItem.quantidade                         AS Quantidade,
		   ( abpusers.NAME + ' ' + abpusers.surname ) AS Usu�rio,
		   mov.Observacao                             AS Observacao
	FROM   estoquemovimentoitem AS MovItem
		   LEFT JOIN estoquemovimento AS Mov
				  ON movitem.movimentoid = mov.id
		   LEFT JOIN Est_Estoque AS Estoque 
				  ON  Estoque.Id = Mov.EstoqueId
		   LEFT JOIN ateatendimento AS Ate
				  ON mov.atendimentoid = ate.id
		   LEFT JOIN sispaciente AS Pac
				  ON ate.sispacienteid = pac.id
		   LEFT JOIN sispessoa AS Pessoa
				  ON Pac.sispessoaid = Pessoa.id
		   LEFT JOIN est_produto AS Prod
				  ON movitem.produtoid = prod.id
		   LEFT JOIN estoquemovimentolotevalidade AS MovLote
				  ON movitem.id = movlote.estoquemovimentoitemid
		   LEFT JOIN lotevalidade AS Lote
				  ON movlote.lotevalidadeid = lote.id
		   LEFT JOIN abpusers
				  ON movitem.creatoruserid = abpusers.id
	WHERE  mov.esttipooperacaoid = 4 
		   AND ( MovItem.creationtime >= @DATAINICIAL AND movitem.creationtime <= @DATAFINAL )
		   AND ( @estoqueId = 0 OR Mov.EstoqueId = @estoqueId)
)

GO


