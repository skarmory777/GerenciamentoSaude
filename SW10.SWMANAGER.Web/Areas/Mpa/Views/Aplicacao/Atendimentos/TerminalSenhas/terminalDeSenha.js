(function () {
    $(function () {       

        var _terminalSenhasService = abp.services.app.terminalSenhas;

        $('.fila-painel').each(function () {
            const that = this;
            $(that).click(function (e) {
                
                console.log(that);
                e.preventDefault();
                $("#EtiquetaSenhaImprimindo").modal({ backdrop: 'static', keyboard: false, show: true });
                $(that).buttonBusy(true);
                _terminalSenhasService.gerarSenhaEImprimir(that.id, $.cookie("impressora_terminal_de_senha"))
                  .done(function () {
                      //document.querySelector("#EtiquetaSenhaImprimindo").innerHTML = '<h5 style="text-align: center;">A Impressão está sendo preparada...</h5>';
                      setTimeout(() => {
                          $('#EtiquetaSenhaImprimindo').modal('hide');
                          $(that).buttonBusy(false);
                      }, 3000);
                  });
            });
        });
    });

    $(document).ready(() => {     
        if (!$.cookie("impressora_etiqueta_visitante") || !$.cookie("impressora_etiqueta_paciente") || !$.cookie("impressora_terminal_de_senha")) {
            var _impressorasModal = new app.ModalManager({
                viewUrl: abp.appPath + 'Mpa/Impressoras/ImpressorasModal',
                scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Impressoras/ImpressorasModal.js',
                modalClass: 'ImpressorasModal'
            });
            _impressorasModal.open();
        }

    });

})();

