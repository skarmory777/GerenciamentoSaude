(function () {
    $(function () {       
        $('.fila-painel').each(function () {
            const that = this;
            $(that).click(function (e) {
                
                document.location.href = `/Mpa/TerminalSenhas/TerminalDeSenha?Id=${$(that).data('id')}`;
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