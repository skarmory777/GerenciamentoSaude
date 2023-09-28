(function ($) {
    app.modals.ModalResumoInternacao = function () {

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
        };

    };
})(jQuery);