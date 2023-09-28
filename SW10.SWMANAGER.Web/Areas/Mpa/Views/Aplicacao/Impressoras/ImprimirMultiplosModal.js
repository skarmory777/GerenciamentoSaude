(function () {
    app.modals.ImprimirMultiplosModal = function() {
        var $imprimirMultiplosForm = null;
        var _modalManager;
        this.init = function (modalManager) {
            _modalManager = modalManager;
            _modalManager.getModal().find('form[name=imprimirMultiplosForm]').parents(".modal-dialog").addClass("modal-sm");

            
            
            $imprimirMultiplosForm = _modalManager.getModal().find('form[name=imprimirMultiplosForm]');
            $imprimirMultiplosForm.validate({
                ignore: "",
                rules: {
                    qty: "required"
                }
            });
        };

        this.save = function() {

            if (!$imprimirMultiplosForm.valid()) {
                return;
            }

            _modalManager.setBusy(true);
            _modalManager.close();

            abp.event.trigger("multiplePrint",
                {
                    targetAction: $("#imprimirMultiplosFormTargetAction").val(),
                    qty: $("#imprimirMultiplosFormQty").val()
                });
        };
    };

})();