(function () {
    $(function () {
        app.modals.historicoItemModal = function () {
            let modalManager;
            this.init = function (_modalManager) {
                modalManager = _modalManager;
                $.fn.modal.Constructor.prototype.enforceFocus = function () { };
                $(_modalManager.getModal()).find(".modal-dialog").css({ 'width': '80%'});
            }
        }
    })
})()