using Hangfire;

namespace SW10.SWMANAGER.ClassesAplicacao.DisparoDeMensagem
{
    using Abp.BackgroundJobs;
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using SW10.SWMANAGER.Senders.WhatsApp;
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class DisparoDeMensagemJob : BackgroundJob<DisparoDeMensagemJobArgs>, ITransientDependency
    {
        [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Delete, Attempts = 5)]
        [UnitOfWork]
        public override void Execute(DisparoDeMensagemJobArgs args)
        {
            try
            {
                var enableDisparoDeMensagem = bool.Parse(ConfigurationManager.AppSettings.Get("EnableDisparoDeMensagem"));
                if (!enableDisparoDeMensagem)
                {
                    return;
                }
                
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    unitOfWorkManager.Object.Current.SetTenantId(args.TenantId);
                    using (var disparoDeMensagemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<DisparoDeMensagem, long>>())
                    {
                        var disparoDeMensagem = disparoDeMensagemRepository.Object.GetAll()
                            .Include(x => x.DisparoDeMensagemItems)
                            .Include(x => x.DisparoDeMensagemItems.Select(z => z.Pessoa))
                            .FirstOrDefault(x => x.Id == args.Id);

                        if (disparoDeMensagem == null || disparoDeMensagem.DisparoAtivo ||
                            disparoDeMensagem.DataFinalDisparo.HasValue)
                        {
                            return;
                        }

                        disparoDeMensagem.DisparoAtivo = true;
                        if (!disparoDeMensagem.DataInicioDisparo.HasValue)
                        {
                            disparoDeMensagem.DataInicioDisparo = DateTime.Now;
                        }

                        disparoDeMensagemRepository.Object.Update(disparoDeMensagem);

                        foreach (var disparoDeMensagemItem in disparoDeMensagem.DisparoDeMensagemItems.Where(x =>
                            !x.DataInicioDisparo.HasValue || !x.DataFinalDisparo.HasValue))
                        {
                            if (disparoDeMensagemItem.Valor.IsNullOrEmpty())
                            {
                                return;
                            }

                            if (!disparoDeMensagemItem.DataInicioDisparo.HasValue)
                            {
                                disparoDeMensagemItem.DataInicioDisparo = DateTime.Now;
                            }

                            DisparoDeMensagemItemAction(disparoDeMensagemItem);
                            disparoDeMensagemItem.DataFinalDisparo = DateTime.Now;
                        }

                        disparoDeMensagem.DataFinalDisparo = DateTime.Now;
                        disparoDeMensagem.DisparoAtivo = false;
                        disparoDeMensagem.TotalEnviado =
                            disparoDeMensagem.DisparoDeMensagemItems.Count(x => x.DataFinalDisparo.HasValue);

                        disparoDeMensagemRepository.Object.Update(disparoDeMensagem);
                    }
                }
            }
            catch (Exception)
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    unitOfWorkManager.Object.Current.SetTenantId(args.TenantId);
                    using (var disparoDeMensagemRepository =
                        IocManager.Instance.ResolveAsDisposable<IRepository<DisparoDeMensagem, long>>())
                    {
                        var disparoDeMensagem = disparoDeMensagemRepository.Object.FirstOrDefault(args.Id);
                        if (disparoDeMensagem == null || disparoDeMensagem.IsTransient())
                        {
                            throw;
                        }
                        
                        disparoDeMensagem.DisparoAtivo = false;
                        disparoDeMensagemRepository.Object.Update(disparoDeMensagem);
                    }
                }

                throw;
            }
        }

        private static void DisparoDeMensagemItemAction(DisparoDeMensagemItem disparoDeMensagemItem)
        {
            if (disparoDeMensagemItem.DisparoDeMensagemItemTipoId == DisparoDeMensagemItemTipo.WhatsApp)
            {
                var mensagem = string.Empty;
                if (!disparoDeMensagemItem.Titulo.IsNullOrEmpty())
                {
                    mensagem = $"{disparoDeMensagemItem.Titulo} {Environment.NewLine}{disparoDeMensagemItem.Mensagem}";
                }
                else
                {
                    mensagem = disparoDeMensagemItem.Mensagem;
                }
                var whatsAppEnvio = new WhatsAppEnvio(new WhatsAppEnvioInformacoes(disparoDeMensagemItem.Pessoa.NomeCompleto, FormatTelefoneFormatter("55" + disparoDeMensagemItem.Valor), mensagem));

                whatsAppEnvio.Enviar();
            }
        }

        private static long FormatTelefoneFormatter(string telefone)
        {
            var result = new StringBuilder();

            foreach (char c in telefone)
            {
                if (!char.IsDigit(c))
                {
                    continue;
                }

                result.Append(c);
            }

            long returnLong;
            if(!long.TryParse(result.ToString(),out returnLong))
            {
                throw new Exception("Não foi possivel enviar para o telefone" + result.ToString());
            }
            return returnLong;
        }
    }
}
