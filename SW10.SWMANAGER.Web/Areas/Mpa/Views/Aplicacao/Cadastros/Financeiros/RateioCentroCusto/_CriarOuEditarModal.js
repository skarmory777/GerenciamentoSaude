(function ($) {
    app.modals.CriarOuEditarRateioCentroCustoModal = function () {


        $(document).ready(function () {

            $('#percentual').mask('000,00', { reverse: true });
        });

        var _rateioCentroCustoService = abp.services.app.rateioCentroCusto;

        var _modalManager;
        var _$rateioCentroCustoInformationsForm = null;


        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$rateioCentroCustoInformationsForm = _modalManager.getModal().find('form[name=rateioCentroCustoInformationsForm]');
            _$rateioCentroCustoInformationsForm.validate();
        };

        this.save = function () {


           
            //if (!_$FormaPagamentoInformationsForm.valid()) {
            //    return;
            //}

            var rateioCentroCusto = _$rateioCentroCustoInformationsForm.serializeFormToObject();


            _modalManager.setBusy(true);
            _rateioCentroCustoService.criarOuEditar(rateioCentroCusto)
                 .done(function (data) {
                     if (data.errors.length > 0) {
                         _ErrorModal.open({ erros: data.errors });
                     }
                     else {

                         abp.notify.info(app.localize('SavedSuccessfully'));
                         _modalManager.close();
                         abp.event.trigger('app.CriarOuEditarFeriadoModalSaved');
                         //location.reload();//seguindo o projeto pronto
                     }
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };








        var _$centroCustoTable = $('#centroCustoTable');

        var lista = [];

        $('#inserir').click(function (e) {
            e.preventDefault();
           


            var _$rateioItemInformationsForm = $('form[name=RateioItemInformationsForm]');
            var rateioItem = _$rateioItemInformationsForm.serializeFormToObject();

            if ($('#centrosCustos').val() != '') {
                lista = JSON.parse($('#centrosCustos').val());
            }

            if ($('#idGrid').val() != '') {

                for (var i = 0; i < lista.length; i++) {
                    if (lista[i].IdGrid == $('#idGrid').val()) {



                        var centroCusto = $('#centroCustoId').select2('data');
                        if (centroCusto && centroCusto.length > 0) {

                            lista[i].CentroCustoDescricao = centroCusto[0].text;
                        }

                        lista[i].CentroCustoId = $('#centroCustoId').val();
                        lista[i].PercentualRateio = $('#percentual').val();

                        _$centroCustoTable.jtable('updateRecord', {
                            record: lista[i]
                        , clientOnly: true
                        });

                    }
                }
            }
            else {
                rateioItem.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;




                var centroCusto = $('#centroCustoId').select2('data');
                if (centroCusto && centroCusto.length > 0) {

                    rateioItem.CentroCustoDescricao = centroCusto[0].text;
                }

                rateioItem.CentroCustoId = $('#centroCustoId').val();
                rateioItem.PercentualRateio =  $('#percentual').val();


                lista.push(rateioItem);


                _$centroCustoTable.jtable('addRecord', {
                    record: rateioItem
                  , clientOnly: true
                });

            }

            $('#centrosCustos').val(JSON.stringify(lista));
            $('#idGrid').val('');

            $('#centroCustoId').val('').trigger('change');
            $('#percentual').val('');

            exibirSomaPercentual();
            $('#centroCustoId').focus();

        });

        _$centroCustoTable.jtable
       ({
           title: app.localize('Itens'),
           //paging: true,
           sorting: true,
           edit: false,
           create: false,
           multiSorting: true,

           fields:
           {
               IdGrid: {
                   key: true,
                   list: false
               },
               actions: {
                   title: app.localize('Actions'),
                   width: '7%',
                   sorting: false,
                   display: function (data) {
                       var $span = $('<span></span>');

                       $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                           .appendTo($span)
                           .click(function (e) {
                               e.preventDefault();
                               //_createOrEditPreMovimentoItemModal.open({ item: JSON.stringify(data.record) });
                               editRateioItem(data.record);
                           });

                       $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                         .appendTo($span)
                         .click(function (e) {
                             e.preventDefault();
                             deleteRateioItem(data.record);
                         });

                       return $span;
                   }
               },

               CentroCustoDescricao: {
                   title: app.localize('CentroCusto'),
                   width: '10%',
                   display: function (data) {
                       if (data.record.CentroCustoDescricao) {
                           return data.record.CentroCustoDescricao;
                       }
                   }
               },

               Percentual: {
                   title: app.localize('Percentual'),
                   width: '10%',
                   display: function (data) {
                       if (data.record.PercentualRateio) {
                           return data.record.PercentualRateio;
                       }
                   }
               },


           }
       });


        function getSubGrupoTable(reload) {


           

            lista = JSON.parse($('#centrosCustos').val());


            var allRows = _$centroCustoTable.jtable('selectedRows');

            if (allRows.length > 0) {
                _$centroCustoTable.jtable('deleteRows', { rows: allRows, clientOnly: true });
            }


            for (var i = 0; i < lista.length; i++) {
                var item = lista[i];
                _$centroCustoTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });
            }
        }

        getSubGrupoTable();


        function editRateioItem(rateioItem) {

            $('#centroCustoId')
              .append($("<option>") //add option tag in select
            .val(rateioItem.CentroCustoId) //set value for option to post it
            .text(rateioItem.CentroCustoDescricao)
          ) //set a text for show in select
    .val(rateioItem.CentroCustoId) //select option of select2
          .trigger("change");

            $('#percentual').val(rateioItem.PercentualRateio);

            $('#idGrid').val(rateioItem.IdGrid);

        }

        function deleteRateioItem(subGrupo) {
            abp.message.confirm(
                app.localize('DeleteWarning', subGrupo.CentroCustoDescricao),
                function (isConfirmed) {
                    if (isConfirmed) {

                        lista = JSON.parse($('#centrosCustos').val());

                        for (var i = 0; i < lista.length; i++) {
                            if (lista[i].IdGrid == subGrupo.IdGrid) {
                                lista.splice(i, 1);
                                $('#centrosCustos').val(JSON.stringify(lista));

                                _$centroCustoTable.jtable('deleteRecord', {
                                    key: subGrupo.IdGrid
                                , clientOnly: true
                                });

                                break;
                            }
                        }

                        exibirSomaPercentual();
                    }
                }
            );
        }

        function exibirSomaPercentual() {
            var soma = 0;
           
            for (var i = 0; i < lista.length; i++) {
                soma += parseFloat(retirarMascara(lista[i].PercentualRateio));
            }

            $('#somaPercentual').val(soma);
        }

        function retirarMascara(_valor) {


            var valor = _valor.toString();
            while (valor.indexOf('_') != -1) valor = valor.replace('_', '');
            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

            valor = valor.replace(',', '.');

            return valor;
        }

        selectSW('.selectcentrocusto', "/api/services/app/centrocusto/ListarDropdownCodigoCentroCusto");


    };
})(jQuery);