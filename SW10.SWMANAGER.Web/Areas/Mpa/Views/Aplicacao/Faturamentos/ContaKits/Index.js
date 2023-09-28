(function () {
    $(function () {
        var _$ContaKitsTable = $('#ContaKitsTable');
        var _ContaKitsService = abp.services.app.contaKit;
        var _$filterForm = $('#ContaKitsFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.ContaKit.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.ContaKit.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.ContaKit.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ContaKits/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ContaKits/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarContaKitModal'
        });

        _$ContaKitsTable.jtable({

            title: app.localize('ContaKits'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ContaKitsService.listar
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
                        var $span = $('<span></span>');
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteContaKits(data.record);
                                });
                        }

                        return $span;
                    }
                },
                nome: {
                    title: app.localize('Nome'),
                    width: '15%'
                }
            }
        });

        function getContaKits(reload) {
            if (reload) {
                _$ContaKitsTable.jtable('reload');
            } else {
                _$ContaKitsTable.jtable('load', {
                    filtro: $('#ContaKitsTableFilter').val(),
                    estadoId: $('#cbo-estados').val() === '' ? 0 : $('#cbo-estados').val()
                });
            }
        }

        function deleteContaKits(contaKit) {

            abp.message.confirm(
                app.localize('DeleteWarning', contaKit.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ContaKitsService.excluir(contaKit)
                            .done(function () {
                                getContaKits(true);
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

        $('#CreateNewContaKitButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarContaKitsParaExcelButton').click(function () {
            _ContaKitsService
                .listarParaExcel({
                    filtro: $('#ContaKitsTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetContaKitsButton, #RefreshContaKitsListButton').click(function (e) {
            e.preventDefault();
            getContaKits();
        });

        //$('#cbo-estados').change(function (e) {
        //    e.preventDefault();
        //    getContaKits();
        //});
        abp.event.on('app.CriarOuEditarContaKitModalSaved', function () {
            getContaKits(true);
        });

        getContaKits();

        $('#ContaKitsTableFilter').focus();


    });
})();