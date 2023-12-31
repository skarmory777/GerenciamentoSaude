USE [LIPP]
GO
/****** Object:  UserDefinedFunction [dbo].[RptAgendamentoDetalhamento]    Script Date: 14/01/2021 17:23:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER FUNCTION [dbo].[RptAgendamentoDetalhamento](@DATAINICIAL DATETIME2, @DATAFINAL DATETIME2)
RETURNS TABLE 
AS
RETURN 
(
	SELECT 
	AgendamentoConsulta.Id,
	agendamentoConsulta.StatusId,
	AteSalaCirurgica.Descricao Sala, 
	FORMAT(CONVERT(datetime, AgendamentoConsulta.HoraAgendamento), 'HH:mm') Hora, 
	CONVERT(DATE,AgendamentoConsulta.DataAgendamento) DataAgendamento,
	SisPessoa.Descricao Paciente, 
	SisMedico.NomeCompleto Medico, 
	SisMedico.Id medicoid,
	SisConvenio.NomeFantasia Convenio,
	AgendamentoConsulta.Notas, 
	fatitem.Descricao Cirurgias,
	FatItem.id numfatitem,
	AteAgendamentoItemFaturamento.Id ItemFatId,
	AteTipoCirurgia.Descricao TipoAgendamento,
	ContagemAgendamentos.QtdAgendamentos,
	ContagemCirurgias.QtdCirurgias

	from AgendamentoConsulta
	left join AteAgendamentoCirurgico on AgendamentoConsulta.Id = AteAgendamentoCirurgico.AgendamentoConsultaId
	left join AgendamentoSalaCirurgicaDisponibilidades on AteAgendamentoCirurgico.AgendamentoSalaCirurgicaDisponibilidadeId = AgendamentoSalaCirurgicaDisponibilidades.Id
	left join AteSalaCirurgica on AgendamentoSalaCirurgicaDisponibilidades.SalaCirurgicaId = AteSalaCirurgica.Id
	left join SisPaciente on AgendamentoConsulta.PacienteId = SisPaciente.Id
	left join SisPessoa  on SisPaciente.SisPessoaId = SisPessoa.Id
	left join SisMedico on AgendamentoConsulta.MedicoId = SisMedico.Id
	left join SisConvenio on AgendamentoConsulta.ConvenioId = SisConvenio.Id
	left join AteAgendamentoItemFaturamento on AteAgendamentoCirurgico.Id = AteAgendamentoItemFaturamento.AgendamentoCirurgicoId
	left join FatItem on AteAgendamentoItemFaturamento.FaturamentoItemId = FatItem.Id
	left join AteTipoCirurgia on AgendamentoSalaCirurgicaDisponibilidades.TipoCirurgiaId = AteTipoCirurgia.Id
	left join (select DataAgendamento, COUNT(DataAgendamento) QtdAgendamentos from AgendamentoConsulta
			    group by DataAgendamento) AS ContagemAgendamentos on AgendamentoConsulta.DataAgendamento = ContagemAgendamentos.DataAgendamento
	left join (select DataAgendamento, COUNT(DataAgendamento) QtdCirurgias 
				from AgendamentoConsulta
				where StatusId = 6
			    group by DataAgendamento) AS ContagemCirurgias on AgendamentoConsulta.DataAgendamento = ContagemCirurgias.DataAgendamento
	
	WHERE AgendamentoConsulta.DataAgendamento>=@DATAINICIAL AND AgendamentoConsulta.DataAgendamento<=@DATAFINAL
	)
	

	
	

