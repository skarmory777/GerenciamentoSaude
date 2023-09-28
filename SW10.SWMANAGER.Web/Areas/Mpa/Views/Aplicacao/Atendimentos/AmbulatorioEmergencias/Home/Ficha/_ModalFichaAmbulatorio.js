(function ($) {
    app.modals.ModalFichaAmbulatorio = function () {

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
                url: localStorage["idAtendimento_caminho"],
                method: 'post',
                cache: false,
                async: false,
                success: function (data) {
                   
                    if (!data.includes("System")) {
                        var urlPath = window.location.href.split(window.location.pathname)[0].split("//")[1];
                        $('#fVisualizar').attr('src', "//" + urlPath + "/libs/pdfjs/web/viewer.html?file=" + localStorage["idAtendimento_path"] + data + "&locale=pt-BR");
                        setTimeout(function () {
                            document.querySelector('#fVisualizar').contentDocument.querySelector('html').style.height = null;
                        }, 1000);
                    } else {
                        $('#fVisualizar').attr('src', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT3c6_5BqAspOoQ3noAE_N1iEOzfH5NQGeJ7SqtpWtVe0P2E4TMPg');
                        //window.frames.document.body.style.marginLeft = "29%";
                        //window.frames.document.body.style.marginRight = "25%";
                    }
                    setTimeout(function () {                        
                        App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
                    }, 1000);
                }
            });
        }

    };
})(jQuery);