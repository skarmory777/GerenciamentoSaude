//using System.Web.Mvc;

using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.Helpers;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos
{
    using Abp.Application.Services.Dto;
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Abp.Runtime.Session;
    using ClassesAplicacao.Atendimentos;
    using ClassesAplicacao.Configuracoes.Empresas;
    using Dapper;
    using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Enumeradores;
    using SW10.SWMANAGER.MultiTenancy;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RegistroArquivoAppService : SWMANAGERAppServiceBase, IRegistroArquivoAppService
    {
        private readonly IRepository<RegistroArquivo, long> _registroArquivoRepository;

        private readonly TenantManager _tenantManager;

        private readonly IIocManager _iocManager;

        public RegistroArquivoAppService(IRepository<RegistroArquivo, long> registroArquivoRepository, TenantManager tenantManager, IIocManager iocManager)
        {
            _registroArquivoRepository = registroArquivoRepository;
            _tenantManager = tenantManager;
            _iocManager = iocManager;

        }

        public RegistroArquivoDto ObterPorId(long id)
        {
            var registroArquivo = this._registroArquivoRepository
                                                            .GetAll()
                                                            .AsNoTracking()
                                                            .FirstOrDefault(w => w.Id == id);

            if (registroArquivo != null)
            {
                return RegistroArquivoDto.Mapear(registroArquivo);
            }

            return null;
        }


        public RegistroArquivoDto ObterPorRegistro(long registroId, long registroTabelaId)
        {
            var registroArquivo = this._registroArquivoRepository.GetAll()
                                                            .AsNoTracking()
                                                            .Where(w => w.RegistroId == registroId
                                                                     && w.RegistroTabelaId == registroTabelaId)
                                                            .OrderByDescending(o => o.CreationTime)
                                                            .FirstOrDefault();

            if (registroArquivo != null)
            {
                return RegistroArquivoDto.Mapear(registroArquivo);
            }

            return null;
        }

        public async Task<IResultDropdownList<long>> ListarRegistroTabelas(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2<RegistroTabela,long>().ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        public long GravarHTMLFormularioDinamico(RegistroHTML registroHtml)
        {
            var registroArquivo = new RegistroArquivo
            {
                RegistroTabelaId = RegistroArquivoDto.ObterTabelaRegistroFormularioDinamico(registroHtml.OperacaoId),
                RegistroId = registroHtml.RegistroId,
                AtendimentoId = registroHtml.AtendimentoId
            };


            try
            {
                Logger.Error($"Gerar Formulario - {registroHtml.AtendimentoId} {registroHtml.RegistroId}");
                Logger.Error(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["App.UploadFilesPath"]));

                var context = ViewRenderer.CreateController<EmptyController>().ControllerContext;
                var renderer = new ViewRenderer(context);

                var model = this.GravarImagemFormularioDinamicoDados(registroHtml.FormRespostaId, registroArquivo.AtendimentoId);

                if (model.Paciente != null)
                {
                    if (registroArquivo.AtendimentoId.HasValue)
                    {
                        using (var atendimentoRepositorio = _iocManager.ResolveAsDisposable<IRepository<Atendimento, long>>())
                        {
                            model.Paciente.Empresa = atendimentoRepositorio.Object.GetAll().AsNoTracking()
                                .Include(x => x.Empresa)
                                .FirstOrDefault(x => x.Id == registroArquivo.AtendimentoId.Value)?.Empresa;
                        }
                    }

                    if (model.Paciente.Empresa == null)
                    {
                        model.Paciente.Empresa = new Empresa();
                    }

                    model.Paciente.ImpressoPor = this.GetCurrentUser()?.FullName;

                    var htmlRender = renderer.RenderViewToString("~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/FormularioDinamicoParaPdf.cshtml", model);

                    var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter
                    {
                        PageFooterHtml =
                            $@"<div style=""width:100%;text-align:right""><span style=""text-align:left;left:0px"">SWManager - TSW Tecnologia em Saúde</span> <span></span> <span class=""page""></span>/<span class=""topage""></span></div>"
                    };
                    var pdfBytes = htmlToPdf.GeneratePdf(htmlRender);

                    var fileName = $"{Guid.NewGuid()}.pdf";
                    var uploadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["App.UploadFilesPath"]);

                    Logger.Error(pdfBytes.Length.ToString());
                    Logger.Error(uploadPath);
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    File.WriteAllBytes(Path.Combine(uploadPath, fileName), pdfBytes);

                    registroArquivo.ArquivoNome = fileName;
                    registroArquivo.ArquivoTipo = "application/pdf";
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, e);
            }

            return this._registroArquivoRepository.InsertAndGetId(registroArquivo);
        }

        public long GravarImagemFormularioDinamico(RegistroHTML registroHtml)
        {
            return this.GravarHTMLFormularioDinamico(registroHtml);
        }

        /// <summary>
        /// The listar arquivos pendentes impressao.
        /// </summary>
        /// <param name="TenancyName">
        /// The tenancy name.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>


        public FormularioDinamicoGerarPdfViewModel GravarImagemFormularioDinamicoDados(long IDResposta, long? IDAtendimento)
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            var tenant = this._tenantManager.Tenants.FirstOrDefault(x => x.Id == this.AbpSession.TenantId.Value);
            var connectionString = ConfigurationManager.ConnectionStrings
                .Cast<ConnectionStringSettings>()
                .FirstOrDefault(v => string.Compare(v.Name, tenant.TenancyName, StringComparison.OrdinalIgnoreCase) == 0)
                ?.ConnectionString;

            using (var conn = new SqlConnection(connectionString))
            {
                var model = new FormularioDinamicoGerarPdfViewModel()
                {
                    Paciente = new DadosPacienteViewModel(),
                    Dados = new List<DadosViewModel>()
                };

                model.Paciente = conn.QueryFirstOrDefault<DadosPacienteViewModel>(@"SELECT 
                                            fr.id idFormResposta, 
                                            fc.id IDFormConfig, 
                                            fc.FontSize FormConfigFontSize,
                                            fc.NOME FormResposta, 
                                            CONVERT(VARCHAR(10), FR.DATARESPOSTA, 103)+' '+Convert(Char(10),FR.DATARESPOSTA,108) DataResposta,
										   A.ID AtendimentoId, A.MATRICULA, CONVERT(VARCHAR, A.DATAREGISTRO, 103) +' '+CONVERT(VARCHAR, A.DATAREGISTRO, 108) DataRegistro,
										   p.id ,pac.id,
										   p.NomeCompleto, p.CPF,
										   CONVERT(VARCHAR(10), pac.nascimento, 103) Nascimento, 
										   (YEAR(GETDATE()) - YEAR(pac.nascimento)) as Ano,
										   (MONTH(GETDATE()) - MONTH(pac.nascimento)) as Mes,
										   (DAY(GETDATE()) - DAY(pac.nascimento)) as Dia, 
										   p.RG+' - '+pac.Emissor RG,
										   sex.descricao Sexo,
										   tsang.descricao fatorRH,
										   med.nomecompleto Medico, med.numeroconselho,
										   esp.nome Especialidade,
										   pconv.NomeFantasia Convenio,
										   plano.descricao Plano,
                                           leito.Descricao as Leito,
										   usr.username Criado,
										   CONVERT(VARCHAR(10), FR.Creationtime, 103)+' '+Convert(Char(10),FR.Creationtime,108) DtCriacao,
										   usra.username Alterado,
										   CONVERT(VARCHAR(10), FR.lastmodificationtime, 103)+' '+Convert(Char(10),FR.lastmodificationtime,108) DtAlteracao
									  FROM ateAtendimento A
									  INNER JOIN AssProntuario prt on prt.ateatendimentoid = a.id
                                      LEFT JOIN sispaciente pac on pac.id = a.sispacienteid 
									  LEFT JOIN sispessoa p on p.Id = pac.sisPessoaId
									  LEFT JOIN SisSexo sex on sex.id = p.sexoid
									  LEFT JOIN sistiposanguineo tsang on tsang.id = pac.sistiposanguineoID
									  LEFT JOIN sismedico med on med.id = a.sismedicoid
									  LEFT JOIN sispessoa pmed on pmed.id = med.sisPessoaId
									  LEFT JOIN sisespecialidade esp on esp.id = a.sisespecialidadeID
									  LEFT JOIN sisconvenio conv on conv.id = a.sisconveniolid
									  LEFT JOIN sispessoa pconv on pconv.id = conv.sisPessoaId
									  LEFT JOIN sisplano plano on plano.id = a.sisplanoid
									  LEFT JOIN AteLeito leito on leito.id = prt.AteLeitoId
									  INNER JOIN SisFormResposta fr on fr.id = prt.sisformrespostaid
									  INNER JOIN SisFormConfig fc on fc.id = fr.formconfigid
									  INNER JOIN abpusers usr on usr.id = fr.creatoruserid
                                      LEFT JOIN abpusers usra on usra.id = fr.lastmodifieruserid
									 WHERE prt.AteAtendimentoId = " + IDAtendimento + @"
									 AND prt.SisFormRespostaId = " + IDResposta);

                if (model.Paciente == null)
                {
                    return model;
                }

                model.Dados = conn.Query<DadosViewModel>(
                                  @"SELECT  frc.id, frc.ordem,
                Colunas.Colunas,
                fcc.ID IDFCC, 
                fcc.Name, 
                fcc.Label, 
                fcc.Type, 
                fcc.Orientation, 
                fcc.PrependText,
                fcc.AppendText, 
                fcc.Offset, 
                fcc.Size,
                FcmoCount.qtd,
                FCMO.OPCAO AS Opcao,
                FormData.Valor
                    FROM SisFormConfig FC
                INNER JOIN SisFormRowConfig FRC ON FRC.FormConfig_Id = FC.Id
                INNER JOIN SisFormColConfig FCC ON FCC.RowConfig_Id = FRC.Id
                INNER JOIN(
                    SELECT fc.id, frc.Id Linha, frc.Ordem, Count(1) Colunas
                    FROM SisFormConfig FC
                INNER JOIN SisFormRowConfig FRC ON FRC.FormConfig_Id = FC.Id
                INNER JOIN SisFormColConfig FCC ON FCC.RowConfig_Id = FRC.Id
                GROUP BY fc.id, frc.Id, frc.Ordem
                    ) Colunas on Colunas.id = FC.Id
                and Colunas.Linha = FRC.Id
                left join(select colconfig_id, count(1) qtd
                    from sisformcolmultioption
                where IsDeleted = 0
                group by colconfig_id) FcmoCount on FcmoCount.colconfig_id = FCC.ID
                left join sisformcolmultioption fcmo on fcmo.colconfig_id = FCC.ID
                AND FCC.TYPE LIKE 'checkbox'
                left join(
                    SELECT Case
                When FCC.Type = 'checkbox' then convert(varchar, FCC.ID) + FD.VALOR	
                else convert(varchar, FCC.ID)
                end NomeCampo,
                    FD.VALOR,
                FCC.Type,
                FCC.ID
                    FROM SisFormData FD
                INNER JOIN SisFormColConfig FCC ON FCC.Id = FD.ColConfigId
                WHERE FormRespostaId = @IDResposta
                AND FD.IsDeleted = @IsDeleted
                    ) FormData on FormData.NomeCampo = Case
                When FCC.Type = 'checkbox' then convert(varchar, FCC.ID) + FCMO.OPCAO
                else convert(varchar, FCC.ID)
                end
                    WHERE FC.Id = @IDFormConfig
                AND fcc.ISDELETED = @IsDeleted
                Order by 
                FRC.ORDEM, id",
                                  new { IsDeleted = false, model.Paciente.IDFormConfig, IDResposta });

                return model;
            }
        }

        public async Task<ListResultDto<RegistroArquivoAtendimentoIndex>> ListarPorAtendimento(ListarRegistroInput input)
        {
            var registrosAgrupados = _registroArquivoRepository.GetAll().AsNoTracking()
                                                      .Where(w => w.AtendimentoId == input.AtendimentoId)
                                                      .Where(m => m.CreationTime >= input.StartDate && m.CreationTime <= input.EndDate)
                                                      .GroupBy(g => new { g.RegistroId, g.RegistroTabelaId })
                                                      .Select(i => i.Max(m => m.Id));

            var registros = _registroArquivoRepository.GetAll()
                                                       .Where(w => registrosAgrupados.Any(a => a == w.Id))
                                                       .Include(i => i.RegistroTabela);
            var registrosAtendimento = new List<RegistroArquivoAtendimentoIndex>();

            if (input.OperacaoId.HasValue)
            {
                registros = registros.Where(x => x.RegistroTabelaId == input.OperacaoId);
            }
            var contarRegistros = registros.Count();

            var registrosOrder = await registros
                .AsNoTracking()
                .OrderByDescending(e => e.CreationTime)
                .PageBy(input)
                .ToListAsync().ConfigureAwait(false);


            foreach (var item in registrosOrder)
            {
                var registroArquivoAtendimentoIndex = new RegistroArquivoAtendimentoIndex
                {
                    RegistroId = item.Id,
                    DataRegistro = item.CreationTime,
                    OperacaoDescricao = item.RegistroTabela.Descricao,
                    IsPDF = item.RegistroTabelaId == (long)EnumArquivoTabela.PrescricaoMedica || (!item.ArquivoTipo.IsNullOrEmpty() && item.ArquivoTipo == "application/pdf")
                };

                registrosAtendimento.Add(registroArquivoAtendimentoIndex);
            }


            return new PagedResultDto<RegistroArquivoAtendimentoIndex>(contarRegistros, registrosAtendimento);
        }

        public async Task<ListResultDto<RegistroArquivoAtendimentoIndex>> ListarPorAtendimentoEReceituarioMedico(ListarRegistroInput input)
        {
            const string RECEITUARIO_MEDICO_CODIGO = "RECEITAMEDICA";
            var registrosAgrupados = _registroArquivoRepository.GetAll()
                                                      .Include(x => x.RegistroTabela)
                                                      .AsNoTracking()
                                                      .Where(w => w.AtendimentoId == input.AtendimentoId && w.RegistroTabela.Codigo == RECEITUARIO_MEDICO_CODIGO)
                                                      .Where(m => m.CreationTime >= input.StartDate && m.CreationTime <= input.EndDate)
                                                      .GroupBy(g => new { g.RegistroId, g.RegistroTabelaId })
                                                      .Select(i => i.Max(m => m.Id));

            var registros = _registroArquivoRepository.GetAll()
                                                       .Where(w => registrosAgrupados.Any(a => a == w.Id))
                                                       .Include(i => i.RegistroTabela);
            var registrosAtendimento = new List<RegistroArquivoAtendimentoIndex>();

            if (input.OperacaoId.HasValue)
            {
                registros = registros.Where(x => x.RegistroTabelaId == input.OperacaoId);
            }
            var contarRegistros = registros.Count();

            var registrosOrder = await registros
                .AsNoTracking()
                .OrderByDescending(e => e.CreationTime)
                .PageBy(input)
                .ToListAsync().ConfigureAwait(false);


            foreach (var item in registrosOrder)
            {
                var registroArquivoAtendimentoIndex = new RegistroArquivoAtendimentoIndex
                {
                    RegistroId = item.Id,
                    DataRegistro = item.CreationTime,
                    OperacaoDescricao = item.RegistroTabela.Descricao,
                    IsPDF = item.RegistroTabelaId == (long)EnumArquivoTabela.PrescricaoMedica || (!item.ArquivoTipo.IsNullOrEmpty() && item.ArquivoTipo == "application/pdf")
                };

                registrosAtendimento.Add(registroArquivoAtendimentoIndex);
            }


            return new PagedResultDto<RegistroArquivoAtendimentoIndex>(contarRegistros, registrosAtendimento);
        }
    }

    public class FormularioDinamicoGerarPdfViewModel
    {
        public DadosPacienteViewModel Paciente { get; set; }

        public IEnumerable<DadosViewModel> Dados { get; set; }

        public static StringBuilder RenderCheckbox(IGrouping<string, DadosViewModel> dados)
        {
            var result = new StringBuilder();
            var colunas = 12 / dados.Count();
            foreach (var item in dados)
            {
                CampoCheckBox(result, item.Opcao, item.Valor, colunas, item.Offset);
            }

            return result;
        }

        public static StringBuilder RenderDefault(DadosViewModel firstItem)
        {
            var colunas = CalculaColunas(firstItem);
            var result = new StringBuilder();
            CampoCheckBox(result, firstItem.Opcao, firstItem.Valor, colunas, firstItem.Offset);
            return result;

        }

        public static FormularioDinamicoGerarCampoViewModel RenderItem(IGrouping<string, DadosViewModel> item)
        {
            var firstItem = item.First();
            var coluna = CalculaColunas(firstItem);
            var model = new FormularioDinamicoGerarCampoViewModel
            {
                Label = $@"<td colspan=""{coluna}"" class=""title"" ><strong>{firstItem.Label}</strong></td>",
                Coluna = coluna,
                FieldSb = new StringBuilder()
            };

            if (firstItem.Type == "checkbox")
            {
                model.FieldSb = RenderCheckbox(item);
            }
            else
            {
                model.FieldSb = RenderDefault(firstItem);
            }

            return model;
        }

        private static int CalculaColunas(DadosViewModel firstItem)
        {
            if (!firstItem.Colunas.IsNullOrEmpty() && firstItem.Colunas != "0")
            {
                return 12 / int.Parse(firstItem.Colunas);
            }

            if (!firstItem.Size.IsNullOrEmpty())
            {
                return int.Parse(firstItem.Size);
            }

            return 1;
        }

        /// <summary>
        /// The gera campo.
        /// </summary>
        /// <param name="label">
        /// The Label.
        /// </param>
        /// <param name="Opcao">
        /// The Opcao.
        /// </param>
        /// <param name="Type">
        /// The Type.
        /// </param>
        /// <param name="Valor">
        /// The Valor.
        /// </param>
        /// <param name="Coluna">
        /// The Coluna.
        /// </param>
        /// <param name="Orientation">
        /// The Orientation.
        /// </param>
        /// <param name="AppendText">
        /// The Append Text.
        /// </param>
        /// <param name="PrependText">
        /// The Prepend Text.
        /// </param>
        /// <param name="CKFcampo">
        /// The ck fcampo.
        /// </param>
        /// <param name="CKVcampo">
        /// The ck vcampo.
        /// </param>
        /// <returns>
        /// The <see cref="FormularioDinamicoGerarCampoViewModel"/>.
        /// </returns>
        public static FormularioDinamicoGerarCampoViewModel GeraCampo(
            string label,
            string opcao,
            string type,
            string valor,
            int coluna,
            string orientation,
            string offSet,
            string appendText,
            string prependText)
        {
            var model = new FormularioDinamicoGerarCampoViewModel
            {
                Label = $@"<td colspan=""{coluna}"" class=""title"" ><strong>{label}</strong></td>"
            };

            switch (type)
            {
                case "checkbox":
                    {
                        CampoCheckBox(model.FieldSb, opcao, valor, coluna, offSet);
                        break;
                    }
                default:
                    {
                        CampoDefault(model.FieldSb, opcao, valor, coluna, offSet);
                        break;
                    }
            }

            //if (Type == "checkbox")
            //{

            //    model.FieldSb.Append($"{CKFcampo}{(!CKFcampo.IsNullOrEmpty() ? "<br/>" : string.Empty)}{valueField}");
            //    model.LineSb = model.LineSb;
            //    model.Vcampo = $@"<td colspan=""{Coluna}"">{CKVcampo}{model.CKFcampo}</td>";
            //}
            //else
            //{
            //    model.Vcampo = $@" <td colspan=""{Coluna}"">{Valor} </td>";
            //}

            return model;
        }

        public static string GerarLabelVazio(long colunas)
        {
            return $@"<td colspan=""{colunas}"" class=""title"" ><strong>&nbsp;</strong></td>";
        }

        private static void CampoCheckBox(StringBuilder field, string opcao, string valor, int coluna, string offset)
        {
            field.Append($@"<td colspan=""{coluna}"" class=""title"" >")
                .Append(valor.IsNullOrEmpty() ? $"<span> [&nbsp;&nbsp;&nbsp;]&nbsp; </span>{opcao}" : $@"<span> [<strong>X&nbsp;</strong>]<strong>&nbsp;{opcao}</strong> </span>")
                .Append("</td>");
        }

        /// <summary>
        /// The campo default.
        /// </summary>
        /// <param name="field">
        /// The field.
        /// </param>
        /// <param name="opcao">
        /// The opcao.
        /// </param>
        /// <param name="valor">
        /// The valor.
        /// </param>
        /// <param name="coluna">
        /// The coluna.
        /// </param>
        /// <param name="offset">
        /// The offset.
        /// </param>
        private static void CampoDefault(StringBuilder field, string opcao, string valor, int coluna, string offset)
        {
            if (!string.IsNullOrEmpty(offset))
            {
                field.Append($@" <td colspan=""{offset}"">&nbsp; </td> ");
            }

            if(!valor.IsNullOrEmpty())
            {
                valor = valor.Replace("\n", "<br/>");
            }

            field.Append($@" <td colspan=""{coluna}"">{valor} </td> ");
        }

        public static string CampoDefaultVazio(long colunas)
        {
            return $@" <td colspan=""{colunas}"">&nbsp; </td> ";
        }
    }

    public static class FormularioDinamicoCsHelper
    {
        public static StringBuilder AbrirLinha(this StringBuilder text)
        {
            return text.Append("<tr>");
        }

        public static StringBuilder FecharLinha(this StringBuilder text)
        {
            return text.Append("</tr>");
        }
    }

    public class FormularioDinamicoGerarCampoViewModel
    {
        public string Label { get; set; }
        public StringBuilder FieldSb { get; set; } = new StringBuilder();

        public int Coluna { get; set; }
    }


    public class DadosViewModel
    {
        public string id { get; set; }
        public string ordem { get; set; }
        public string Colunas { get; set; }
        public string IDFCC { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }

        public string Orientation { get; set; }
        public string PrependText { get; set; }
        public string AppendText { get; set; }

        public string Offset { get; set; }
        public string Size { get; set; }

        public string qtd { get; set; }
        public string Opcao { get; set; }
        public string Valor { get; set; }
    }

    public class DadosPacienteViewModel
    {
        public string AtendimentoId { get; set; }

        public string idFormResposta { get; set; }
        public string IDFormConfig { get; set; }
        
        public string FormConfigFontSize { get; set; }
        public string FormResposta { get; set; }
        public string DataResposta { get; set; }
        public string Matricula { get; set; }
        public string DataRegistro { get; set; }
        public string NomeCompleto { get; set; }
        public string CPF { get; set; }
        public string Nascimento { get; set; }
        public string Ano { get; set; }
        public string Mes { get; set; }
        public string Dia { get; set; }
        public string RG { get; set; }
        public string Sexo { get; set; }
        public string fatorRH { get; set; }
        public string Medico { get; set; }
        public string numeroconselho { get; set; }
        public string Especialidade { get; set; }
        public string Convenio { get; set; }
        public string Plano { get; set; }
        
        public string Leito { get; set; }
        public string Criado { get; set; }
        public string DtCriacao { get; set; }
        public string Alterado { get; set; }
        public string DtAlteracao { get; set; }
        public Empresa Empresa { get; set; }
        public string ImpressoPor { get; set; }
    }

    /// <summary>
    /// Class that renders MVC views to a string using the
    /// standard MVC View Engine to render the view. 
    /// Requires that ASP.NET HttpContext is present to
    /// work, but works outside of the context of MVC
    /// </summary>
    public class ViewRenderer
    {
        /// <summary>
        /// Required Controller Context
        /// </summary>
        protected ControllerContext Context { get; set; }

        /// <summary>
        /// Initializes the ViewRenderer with a Context.
        /// </summary>
        /// <param name="controllerContext">
        /// If you are running within the context of an ASP.NET MVC request pass in
        /// the controller's context. 
        /// Only leave out the context if no context is otherwise available.
        /// </param>
        public ViewRenderer(ControllerContext controllerContext = null)
        {
            // Create a known controller from HttpContext if no context is passed
            if (controllerContext == null)
            {
                if (HttpContext.Current != null)
                {
                    controllerContext = CreateController<EmptyController>().ControllerContext;
                }
                else
                {
                    throw new InvalidOperationException(
                        "ViewRenderer must run in the context of an ASP.NET " +
                        "Application and requires HttpContext.Current to be present.");
                }
            }
            Context = controllerContext;
        }

        /// <summary>
        /// Renders a full MVC view to a string. Will render with the full MVC
        /// View engine including running _ViewStart and merging into _Layout        
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">The model to render the view with</param>
        /// <returns>String of the rendered view or null on error</returns>
        public string RenderViewToString(string viewPath, object model = null)
        {
            return RenderViewToStringInternal(viewPath, model, false);
        }

        /// <summary>
        /// Renders a full MVC view to a writer. Will render with the full MVC
        /// View engine including running _ViewStart and merging into _Layout        
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">The model to render the view with</param>
        /// <returns>String of the rendered view or null on error</returns>
        public void RenderView(string viewPath, object model, TextWriter writer)
        {
            RenderViewToWriterInternal(viewPath, writer, model, false);
        }


        /// <summary>
        /// Renders a partial MVC view to string. Use this method to render
        /// a partial view that doesn't merge with _Layout and doesn't fire
        /// _ViewStart.
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">The model to pass to the viewRenderer</param>
        /// <returns>String of the rendered view or null on error</returns>
        public string RenderPartialViewToString(string viewPath, object model = null)
        {
            return RenderViewToStringInternal(viewPath, model, true);
        }

        /// <summary>
        /// Renders a partial MVC view to given Writer. Use this method to render
        /// a partial view that doesn't merge with _Layout and doesn't fire
        /// _ViewStart.
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">The model to pass to the viewRenderer</param>
        /// <param name="writer">Writer to render the view to</param>
        public void RenderPartialView(string viewPath, object model, TextWriter writer)
        {
            RenderViewToWriterInternal(viewPath, writer, model, true);
        }

        /// <summary>
        /// Renders a full MVC view to a writer. Will render with the full MVC
        /// View engine including running _ViewStart and merging into _Layout
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">The model to pass to the viewRenderer</param>
        /// <param name="controllerContext">Active Controller context</param>
        /// <returns>String of the rendered view or null on error</returns>
        public static string RenderView(string viewPath, object model = null,
                                        ControllerContext controllerContext = null)
        {
            ViewRenderer renderer = new ViewRenderer(controllerContext);
            return renderer.RenderViewToString(viewPath, model);
        }

        /// <summary>
        /// Renders a full MVC view to a writer. Will render with the full MVC
        /// View engine including running _ViewStart and merging into _Layout
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">The model to pass to the viewRenderer</param>
        /// <param name="writer">Writer to render the view to</param>
        /// <param name="controllerContext">Active Controller context</param>
        /// <returns>String of the rendered view or null on error</returns>
        public static void RenderView(string viewPath, TextWriter writer, object model,
                                        ControllerContext controllerContext)
        {
            ViewRenderer renderer = new ViewRenderer(controllerContext);
            renderer.RenderView(viewPath, model, writer);
        }

        /// <summary>
        /// Renders a full MVC view to a writer. Will render with the full MVC
        /// View engine including running _ViewStart and merging into _Layout
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">The model to pass to the viewRenderer</param>
        /// <param name="controllerContext">Active Controller context</param>
        /// <param name="errorMessage">optional out parameter that captures an error message instead of throwing</param>
        /// <returns>String of the rendered view or null on error</returns>
        public static string RenderView(string viewPath, object model,
                                        ControllerContext controllerContext,
                                        out string errorMessage)
        {
            errorMessage = null;
            try
            {
                ViewRenderer renderer = new ViewRenderer(controllerContext);
                return renderer.RenderViewToString(viewPath, model);
            }
            catch (Exception ex)
            {
                errorMessage = ex.GetBaseException().Message;
            }
            return null;
        }

        /// <summary>
        /// Renders a full MVC view to a writer. Will render with the full MVC
        /// View engine including running _ViewStart and merging into _Layout
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">The model to pass to the viewRenderer</param>
        /// <param name="controllerContext">Active Controller context</param>
        /// <param name="writer">Writer to render the view to</param>
        /// <param name="errorMessage">optional out parameter that captures an error message instead of throwing</param>
        /// <returns>String of the rendered view or null on error</returns>
        public static void RenderView(string viewPath, object model, TextWriter writer,
                                        ControllerContext controllerContext,
                                        out string errorMessage)
        {
            errorMessage = null;
            try
            {
                ViewRenderer renderer = new ViewRenderer(controllerContext);
                renderer.RenderView(viewPath, model, writer);
            }
            catch (Exception ex)
            {
                errorMessage = ex.GetBaseException().Message;
            }
        }


        /// <summary>
        /// Renders a partial MVC view to string. Use this method to render
        /// a partial view that doesn't merge with _Layout and doesn't fire
        /// _ViewStart.
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">The model to pass to the viewRenderer</param>
        /// <param name="controllerContext">Active controller context</param>
        /// <returns>String of the rendered view or null on error</returns>
        public static string RenderPartialView(string viewPath, object model = null,
                                                ControllerContext controllerContext = null)
        {
            ViewRenderer renderer = new ViewRenderer(controllerContext);
            return renderer.RenderPartialViewToString(viewPath, model);
        }

        /// <summary>
        /// Renders a partial MVC view to string. Use this method to render
        /// a partial view that doesn't merge with _Layout and doesn't fire
        /// _ViewStart.
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">The model to pass to the viewRenderer</param>
        /// <param name="controllerContext">Active controller context</param>
        /// <param name="writer">Text writer to render view to</param>
        /// <param name="errorMessage">optional output parameter to receive an error message on failure</param>
        public static void RenderPartialView(string viewPath, TextWriter writer, object model = null,
                                                ControllerContext controllerContext = null)
        {
            ViewRenderer renderer = new ViewRenderer(controllerContext);
            renderer.RenderPartialView(viewPath, model, writer);
        }


        /// <summary>
        /// Internal method that handles rendering of either partial or 
        /// or full views.
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">Model to render the view with</param>
        /// <param name="partial">Determines whether to render a full or partial view</param>
        /// <param name="writer">Text writer to render view to</param>
        protected void RenderViewToWriterInternal(string viewPath, TextWriter writer, object model = null, bool partial = false)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            if (partial)
            {
                viewEngineResult = ViewEngines.Engines.FindPartialView(Context, viewPath);
            }
            else
            {
                viewEngineResult = ViewEngines.Engines.FindView(Context, viewPath, null);
            }

            if (viewEngineResult == null)
            {
                throw new FileNotFoundException();
            }

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            Context.Controller.ViewData.Model = model;

            var ctx = new ViewContext(Context, view,
                                        Context.Controller.ViewData,
                                        Context.Controller.TempData,
                                        writer);
            view.Render(ctx, writer);
        }

        /// <summary>
        /// Internal method that handles rendering of either partial or 
        /// or full views.
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">Model to render the view with</param>
        /// <param name="partial">Determines whether to render a full or partial view</param>
        /// <returns>String of the rendered view</returns>
        private string RenderViewToStringInternal(string viewPath, object model,
                                                    bool partial = false)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            if (partial)
            {
                viewEngineResult = ViewEngines.Engines.FindPartialView(Context, viewPath);
            }
            else
            {
                viewEngineResult = ViewEngines.Engines.FindView(Context, viewPath, null);
            }

            if (viewEngineResult == null || viewEngineResult.View == null)
            {
                throw new FileNotFoundException();
            }

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            Context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(Context, view,
                                            Context.Controller.ViewData,
                                            Context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }


        /// <summary>
        /// Creates an instance of an MVC controller from scratch 
        /// when no existing ControllerContext is present       
        /// </summary>
        /// <typeparam name="T">Type of the controller to create</typeparam>
        /// <returns>Controller for T</returns>
        /// <exception cref="InvalidOperationException">thrown if HttpContext not available</exception>
        public static T CreateController<T>(RouteData routeData = null, params object[] parameters)
                    where T : Controller, new()
        {
            // create a disconnected controller instance
            T controller = (T)Activator.CreateInstance(typeof(T), parameters);

            // get context wrapper from HttpContext if available
            HttpContextBase wrapper = null;
            if (HttpContext.Current != null)
            {
                wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);
            }
            else
            {
                throw new InvalidOperationException(
                    "Can't create Controller Context if no active HttpContext instance is available.");
            }

            if (routeData == null)
            {
                routeData = new RouteData();
            }

            // add the controller routing if not existing
            if (!routeData.Values.ContainsKey("controller") && !routeData.Values.ContainsKey("Controller"))
            {
                routeData.Values.Add("controller", controller.GetType().Name
                                                            .ToLower()
                                                            .Replace("controller", string.Empty));
            }

            controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);
            return controller;
        }
        
        public static T CreateController<T>(HttpContext httpContextCurrent,RouteData routeData = null, params object[] parameters)
            where T : Controller, new()
        {
            // create a disconnected controller instance
            T controller = (T)Activator.CreateInstance(typeof(T), parameters);

            // get context wrapper from HttpContext if available
            HttpContextBase wrapper = null;
            if (httpContextCurrent != null)
            {
                wrapper = new HttpContextWrapper(httpContextCurrent);
            }
            else
            {
                throw new InvalidOperationException(
                    "Can't create Controller Context if no active HttpContext instance is available.");
            }

            if (routeData == null)
            {
                routeData = new RouteData();
            }

            // add the controller routing if not existing
            if (!routeData.Values.ContainsKey("controller") && !routeData.Values.ContainsKey("Controller"))
            {
                routeData.Values.Add("controller", controller.GetType().Name
                    .ToLower()
                    .Replace("controller", string.Empty));
            }

            controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);
            return controller;
        }

    }

    /// <summary>
    /// Empty MVC Controller instance used to 
    /// instantiate and provide a new ControllerContext
    /// for the ViewRenderer
    /// </summary>
    public class EmptyController : Controller
    {
    }


    public static class SessionHelper
    {
        public static bool IsDefault(IAbpSession abpSession)
        {
            return (abpSession.TenantId == 1);
        }

        public static bool IsAmerican(IAbpSession abpSession)
        {
            return (abpSession.TenantId == 7);
        }

        public static bool IsLipp(IAbpSession abpSession)
        {
            return (abpSession.TenantId == 8);
        }

        public static string GetConnectionStringByTenant(IAbpSession abpSession)
        {
            var connectionString = string.Empty;
            if (IsDefault(abpSession))
            {

            }
            else if (IsAmerican(abpSession))
            {
                connectionString = ConfigurationManager.ConnectionStrings["American"].ConnectionString;
            }
            else if (IsLipp(abpSession))
            {
                connectionString = ConfigurationManager.ConnectionStrings["LIPP"].ConnectionString;
            }

            return connectionString;
        }
    }
}
