(function () {
    $(function () {
        var _$tipoDocumentoFilterFormTable = $('#tipoDocumentoTable');
        var _tipoDocumentoService = abp.services.app.tipoDocumento;
        var _$tipoDocumentoFilterFormFilterForm = $('#tipoDocumentoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.TipoDocumento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.TipoDocumento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.TipoDocumento.Delete')
        };



        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TipoDocumento/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/TipoDocumento/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTipoDocumentoModal'
        });




        _$tipoDocumentoFilterFormTable.jtable({

            title: app.localize('TipoDocumentos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _tipoDocumentoService.listar
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
                    width: '35%'
                }
            }
        });

        function getRegistros(reload) {
            if (reload) {
                _$tipoDocumentoFilterFormTable.jtable('reload');
            } else {
                _$tipoDocumentoFilterFormTable.jtable('load', {
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
                        _tipoDocumentoService.excluir(record)
                            .done(function () {
                                App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
                                getRegistros(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
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