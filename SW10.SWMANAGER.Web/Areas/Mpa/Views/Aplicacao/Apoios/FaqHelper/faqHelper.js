(function ($) {
    app.modals.faqHelperModal = function () {
        this.init = function (modalManager) {
            _modalManager = modalManager;
            $('.modal-dialog').css({ 'min-width': '95%', 'min-height': '95%' });
        }
    }
});