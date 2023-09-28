IF EXISTS (SELECT sysobjects.id
           FROM sysobjects
		   WHERE sysobjects.name = 'FnRptMapaOcupacao')
  DROP FUNCTION FnRptMapaOcupacao
go

CREATE FUNCTION FnRptMapaOcupacao()
RETURNS TABLE 
AS
RETURN 
SELECT		DISTINCT 
			Unit.Descricao										AS Local,
			L.Descricao											AS Leito,
			A.Id												AS CodigoPaciente,
			P.NomeCompleto										AS Paciente, 
			(DATENAME (YEAR, GETDATE())) - YEAR(P.Nascimento)	AS Idade,
			A.DataRegistro										AS DataInternacao,
			Med.NomeCompleto									AS MedicoResponsavel,
			C.NomeFantasia										AS Convenio,
			AC.Descricao										AS LeitoContratual,
			O.Descricao											AS Origem,
			DiasAutorizacao										AS DiasAutorizados
FROM AteAtendimento A	(NOLOCK) 
--INNER JOIN AssPrescricaoMedica PM (NOLOCK) ON A.Id = PM.AteAtendimentoId
INNER JOIN AteLeito L	(NOLOCK) ON A.AteLeitoId = L.Id
INNER JOIN SisUnidadeOrganizacional Unit ON L.SisUnidadeOrganizacionalId = Unit.Id
INNER JOIN SisPaciente P (NOLOCK) ON A.SisPacienteId = P.Id
INNER JOIN SisMedico Med (NOLOCK) ON A.SisMedicoId = Med.Id
INNER JOIN SisConvenio C (NOLOCK) ON A.SisConveniolId = C.Id
INNER JOIN SisTipoAcomodacao AC ON A.SisTipoAcomodacaoId = AC.Id
INNER JOIN SisOrigem O ON A.SisOrigemId = O.Id
WHERE A.DataAlta IS NULL 
and a.IsDeleted = 0

UNION

SELECT 		Unit.Descricao							AS Local,
			L.Descricao								AS Leito,
			null									AS CodigoPaciente,
			LS.Descricao							AS Paciente, 
			null           	                        AS Idade,
			getdate()								AS DataInternacao,
			null									AS MedicoResponsavel,
			null									AS Convenio,
			null									AS LeitoContratual,
			null									AS Origem,
			0										AS DiasAutorizados
FROM AteLeito L
INNER JOIN LeitoStatus LS               ON L.AteLeitoStatusId = LS.Id
LEFT JOIN SisUnidadeOrganizacional Unit ON L.SisUnidadeOrganizacionalId = Unit.Id
LEFT JOIN SisTipoAcomodacao AC          ON L.SisTipoAcomodacaoId = AC.Id
WHERE LS.Id <> 2
AND L.IsDeleted = 0
go