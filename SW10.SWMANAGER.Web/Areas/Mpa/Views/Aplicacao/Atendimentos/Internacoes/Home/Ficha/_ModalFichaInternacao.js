(function ($) {
    app.modals.ModalFichaInternacao = function () {
       
        var _modalManager;
        
        this.init = function (modalManager) {
            _modalManager = modalManager;
            $('.modal-dialog:last').css('width', '1100px');

            // Design modal de exibicao de pdf
            $('.modal-content').css('border', '1px solid');
            $('.modal-content').css('border-radius', '15px 15px 15px 15px');
            $('.modal-header').css('border', '0px solid');
            $('.modal-header').css('border-radius', '15px 15px 0px 0px');
            $('.modal-body.container-fluid').css('border', '0px solid');
            $('.modal-body.container-fluid').css('border-radius', '0px 0px 15px 15px');
            // Fim - design modal de exibicao de pdf
            exibirRelatorio();
        };

        function exibirRelatorio() {
           
            $.ajax({
                url: localStorage["idAtendimento_caminho_internacao"],
                method: 'post',
                cache: false,
                async: false,
                success: function (data) {
                    var path = window.location.href.split(window.location.pathname)[0].split("//")[1];
                    $('#FichaInternacao').attr('src', "//" + path + "/libs/pdfjs/web/viewer.html?file=" + localStorage["idAtendimento_path_internacao"] + data + "&locale=pt-BR");
                    setTimeout(function () {
                        document.querySelector('#FichaInternacao').contentDocument.querySelector('html').style.height = null
                        App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
                    }, 1000);
                }
            });
        }

    };
})(jQuery);