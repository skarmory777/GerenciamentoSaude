(function () {
    $(function () {
        var _$CentrosCustosTable = $('#CentrosCustosTable');
        var _CentrosCustosService = abp.services.app.centroCusto;
        var _$filterForm = $('#CentrosCustosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('PPages.Tenant.Cadastros.CadastrosGlobais.CentroCustos.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.CentroCustos.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.CentroCustos.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/CentrosCustos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/CentrosCustos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarCentroCustoModal'
        });

        _$CentrosCustosTable.jtable({

            title: app.localize('CentroCusto'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _CentrosCustosService.listar
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
                                    deleteCentrosCustos(data.record);
                                });
                        }

                        return $span;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '33%'
                },
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '33%',
                    display: function (data) {
                        return moment(data.record.creationTime).format('L');
                    }
                },

            }
        });

        function getCentrosCustos(reload) {
            if (reload) {
                _$CentrosCustosTable.jtable('reload');
            } else {
                _$CentrosCustosTable.jtable('load', {
                    filtro: $('#CentrosCustosTableFilter').val()
                });
            }
        }

        function deleteCentrosCustos(centroCusto) {

            abp.message.confirm(
                app.localize('DeleteWarning', centroCusto.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _CentrosCustosService.excluir(centroCusto)
                            .done(function () {
                                getCentrosCustos(true);
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

        $('#CreateNewCentroCustoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarCentrosCustosParaExcelButton').click(function () {
            _CentrosCustosService
                .listarParaExcel({
                    filtro: $('#CentrosCustosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetCentrosCustosButton, #RefreshCentrosCustosListButton').click(function (e) {
            e.preventDefault();
            getCentrosCustos();
        });

        abp.event.on('app.CriarOuEditarCentroCustoModalSaved', function () {
            getCentrosCustos(true);
        });

        getCentrosCustos();

        $('#CentrosCustosTableFilter').focus();
    });
})();