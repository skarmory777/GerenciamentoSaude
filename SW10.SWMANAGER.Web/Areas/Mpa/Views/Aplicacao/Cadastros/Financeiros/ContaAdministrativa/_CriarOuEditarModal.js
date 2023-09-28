(function ($) {
    app.modals.CriarOuEditarContaAdministrativaModal = function () {


        $(document).ready(function () {
            CamposRequeridos();
            $('#percentual').mask('000,00', { reverse: true });
        });

        var _contaAdministrativaService = abp.services.app.contaAdministrativa;
        var _rateioCentroCustoService = abp.services.app.rateioCentroCusto;

        $.validator.setDefaults({ ignore: ":hidden:not(select)" });

        var _modalManager;
        var _$contaAdministrativaInformationsForm = null;


        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$contaAdministrativaInformationsForm = _modalManager.getModal().find('form[name=contaAdministrativaInformationsForm]');
            _$contaAdministrativaInformationsForm.validate();
        };

        this.save = function () {


           
            _$contaAdministrativaInformationsForm.validate()
            if (!_$contaAdministrativaInformationsForm.valid()) {
                return;
            }

            var contaAdministrativa = _$contaAdministrativaInformationsForm.serializeFormToObject();

            contaAdministrativa.IsReceita = contaAdministrativa.options == 'isReceita';
            contaAdministrativa.IsDespesa = contaAdministrativa.options == 'isDespesa';

            _modalManager.setBusy(true);
            _contaAdministrativaService.criarOuEditar(contaAdministrativa)
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
            //_$rateioItemInformationsForm.validate();

            //if (!_$rateioItemInformationsForm.valid()) {
            //    return;
            //}



            var rateioItem = _$rateioItemInformationsForm.serializeFormToObject();

            if ($('#centroCustoId').val() == '' || $('#percentual').val() == '')
            {
                return;
            }

            if ($('#centrosCustos').val() != '') {
                lista = JSON.parse($('#centrosCustos').val());
            }

          
            if ($('#idGridCentroCusto').val() != '') {

                for (var i = 0; i < lista.length; i++) {
                    if (lista[i].IdGrid == $('#idGridCentroCusto').val()) {



                        var centroCusto = $('#centroCustoId').select2('data');
                        if (centroCusto && centroCusto.length > 0) {

                            lista[i].CentroCustoDescricao = centroCusto[0].text;
                        }
                       

                        lista[i].CentroCustoId = $('#centroCustoId').val();
                        lista[i].PercentualRateio = retirarMascara($('#percentual').val());// $('#percentual').val();
                        lista[i].Id = $('#idRateioCentroCusto').val();
                        lista[i].Exibir = 'true';

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
                rateioItem.PercentualRateio = retirarMascara($('#percentual').val()); //$('#percentual').val();
                rateioItem.Id = $('#idRateioCentroCusto').val();

                rateioItem.Exibir = 'true';

                lista.push(rateioItem);


                _$centroCustoTable.jtable('addRecord', {
                    record: rateioItem
                  , clientOnly: true
                });

            }

            $('#centrosCustos').val(JSON.stringify(lista));
            $('#idGridCentroCusto').val('');

            $('#centroCustoId').val('').trigger('change');
            $('#percentual').val('');
            $('#idRateioCentroCusto').val('');

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


           rowInserted: function (event, data) {

               select: true
           },



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
                       if (data.record.Exibir == 'true') {
                           $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                               .appendTo($span)
                               .click(function (e) {
                                   e.preventDefault();
                                   //_createOrEditPreMovimentoItemModal.open({ item: JSON.stringify(data.record) });
                                   editRateioItem(data.record);
                               });
                       }
                       if (data.record.Exibir == 'true') {
                           $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                             .appendTo($span)
                             .click(function (e) {
                                 e.preventDefault();
                                 deleteRateioItem(data.record);
                             });
                       }
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
                           return formatarValor(data.record.PercentualRateio);
                       }
                   }
               },


           }
       });


        function getSubGrupoTable(reload) {

            lista = JSON.parse($('#centrosCustos').val());

            var allRows = _$centroCustoTable.find('.jtable-data-row')

            $.each(allRows, function () {
                var id = $(this).attr('data-record-key');

                _$centroCustoTable.jtable('deleteRecord', { key: id, clientOnly: true });

            });

            for (var i = 0; i < lista.length; i++) {
                var item = lista[i];
                item.Exibir = $('#exibir').val();
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

            $('#percentual').val(formatarValor(rateioItem.PercentualRateio));

            $('#idRateioCentroCusto').val(rateioItem.Id);
            
            $('#idGridCentroCusto').val(rateioItem.IdGrid);

        }

        function deleteRateioItem(subGrupo) {
            abp.message.confirm(
                app.localize('DeleteWarning', subGrupo.Descricao),
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
                soma += parseFloat(lista[i].PercentualRateio);
            }

            $('#somaPercentual').val(formatarValor(soma));
        }

        function retirarMascara(_valor) {
            var valor = _valor.toString();
            while (valor.indexOf('_') != -1) valor = valor.replace('_', '');
            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

            valor = valor.replace(',', '.');
            return valor;
        }

        $('#rateioCentroCustoId').on('change', function (e) {
            e.preventDefault();

           


            if ($('#rateioCentroCustoId').val() != '' && $('#rateioCentroCustoId').val() != null) {

                $('#dadosInsert').hide();
                $('#exibir').val('false');


                _rateioCentroCustoService.obter2($('#rateioCentroCustoId').val(), { async: false, method: 'post' })
                    .done(function (data) {
                        $('#centrosCustos').val(data);

                    })
                 .always(function () {
                 });
            }
            else {
                $('#dadosInsert').show();
                $('#exibir').val('true');
            }

            getSubGrupoTable();
        });


        var _$empresaTable = $('#empresaTable');




        _$empresaTable.jtable
       ({
           title: app.localize('Empresas'),
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
                               editEmpresa(data.record);
                           });
                       $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                         .appendTo($span)
                         .click(function (e) {
                             e.preventDefault();
                             deleteEmpresa(data.record);
                         });
                       return $span;
                   }
               },

               EmpresaDescricao: {
                   title: app.localize('Empresa'),
                   width: '10%',
                   display: function (data) {
                       if (data.record.EmpresaDescricao) {
                           return data.record.EmpresaDescricao;
                       }
                   }
               },

              


           }
       });




        var listaEmpresa = [];

        $('#inserirEmpresa').click(function (e) {
            e.preventDefault();
           



            if ($('#empresaId').val() == '' || $('#empresaId').val() == null) {
                return;
            }


            var _$empresaInformationsForm = $('form[name=EmpresaInformationsForm]');
            var empresa = _$empresaInformationsForm.serializeFormToObject();

            if ($('#empresas').val() != '') {
                listaEmpresa = JSON.parse($('#empresas').val());
            }

            if ($('#idGridEmpresa').val() != '') {

                for (var i = 0; i < listaEmpresa.length; i++) {
                    if (listaEmpresa[i].IdGrid == $('#idGridEmpresa').val()) {

                        var empresa = $('#empresaId').select2('data');
                        if (empresa && empresa.length > 0) {

                            listaEmpresa[i].EmpresaDescricao = empresa[0].text;
                        }


                        listaEmpresa[i].EmpresaId = $('#empresaId').val();

                        _$empresaTable.jtable('updateRecord', {
                            record: listaEmpresa[i]
                        , clientOnly: true
                        });

                    }
                }
            }
            else {
                empresa.IdGrid = listaEmpresa.length == 0 ? 1 : listaEmpresa[listaEmpresa.length - 1].IdGrid + 1;

                var campoEmpresa = $('#empresaId').select2('data');
                if (campoEmpresa && campoEmpresa.length > 0) {
                    empresa.EmpresaDescricao = campoEmpresa[0].text;
                }
                empresa.EmpresaId = $('#empresaId').val();

                listaEmpresa.push(empresa);
                _$empresaTable.jtable('addRecord', {
                    record: empresa
                  , clientOnly: true
                });
            }

            $('#empresas').val(JSON.stringify(listaEmpresa));
            $('#idGridEmpresa').val('');
            $('#empresaId').val('').trigger('change');
            $('#empresaId').focus();
        
        });

    function getEmpresaTable(reload) {

        listaEmpresa = JSON.parse($('#empresas').val());

        //var allRows = _$centroCustoTable.find('.jtable-data-row')

        //$.each(allRows, function () {
        //    var id = $(this).attr('data-record-key');

        //    _$centroCustoTable.jtable('deleteRecord', { key: id, clientOnly: true });

        //});

        for (var i = 0; i < listaEmpresa.length; i++) {
            var item = listaEmpresa[i];
            //   item.Exibir = $('#exibir').val();
            _$empresaTable.jtable('addRecord', {
                record: item
                , clientOnly: true
            });
        }
    }

    getEmpresaTable();




    function editEmpresa(empresa) {

        $('#empresaId')
          .append($("<option>") //add option tag in select
        .val(empresa.EmpresaId) //set value for option to post it
        .text(empresa.EmpresaDescricao)
      ) //set a text for show in select
.val(empresa.EmpresaId) //select option of select2
      .trigger("change");

        $('#idGridEmpresa').val(empresa.IdGrid);
    }

    function deleteEmpresa(empresa) {

       
        abp.message.confirm(
            app.localize('DeleteWarning', empresa.EmpresaDescricao),
            function (isConfirmed) {
                if (isConfirmed) {



                    listaEmpresa = JSON.parse($('#empresas').val());

                    for (var i = 0; i < listaEmpresa.length; i++) {
                        if (listaEmpresa[i].IdGrid == empresa.IdGrid) {
                            listaEmpresa.splice(i, 1);
                            $('#empresas').val(JSON.stringify(listaEmpresa));

                            _$empresaTable.jtable('deleteRecord', {
                                key: empresa.IdGrid
                            , clientOnly: true
                            });

                            break;
                        }
                    }

                }
            }
        );
    }



    selectSW('.selectsubgrupoContaadministrativa', "/api/services/app/SubGrupoContaAdministrativa/ListarDropdown");
    selectSW('.selectrateiopadrao', "/api/services/app/RateioCentroCusto/ListarDropdown");
    selectSW('.selectcentrocusto', "/api/services/app/centrocusto/ListarDropdownCodigoCentroCustoReceberLancamento");
    selectSW('.selectempresa', "/api/services/app/empresa/ListarDropdownPorUsuario");
        


};
})(jQuery);