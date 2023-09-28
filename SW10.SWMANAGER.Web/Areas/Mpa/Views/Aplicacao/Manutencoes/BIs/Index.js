(function () {
    $(function () {
        var _$BIsTable = $('#BIsTable');
        var _BIsService = abp.services.app.bi;
        var _$filterForm = $('#BIsFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Manutencao.BI.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Manutencao.BI.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Manutencao.BI.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/BIs/CriarOuEditar',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Manutencoes/BIs/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarBIModal'
        });

        _$BIsTable.jtable({

            title: app.localize('BI'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _BIsService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '2%',
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
                                    deleteBIs(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '10%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '40%'
                },
                url: {
                    title: app.localize('CreationTime'),
                    width: '40%'
                }
            }
        });

        function getBIs(reload) {
            if (reload) {
                _$BIsTable.jtable('reload');
            } else {
                _$BIsTable.jtable('load', {
                    filtro: $('#BIsTableFilter').val()
                });
            }
        }

        function deleteBIs(bi) {

            abp.message.confirm(
                app.localize('DeleteWarning', bi.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _BIsService.excluir(bi)
                            .done(function () {
                                getBIs(true);
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

        $('#CreateNewBIButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarBIsParaExcelButton').click(function () {
            _BIsService
                .listarParaExcel({
                    filtro: $('#BIsTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetBIsButton, #RefreshBIsListButton').click(function (e) {
            e.preventDefault();
            getBIs();
        });

        abp.event.on('app.CriarOuEditarBIModalSaved', function () {
            getBIs(true);
        });

        getBIs();

        $('#BIsTableFilter').focus();
    });
})();