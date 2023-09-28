(function ($) {
    app.modals.CriarOuEditarGrupoContaAdministrativaModal = function () {

        var _$subGrupoTable = $('#subGrupoTable');


        $(document).ready(function () {

            CamposRequeridos();
        });

        var _grupoContaAdministrativaService = abp.services.app.grupoContaAdministrativa;

        var _modalManager;
        var _$grupoContaAdministrativaInformationsForm = null;


        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$grupoContaAdministrativaInformationsForm = _modalManager.getModal().find('form[name=grupoContaAdministrativaInformationsForm]');
            _$grupoContaAdministrativaInformationsForm.validate();
        };

        this.save = function () {


           
            if (!_$grupoContaAdministrativaInformationsForm.valid()) {
                return;
            }

            var grupoContaAdministrativa = _$grupoContaAdministrativaInformationsForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _grupoContaAdministrativaService.criarOuEditar(grupoContaAdministrativa)
                 .done(function (data) {
                     if (data.errors.length > 0) {
                         _ErrorModal.open({ erros: data.errors });
                     }
                     else {

                         abp.notify.info(app.localize('SavedSuccessfully'));
                         _modalManager.close();
                         abp.event.trigger('app.CriarOuEditarFeriadoModalSaved');
                     }
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        var lista = [];

        $('#inserir').click(function (e) {
            e.preventDefault();
           


            var _$SubGrupoContasAdministrativaInformationsForm = $('form[name=SubGrupoContasAdministrativaInformationsForm]');
            var subGrupo = _$SubGrupoContasAdministrativaInformationsForm.serializeFormToObject();

            if ($('#subGrupos').val() != '') {
                lista = JSON.parse($('#subGrupos').val());
            }

            if ($('#idGrid').val() != '') {

                for (var i = 0; i < lista.length; i++) {
                    if (lista[i].IdGrid == $('#idGrid').val()) {

                        lista[i].Codigo = $('#codigoSubGrupo').val();
                        lista[i].Descricao = $('#descricaoSubGrupo').val();
                        lista[i].IsSubGrupoContaNaoOperacional = $('#isSubGrupoContaNaoOperacional')[0].checked;
                        lista[i].IsUtilizadoCalculoSalario = $('#isUtilizadoCalculoSalario')[0].checked;
                        lista[i].IsSomandoDespesas = $('#isSomadoDespesas')[0].checked;
                        lista[i].IsUsarFormula = $('#isUsaFormula')[0].checked;
                        lista[i].IsNaoDetalharContaAdministrativa = $('#isNaoDetalhaContaAdministrativa')[0].checked;

                        _$subGrupoTable.jtable('updateRecord', {
                            record: lista[i]
                        , clientOnly: true
                        });

                    }
                }
            }
            else {
                subGrupo.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                subGrupo.Codigo = $('#codigoSubGrupo').val();
                subGrupo.Descricao = $('#descricaoSubGrupo').val();
                subGrupo.IsSubGrupoContaNaoOperacional = $('#isSubGrupoContaNaoOperacional')[0].checked;
                subGrupo.IsUtilizadoCalculoSalario = $('#isUtilizadoCalculoSalario')[0].checked;
                subGrupo.IsSomandoDespesas = $('#isSomadoDespesas')[0].checked;
                subGrupo.IsUsarFormula = $('#isUsaFormula')[0].checked;
                subGrupo.IsNaoDetalharContaAdministrativa = $('#isNaoDetalhaContaAdministrativa')[0].checked;


                lista.push(subGrupo);


                _$subGrupoTable.jtable('addRecord', {
                    record: subGrupo
                  , clientOnly: true
                });

            }

            $('#subGrupos').val(JSON.stringify(lista));
            $('#idGrid').val('');

            $('#codigoSubGrupo').val('');
            $('#descricaoSubGrupo').val('');
            $('#isSubGrupoContaNaoOperacional').attr("checked", false);

            $('#isUtilizadoCalculoSalario').attr("checked", false);
            $('#isSomadoDespesas').attr("checked", false);
            $('#isUsaFormula').attr("checked", false);
            $('#isNaoDetalhaContaAdministrativa').attr("checked", false);

            $('#codigoSubGrupo').focus();

        });

        _$subGrupoTable.jtable
       ({
           title: app.localize('Itens'),
           //paging: true,
           sorting: true,
           edit: false,
           create: false,
           multiSorting: true,


           rowInserted: function (event, data) {
               if (data) {
                   if (data.record.ItemSelecionado) {
                       data.row.css("background", "#F5ECCE");
                   }
               }
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

                       $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                           .appendTo($span)
                           .click(function (e) {
                               e.preventDefault();
                               //_createOrEditPreMovimentoItemModal.open({ item: JSON.stringify(data.record) });
                                editSubGrupo(data.record);
                           });

                       $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                         .appendTo($span)
                         .click(function (e) {
                             e.preventDefault();
                               deleteSubGrupo(data.record);
                         });

                       return $span;
                   }
               },

               Codigo: {
                   title: app.localize('Codigo'),
                   width: '10%',
                   display: function (data) {
                       if (data.record.Codigo) {
                           return data.record.Codigo;
                       }
                   }
               },

               Descricao: {
                   title: app.localize('Descricao'),
                   width: '10%',
                   display: function (data) {
                       if (data.record.Descricao) {
                           return data.record.Descricao;
                       }
                   }
               },


           }
       });


        function getSubGrupoTable(reload) {
        

           

            lista = JSON.parse($('#subGrupos').val());


            var allRows = _$subGrupoTable.jtable('selectedRows');

            if (allRows.length > 0) {
                _$subGrupoTable.jtable('deleteRows', { rows: allRows, clientOnly: true });
            }


            for (var i = 0; i < lista.length; i++) {
                var item = lista[i];
                _$subGrupoTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });
            }
        }

        getSubGrupoTable();


        function editSubGrupo(subGrupo) {


            $('#codigoSubGrupo').val(subGrupo.Codigo);
            $('#descricaoSubGrupo').val(subGrupo.Descricao);
            $('#isSubGrupoContaNaoOperacional').attr("checked", subGrupo.IsSubGrupoContaNaoOperacional);
            $('#isUtilizadoCalculoSalario').attr("checked", subGrupo.IsUtilizadoCalculoSalario);
            $('#isSomadoDespesas').attr("checked", subGrupo.IsSomandoDespesas);
            $('#isUsaFormula').attr("checked", subGrupo.IsUsarFormula);
            $('#isNaoDetalhaContaAdministrativa').attr("checked", subGrupo.IsNaoDetalharContaAdministrativa);

            $('#idGrid').val(subGrupo.IdGrid);

        }

        function deleteSubGrupo(subGrupo) {
            abp.message.confirm(
                app.localize('DeleteWarning', subGrupo.Descricao),
                function (isConfirmed) {
                    if (isConfirmed) {



                        lista = JSON.parse($('#subGrupos').val());

                        for (var i = 0; i < lista.length; i++) {
                            if (lista[i].IdGrid == subGrupo.IdGrid) {
                                lista.splice(i, 1);
                                $('#subGrupos').val(JSON.stringify(lista));

                                _$subGrupoTable.jtable('deleteRecord', {
                                    key: subGrupo.IdGrid
                                , clientOnly: true
                                });

                                break;
                            }
                        }

                        // getAutorizacaoItemTable();
                    }
                }
            );
        }


        selectSW('.selectgrupodre', "/api/services/app/grupoDRE/ListarDropdown");

    };
})(jQuery);