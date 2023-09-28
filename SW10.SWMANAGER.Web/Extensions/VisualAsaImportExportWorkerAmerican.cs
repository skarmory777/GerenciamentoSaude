using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.VisualASA;
using SW10.SWMANAGER.ClassesAplicacao.VisualAsaImportExportLogs;
using SW10.SWMANAGER.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using static SW10.SWMANAGER.ADORepositorio.Repositorios.RepositoriosAsa;

namespace SW10.SWMANAGER.Web.Extensions
{
    public class VisualAsaImportExportWorkerAmerican : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        //private readonly IRepository<Sis_Paciente, long> _pacienteRepositorio;
        //private readonly IRepository<Sis_Pessoa, long> _pessoaRepositorio;
        //private readonly IRepository<SisPessoa, long> _pessoaSWRepositorio;
        //private readonly IRepository<Sis_Atendimento, long> _atendimentoRepositorio;
        //private readonly IRepository<Sis_Internacao, long> _internacaoRepositorio;
        //private readonly IRepository<Sis_ContaMedica, long> _contaMedicaRepositorio;
        private SisPacienteRepositorio _pacienteAsaRepositorio;
        private SisPessoaRepositorio _pessoaAsaRepositorio;
        private SisAtendimentoRepositorio _atendimentoAsaRepositorio;
        private SisInternacaoRepositorio _internacaoAsaRepositorio;
        private SisAmbulatorioRepositorio _ambulatorioAsaRepositorio;
        private SisContaMedicaRepositorio _contamedicaAsaRepositorio;
        private ProReqExameMovRepositorio _reqExameMovAsaRepositorio;
        private ProReqExameMovItemRepositorio _reqExameMovItemAsaRepositorio;

        private SWMANAGERDbContext dbPaciente, dbPessoa, dbAtendimento, dbInternacao, dbAmbulatorio, dbContaMedica, dbRegExameMov, dbRegExameMovItem;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private int? tenantId;
        private static bool EmExecucao { get; set; }
        public VisualAsaImportExportWorkerAmerican(
            AbpTimer timer,
            IUnitOfWorkManager unitOfWorkManager
        //IVisualAppService visualAppService
        )
        : base(timer)
        {
            //Timer.Period = 60000;
            Timer.Period = 5000;
            _unitOfWorkManager = unitOfWorkManager;
            //_visualAppService = visualAppService;
        }

        [UnitOfWork(false)]
        protected override async void DoWork()
        {
            Timer.Period = 200000;

            if (!EmExecucao)
            {
                EmExecucao = true;
                try
                {
                    tenantId = _unitOfWorkManager.Current.GetTenantId(); //CurrentUnitOfWork.GetTenantId();

                    if (tenantId == null)
                    {
                        throw new Exception(L("InformarTenant"));
                    }

                    SWMANAGERDbContext db = new SWMANAGERDbContext();
                    var tenantConfig = await db.TenantImportConfigs.Where(m => m.TenantId == tenantId).FirstOrDefaultAsync();
                    db.Dispose();

                    var maquina = Environment.MachineName;
                    //if (maquina.ToUpper() == "SERVERWEBSW")
                    //{
                    //using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                    //{
                    _pessoaAsaRepositorio = new SisPessoaRepositorio(tenantConfig.ConnectionStringNameAsa);
                    _pacienteAsaRepositorio = new SisPacienteRepositorio(tenantConfig.ConnectionStringNameAsa);
                    _atendimentoAsaRepositorio = new SisAtendimentoRepositorio(tenantConfig.ConnectionStringNameAsa);
                    _contamedicaAsaRepositorio = new SisContaMedicaRepositorio(tenantConfig.ConnectionStringNameAsa);
                    _internacaoAsaRepositorio = new SisInternacaoRepositorio(tenantConfig.ConnectionStringNameAsa);
                    _ambulatorioAsaRepositorio = new SisAmbulatorioRepositorio(tenantConfig.ConnectionStringNameAsa);
                    _reqExameMovAsaRepositorio = new ProReqExameMovRepositorio(tenantConfig.ConnectionStringNameAsa);
                    _reqExameMovItemAsaRepositorio = new ProReqExameMovItemRepositorio(tenantConfig.ConnectionStringNameAsa);
                    //db.AbpSession.TenantId = 7;

                    #region pessoa
                    await ExportarPessoa(tenantConfig.ConnectionStringNameSw);
                    #endregion

                    #region paciente
                    await ExportarPaciente(tenantConfig.ConnectionStringNameSw);
                    #endregion

                    #region atendimento
                    await ExportarAtendimento(tenantConfig.ConnectionStringNameSw);
                    #endregion

                    #region internacao
                    await ExportarInternacao(tenantConfig.ConnectionStringNameSw);
                    #endregion

                    #region ambulatorio
                    await ExportarAmbulatorio(tenantConfig.ConnectionStringNameSw);
                    #endregion

                    #region contamedica
                    await ExportarContaMedica(tenantConfig.ConnectionStringNameSw);
                    #endregion

                    #region ReqExameMov
                    // await ExportarReqExameMov(tenantConfig.ConnectionStringNameSw);
                    #endregion

                    #region ReqExameMovItem
                    //   await ExportarReqExameMovItem(tenantConfig.ConnectionStringNameSw);
                    #endregion


                    _pessoaAsaRepositorio.Dispose();
                    _pacienteAsaRepositorio.Dispose();
                    _atendimentoAsaRepositorio.Dispose();
                    _internacaoAsaRepositorio.Dispose();
                    _ambulatorioAsaRepositorio.Dispose();
                    _contamedicaAsaRepositorio.Dispose();
                    //}
                    // }
                }
                catch (Exception ex)
                {
                    using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
                    {
                        smtp.Host = "smtp.smwe.com.br";
                        smtp.Port = 587;
                        //smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("webmaster@smwe.com.br", "SM*123456w");
                        smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                        var mail = new System.Net.Mail.MailMessage();
                        mail.From = new System.Net.Mail.MailAddress("webmaster@smwe.com.br");

                        mail.To.Add(new System.Net.Mail.MailAddress("webmaster@smwe.com.br"));

                        mail.IsBodyHtml = true;
                        mail.Subject = "Erro no VisualAsaImportExportWorker";
                        mail.Body = string.Format("Mensagem: {0}<br>Stacktrace:{1}", ex.Message.ToString(), ex.StackTrace);
                        if (ex.InnerException != null)
                        {
                            mail.Body += string.Format("Mensagem: {0}<br>Stacktrace:{1}", ex.InnerException.Message.ToString(), ex.InnerException.StackTrace);
                            if (ex.InnerException.InnerException != null)
                            {
                                mail.Body += string.Format("Mensagem: {0}<br>Stacktrace:{1}", ex.InnerException.InnerException.Message.ToString(), ex.InnerException.InnerException.StackTrace);
                                if (ex.InnerException.InnerException.InnerException != null)
                                {
                                    mail.Body += string.Format("Mensagem: {0}<br>Stacktrace:{1}", ex.InnerException.InnerException.InnerException.Message.ToString(), ex.InnerException.InnerException.InnerException.StackTrace);
                                    if (ex.InnerException.InnerException.InnerException.InnerException != null)
                                    {
                                        mail.Body += string.Format("Mensagem: {0}<br>Stacktrace:{1}", ex.InnerException.InnerException.InnerException.InnerException.Message.ToString(), ex.InnerException.InnerException.InnerException.InnerException.StackTrace);
                                    }
                                }
                            }
                        }
                        smtp.Send(mail);
                    }
                }
                finally
                {
                    EmExecucao = false;
                }
            }
        }

        [UnitOfWork(false)]
        private async Task ExportarContaMedica(string cnSW)
        {
            using (dbContaMedica = new SWMANAGERDbContext(cnSW))//logo após o teste colocar AMERICAN
            {
                var contasmedicas = await dbContaMedica
                    .Sis_ContaMedicasVisualASA
                    .Where(m => m.TenantId == tenantId)
                    .ToListAsync();
                for (int i = 0; i < contasmedicas.Count(); i++)
                {
                    var log = new VisualAsaImportExportLog();
                    log.Tabela = "SIS_CONTAMEDICA";
                    log.IdSw = contasmedicas[i].IDSW.Value;

                    var contamedica = await _contamedicaAsaRepositorio.Obter(contasmedicas[i].IDSW.Value.ToString());
                    if (!contamedica.IDAtendimento.HasValue)
                    {
                        var id = contasmedicas[i].IDSW.Value;
                        var query = from f in dbContaMedica.FaturamentoConta
                                    join a in dbContaMedica.Atendimentos on f.AtendimentoId equals a.Id
                                    where f.Id == id
                                    select a;
                        var ateSw = await query.FirstOrDefaultAsync();
                        /*await db.FaturamentoConta
                        .Include(m => m.Atendimento)
                        .Where(m => m.Id == id)
                        .FirstOrDefaultAsync();*/
                        //contamedica.IDContaMedica = contamedica.IDContaMedica;
                        contamedica.IDAtendimento = ateSw.ImportaId;
                        //contasmedicas[i].IDContaMedica = ateSw.ImportaId;
                        contasmedicas[i].IDAtendimento = ateSw.ImportaId;
                    }

                    if (contamedica != null && (contamedica.IDContaMedica.HasValue && contamedica.IDSW.HasValue))
                    {
                        var atendimentoAsa = await _atendimentoAsaRepositorio.Obter(contamedica.IDAtendimento.Value);
                        if (atendimentoAsa == null || (atendimentoAsa != null && !atendimentoAsa.IDAtendimento.HasValue))
                        {
                            //_visualAppService.MigrarVisualASA(contamedica.IDAtendimento.Value);
                            CarregarSisAtendimento(contamedica.IDAtendimento.Value, cnSW);
                            atendimentoAsa = await dbContaMedica
                                .Sis_AtendimentosVisualASA
                                .FirstOrDefaultAsync(m =>
                                    m.ImportaId == contamedica.IDAtendimento.Value &&
                                    m.TenantId == tenantId
                                );
                            var idAteAsa = await _atendimentoAsaRepositorio.Inserir(atendimentoAsa);
                            dbContaMedica.Database.ExecuteSqlCommand("UPDATE SISATENDIMENTO SET IMPORTAID=" + idAteAsa + " WHERE ID=" + contamedica.IDAtendimento.Value);
                            dbContaMedica.Entry(atendimentoAsa).State = EntityState.Deleted;
                            contamedica.IDAtendimento = idAteAsa;
                        }
                        contasmedicas[i].IDContaMedica = contamedica.IDContaMedica;
                        contasmedicas[i].IDAtendimento = atendimentoAsa.IDAtendimento;
                        await _contamedicaAsaRepositorio.Alterar(contasmedicas[i]);
                        log.IdAsa = contasmedicas[i].IDContaMedica.Value;
                        log.Operacao = "ALTERAÇÃO";
                    }
                    else
                    {
                        if (contasmedicas[i].IDAtendimento != null)
                        {
                            var atendimentoAsa = await _atendimentoAsaRepositorio.Obter(contasmedicas[i].IDAtendimento.Value);
                            if (atendimentoAsa == null || (atendimentoAsa != null && !atendimentoAsa.IDAtendimento.HasValue))
                            {
                                //_visualAppService.MigrarVisualASA(contamedica.IDAtendimento.Value);
                                CarregarSisAtendimento(contamedica.IDAtendimento.Value, cnSW);
                                atendimentoAsa = await dbContaMedica
                                    .Sis_AtendimentosVisualASA
                                    .FirstOrDefaultAsync(m =>
                                        m.ImportaId == contamedica.IDAtendimento.Value &&
                                        m.TenantId == tenantId
                                    );
                                var idAteAsa = await _atendimentoAsaRepositorio.Inserir(atendimentoAsa);
                                dbContaMedica.Database.ExecuteSqlCommand("UPDATE SISATENDIMENTO SET IMPORTAID=" + idAteAsa + " WHERE ID=" + contamedica.IDAtendimento.Value);
                                dbContaMedica.Entry(atendimentoAsa).State = EntityState.Deleted;
                                contamedica.IDAtendimento = idAteAsa;
                            }
                            contasmedicas[i].IDAtendimento = atendimentoAsa?.IDAtendimento;
                        }
                        //contasmedicas[i].IDSW = contamedica.IDSW.Value;

                        if (contasmedicas[i].IDAtendimento != null)
                        {
                            var idAsa = await _contamedicaAsaRepositorio.Inserir(contasmedicas[i]);
                            dbContaMedica.Database.ExecuteSqlCommand("UPDATE FATCONTA SET IMPORTAID=" + idAsa + " WHERE ID=" + contasmedicas[i].IDSW.Value);
                            log.IdAsa = idAsa;
                            log.Operacao = "INCLUSÃO";
                        }
                    }
                    dbContaMedica.Entry(contasmedicas[i]).State = EntityState.Deleted;
                    dbContaMedica.VisualAsaImportExportLogs.Add(log);
                    dbContaMedica.SaveChanges();
                }
                //CurrentUnitOfWork.SaveChanges();
                dbContaMedica.Dispose();
            }
        }

        [UnitOfWork(false)]
        private async Task ExportarAmbulatorio(string cnSW)
        {
            using (dbAmbulatorio = new SWMANAGERDbContext(cnSW))//logo após o teste colocar AMERICAN
            {
                var ambulatorios = await dbAmbulatorio
                    .Sis_AmbulatoriosVisualASA
                    .Where(m => m.TenantId == tenantId)
                    .ToListAsync();
                for (int i = 0; i < ambulatorios.Count(); i++)
                {
                    var log = new VisualAsaImportExportLog();
                    log.Tabela = "SIS_AMBULATORIO";
                    log.IdSw = ambulatorios[i].IDSW.Value;

                    var ambulatorio = await _ambulatorioAsaRepositorio.Obter(ambulatorios[i].IDSW.ToString());

                    if (ambulatorio == null) // não cadastrado - inserir
                    {
                        if (ambulatorios[i].IDAmbulatorio.HasValue)
                        {
                            var idAsa = await _ambulatorioAsaRepositorio.Inserir(ambulatorios[i]);
                            log.IdAsa = ambulatorios[i].IDAmbulatorio.Value;
                            log.Operacao = "INCLUSÃO";
                        }
                        else
                        {
                            var id = ambulatorios[i].IDSW.Value;
                            var ateSw = await dbAmbulatorio.Atendimentos
                                .Include(m => m.Medico)
                                .Where(m => m.Id == id)
                                .FirstOrDefaultAsync();

                            ambulatorios[i].IDAmbulatorio = ateSw?.ImportaId;
                            if (ambulatorios[i].IDAmbulatorio != null)
                            {
                                await _ambulatorioAsaRepositorio.Inserir(ambulatorios[i]);
                                log.IdAsa = ambulatorios[i].IDAmbulatorio.Value;
                                log.Operacao = "INCLUSÃO";
                            }
                        }
                    }
                    else
                    {
                        if (ambulatorios[i].IDAmbulatorio.HasValue)
                        {
                            await _ambulatorioAsaRepositorio.Alterar(ambulatorios[i]);
                            log.IdAsa = ambulatorios[i].IDAmbulatorio.Value;
                            log.Operacao = "ALTERAÇÃO";
                        }
                        else
                        {
                            var id = ambulatorios[i].IDSW.Value;
                            var ateSw = await dbAmbulatorio.Atendimentos
                                .Include(m => m.Medico)
                                .Where(m => m.Id == id)
                                .FirstOrDefaultAsync();

                            ambulatorios[i].IDAmbulatorio = ateSw?.ImportaId;

                            if (!ambulatorios[i].ImportaId.HasValue)
                            {
                                var atend = await _atendimentoAsaRepositorio.Obter(id.ToString());

                                ambulatorios[i].IDAmbulatorio = atend?.IDAtendimento;
                                if (!ambulatorios[i].IDAmbulatorio.HasValue)
                                {
                                    throw new Exception("Atendimento não encontrado");
                                }
                            }

                            await _ambulatorioAsaRepositorio.Alterar(ambulatorios[i]);
                            log.IdAsa = ambulatorios[i].IDAmbulatorio.Value;
                            log.Operacao = "ALTERAÇÃO";
                        }
                    }
                    //if (ambulatorio != null && (ambulatorio.IDAmbulatorio.HasValue && ambulatorio.IDMedicoAtendendo.HasValue))
                    //{
                    //    //ambulatorios[i].IDAmbulatorio = ambulatorio.IDAmbulatorio;
                    //    await _ambulatorioAsaRepositorio.Alterar(ambulatorios[i]);
                    //}
                    //else
                    //{
                    //    var alterar = false;
                    //    if (!ambulatorios[i].IDAmbulatorio.HasValue)
                    //    {
                    //        var id = ambulatorios[i].IDSW.Value;
                    //        var atendimentoAmb = await _atendimentoAsaRepositorio.Obter(id.ToString()); //await db.Sis_AtendimentosVisualASA.Where(m => m.IDSW == id).FirstOrDefaultAsync();
                    //        ambulatorios[i].IDAmbulatorio = atendimentoAmb.IDAtendimento;
                    //        var ateAsa1 = await _ambulatorioAsaRepositorio.Obter(atendimentoAmb.IDAtendimento.Value);
                    //        if (ateAsa1.IDAmbulatorio.HasValue)
                    //        {
                    //            alterar = true;
                    //        }

                    //    }
                    //    if (alterar)
                    //    {
                    //        await _ambulatorioAsaRepositorio.Alterar(ambulatorios[i]);
                    //    }
                    //    else
                    //    {
                    //        var idAsa = await _ambulatorioAsaRepositorio.Inserir(ambulatorios[i]);
                    //    }
                    //    //db.Database.ExecuteSqlCommand("UPDATE ATEATENDIMENTO SET IMPORTAID=" + idAsa + " WHERE ID=" + ambulatorios[i].IDSW.Value);
                    //}
                    dbAmbulatorio.Entry(ambulatorios[i]).State = EntityState.Deleted;
                    dbAmbulatorio.VisualAsaImportExportLogs.Add(log);
                    dbAmbulatorio.SaveChanges();
                }
                //CurrentUnitOfWork.SaveChanges();
                dbAmbulatorio.Dispose();
            }
        }

        [UnitOfWork(false)]
        private async Task ExportarInternacao(string cnSW)
        {
            using (dbInternacao = new SWMANAGERDbContext(cnSW))//logo após o teste colocar AMERICAN
            {
                var internacoes = await dbInternacao
                    .Sis_InternacoesVisualASA
                    .Where(m => m.TenantId == tenantId)
                    .ToListAsync();
                for (int i = 0; i < internacoes.Count(); i++)
                {
                    var log = new VisualAsaImportExportLog();
                    log.IdSw = internacoes[i].IDSW.Value;
                    log.Tabela = "SIS_INTERNACAO";
                    var internacao = await _internacaoAsaRepositorio.Obter(internacoes[i].IDSW.ToString());
                    if (internacao == null)
                    {
                        if (internacoes[i].IDInternacao.HasValue)
                        {
                            var idAsa = await _internacaoAsaRepositorio.Inserir(internacoes[i]);
                            log.IdAsa = internacoes[i].IDInternacao.Value;
                            log.Operacao = "INCLUSÃO";
                        }
                        else
                        {
                            var id = internacoes[i].IDSW.Value;
                            var ateSw = await dbInternacao.Atendimentos
                                .Where(m => m.Id == id)
                                .FirstOrDefaultAsync();

                            internacoes[i].IDInternacao = ateSw?.ImportaId;
                            if (internacoes[i].IDInternacao != null)
                            {
                                await _internacaoAsaRepositorio.Inserir(internacoes[i]);
                                log.IdAsa = internacoes[i].IDInternacao.Value;
                                log.Operacao = "INCLUSÃO";
                            }
                        }
                    }
                    else
                    {
                        if (internacoes[i].IDInternacao.HasValue)
                        {
                            await _internacaoAsaRepositorio.Alterar(internacoes[i]);
                            log.IdAsa = internacoes[i].IDInternacao.Value;
                            log.Operacao = "ALTERAÇÃO";
                        }
                        else
                        {
                            var id = internacoes[i].IDSW.Value;
                            var ateSw = await dbInternacao.Atendimentos
                                .Where(m => m.Id == id)
                                .FirstOrDefaultAsync();
                            internacoes[i].IDInternacao = ateSw.ImportaId;
                            if (!internacoes[i].ImportaId.HasValue)
                            {
                                var atend = await _atendimentoAsaRepositorio.Obter(id.ToString());

                                internacoes[i].IDInternacao = atend.IDAtendimento;
                                if (!internacoes[i].IDInternacao.HasValue)
                                {
                                    throw new Exception("Atendimento não encontrado");
                                }
                            }
                            await _internacaoAsaRepositorio.Alterar(internacoes[i]);
                            log.IdAsa = internacoes[i].IDInternacao.Value;
                            log.Operacao = "ALTERAÇÃO";
                        }
                    }
                    //if (!internacao.IDInternacao.HasValue)
                    //{
                    //    var id = internacoes[i].IDSW.Value;
                    //    var ateSw = await dbInternacao.Atendimentos
                    //        .Where(m => m.Id == id)
                    //        .FirstOrDefaultAsync();
                    //    internacao.IDInternacao = ateSw.ImportaId;
                    //}
                    //if (internacao != null && (internacao.IDInternacao.HasValue && internacao.IDSW.HasValue))
                    //{
                    //    internacoes[i].IDInternacao = internacao.IDInternacao;
                    //    await _internacaoAsaRepositorio.Alterar(internacoes[i]);
                    //}
                    //else
                    //{
                    //    var id = internacoes[i].IDSW.Value;
                    //    var atendimento = await _atendimentoAsaRepositorio.Obter(id.ToString()); //await db.Sis_AtendimentosVisualASA.Where(m => m.IDSW == id).FirstOrDefaultAsync();
                    //    internacoes[i].IDInternacao = atendimento.IDAtendimento;
                    //    var idAsa = await _internacaoAsaRepositorio.Inserir(internacoes[i]);
                    //    //db.Database.ExecuteSqlCommand("UPDATE ATEATENDIMENTO SET IMPORTAID=" + idAsa + " WHERE ID=" + internacoes[i].IDSW.Value);
                    //}
                    dbInternacao.Entry(internacoes[i]).State = EntityState.Deleted;
                    dbInternacao.VisualAsaImportExportLogs.Add(log);
                    dbInternacao.SaveChanges();
                }
                //CurrentUnitOfWork.SaveChanges();
                dbInternacao.Dispose();
            }
        }

        [UnitOfWork(false)]
        private async Task ExportarAtendimento(string cnSW)
        {
            using (dbAtendimento = new SWMANAGERDbContext(cnSW))//logo após o teste colocar AMERICAN
            {
                var atendimentos = Task.Run(() => dbAtendimento.Sis_AtendimentosVisualASA.Where(m => m.TenantId == tenantId).ToList()).Result;
                //var qAte = from intern in db.Sis_InternacoesVisualASA
                //           join ate in db.Sis_AtendimentosVisualASA on intern.IDSW equals ate.IDSW
                //           select ate;

                //var atendimentos = await qAte.ToListAsync();
                //for (int i = 0; i < atendimentos.Count(); i++)
                foreach (var item in atendimentos)
                {
                    var log = new VisualAsaImportExportLog();
                    log.Tabela = "SIS_ATENDIMENTO";
                    log.IdSw = item.IDSW.Value;
                    //           if (!item.IDPaciente.HasValue)
                    //         {
                    var query = from ate in dbAtendimento.Atendimentos
                                .Include(m => m.Paciente)
                                .Include(m => m.Paciente.SisPessoa)
                                where ate.Id == item.IDSW
                                select ate.Paciente;

                    var pacSw = query.FirstOrDefault();
                    item.IDPaciente = pacSw?.ImportaId ?? 0;
                    //       }

                    var atendimento = Task.Run(() => _atendimentoAsaRepositorio.Obter(item.IDSW.ToString())).Result;
                    if (atendimento == null) //inserir
                    {
                        if (item.IDPaciente != null && item.IDEmpresa != null)
                        {
                            var idAsa = Task.Run(() => _atendimentoAsaRepositorio.Inserir(item)).Result;
                            dbAtendimento.Database.ExecuteSqlCommand("UPDATE ATEATENDIMENTO SET IMPORTAID=" + idAsa + " WHERE ID=" + item.IDSW.Value);
                            log.IdAsa = idAsa;
                            log.Operacao = "INCLUSÃO";
                        }
                    }
                    else //alterar
                    {
                        if (!item.IDAtendimento.HasValue)
                        {
                            item.IDAtendimento = atendimento.IDAtendimento;
                        }
                        await _atendimentoAsaRepositorio.Alterar(item);
                        log.IdAsa = item.IDPaciente.Value;
                        log.Operacao = "ALTERAÇÃO";
                    }
                    //if (atendimento != null && atendimento.IDAtendimento.HasValue)
                    //{
                    //    item.IDAtendimento = atendimento.IDAtendimento;
                    //    item.IDUsuarioAlteracao = 0; //atendimento.IDUsuarioAlteracao.HasValue? atendimento.IDUsuarioAlteracao.Value : 0;
                    //    item.DataAlteracao = DateTime.Now;
                    //    if (!item.IDPaciente.HasValue)
                    //    {
                    //        int pId = 0;
                    //        pId = item.IDSW.Value;

                    //        var query = from at in dbAtendimento.Atendimentos
                    //                    .Include(m => m.Paciente)
                    //                    .Include(m => m.Paciente.SisPessoa)
                    //                    where at.Id == pId
                    //                    select at;

                    //        var ateSw = query.FirstOrDefault();

                    //        var pSw = ateSw.Paciente;

                    //        if (pSw != null && pSw.SisPessoa.ImportaId.HasValue)
                    //        {
                    //            item.IDPaciente = pSw.SisPessoa.ImportaId;
                    //        }
                    //        else
                    //        {
                    //            var pacienteAsa = Task.Run(() => _pacienteAsaRepositorio.Obter(pId.ToString())).Result;
                    //            if (pacienteAsa.IDPaciente.HasValue)
                    //            {
                    //                item.IDPaciente = pacienteAsa.IDPaciente;
                    //            }
                    //            else
                    //            {
                    //                //_visualAppService.MigrarSisPessoa(pSw.SisPessoaId.Value);
                    //                CarregarSisPessoa(pSw.SisPessoaId.Value, "AMERICANSERVER");
                    //                var pessoaAsa = Task.Run(() => dbAtendimento.Sis_PessoasVisualASA.FirstOrDefault(m => m.IDSW == pSw.SisPessoaId.Value)).Result;
                    //                var idPessoaAsa = Task.Run(() => _pessoaAsaRepositorio.Inserir(pessoaAsa)).Result;
                    //                dbAtendimento.Database.ExecuteSqlCommand("UPDATE SISPESSOA SET IMPORTAID=" + idPessoaAsa + " WHERE ID=" + pSw.SisPessoaId.Value);
                    //                dbAtendimento.Entry(pessoaAsa).State = EntityState.Deleted;
                    //                pacienteAsa = Task.Run(() => dbAtendimento.Sis_PacientesVisualASA.FirstOrDefault(m => m.IDSW == pSw.SisPessoaId.Value)).Result;
                    //                var idPaciente = Task.Run(() => _pacienteAsaRepositorio.Inserir(pacienteAsa)).Result;
                    //                pacienteAsa.IDPaciente = idPaciente;
                    //                dbAtendimento.Entry(pacienteAsa).State = EntityState.Deleted;
                    //                item.IDPaciente = pacienteAsa.IDPaciente;
                    //            }
                    //        }
                    //    }
                    //    await _atendimentoAsaRepositorio.Alterar(item);
                    //    //if (!item.IDPaciente.HasValue)
                    //    //{
                    //    //    int id = 0;
                    //    //    id = item.IDSW.Value;
                    //    //    var ateSw = await dbAtendimento.Atendimentos
                    //    //        .Include(m => m.Paciente)
                    //    //        .Include(m => m.Paciente.SisPessoa)
                    //    //        .Where(m => m.Id == id)
                    //    //        .FirstOrDefaultAsync();
                    //    //    item.IDPaciente = ateSw.Paciente.SisPessoa.ImportaId;
                    //    //}
                    //    //var pacienteId = item.IDPaciente.Value;
                    //    //var paciente = dbAtendimento.Pacientes.FirstOrDefault(m => m.ImportaId == pacienteId);
                    //    //var pacienteAsa = await _pacienteAsaRepositorio.Obter(paciente.SisPessoaId.Value.ToString());
                    //    //if (pacienteAsa == null || (pacienteAsa != null && !pacienteAsa.IDPaciente.HasValue))
                    //    //{
                    //    //    //_visualAppService.MigrarSisPessoa(paciente.SisPessoaId.Value);
                    //    //    CarregarSisPessoa(paciente.SisPessoaId.Value, "AMERICAN");
                    //    //    var pessoaAsa = await dbAtendimento.Sis_PessoasVisualASA.FirstOrDefaultAsync(m => m.IDSW == paciente.SisPessoaId.Value);
                    //    //    var idPessoaAsa = await _pessoaAsaRepositorio.Inserir(pessoaAsa);
                    //    //    dbAtendimento.Database.ExecuteSqlCommand("UPDATE SISPESSOA SET IMPORTAID=" + idPessoaAsa + " WHERE ID=" + paciente.SisPessoaId.Value);
                    //    //    dbAtendimento.Entry(pessoaAsa).State = EntityState.Deleted;
                    //    //    pacienteAsa = await dbAtendimento.Sis_PacientesVisualASA.FirstOrDefaultAsync(m => m.IDSW == paciente.SisPessoaId.Value);
                    //    //    var idPaciente = await _pacienteAsaRepositorio.Inserir(pacienteAsa);
                    //    //    pacienteAsa.IDPaciente = idPaciente;
                    //    //    dbAtendimento.Entry(pacienteAsa).State = EntityState.Deleted;
                    //    //}
                    //}
                    //else
                    //{
                    //    item.IDImportado = item.IDSW.Value;
                    //    if (!item.IDPaciente.HasValue)
                    //    {
                    //        int pId = 0;
                    //        pId = item.IDSW.Value;

                    //        var query = from at in dbAtendimento.Atendimentos
                    //                    .Include(m => m.Paciente)
                    //                    .Include(m => m.Paciente.SisPessoa)
                    //                    where at.Id == pId
                    //                    select at;

                    //        var ateSw = query.FirstOrDefault();

                    //        var pSw = ateSw.Paciente;
                    //        var pac = ateSw.PacienteId.Value;

                    //        if (pSw != null && pSw.SisPessoa.ImportaId.HasValue)
                    //        {
                    //            item.IDPaciente = pSw.SisPessoa.ImportaId;
                    //        }
                    //        else
                    //        {
                    //            var pacienteAsa = Task.Run(() => _pacienteAsaRepositorio.Obter(pac.ToString())).Result;
                    //            if (pacienteAsa == null)
                    //            {

                    //            }
                    //            if (pacienteAsa.IDPaciente.HasValue)
                    //            {
                    //                item.IDPaciente = pacienteAsa.IDPaciente;
                    //            }
                    //            else
                    //            {
                    //                CarregarSisPessoa(pSw.SisPessoaId.Value, "AMERICANSERVER");
                    //                var pessoaAsa = Task.Run(() => dbAtendimento.Sis_PessoasVisualASA.FirstOrDefault(m => m.IDSW == pSw.SisPessoaId.Value)).Result;
                    //                var idPessoaAsa = Task.Run(() => _pessoaAsaRepositorio.Inserir(pessoaAsa)).Result;
                    //                dbAtendimento.Database.ExecuteSqlCommand("UPDATE SISPESSOA SET IMPORTAID=" + idPessoaAsa + " WHERE ID=" + pSw.SisPessoaId.Value);
                    //                dbAtendimento.Entry(pessoaAsa).State = EntityState.Deleted;
                    //                pacienteAsa = Task.Run(() => dbAtendimento.Sis_PacientesVisualASA.FirstOrDefault(m => m.IDSW == pSw.SisPessoaId.Value)).Result;
                    //                var idPaciente = Task.Run(() => _pacienteAsaRepositorio.Inserir(pacienteAsa)).Result;
                    //                pacienteAsa.IDPaciente = idPaciente;
                    //                dbAtendimento.Entry(pacienteAsa).State = EntityState.Deleted;
                    //                item.IDPaciente = pacienteAsa.IDPaciente;
                    //            }
                    //        }
                    //    }
                    //    var idAsa = await _atendimentoAsaRepositorio.Inserir(item);
                    //    dbAtendimento.Database.ExecuteSqlCommand("UPDATE ATEATENDIMENTO SET IMPORTAID=" + idAsa + " WHERE ID=" + item.IDSW.Value);
                    //}
                    dbAtendimento.Entry(item).State = EntityState.Deleted;
                    dbAtendimento.VisualAsaImportExportLogs.Add(log);
                    dbAtendimento.SaveChanges();
                }
                //CurrentUnitOfWork.SaveChanges();
                dbAtendimento.Dispose();
            }
        }

        [UnitOfWork(false)]
        private async Task ExportarPaciente(string cnSW)
        {
            using (dbPaciente = new SWMANAGERDbContext(cnSW))//logo após o teste colocar AMERICAN
            {
                var pacientes = await dbPaciente
                    .Sis_PacientesVisualASA
                    .Where(m => m.TenantId == tenantId)
                    .ToListAsync();
                for (int i = 0; i < pacientes.Count(); i++)
                {
                    var log = new VisualAsaImportExportLog();
                    log.Tabela = "SIS_PACIENTE";
                    log.IdSw = pacientes[i].IDSW.Value;

                    var paciente = await _pacienteAsaRepositorio.Obter(pacientes[i].IDSW.ToString());
                    if (paciente != null)
                    {
                        pacientes[i].IDPaciente = paciente.IDPaciente;
                        await _pacienteAsaRepositorio.Alterar(pacientes[i]);
                        log.IdAsa = pacientes[i].IDPaciente.Value;
                        log.Operacao = "ALTERAÇÃO";
                    }
                    else
                    {
                        var id = pacientes[i].IDSW.Value;
                        var pessoa = await _pessoaAsaRepositorio.Obter(id.ToString());// db.Database.SqlQuery<SisPessoa>("SELECT * FROM SISPESSOA WHERE Id="+id).FirstOrDefaultAsync();

                        if (pessoa != null)
                        {

                            pacientes[i].IDPaciente = pessoa.IDPessoa;
                            var pacienteAsa = await _pacienteAsaRepositorio.Obter(pessoa.IDPessoa.Value);
                            if (pacienteAsa == null || (pacienteAsa != null && !pacienteAsa.IDPaciente.HasValue))
                            {
                                var idAsa = await _pacienteAsaRepositorio.Inserir(pacientes[i]);
                                log.IdAsa = idAsa;
                                log.Operacao = "INCLUSÃO";
                            }
                            else
                            {
                                await _pacienteAsaRepositorio.Alterar(pacientes[i]);
                                log.IdAsa = pacientes[i].IDPaciente.Value;
                                log.Operacao = "ALTERAÇÃO";
                            }
                        }
                        //db.Database.ExecuteSqlCommand("UPDATE SISPACIENTE SET IMPORTAID=" + idAsa + " WHERE ID=" + pacientes[i].IDSW.Value);
                    }
                    dbPaciente.Entry(pacientes[i]).State = EntityState.Deleted;
                    dbPaciente.VisualAsaImportExportLogs.Add(log);
                    dbPaciente.SaveChanges();
                }
                //CurrentUnitOfWork.SaveChanges();
                dbPaciente.Dispose();
            }
        }

        [UnitOfWork(false)]
        private async Task ExportarPessoa(string cnSW)
        {
            using (dbPessoa = new SWMANAGERDbContext(cnSW))//logo após o teste colocar AMERICAN
            {
                var pessoas = await dbPessoa
                    .Sis_PessoasVisualASA
                    .Where(m => m.TenantId == tenantId)
                    .ToListAsync();

                for (int i = 0; i < pessoas.Count(); i++)
                {
                    var log = new VisualAsaImportExportLog();
                    log.Tabela = "SIS_PESSOA";
                    log.IdSw = pessoas[i].IDSW.Value;

                    var pessoa = await _pessoaAsaRepositorio.Obter(pessoas[i].IDSW.ToString());
                    if (pessoa != null && pessoa.IDPessoa.HasValue)
                    {
                        pessoas[i].IDPessoa = pessoa.IDPessoa;
                        pessoas[i].IDUsuarioInclusao = pessoa.IDUsuarioInclusao;
                        pessoas[i].DataInclusao = pessoa.DataInclusao;
                        await _pessoaAsaRepositorio.Alterar(pessoas[i]);
                        log.IdAsa = pessoas[i].IDPessoa.Value;
                        log.Operacao = "ALTERAÇÃO";
                    }
                    else
                    {
                        pessoas[i].IDImportado = pessoas[i].IDSW;
                        var idAsa = await _pessoaAsaRepositorio.Inserir(pessoas[i]);
                        dbPessoa.Database.ExecuteSqlCommand("UPDATE SISPESSOA SET IMPORTAID=" + idAsa + " WHERE ID=" + pessoas[i].IDSW.Value).ToString();
                        log.IdAsa = idAsa;
                        log.Operacao = "INCLUSÃO";
                    }
                    dbPessoa.Entry(pessoas[i]).State = EntityState.Deleted;
                    dbPessoa.VisualAsaImportExportLogs.Add(log);
                    dbPessoa.SaveChanges();
                }
                //CurrentUnitOfWork.SaveChanges();
                dbPessoa.Dispose();
            }
        }


        [UnitOfWork(false)]
        private async Task ExportarReqExameMov(string cnSW)
        {
            using (dbRegExameMovItem = new SWMANAGERDbContext(cnSW))//logo após o teste colocar AMERICAN
            {
                var exames = await dbRegExameMovItem
                    .Pro_ReqExamesMovVisualAsa
                    //.Where(m => m.TenantId == tenantId)
                    .ToListAsync();
                for (int i = 0; i < exames.Count(); i++)
                {
                    var log = new VisualAsaImportExportLog();
                    log.Tabela = "PRO_REQEXAMEMOV";
                    log.IdSw = exames[i].IDSW.Value;

                    var exame = await _reqExameMovAsaRepositorio.Obter(exames[i].IDSW.Value.ToString());

                    if (exame != null && (exame.IdRequisicaoMov > 0 && exame.IDSW.HasValue))
                    {
                        exames[i].IdRequisicaoMov = exame.IdRequisicaoMov;
                        exames[i].IdAtendimento = exame.IdAtendimento;
                        await _reqExameMovAsaRepositorio.Alterar(exames[i]);
                        log.IdAsa = exames[i].IdRequisicaoMov;
                        log.Operacao = "ALTERAÇÃO";
                    }
                    else
                    {
                        var id = exames[i].IDSW.Value;
                        var exameSw = await dbRegExameMovItem.Resultados
                            .Include(m => m.Atendimento)
                            .Include(m => m.MedicoSolicitante)
                            .Where(w => w.Id == id)
                            .FirstOrDefaultAsync();

                        exame.IdAtendimento = exameSw.Atendimento.ImportaId ?? 0;
                        exames[i].IdAtendimento = exame.IdAtendimento;

                        var idAsa = await _reqExameMovAsaRepositorio.Inserir(exames[i]);
                        dbRegExameMovItem.Database.ExecuteSqlCommand("UPDATE LABRESULTADO SET IMPORTAID=" + idAsa + " WHERE ID=" + exames[i].IDSW.Value);
                        dbRegExameMovItem.Database.ExecuteSqlCommand("UPDATE PRO_REQEXAMEMOVEITEM SET IDREQUISICAOMOV=" + idAsa + " WHERE PRO_REQEXAMEMOVID=" + exames[i].IDSW.Value + " AND ISDELETED=0");
                        log.IdAsa = idAsa;
                        log.Operacao = "INCLUSÃO";
                    }
                    dbRegExameMovItem.Entry(exames[i]).State = EntityState.Deleted;
                    dbRegExameMovItem.VisualAsaImportExportLogs.Add(log);
                    dbRegExameMovItem.SaveChanges();
                }
                //CurrentUnitOfWork.SaveChanges();
                dbRegExameMovItem.Dispose();
            }
        }

        [UnitOfWork(false)]
        private async Task ExportarReqExameMovItem(string cnSW)
        {
            using (dbRegExameMovItem = new SWMANAGERDbContext(cnSW))//logo após o teste colocar AMERICAN
            {
                var exames = await dbRegExameMovItem
                    .Pro_ReqExamesMovItensVisualASA
                    //.Where(m => m.TenantId == tenantId)
                    .ToListAsync();

                for (int i = 0; i < exames.Count(); i++)
                {
                    var log = new VisualAsaImportExportLog();
                    log.Tabela = "PRO_REQEXAMEMOVITEM";
                    log.IdSw = exames[i].IDSW.Value;

                    var exame = await _reqExameMovItemAsaRepositorio.Obter(exames[i].IDSW.Value.ToString());
                    if (exame != null && (exame.IdRequisicaoMovItem > 0 && exame.IDSW.HasValue))
                    {
                        //var atendimentoAsa = await _atendimentoAsaRepositorio.Obter(exame.IdAtendimento);
                        //if (atendimentoAsa == null || (atendimentoAsa != null && !atendimentoAsa.IDAtendimento.HasValue))
                        //{
                        //    //_visualAppService.MigrarVisualASA(contamedica.IDAtendimento.Value);
                        //    CarregarSisAtendimento(exame.IDAtendimento.Value, cnSW);
                        //    atendimentoAsa = await dbRegExameMov
                        //        .Sis_AtendimentosVisualASA
                        //        .FirstOrDefaultAsync(m =>
                        //            m.ImportaId == exame.IDAtendimento.Value &&
                        //            m.TenantId == tenantId
                        //        );
                        //    var idAteAsa = await _atendimentoAsaRepositorio.Inserir(atendimentoAsa);
                        //    dbRegExameMov.Database.ExecuteSqlCommand("UPDATE SISATENDIMENTO SET IMPORTAID=" + idAteAsa + " WHERE ID=" + exame.IDAtendimento.Value);
                        //    dbRegExameMov.Entry(atendimentoAsa).State = EntityState.Deleted;
                        //    exame.IDAtendimento = idAteAsa;
                        //}
                        exames[i].IdRequisicaoMov = exame.IdRequisicaoMov;
                        exames[i].IdRequisicaoMovItem = exame.IdRequisicaoMovItem;
                        exames[i].IdItem = exame.IdItem;
                        await _reqExameMovItemAsaRepositorio.Alterar(exames[i]);
                        log.IdAsa = exames[i].IdRequisicaoMovItem;
                        log.Operacao = "ALTERAÇÃO";
                    }
                    else
                    {

                        var id = exames[i].IDSW.Value;
                        var exameSw = await dbRegExameMovItem.ResultadosExames
                            .Include(m => m.FaturamentoItem)
                            .Where(w => w.Id == id)
                            .FirstOrDefaultAsync();

                        exame.IdItem = exameSw.FaturamentoItem.ImportaId ?? 0;
                        //contasmedicas[i].IDContaMedica = ateSw.ImportaId;
                        exames[i].IdItem = exame.IdItem;


                        var idAsa = await _reqExameMovItemAsaRepositorio.Inserir(exames[i]);
                        dbRegExameMovItem.Database.ExecuteSqlCommand("UPDATE LABRESULTADOEXAME SET IMPORTAID=" + idAsa + " WHERE ID=" + exames[i].IDSW.Value);
                        log.IdAsa = idAsa;
                        log.Operacao = "INCLUSÃO";
                    }
                    dbRegExameMovItem.Entry(exames[i]).State = EntityState.Deleted;
                    dbRegExameMovItem.VisualAsaImportExportLogs.Add(log);
                    dbRegExameMovItem.SaveChanges();
                }
                //CurrentUnitOfWork.SaveChanges();
                dbRegExameMovItem.Dispose();
            }
        }

        //métodos do VisualAsaService
        //Tive que fazer isso porque o Abp está se perdendo na hora de buscar a connectionstring
        [UnitOfWork(false)]
        private void CarregarSisPessoa(long pessoaId, string strcontext)
        {
            try
            {
                using (var context = new SWMANAGERDbContext(strcontext))
                {
                    var query = from p in context.SisPessoas
                        .Include(m => m.Naturalidade)
                        .Include(m => m.TipoPessoa)
                        .Include(m => m.Profissao)
                        .Include(m => m.TipoLogradouro)
                        .Include(m => m.Escolaridade)
                        .Include(m => m.Religiao)
                                where p.Id == pessoaId
                                select p;

                    var pessoa = query.FirstOrDefault();

                    Sis_Pessoa sis_Pessoa = null;
                    if (pessoa != null)
                    {
                        var query1 = from p in context.Sis_PessoasVisualASA
                                     where p.IDSW == pessoaId
                                     select p;

                        sis_Pessoa = query1.FirstOrDefault();

                        if (sis_Pessoa == null)
                        {
                            sis_Pessoa = new Sis_Pessoa();
                        }

                        sis_Pessoa.Agencia1 = null;
                        sis_Pessoa.Agencia2 = null;
                        sis_Pessoa.CEP = null;
                        sis_Pessoa.CGC = null;
                        sis_Pessoa.Codigo = pessoa.Codigo;
                        sis_Pessoa.CodPessoa = null; //pessoa.Codigo;
                        sis_Pessoa.Complemento = null;
                        sis_Pessoa.ContaCorrente1 = null;
                        sis_Pessoa.ContaCorrente2 = null;
                        sis_Pessoa.ContaPadrao = null;
                        sis_Pessoa.CPF = pessoa.Cpf;
                        //sis_Pessoa.CreationTime = null;
                        sis_Pessoa.CreatorUserId = null;
                        sis_Pessoa.DataExclusao = null;
                        sis_Pessoa.DataInclusao = null;
                        sis_Pessoa.DataUltimaAlteracao = null;
                        sis_Pessoa.DataUltimoLancamento = null;
                        sis_Pessoa.DeleterUserId = null;
                        sis_Pessoa.DeletionTime = null;
                        sis_Pessoa.Desativado = null;
                        sis_Pessoa.Descricao = pessoa.NomeCompleto;
                        sis_Pessoa.EmissaoIdentidade = null;
                        sis_Pessoa.Endereco = null;
                        sis_Pessoa.EstadoCivil = null;
                        sis_Pessoa.Foto = null;
                        sis_Pessoa.Hidden = null;
                        sis_Pessoa.HomePage = null;
                        sis_Pessoa.IDBairro = null;
                        sis_Pessoa.IDBanco1 = null;
                        sis_Pessoa.IDBanco2 = null;
                        sis_Pessoa.IDCentroCustoLocal = null;
                        sis_Pessoa.IDCidade = null;
                        sis_Pessoa.IDCNAE = null;
                        sis_Pessoa.IDCobranca = null;
                        sis_Pessoa.IDContaTesouraria = null;
                        sis_Pessoa.IDDocumentoTipo = null;
                        sis_Pessoa.Identidade = null;
                        sis_Pessoa.IDEstado = null;
                        sis_Pessoa.IDExterno = null;
                        sis_Pessoa.IDFilialSin = null;
                        sis_Pessoa.IDImportado = null;
                        sis_Pessoa.IDInstrucao = null;
                        sis_Pessoa.IDMeioPagamento = null;
                        //if (pessoa.NacionalidadeId.HasValue)
                        //{
                        //    sis_Pessoa.IDNaturalidade = pessoa.Naturalidade.ImportaId; //(int)pessoa.NaturalidadeId;
                        //}
                        sis_Pessoa.IDNFDescricao = null;
                        sis_Pessoa.IDPessoa = pessoa.ImportaId; //null;//antigo
                        sis_Pessoa.IDSW = (int)pessoa.Id;//novo
                        sis_Pessoa.IDPessoaTipo = pessoa.TipoPessoa.ImportaId; //(int)pessoa.TipoPessoaId;
                                                                               //if (pessoa.ProfissaoId.HasValue)
                                                                               //{
                                                                               //    sis_Pessoa.IDProfissao = pessoa.Profissao.ImportaId; //(int)pessoa.ProfissaoId;
                                                                               //}
                                                                               //if (pessoa.TipoLogradouroId.HasValue)
                                                                               //{
                                                                               //    sis_Pessoa.IDTipoLogradouro = pessoa.TipoLogradouro.ImportaId; //(int)pessoa.TipoLogradouroId;
                                                                               //}
                        sis_Pessoa.IDUsuarioAlteracao = null;
                        sis_Pessoa.IDUsuarioExclusao = null;
                        sis_Pessoa.IDUsuarioInclusao = null;
                        sis_Pessoa.InscricaoEstadual = pessoa.InscricaoEstadual;
                        sis_Pessoa.InscricaoMunicipal = pessoa.InscricaoMunicipal;
                        sis_Pessoa.IsAgendaTel = null;
                        sis_Pessoa.IsAlterado = null;
                        sis_Pessoa.IsDeleted = false;
                        sis_Pessoa.IsFuncionario = null;
                        sis_Pessoa.IsMalaDireta = null;
                        sis_Pessoa.IsRecolheISS = null;
                        sis_Pessoa.IsSincronizado = null;
                        sis_Pessoa.IsSistema = false;
                        sis_Pessoa.Juridico = null;
                        sis_Pessoa.LastModificationTime = null;
                        sis_Pessoa.LastModifierUserId = null;
                        sis_Pessoa.Nacionalidade = null;
                        sis_Pessoa.Nascimento = pessoa.Nascimento;
                        sis_Pessoa.Nominal = null;
                        sis_Pessoa.Numero = null;
                        sis_Pessoa.NumeroLancamentos = null;
                        sis_Pessoa.ObsPessoa = null;
                        sis_Pessoa.OrgaoEmissor = null;
                        sis_Pessoa.Pais = null;
                        sis_Pessoa.Pessoa = pessoa.NomeCompleto;
                        sis_Pessoa.RazaoSocial = pessoa.RazaoSocial;
                        sis_Pessoa.SaldoAtual = null;
                        sis_Pessoa.Sexo = pessoa.SexoId == 1 ? "M" : "F";
                        sis_Pessoa.System = null;
                        sis_Pessoa.Titular1 = null;
                        sis_Pessoa.Titular2 = null;
                        sis_Pessoa.TotalPrevisto = null;
                        sis_Pessoa.TotalQuitado = null;
                        sis_Pessoa.TenantId = tenantId;

                        long sisPessoa = 0;

                        if (sis_Pessoa.Id == 0)
                        {
                            var result = context.Sis_PessoasVisualASA.Add(sis_Pessoa); //_sis_PessoaRepository.InsertAndGetId(sis_Pessoa);
                            context.SaveChanges();
                            sisPessoa = result.Id;
                        }
                        else
                        {
                            //_sis_PessoaRepository.Update(sis_Pessoa);
                            context.Entry(sis_Pessoa).State = EntityState.Modified;
                            context.SaveChanges();
                            pessoaId = sis_Pessoa.Id;

                        }
                        CarregarSisPaciente(pessoa, pessoa.Id, strcontext);
                    }
                    //CurrentUnitOfWork.SaveChanges();
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork(false)]
        private void CarregarSisPaciente(SisPessoa pessoa, long sis_pessoaId, string strcontext)
        {
            try
            {
                using (var context = new SWMANAGERDbContext(strcontext))
                {

                    Sis_Paciente sis_Paciente = null;

                    if (pessoa != null)
                    {
                        var query = from p in context.Sis_PacientesVisualASA //_sis_PacienteRepository.GetAll()
                                    where p.IDPaciente == sis_pessoaId
                                    select p;

                        sis_Paciente = query.FirstOrDefault();

                        if (sis_Paciente == null)
                        {
                            sis_Paciente = new Sis_Paciente();
                        }
                        sis_Paciente.Categoria = null;
                        sis_Paciente.CNS = null;
                        sis_Paciente.Codigo = pessoa.Codigo;
                        sis_Paciente.CodPaciente = null; // paciente.Codigo;
                                                         //sis_Paciente.CreatorUserId = null;
                        sis_Paciente.DataUltimaMalaDir = null;
                        sis_Paciente.DeleterUserId = null;
                        sis_Paciente.DeletionTime = null;
                        sis_Paciente.Descricao = pessoa.NomeCompleto;
                        if (pessoa.EscolaridadeId.HasValue)
                        {
                            sis_Paciente.Escolaridade = pessoa.Escolaridade.ImportaId; //(int)pessoa.EscolaridadeId;
                        }
                        sis_Paciente.GrauDependente = null;
                        //sis_Paciente.Id = null;
                        sis_Paciente.IDEmpresaPac = null;
                        sis_Paciente.IDEtnia = null;
                        sis_Paciente.IDExterno = null;
                        sis_Paciente.IDPaciente = pessoa.ImportaId; //null;
                        sis_Paciente.IDSW = (int)sis_pessoaId;
                        if (pessoa.ReligiaoId.HasValue)
                        {
                            sis_Paciente.IDReligiao = pessoa.Religiao.ImportaId; //(int)pessoa.ReligiaoId;
                        }
                        sis_Paciente.IsCartao = null;
                        sis_Paciente.IsDeleted = false;
                        sis_Paciente.IsRecebeEmail = null;
                        sis_Paciente.IsSistema = false;
                        sis_Paciente.IsSUS = null;
                        sis_Paciente.JustificativaNumDeclNascVivo = null;
                        sis_Paciente.LastModificationTime = null;
                        sis_Paciente.LastModifierUserId = null;
                        sis_Paciente.Mae = null;
                        sis_Paciente.Matricula = null;
                        sis_Paciente.NumDeclNascVivo = null;
                        sis_Paciente.Observacao = null;
                        sis_Paciente.Pai = pessoa.NomePai;
                        sis_Paciente.RacaCor = null;
                        sis_Paciente.SenhaAgendaWeb = null;
                        sis_Paciente.SenhaWeb = null;
                        sis_Paciente.UsuarioAgendaWeb = null;
                        sis_Paciente.UsuarioWeb = null;
                        sis_Paciente.ValorEscala = null;
                        sis_Paciente.TenantId = tenantId;

                        if (sis_Paciente.Id == 0)
                        {
                            //_sis_PacienteRepository.Insert(sis_Paciente);
                            context.Sis_PacientesVisualASA.Add(sis_Paciente);
                        }
                        else
                        {
                            //_sis_PacienteRepository.Update(sis_Paciente);
                            context.Entry(sis_Paciente).State = EntityState.Modified;
                        }
                        context.SaveChanges();

                    }
                    //CurrentUnitOfWork.SaveChanges();
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork(false)]
        private void CarregarSisAtendimento(long atendimentoId, string strcontext)
        {
            try
            {
                using (var context = new SWMANAGERDbContext(strcontext))
                {
                    var query = from at in context.Atendimentos  // _atendimentoRepository.GetAll()
                                                        .Include(m => m.AtendimentoStatus)
                                                        .Include(m => m.Empresa)
                                                        .Include(m => m.Especialidade)
                                                        .Include(m => m.UnidadeOrganizacional)
                                                        .Include(m => m.Leito)
                                                        .Include(m => m.Medico)
                                                        .Include(m => m.Origem)
                                                        .Include(m => m.Paciente)
                                                        .Include(m => m.Convenio)
                                                        .Include(m => m.Plano)
                                                        .Include(m => m.TipoAcomodacao)
                                                        .Include(m => m.ServicoMedicoPrestado)
                                                        .Include(m => m.AtendimentoTipo)
                                                        .Include(m => m.FatGuia)
                                where at.Id == atendimentoId
                                select at;

                    var atendimento = query.FirstOrDefault();
                    Sis_Atendimento sis_Atendimento = null;

                    if (atendimento != null)
                    {
                        var query1 = from atAsa in context.Sis_AtendimentosVisualASA //_sis_AtendimentoRepository.GetAll()
                                     where atAsa.IDImportado == atendimentoId
                                     select atAsa;

                        sis_Atendimento = query1.FirstOrDefault();

                        if (sis_Atendimento == null)
                        {
                            sis_Atendimento = new Sis_Atendimento();
                        }
                        sis_Atendimento.AgudaCronica = null;
                        sis_Atendimento.Ano = null;
                        sis_Atendimento.CodAtendimento = null;  //atendimento.Codigo;
                        sis_Atendimento.Codigo = atendimento.Codigo;
                        //sis_Atendimento.CreationTime = null;
                        sis_Atendimento.CreatorUserId = null;
                        sis_Atendimento.DataAlteracao = null;
                        sis_Atendimento.DataAtendimento = atendimento.DataRegistro;
                        sis_Atendimento.DataCancelamento = null;
                        sis_Atendimento.DataCancelaRecebimento = null;
                        sis_Atendimento.DataConclusao = null;
                        sis_Atendimento.DataInclusao = atendimento.DataRegistro;
                        sis_Atendimento.DataMedicoConsulta = atendimento.DataRegistro;
                        sis_Atendimento.DataObsRecebimento = null;
                        sis_Atendimento.DataRecebimento = null;
                        sis_Atendimento.DataRetorno = atendimento.DataRetorno;
                        sis_Atendimento.DeleterUserId = null;
                        sis_Atendimento.DeletionTime = null;
                        sis_Atendimento.Desativado = null;
                        sis_Atendimento.Descricao = atendimento.Descricao;
                        sis_Atendimento.Hidden = null;
                        //sis_Atendimento.Id = "";
                        sis_Atendimento.Idade = null;
                        sis_Atendimento.IDAteMotCancelamento = null;
                        sis_Atendimento.IDAtendimento = null;//antigo
                        sis_Atendimento.IDSW = (int)atendimento.Id;//novo
                                                                   //sis_Atendimento.IDAtendimentoInicial = (int)atendimento.AtendimentoTipoId; //verificar com márcio não faz sentido este relacionamento.
                        if (atendimento.AtendimentoStatusId.HasValue)
                        {
                            sis_Atendimento.IDAtendimentoStatus = atendimento.AtendimentoStatus.ImportaId; // (int)atendimento.AtendimentoStatusId.Value;// esse aqui que apresenta inconsistência
                        }
                        sis_Atendimento.IDClinica = null;
                        sis_Atendimento.IDConvenioAtend = atendimento.Convenio.ImportaId;
                        sis_Atendimento.IDEmpresa = atendimento.Empresa.ImportaId; //(int)atendimento.EmpresaId;
                        sis_Atendimento.IDEspecialidade = atendimento.Especialidade.ImportaId; //(int)atendimento.EspecialidadeId;
                        sis_Atendimento.IDEspecialidadeMedIndica = null;
                        sis_Atendimento.IDFilial = atendimento.UnidadeOrganizacional.ImportaId; //(int)atendimento.UnidadeOrganizacionalId;
                        sis_Atendimento.IDFilialSin = null;
                        sis_Atendimento.IDImportado = atendimento.ImportaId; //(int)atendimento.Id;
                        sis_Atendimento.IDIndicadorAcidente = null;
                        sis_Atendimento.IDMedico = atendimento.Medico.ImportaId; //(int)atendimento.MedicoId;
                        sis_Atendimento.IDMedicoConsulta = atendimento.Medico.ImportaId; //(int)atendimento.MedicoId;
                        sis_Atendimento.IDMedicoIndica = null;
                        sis_Atendimento.IDOrigem = atendimento.Origem.ImportaId; //(int)atendimento.OrigemId;
                        sis_Atendimento.IDPaciente = atendimento.Paciente.ImportaId; //(int)atendimento.PacienteId;
                        sis_Atendimento.IDRevisaoEntrega = null;
                        sis_Atendimento.IDUltUsuConfEmail = false;
                        sis_Atendimento.IDUsuarioAlteracao = null;
                        sis_Atendimento.IDUsuarioCancelamento = null;
                        sis_Atendimento.IDUsuarioCancelaRecebimento = null;
                        sis_Atendimento.IDUsuarioInclusao = null;
                        sis_Atendimento.IDUsuarioObsRecebimento = null;
                        sis_Atendimento.IDUsuarioRecebimento = null;
                        sis_Atendimento.IDUsuarioRetorno = null;
                        sis_Atendimento.IsAlterado = false;
                        sis_Atendimento.IsDeleted = false;
                        sis_Atendimento.IsEncaminhado = null;
                        sis_Atendimento.IsInternou = atendimento.IsInternacao;
                        sis_Atendimento.IsSincronizado = false;
                        sis_Atendimento.IsSistema = false;
                        sis_Atendimento.IsSMSConfirmado = false;
                        sis_Atendimento.IsSMSEnviado = false;
                        sis_Atendimento.JustificativaNumDeclNascVivo = "";
                        sis_Atendimento.LastModificationTime = null;
                        sis_Atendimento.LastModifierUserId = null;
                        sis_Atendimento.Mes = null;
                        sis_Atendimento.ObsRecebimento = "";
                        //sis_Atendimento.ObsRetorno = null;
                        sis_Atendimento.PacienteCaixa = "";
                        sis_Atendimento.System = null;
                        sis_Atendimento.TenantId = tenantId;

                        long atendId = 0;

                        if (sis_Atendimento.Id == 0)
                        {
                            //atendId = _sis_AtendimentoRepository.InsertAndGetId(sis_Atendimento);
                            var result = context.Sis_AtendimentosVisualASA.Add(sis_Atendimento);
                            context.SaveChanges();
                            atendId = result.Id;
                        }
                        else
                        {
                            //_sis_AtendimentoRepository.Update(sis_Atendimento);
                            context.Entry(sis_Atendimento).State = EntityState.Modified;
                            context.SaveChanges();

                            atendId = sis_Atendimento.Id;
                        }

                        if (atendimento.IsAmbulatorioEmergencia)
                        {
                            CarregarSisAmbulatorio(atendimento, atendId, strcontext);
                        }

                        if (atendimento.IsInternacao)
                        {
                            CarregarSisInternacao(atendimento, atendId, strcontext);
                        }
                        //CurrentUnitOfWork.SaveChanges();
                        context.Dispose();
                        CarregarSisContaMedica(atendimento, atendId, strcontext);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork(false)]
        private void CarregarSisAmbulatorio(Atendimento ambulatorio, long sis_ambulatorioId, string strcontext)
        {
            using (var context = new SWMANAGERDbContext(strcontext))
            {
                Sis_Ambulatorio sis_Ambulatorio = null;

                if (ambulatorio != null)
                {
                    var query = from amb in context.Sis_AmbulatoriosVisualASA //_sis_AmbulatorioRepository.GetAll()
                                where amb.IDAmbulatorio == sis_ambulatorioId
                                select amb;

                    sis_Ambulatorio = query.FirstOrDefault();

                    if (sis_Ambulatorio == null)
                    {
                        sis_Ambulatorio = new Sis_Ambulatorio();
                    }

                    sis_Ambulatorio.CodAmbulatorio = null; // ambulatorio.Codigo;
                    sis_Ambulatorio.CodAmbulatorioSUS = null;
                    sis_Ambulatorio.Codigo = ambulatorio.Codigo;
                    //sis_Ambulatorio.CreationTime = null;
                    sis_Ambulatorio.CreatorUserId = null;
                    sis_Ambulatorio.DadosClinicos = null;
                    sis_Ambulatorio.DataAltaAdministrativa = null;
                    sis_Ambulatorio.DataAltaAmbulatorial = null;
                    sis_Ambulatorio.DataAltaMedica = ambulatorio.DataAltaMedica;
                    sis_Ambulatorio.DataAtendAmbulatorial = null;
                    sis_Ambulatorio.DataExame = null;
                    sis_Ambulatorio.DataFimInfoClinicas = null;
                    sis_Ambulatorio.DataFimPreAtend = null;
                    sis_Ambulatorio.DataInicio = ambulatorio.DataRegistro;
                    sis_Ambulatorio.DataIniInfoClinicas = ambulatorio.DataRegistro;
                    sis_Ambulatorio.DataIniPreAtend = ambulatorio.DataPreatendimento;
                    sis_Ambulatorio.DataLiberacao = null;
                    sis_Ambulatorio.DataPreAtend = null;
                    sis_Ambulatorio.DataRetorno = ambulatorio.DataRetorno;
                    sis_Ambulatorio.DataSolicitacao = null;
                    sis_Ambulatorio.DeleterUserId = null;
                    sis_Ambulatorio.DeletionTime = null;
                    sis_Ambulatorio.Descricao = ambulatorio.Descricao;
                    sis_Ambulatorio.Diagnostico = "";
                    //sis_Ambulatorio.Id = ambulatorio.Id;
                    if (ambulatorio.MotivoAltaId.HasValue)
                    {
                        sis_Ambulatorio.IDAlta = ambulatorio.MotivoAlta.ImportaId; //(int)ambulatorio.MotivoAltaId;
                    }
                    sis_Ambulatorio.IDAltaAmbulatorial = null;
                    sis_Ambulatorio.IDAmbulatorio = ambulatorio.ImportaId; //null;//antigo
                    sis_Ambulatorio.IDSW = (int)ambulatorio.Id;//novo
                    sis_Ambulatorio.IDAtendRevisao = null;
                    sis_Ambulatorio.IDMedicoAtendendo = ambulatorio.Medico.ImportaId; //(int)ambulatorio.MedicoId;
                    sis_Ambulatorio.IDMedPreAtend = ambulatorio.Medico.ImportaId; //(int)ambulatorio.MedicoId;
                    sis_Ambulatorio.IDPrioridadeAtendimento = null;
                    sis_Ambulatorio.IDProtocoloEmergencia = null;
                    sis_Ambulatorio.IDSetor = null;
                    sis_Ambulatorio.IDUsuarioAltaInc = null;
                    sis_Ambulatorio.IDUsuarioLiberacao = null;
                    sis_Ambulatorio.IDUsuarioRevelia = null;
                    sis_Ambulatorio.IsAlergiaSzn = null;
                    sis_Ambulatorio.IsAltaRevelia = null;
                    sis_Ambulatorio.IsAtendendo = null;
                    sis_Ambulatorio.IsDeleted = false;
                    sis_Ambulatorio.IsHoraMarcada = ambulatorio.IsAmbulatorioEmergencia;
                    sis_Ambulatorio.IsRevisao = null;
                    sis_Ambulatorio.IsSistema = false;
                    sis_Ambulatorio.IsVacina = null;
                    sis_Ambulatorio.LastModificationTime = null;
                    sis_Ambulatorio.LastModifierUserId = null;
                    sis_Ambulatorio.NumeroSeq = "";
                    sis_Ambulatorio.PrimConsulta = null;
                    sis_Ambulatorio.QualAlergiaSzn = null;
                    if (ambulatorio.ServicoMedicoPrestadoId.HasValue)
                    {
                        sis_Ambulatorio.TipoConsulta = ambulatorio.ServicoMedicoPrestado.ImportaId.Value.ToString();
                    }
                    sis_Ambulatorio.Tratamento = null;
                    sis_Ambulatorio.StatusProntoAtend = null;
                    //sis_Ambulatorio.TipoConsulta = ambulatorio.AtendimentoTipo;
                    sis_Ambulatorio.TenantId = tenantId;

                    if (sis_Ambulatorio.Id == 0)
                    {
                        //_sis_AmbulatorioRepository.Insert(sis_Ambulatorio);
                        context.Sis_AmbulatoriosVisualASA.Add(sis_Ambulatorio);

                    }
                    else
                    {
                        //_sis_AmbulatorioRepository.Update(sis_Ambulatorio);
                        context.Entry(sis_Ambulatorio).State = EntityState.Modified;
                    }
                    context.SaveChanges();
                }
                //CurrentUnitOfWork.SaveChanges();
                context.Dispose();
            }
        }

        [UnitOfWork(false)]
        private void CarregarSisInternacao(Atendimento internacao, long sis_internacaoId, string strcontext)
        {
            try
            {
                using (var context = new SWMANAGERDbContext(strcontext))
                {
                    Sis_Internacao sis_Internacao = null;

                    if (internacao != null)
                    {
                        var query = from intAsa in context.Sis_InternacoesVisualASA //_sis_InternacaoRepository.GetAll()
                                    where intAsa.IDInternacao == sis_internacaoId
                                    select intAsa;

                        sis_Internacao = query.FirstOrDefault();
                        if (sis_Internacao == null)
                        {
                            sis_Internacao = new Sis_Internacao();
                        }
                        sis_Internacao.CEPResponsa = null;
                        sis_Internacao.CGCResponsa = null;
                        sis_Internacao.Cobertura = null;
                        sis_Internacao.Codigo = internacao.Codigo;
                        sis_Internacao.CodInternacao = null; // internacao.Codigo;
                        sis_Internacao.CompResponsa = null;
                        sis_Internacao.CPFResponsa = internacao.CpfResponsavel;
                        //sis_Internacao.CreationTime = null;
                        sis_Internacao.CreatorUserId = null;
                        sis_Internacao.DataAlta = internacao.DataAlta;
                        sis_Internacao.DataAltaAlt = null;
                        sis_Internacao.DataAltaDel = null;
                        sis_Internacao.DataAltaInc = null;
                        sis_Internacao.DataPrevAltaAlt = internacao.DataPrevistaAlta;
                        sis_Internacao.DataPrevAltaDel = null;
                        sis_Internacao.DataPrevAltaInc = null;
                        //sis_Internacao.DataPrevisaoAlta = internacao.DataPrevistaAlta;
                        sis_Internacao.DataPront = null;
                        sis_Internacao.DeleterUserId = null;
                        sis_Internacao.DeletionTime = null;
                        sis_Internacao.Descricao = internacao.Descricao;
                        sis_Internacao.DietaAtual = null;
                        sis_Internacao.EmisIdtResponsa = null;
                        sis_Internacao.EndResponsa = null;
                        //sis_Internacao.Id = internacao.Id;
                        sis_Internacao.IDAcompanhante = null;
                        if (internacao.MotivoAltaId.HasValue)
                        {
                            sis_Internacao.IDAlta = internacao.MotivoAlta.ImportaId; //(int)internacao.MotivoAltaId;
                        }
                        sis_Internacao.IDBairroResponsa = null;
                        sis_Internacao.IDCidadeResponsa = null;
                        if (internacao.AltaGrupoCIDId.HasValue)
                        {
                            sis_Internacao.IDCIDObito = internacao.AltaGrupoCID.ImportaId; //(int)internacao.AltaGrupoCIDId;
                        }
                        sis_Internacao.IDEstadoPac = null;
                        sis_Internacao.IDEstadoResponsa = null;
                        sis_Internacao.IDInternacao = internacao.ImportaId; //null;//antigo
                        sis_Internacao.IDSW = (int)internacao.Id;//novo
                        if (internacao.LeitoId.HasValue)
                        {
                            sis_Internacao.IDLeito = internacao.Leito.ImportaId; //(int)internacao.LeitoId;
                        }
                        if (internacao.TipoAcomodacaoId.HasValue)
                        {
                            sis_Internacao.IDLeitoTipo = internacao.TipoAcomodacao.ImportaId; //(int)internacao.TipoAcomodacaoId;
                        }
                        sis_Internacao.IdtResponsa = internacao.RgResponsavel;
                        sis_Internacao.IDUsuarioAltaAlt = null;
                        sis_Internacao.IDUsuarioAltaDel = null;
                        sis_Internacao.IDUsuarioAltaInc = null;
                        sis_Internacao.IDUsuarioPrevAltaAlt = null;
                        sis_Internacao.IDUsuarioPrevAltaDel = null;
                        sis_Internacao.IDUsuarioPrevAltaInc = null;
                        sis_Internacao.IDUsuarioPront = null;
                        sis_Internacao.IsAborto = null;
                        sis_Internacao.IsAlergiaSzn = null;
                        sis_Internacao.IsAtendRNSalaParto = null;
                        sis_Internacao.IsBxPeso = null;
                        sis_Internacao.IsCesarea = null;
                        sis_Internacao.IsCompNeoNatal = null;
                        sis_Internacao.IsCompPuerperio = null;
                        sis_Internacao.IsDeleted = false;
                        sis_Internacao.IsEletiva = null;
                        sis_Internacao.IsGestacao = null;
                        sis_Internacao.IsInternacaoObstetrica = null;
                        sis_Internacao.IsNormal = null;
                        sis_Internacao.IsObitoNeoNatal = null;
                        sis_Internacao.IsSistema = false;
                        sis_Internacao.IsTransMat = null;
                        sis_Internacao.JustificativaSUS20 = null;
                        sis_Internacao.JustificativaSUS21 = null;
                        sis_Internacao.JustificativaSUS22 = null;
                        sis_Internacao.LastModificationTime = null;
                        sis_Internacao.LastModifierUserId = null;
                        sis_Internacao.NumDeclNascVivos1 = null;
                        sis_Internacao.NumDeclNascVivos2 = null;
                        sis_Internacao.NumDeclNascVivos3 = null;
                        sis_Internacao.NumDeclNascVivos4 = null;
                        sis_Internacao.NumDeclNascVivos5 = null;
                        sis_Internacao.NumObito = null;
                        sis_Internacao.OrgEmisResponsa = null;
                        sis_Internacao.PaisResponsa = null;
                        sis_Internacao.QtdeAlta = null;
                        sis_Internacao.QtdeNascMortos = null;
                        sis_Internacao.QtdeNascVivosPrematuro = null;
                        sis_Internacao.QtdeNascVivosTermo = null;
                        sis_Internacao.QtdeObitoNeonatalPrecoce = null;
                        sis_Internacao.QtdeObitoNeonatalTardio = null;
                        sis_Internacao.QtdeTransf = null;
                        sis_Internacao.QualAlergiaSzn = null;
                        sis_Internacao.QuantFralda = null;
                        sis_Internacao.Responsavel = null;
                        sis_Internacao.SeObitoMulher = null;
                        sis_Internacao.SisPreNatal = null;
                        sis_Internacao.StatusPront = null;
                        sis_Internacao.TemAcompanhante = null;
                        sis_Internacao.TemCafe = null;
                        sis_Internacao.TemFralda = null;
                        sis_Internacao.TemPernoite = null;
                        sis_Internacao.TemRefeicao = null;
                        sis_Internacao.TemRefeicaoAcompanhante = null;
                        sis_Internacao.TvTelefone = null;
                        sis_Internacao.TenantId = tenantId;

                        if (sis_Internacao.Id == 0)
                        {
                            //_sis_InternacaoRepository.Insert(sis_Internacao);
                            context.Sis_InternacoesVisualASA.Add(sis_Internacao);
                        }
                        else
                        {
                            //_sis_InternacaoRepository.Update(sis_Internacao);
                            context.Entry(sis_Internacao).State = EntityState.Modified;
                        }
                        context.SaveChanges();
                    }
                    //CurrentUnitOfWork.SaveChanges();
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork(false)]
        private void CarregarSisContaMedica(Atendimento atendimento, long sis_ContaMedicaId, string strcontext)
        {
            try
            {
                using (var context = new SWMANAGERDbContext(strcontext))
                {
                    var query = from fat in context.FaturamentoConta //_faturamentoContaRepository.GetAll()
                                .Include(m => m.Empresa)
                                .Include(m => m.UnidadeOrganizacional)
                                // .Include(m => m.TipoLeito)
                                .Include(m => m.TipoAcomodacao)
                                where fat.AtendimentoId == atendimento.Id
                                select fat;

                    var fatConta = query.FirstOrDefault();

                    Sis_ContaMedica sis_ContaMedica = null;

                    if (atendimento != null)
                    {
                        var query1 = from fatAsa in context.Sis_ContaMedicasVisualASA //_sis_ContaMedicaRepository.GetAll()
                                     where fatAsa.IDContaMedica == sis_ContaMedicaId
                                     select fatAsa;

                        sis_ContaMedica = query1.FirstOrDefault();

                        if (sis_ContaMedica == null)
                        {
                            sis_ContaMedica = new Sis_ContaMedica();
                        }

                        sis_ContaMedica.CodDependente = fatConta.CodDependente;
                        sis_ContaMedica.Codigo = atendimento.Codigo;
                        //sis_ContaMedica.CreationTime = null;
                        sis_ContaMedica.CreatorUserId = fatConta.CreatorUserId;
                        sis_ContaMedica.DataAlteracao = null;
                        sis_ContaMedica.DataAutorizacao = atendimento.DataAutorizacao; //null;
                        sis_ContaMedica.DataEntrBolAnest = null;
                        sis_ContaMedica.DataEntrCDFilme = null;
                        sis_ContaMedica.DataEntrDescCir = fatConta.DataEntrDescCir;
                        sis_ContaMedica.DataEntrFolhaSala = fatConta.DataEntrFolhaSala;
                        sis_ContaMedica.DataFim = fatConta.DataFim;
                        sis_ContaMedica.DataInicio = fatConta.DataInicio;
                        sis_ContaMedica.DataUltimaConferencia = null;
                        sis_ContaMedica.DataUsuarioResponsavel = null;
                        sis_ContaMedica.DataValidadeSenha = atendimento.ValidadeSenha;
                        sis_ContaMedica.DeleterUserId = null;
                        sis_ContaMedica.DeletionTime = null;
                        sis_ContaMedica.Descricao = null;
                        if (atendimento.DiasAutorizacao.HasValue)
                        {
                            sis_ContaMedica.DiasAutorizados = (int)atendimento.DiasAutorizacao;
                        }
                        sis_ContaMedica.DiaSerie1 = fatConta.DiaSerie1;
                        sis_ContaMedica.DiaSerie10 = fatConta.DiaSerie10;
                        sis_ContaMedica.DiaSerie2 = fatConta.DiaSerie2;
                        sis_ContaMedica.DiaSerie3 = fatConta.DiaSerie3;
                        sis_ContaMedica.DiaSerie4 = fatConta.DiaSerie3;
                        sis_ContaMedica.DiaSerie5 = fatConta.DiaSerie4;
                        sis_ContaMedica.DiaSerie6 = fatConta.DiaSerie5;
                        sis_ContaMedica.DiaSerie7 = fatConta.DiaSerie7;
                        sis_ContaMedica.DiaSerie8 = fatConta.DiaSerie8;
                        sis_ContaMedica.DiaSerie9 = fatConta.DiaSerie9;
                        sis_ContaMedica.DtPagamento = fatConta.DataPagamento;
                        sis_ContaMedica.GuiaOperadora = fatConta.GuiaOperadora;
                        sis_ContaMedica.GuiaPrincipal = fatConta.GuiaPrincipal;
                        //sis_ContaMedica.Id = null;
                        if (atendimento.MotivoAltaId.HasValue)
                        {
                            sis_ContaMedica.IDAlta = atendimento.MotivoAlta.ImportaId; //(int)atendimento.MotivoAltaId;
                        }
                        sis_ContaMedica.IDAtendimento = atendimento.ImportaId; // (int)atendimento.Id;
                                                                               //sis_ContaMedica.IDContaMedica = null;
                        sis_ContaMedica.IDSW = (int)fatConta.Id;
                        sis_ContaMedica.IDConvenio = atendimento.Convenio.ImportaId; //(int)atendimento.ConvenioId;

                        sis_ContaMedica.IDEmpresaPac = null; // fatconta.Empresa.ImportaId; //(int)fatconta.EmpresaId;
                        sis_ContaMedica.IdentAcompanhante = fatConta.IdentAcompanhante;
                        sis_ContaMedica.IDFilialSin = fatConta.UnidadeOrganizacional.ImportaId; //(int)fatconta.UnidadeOrganizacionalId;
                        sis_ContaMedica.IDFormatoMatricula = null;
                        sis_ContaMedica.IDGuia = atendimento.FatGuia.ImportaId; //(int)atendimento.FatGuiaId;
                        sis_ContaMedica.IDImportado = null;
                        //if (fatConta.TipoLeitoId.HasValue)
                        //{
                        //    sis_ContaMedica.IDLeitoTipo = fatConta.TipoLeito.ImportaId; //(int)atendimento.LeitoId;
                        //}
                        if (fatConta.TipoAcomodacaoId.HasValue)
                        {
                            sis_ContaMedica.IDLeitoTipo = fatConta.TipoAcomodacao.ImportaId; //(int)atendimento.LeitoId;
                        }
                        else if (atendimento.IsInternacao)
                        {
                            sis_ContaMedica.IDLeitoTipo = atendimento.TipoAcomodacao.ImportaId;
                        }
                        sis_ContaMedica.IDMedico = atendimento.Medico.ImportaId; // (int)atendimento.MedicoId;
                        sis_ContaMedica.IDPendenciaMotivo = null;
                        sis_ContaMedica.IDPlano = atendimento.Plano.ImportaId; //(int)atendimento.PlanoId;
                        sis_ContaMedica.IDUsuarioAlteracao = null;
                        //if (fatconta.UsuarioConferenciaId.HasValue)
                        //{
                        //    sis_ContaMedica.IDUsuarioConferencia = (int)fatconta.UsuarioConferenciaId;
                        //}
                        sis_ContaMedica.IDUsuarioResponsavel = null;
                        sis_ContaMedica.IndicacaoClinica = null;
                        sis_ContaMedica.IsAlterado = null;
                        sis_ContaMedica.IsAutorizado = null;
                        sis_ContaMedica.IsAutorizador = fatConta.IsAutorizador;
                        sis_ContaMedica.IsComplementar = null;
                        sis_ContaMedica.IsDeleted = false;
                        sis_ContaMedica.IsImprimeGuia = null;
                        sis_ContaMedica.IsSemAutorizacao = null;
                        sis_ContaMedica.IsSincronizado = null;
                        sis_ContaMedica.IsSistema = false;
                        sis_ContaMedica.LastModificationTime = null;
                        sis_ContaMedica.LastModifierUserId = null;
                        sis_ContaMedica.Matricula = atendimento.Matricula;
                        sis_ContaMedica.NumeroGuia = atendimento.GuiaNumero;
                        sis_ContaMedica.NumeroSeq = null;
                        sis_ContaMedica.Observacao = null;//mudar tipo de dados
                        sis_ContaMedica.Ordem = null;
                        sis_ContaMedica.SenhaAutorizacao = atendimento.Senha;
                        sis_ContaMedica.StatusEntrega = null; //mudar tipo de dados
                        if (atendimento.AtendimentoTipoId.HasValue)
                        {
                            sis_ContaMedica.TipoAtendimento = atendimento.AtendimentoTipoId.Value.ToString("00"); //null;//mudar tipo de dados
                        }
                        else if (fatConta.TipoAtendimento > 0)
                        {
                            sis_ContaMedica.TipoAtendimento = fatConta.TipoAtendimento.ToString("00"); //null;//mudar tipo de dados
                        }
                        sis_ContaMedica.Titular = atendimento.Titular;
                        sis_ContaMedica.TrilhaCartao = null;
                        sis_ContaMedica.ValCarteira = atendimento.ValidadeCarteira;
                        sis_ContaMedica.TenantId = tenantId;

                        if (sis_ContaMedica.Id == 0)
                        {
                            //_sis_ContaMedicaRepository.Insert(sis_ContaMedica);
                            context.Sis_ContaMedicasVisualASA.Add(sis_ContaMedica);
                        }
                        else
                        {
                            //_sis_ContaMedicaRepository.Update(sis_ContaMedica);
                            context.Entry(sis_ContaMedica).State = EntityState.Modified;
                        }
                        context.SaveChanges();
                    }
                    //CurrentUnitOfWork.SaveChanges();
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }
        //Fim métodos VisualAsaService

    }
}