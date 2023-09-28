(function ($) {
    app.modals.AtendimentoEtiquetaModal = function () {
        var _modalManager;
        this.init = function (modalManager) {
            _modalManager = modalManager;
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
        };
        $('#btn-visualizar').on('click', function (e) {
            e.preventDefault();
            var linhas = $('#linhas').val();
            //_modalManager.close();
            _modalManager.open({ linhas: linhas });
        });
    };
})(jQuery);