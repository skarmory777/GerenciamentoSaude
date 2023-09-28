(function($) {
  app.modals.CriarOuEditarSubItensPrescricaoItemModal = function () {
    let configuracaoPrescricaoItem;
    const prescricaoItemAppService = abp.services.app.prescricaoItem;
    let _modalManager;
    this.init = function(modalManager)
    {
      _modalManager = modalManager;
      $.fn.modal.Constructor.prototype.enforceFocus = function () { };
      $(_modalManager.getModal()).find(".modal-dialog").css({ 'width': '90%'});
      $(_modalManager.getModal()).find(".modal-dialog").css({'height':'850px'})
      configuracaoPrescricaoItem = BuildConfiguracaoPrescricaoItem();
    
      configuracaoPrescricaoItem.renderSubPrescricaoItem();
    }
    
    this.save = function () {
      $(this).buttonBusy(true);
      debugger;
      
      const data = {
        id: $(_modalManager.getModal()).find("[name='subPrescricaoItemId']").val(),
        prescricaoItemId:  $(_modalManager.getModal()).find("[name='prescricaoItemId']").val(),
        codigo: $(_modalManager.getModal()).find("[name='codigo']").val(),
        descricao: $(_modalManager.getModal()).find("[name='descricao']").val(),
      };
      
      prescricaoItemAppService.criarOuEditarSubItem(data).then(res => {
        debugger;
        configuracaoPrescricaoItem.save(res.id).then(() => {
          _modalManager.close();
          abp.event.trigger('LoadSubPrescricoesItens');
        })
      }).always(()=> {
        $(this).buttonBusy(false);
      })
    }
  }
})(jQuery);