USE [AMERICAN]
GO

/****** Object:  UserDefinedFunction [dbo].[FnRptPerdaEstoque]    Script Date: 06/07/2021 16:38:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[FnRptPerdaEstoque](@estoqueId int, @DATAINICIAL DATETIME2, @DATAFINAL DATETIME2)
RETURNS TABLE 
AS
RETURN 
(
	SELECT 
		   CONCAT(
			CONVERT(varchar,movitem.creationtime,103),
			' ',
			CONVERT(varchar,movitem.creationtime,108)
			)										  AS DataMovimento,
		   Estoque.Descricao						  AS Estoque,
		   movitem.Id                                 AS IdItemMov,
		   isnull(perda.Descricao,perda2.Descricao)   AS MotivoPerda,		                            
		   prod.descricao                             AS Produto,
		   Lote.lote                                  AS Lote,
		   CONCAT(
			CONVERT(varchar,lote.Validade,103),
			' ',
			CONVERT(varchar,lote.Validade,108)
			)							  AS Validade,
		   MovItem.quantidade                         AS Quantidade,
		   ( abpusers.NAME + ' ' + abpusers.surname ) AS Usuario,
		   mov.Observacao                             AS Observacao
	FROM   estoquemovimentoitem AS MovItem
		   LEFT JOIN estoquemovimento AS Mov
				  ON movitem.movimentoid = mov.id
		   LEFT JOIN est_produto AS Prod
				  ON movitem.produtoid = prod.id
		   LEFT JOIN estoquemovimentolotevalidade AS MovLote
				  ON movitem.id = movlote.estoquemovimentoitemid
		   LEFT JOIN Est_Estoque AS Estoque 
				  ON  Estoque.Id = Mov.EstoqueId
		   LEFT JOIN lotevalidade AS Lote
				  ON movlote.lotevalidadeid = lote.id
		   LEFT JOIN abpusers
				  ON movitem.creatoruserid = abpusers.id
		   LEFT JOIN EstoquePreMovimento AS premov
				  ON mov.EstoquePreMovimentoId = premov.Id
		   LEFT JOIN EstoquePreMovimento AS premovOrigem
		          ON premov.EstoquePreMovimentoParentId = premovOrigem.Id
		   LEFT JOIN MotivoPerdaProduto AS Perda
				  ON premovOrigem.MotivoPerdaProdutoId = Perda.Id
		   LEFT JOIN MotivoPerdaProduto AS Perda2
		          ON premov.MotivoPerdaProdutoId = perda2.Id
	WHERE  ( mov.esttipomovimentoid = 4 AND mov.esttipooperacaoid = 3 )   
			 AND ( MovItem.creationtime >= @DATAINICIAL AND movitem.creationtime <= @DATAFINAL )
			 AND ( @estoqueId = 0 OR Mov.EstoqueId = @estoqueId)
		
		   
		   
)

GO


