(function () {
    $(function () {
        var _$talaoChequeFilterFormTable = $('#talaoChequeTable');
        var _talaoChequeService = abp.services.app.talaoCheque;

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.Bancario.TalaoCheque.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.Bancario.TalaoCheque.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.Bancario.TalaoCheque.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TalaoCheque/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/Bancarios/TalaoCheque/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTalaoCheque'
        });

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        _$talaoChequeFilterFormTable.jtable({

            title: app.localize('EditTalaoCheque'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _talaoChequeService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    App.startPageLoading({ animate: true }); document.querySelector('.loadingCommon').style.display = null;
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {                                    
                                    deleteRegistro(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '5%'
                },

                descricao: {
                    title: app.localize('Descricao'),
                    width: '20%'
                },
                numeroInicial: {
                    title: app.localize('NumeroInicial'),
                    width: '5%'
                },
                numeroFinal: {
                    title: app.localize('NumeroFinal'),
                    width: '5%'
                },
                dataAbertura: {
                    title: app.localize('DataAbertura'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.dataAbertura != null) {
                            return moment(data.record.dataAbertura).format('L')
                        }
                        else {
                            return '';
                        }
                    }
                }

                //agencia: {
                //    title: app.localize('Agencia'),
                //    width: '10%',
                //    display: function (data) {
                //        if (data) {                            
                //            return data.record.agencia.descricao || '';
                //        }
                //    }
                //},
               
            }
        });

        function getRegistros(reload) {
            if (reload) {
                _$talaoChequeFilterFormTable.jtable('reload');
            } else {
                _$talaoChequeFilterFormTable.jtable('load', {
                    filtro: $('#tableFilter').val()
                });
            }
        }

        function deleteRegistro(record) {
            abp.message.confirm(
                app.localize('DeleteWarning', record.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        App.startPageLoading({ animate: true }); document.querySelector('.loadingCommon').style.display = null;
                        _talaoChequeService.excluir(record)
                             .done(function (data) {
                                 App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
                                 if (data.errors.length > 0) {
                                     _ErrorModal.open({ erros: data.errors });
                                 }
                                 else {
                                     getRegistros(true);
                                     abp.notify.success(app.localize('SuccessfullyDeleted'));
                                 }
                             }).always(function () {
                                 App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
                             });
                    }
                }
            );
        }

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms);
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewButton').click(function () {
            App.startPageLoading({ animate: true }); document.querySelector('.loadingCommon').style.display = null;
            _createOrEditModal.open();
        });


        $('#GetFeriadosButton, #RefreshFeriadosListButton').click(function (e) {
            e.preventDefault();
            getRegistros();
        });

        abp.event.on('app.CriarOuEditarFeriadoModalSaved', function () {
            getRegistros(true);
        });

        getRegistros();

        $('#tableFilter').focus();

    });
})();