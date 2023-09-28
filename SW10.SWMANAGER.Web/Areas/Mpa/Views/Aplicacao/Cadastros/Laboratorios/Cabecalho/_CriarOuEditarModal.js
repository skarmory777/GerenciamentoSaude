(function ($) {
   // app.modals.CriarOuEditarFormataModal = function () {
       

     //   this.init = function (modalManager) {

    $(function () {
       
        var _cabecalhoService = abp.services.app.cabecalho;
        
        _$CabecalhoInformationsForm = $('form[name=CabecalhoInformationsForm]');
     

        $('.text-editor').summernote({
      //      tableClassName: 'table table-striped',
            toolbar: [
       ['style', ['bold', 'italic', 'underline']],
       ['fontsize', ['fontsize']],
       ['color', ['color']],
       ['para', ['ul', 'ol', 'paragraph']],
       ['table', ['table']]
            ],
            height: 300,
            width: 800,
            padding: 30
            // NA DOCUMENTACAO TEM VARIOS OUTROS BAGULHOS INTERESSANTES
        });
        $("#formatacao").summernote("code", valorFormatacao);
        // };

        // this.save = function () {

        $('#salvar').click(function (e)
        {
            e.preventDefault();
        

           
            if (!_$CabecalhoInformationsForm.valid()) {
                return;
            }

            var cabecalho = _$CabecalhoInformationsForm.serializeFormToObject();
            cabecalho.DescricaoCabecalho = $('#formatacao').summernote('code');
           
            _cabecalhoService.editar(cabecalho)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarFormataModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        });
   

    });
})(jQuery);