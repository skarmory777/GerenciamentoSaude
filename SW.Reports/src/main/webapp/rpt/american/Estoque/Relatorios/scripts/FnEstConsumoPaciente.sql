USE [AMERICAN]
GO

/****** Object:  UserDefinedFunction [dbo].[FnEstConsumoPaciente]    Script Date: 06/07/2021 16:38:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[FnEstConsumoPaciente](@PACIENTEID BIGINT, @DATAINICIAL DATETIME2, @DATAFINAL DATETIME2)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ate.Id									  AS IdAtendimento,
		   ate.codigo                                 AS CodigoAtendimento,
		   ate.dataregistro                           AS DataAtendimento,		   							
           ate.DataAlta                               AS DataAlta,
		   ( CASE
			   WHEN isinternacao = 1 THEN 'Internação'
			   ELSE 'Emergência'
			 END )                                    AS Emergencia_Internacao,
           pac.Id                                     AS IdPaciente,   
		   pac.nomecompleto                           AS Paciente,
		   movitem.creationtime                       AS DataMovimento,
		   movitem.Id                                 AS IdItemMov,
		   prod.descricao                             AS Produto,
		   Lote.lote                                  AS Lote,
		   lote.Validade							  AS Validade,
		   MovItem.quantidade                         AS Quantidade,
		   ( abpusers.NAME + ' ' + abpusers.surname ) AS Usuário
	FROM   estoquemovimentoitem AS MovItem
		   LEFT JOIN estoquemovimento AS Mov
				  ON movitem.movimentoid = mov.id
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
	WHERE  ( mov.esttipomovimentoid = 3
			 AND mov.esttipooperacaoid = 3 )
		   AND ( MovItem.creationtime >= @DATAINICIAL
				 AND movitem.creationtime <= @DATAFINAL )
		   AND pac.id = @PACIENTEID 
		
)
GO


