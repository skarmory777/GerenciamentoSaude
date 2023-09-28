IF EXISTS (SELECT sysobjects.id
           FROM sysobjects
		   WHERE sysobjects.name = 'FnRptDispensacao')
 DROP FUNCTION FnRptDispensacao
 go

CREATE FUNCTION FnRptDispensacao
(
  @dataInicial DATETIME2,
  @dataFinal DATETIME2,
  @UndOrganizacionalId BigInt
)
RETURNS TABLE 
AS
RETURN 
(
SELECT			DISTINCT
				h.Hora						AS HoraAdm,
				atend.Titular				AS Paciente,
				i.Descricao					AS Medicamento,
				pr.Quantidade				AS DoseAdm,
				u.Descricao					AS UND,
				l.Descricao					AS Leito
		
		FROM AssPrescricaoItemResposta pr
			JOIN AssPrescricaoMedica pm
				ON pr.AssPrescricaoMedicaId = pm.Id
			JOIN AssPrescricaoItem i
				ON pr.AssPrescricaoItemId = i.Id
			JOIN AssPrescricaoItemHora h
				ON h.AssPrescricaoItemRespostaId = pr.Id
			JOIN AteAtendimento atend
				ON pm.AteAtendimentoId = atend.Id
			JOIN AteLeito l
				ON atend.AteLeitoId = l.Id
			JOIN SisUnidadeOrganizacional uo
				ON l.SisUnidadeOrganizacionalId = uo.Id
			JOIN AssFrequencia f
				ON pr.AssFrequenciaId = f.Id
			JOIN Est_Unidade u
				ON pr.EstUnidadeId = u.Id

WHERE pm.IsDeleted = 0
AND pr.IsDeleted = 0
AND i.IsDeleted = 0
AND pm.DataPrescricao BETWEEN @dataInicial AND @dataFinal
AND (@UndOrganizacionalId = 0 OR uo.Id = @UndOrganizacionalId)
)
go