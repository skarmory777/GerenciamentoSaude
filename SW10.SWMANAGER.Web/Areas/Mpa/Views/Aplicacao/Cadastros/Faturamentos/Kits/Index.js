(function () {
    $(function () {

        var _$KitsTable = $('#KitsTable');
        var _KitsService = abp.services.app.faturamentoKit;
        var _$filterForm = $('#KitsFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Kits.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Kits.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Kits.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturamentoKits/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Kits/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFaturamentoKitModal'
        });

        _$KitsTable.jtable({

            title: app.localize('Kits'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _KitsService.listar
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
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                _createOrEditModal.open({ id: data.record.id });
                            });


                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                deleteKits(data.record);
                            });

                        return $span;
                    }
                }
                ,
                descricao: {
                    title: app.localize('Descricao'),
                    width: '20%'
                }
                ,
                observacao: {
                    title: app.localize('Observacao'),
                    width: '20%'
                }                                
            }
        });

        function getKits(reload) {
            if (reload) {
                _$KitsTable.jtable('reload');
            } else {
                _$KitsTable.jtable('load', {
                    filtro: $('#KitsTableFilter').val()
                });
            }
        }

        function deleteKits(kit) {

            abp.message.confirm(
                app.localize('DeleteWarning', kit.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _KitsService.excluir(kit)
                            .done(function () {
                                getKits(true);
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

        $('#CreateNewKitButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarKitsParaExcelButton').click(function () {
            _KitsService
                .listarParaExcel({
                    filtro: $('#KitsTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetKitsButton, #RefreshKitsListButton').click(function (e) {
            e.preventDefault();
            getKits();
        });

        abp.event.on('app.CriarOuEditarKitModalSaved', function () {
            getKits(true);
        });

        getKits();

        $('#KitsTableFilter').focus();
    });
})();