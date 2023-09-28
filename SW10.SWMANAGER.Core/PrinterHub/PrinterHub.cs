// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrinterHub.cs" company="">
//   
// </copyright>
// <summary>
//   The printer hub.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using SW10.SWMANAGER.SignalR;

namespace SW10.SWMANAGER.PrinterHub
{
    using Abp.Dependency;
    using Abp.Runtime.Session;
    using Castle.Core.Logging;
    using Microsoft.AspNet.SignalR;
    using System.Collections.Generic;
    using System.Linq.Dynamic;
    using System.Threading.Tasks;

    /// <inheritdoc />
    public class PrinterHub : BaseHub, ITransientDependency
    {
        /// <summary>
        /// The abp session.
        /// </summary>
        public readonly IAbpSession AbpSession;

        /// <summary>
        /// The logger.
        /// </summary>
        public readonly ILogger Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterHub"/> class.
        /// </summary>
        public PrinterHub()
        {
            this.AbpSession = NullAbpSession.Instance;
            this.Logger = NullLogger.Instance;
        }

        /// <summary>
        /// The document to printer.
        /// </summary>
        /// <param name="printerHubContext">
        /// The printer Hub Context.
        /// </param>
        /// <param name="printerDto">
        /// The printer data transfer object.
        /// </param>
        public static void DocumentToPrinter(IHubContext printerHubContext, PrintDocumentDto printerDto)
        {
            printerHubContext.Clients.Group("printers").printDocument(printerDto);
        }

        /// <summary>
        /// The set printer info.
        /// </summary>
        /// <param name="printerNames">
        /// The printer names.
        /// </param>
        public void SetPrinterInfo(List<string> printerNames)
        {
            if (printerNames.Any())
            {
                this.Groups.Add(this.Context.ConnectionId, "printers");
            }
        }

        /// <inheritdoc />
        public override async Task OnConnected()
        {
            await base.OnConnected().ConfigureAwait(false);
            this.Logger.Debug("A client connected to PrinterHub: " + this.Context.ConnectionId);
        }

        /// <inheritdoc />
        public override async Task OnDisconnected(bool stopCalled)
        {
            await base.OnDisconnected(stopCalled).ConfigureAwait(false);

            this.Logger.Debug("A client disconnected from PrinterHub: " + this.Context.ConnectionId);
        }
    }
}
