(function ($) {
    app.modals.CriarOuEditarTipoLocalChamadaModal = function () {

        let _args = null;

        var _$subGrupoTable = $('#subGrupoTable');

        $(document).ready(function () {
            CamposRequeridos();
        });

        var _tipoLocalChamadaService = abp.services.app.tipoLocalChamadaCore;

        var _modalManager;
        var _$tipoLocalChamadaInformationsForm = null;


        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });


        this.init = function (modalManager) {
            
            _args = modalManager.getArgs();
            _modalManager = modalManager;
            App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';

            if (_args.id != undefined) {
                _tipoLocalChamadaService.obter(_args.id)
                 .done(function (data) {
                     $('#tipoLocalChamadaId').val(data.id);
                     $('#codigo').val(data.codigo);
                     $('#descricao').val(data.descricao);
                     for (var i = 0; i < data.localChamadas.length; i++) {
                         lista.push({
                             IdGrid: lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1,
                             Id: data.localChamadas[i].id,
                             TipoLocalChamadaId: data.localChamadas[i].tipoLocalChamadaId,
                             Codigo: data.localChamadas[i].codigo,
                             Descricao: data.localChamadas[i].descricao
                         });
                     }
                     getSubGrupoTable();
                 });
            }

            _$tipoLocalChamadaInformationsForm = _modalManager.getModal().find('form[name=tipoLocalChamadaInformationsForm]');
            _$tipoLocalChamadaInformationsForm.validate();
        };

        this.save = function () {

            if (!_$tipoLocalChamadaInformationsForm.valid()) {
                return;
            }

            var tipoLocalChamada = _$tipoLocalChamadaInformationsForm.serializeFormToObject();

            tipoLocalChamada.localChamadas = lista;
            _modalManager.setBusy(true);

            App.startPageLoading({ animate: true }); document.querySelector('.loadingCommon').style.display = null;
            _tipoLocalChamadaService.criarOuEditar(tipoLocalChamada)
                 .done(function (data) {
                     App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
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
                    App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
                    _modalManager.setBusy(false);
                });
        };

        var lista = [];

        $('#inserir').click(function (e) {

            e.preventDefault();

            var _$LocalChamadaInformationsForm = $('form[name=LocalChamadaInformationsForm]');
            var subGrupo = _$LocalChamadaInformationsForm.serializeFormToObject();

            if ($('#idGrid').val() != '') {

                for (var i = 0; i < lista.length; i++) {
                    if (lista[i].IdGrid == $('#idGrid').val()) {

                        lista[i].Codigo = $('#codigoSubGrupo').val();
                        lista[i].Descricao = $('#descricaoSubGrupo').val();

                        _$subGrupoTable.jtable('updateRecord', {
                            record: lista[i]
                        , clientOnly: true
                        });

                    }
                }
            }
            else {

                if ($('#codigoSubGrupo').val().length == 0 || $('#descricaoSubGrupo').val().length == 0) {
                    return false;
                }

                subGrupo.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                subGrupo.Codigo = $('#codigoSubGrupo').val();
                subGrupo.Descricao = $('#descricaoSubGrupo').val();

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
            $('#codigoSubGrupo').focus();

        });

        _$subGrupoTable.jtable
       ({
           title: app.localize('Itens'),
           //paging: true,
           sorting: false,
           edit: false,
           create: false,
           multiSorting: true,

           //actions: {
           //    listAction: {
           //        method: _tipoLocalChamadaService.listarLocalChamada
           //    }
           //},

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

            //lista = JSON.parse($('#subGrupos').val());

            var allRows = _$subGrupoTable.jtable('selectedRows');

            if (allRows.length > 0) {
                _$subGrupoTable.jtable('deleteRows', { rows: allRows, clientOnly: true });
            }


            for (var i = 0; i < lista.length; i++) {
                var item = lista[i];
                _$subGrupoTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true,
                    filtro: ''
                });
            }
        }

        getSubGrupoTable();


        function editSubGrupo(subGrupo) {
            $('#codigoSubGrupo').val(subGrupo.Codigo);
            $('#descricaoSubGrupo').val(subGrupo.Descricao);

            $('#idGrid').val(subGrupo.IdGrid);
        }

        function deleteSubGrupo(subGrupo) {

            abp.message.confirm(
                app.localize('DeleteWarning', subGrupo.Descricao),
                function (isConfirmed) {
                    if (isConfirmed) {

                        if ($('#subGrupos').val() != undefined) {
                            lista = JSON.parse($('#subGrupos').val());
                        }

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


        selectSW('.selectUnidadeOrganizacional', "/api/services/app/unidadeOrganizacional/ListarDropdown");

    };
})(jQuery);