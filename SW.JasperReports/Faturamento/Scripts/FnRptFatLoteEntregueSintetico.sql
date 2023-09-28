IF EXISTS (SELECT sysobjects.id
           FROM sysobjects
		   WHERE sysobjects.name = 'FnRptFatLoteEntregueSintetico')
  DROP FUNCTION FnRptFatLoteEntregueSintetico
go

CREATE FUNCTION FnRptFatLoteEntregueSintetico
(
  @DataInicial datetime2,
  @DataFinal datetime2,
  @CodigoLote nvarchar,
  @Convenio nvarchar,
  @Local int
)
RETURNS TABLE 
AS
RETURN 
(
 SELECT			Conv.NomeFantasia						Convenio,
				DataEntrega, 
				CodEntregaLote							CodigoLote, 
				DataInicial								PeriodoInicial, 
				DataFinal								PeriodoFinal,
				FORMAT(ValorFatura, 'c', 'pt-br')		ValorLiquido,
				FORMAT(ValorTaxas, 'c', 'pt-br')		ValorTaxas,
				FORMAT(ValorFatura + ValorTaxas,'c', 'pt-br')	ValorTotal,
				CASE WHEN IsInternacao = 1 THEN 'INT' ELSE 'AMB' END LOCAL
FROM FatEntregaLote Lote
INNER JOIN SisConvenio Conv ON Lote.SisConvenioId = Conv.Id
WHERE Desativado = 0
AND ((@DataInicial IS NULL) OR (Lote.DataInicial = @DataInicial))
AND ((@DataFinal IS NULL) OR (Lote.DataFinal = @DataFinal))
AND ((@CodigoLote IS NULL) OR (Lote.CodEntregaLote = @CodigoLote))
AND ((@Convenio IS NULL) OR (Conv.NomeFantasia LIKE '%' + @Convenio + '%'))
)
go