(function () {
    $(function () {
        var _$SisMoedasTable = $('#SisMoedasTable');
        var _SisMoedasService = abp.services.app.sisMoeda;
        var _$filterForm = $('#SisMoedasFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.SisMoedas.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.SisMoedas.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.SisMoedas.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturamentoSisMoedas/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/SisMoedas/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFaturamentoSisMoedaModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/SisMoedas/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        _$SisMoedasTable.jtable({

            title: app.localize('SisMoedas'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _SisMoedasService.listar
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
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteSisMoedas(data.record);
                                });
                        }

                        return $span;
                    }
                }
                ,
                descricao: {
                    title: app.localize('Descricao'),
                    width: '20%'
                }
                ,
                tipo: {
                    title: app.localize('Tipo'),
                    width: '10%'
                }
                ,
                isCobraCoch: {
                    title: app.localize('IsCobraCoch'),
                    width: '5%'
                }
               
            }
        });

        function getSisMoedas(reload) {
            if (reload) {
                _$SisMoedasTable.jtable('reload');
            } else {
                _$SisMoedasTable.jtable('load', {
                    filtro: $('#SisMoedasTableFilter').val()
                });
            }
        }

        function deleteSisMoedas(sisMoeda) {

            abp.message.confirm(
                app.localize('DeleteWarning', sisMoeda.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _SisMoedasService.excluir(sisMoeda)
                            .done(function () {
                                getSisMoedas(true);
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

        $('#CreateNewSisMoedaButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarSisMoedasParaExcelButton').click(function () {
            _SisMoedasService
                .listarParaExcel({
                    filtro: $('#SisMoedasTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetSisMoedasButton, #RefreshSisMoedasListButton').click(function (e) {
            e.preventDefault();
            getSisMoedas();
        });

        abp.event.on('app.CriarOuEditarSisMoedaModalSaved', function () {
            getSisMoedas(true);
        });

        getSisMoedas();

        $('#SisMoedasTableFilter').focus();
    });
})();