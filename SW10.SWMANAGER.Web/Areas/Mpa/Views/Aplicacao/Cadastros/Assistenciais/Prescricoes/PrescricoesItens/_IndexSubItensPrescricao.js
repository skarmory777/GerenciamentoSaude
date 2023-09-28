(function($) {
  $(function() {
      const subItensPrescricaoTable = $("#subItensPrescricaoTable");
      const prescricoesItensService = abp.services.app.prescricaoItem;
      var subItensPrescricaoModal = new app.ModalManager({
          viewUrl: abp.appPath + 'Mpa/PrescricoesItens/CriarOuEditarSubItensPrescricaoModal',
          scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_CriarOuEditarSubItensPrescricaoModal.js',
          modalClass: 'CriarOuEditarSubItensPrescricaoItemModal'
      });
      
      subItensPrescricaoTable.jtable({
          title: app.localize('Sub Itens Prescrição'),
          paging: true,
          sorting: true,
          multiSorting: true,
          actions: {
              listAction: {
                  method: prescricoesItensService.listarPorPrescricaoItemId
              }
          },
          fields: {
              id: {
                  key: true,
                  list: false
              },
              actions: {
                  title: app.localize('Actions'),
                  width: '8%',
                  sorting: false,
                  display: function (data) {
                      let $span = $('<span></span>');
                      $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>').appendTo($span).on("click", function (e) {
                          editSubItem(data.record, e);
                      })

                      $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>').appendTo($span).on("click", function (e) {
                          deleteSubItem(data.record, e);
                      })

                      return $span;
                  },
              },
              descricao: {
                  width: '44%',
                  title: app.localize('Descrição'),
                  sorting: false,
              },
              quantidade:{
                  width: '8%',
                  title: app.localize('Quantidade'),
                  sorting: false,
              },
              unidade:{
                  width: '10%',
                  title: app.localize('Unidade'),
                  sorting: false,
              },
              formaDeAplicacao:{
                  width: '10%',
                  title: app.localize('Forma de aplicação'),
                  sorting: false,
              },
              frequencia:{
                  width: '10%',
                  title: app.localize('Frequência'),
                  sorting: false,
              },
              viaDeAplicacao:{
                  width: '10%',
                  title: app.localize('Via de Aplicação'),
                  sorting: false,
              }
          }
      });
      
      $(".btn-add-sub-item-prescricao").on("click",addItem)


      function getSubPrescricoesItens() {
          subItensPrescricaoTable.jtable('load', {
              id: $('#prescricao-item-id').val()
          });
      }
      
      function addItem(e) {
          e.stopImmediatePropagation();
          subItensPrescricaoModal.open({prescricaoItemId: $("#prescricao-item-id").val()});
      }
      
      function editSubItem(data, e)
      {
          e.stopImmediatePropagation();
          subItensPrescricaoModal.open({prescricaoItemId: $("#prescricao-item-id").val(), subItemPrescricaoId:data.id});
      }

      function deleteSubItem(data, e)
      {
          abp.message.confirm(
              app.localize('DeleteWarning', data.descricao),
              function (isConfirmed) {
                  if (isConfirmed) {
                      prescricoesItensService.excluir(data.id)
                          .done(function () {
                              getSubPrescricoesItens();
                              abp.notify.success(app.localize('SuccessfullyDeleted'));
                          });
                  }
              }
          );
      }

      getSubPrescricoesItens();
      abp.event.off('LoadSubPrescricoesItens', getSubPrescricoesItens);
      abp.event.on('LoadSubPrescricoesItens', getSubPrescricoesItens);
      
  })  
})(jQuery);