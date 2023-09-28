(function () {
    $(function () {
        var _$UnidadeTable = $('#UnidadeTable');
        var _UnidadeService = abp.services.app.unidadeLaboratorio;
        var _$filterForm = $('#UnidadeFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Unidade.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Unidade.Edit'),
            delete: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Unidade.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Laboratorio/Cadastros/UnidadesCriarOuEditarModal',
            scriptUrl: abp.appPath + 'scripts/laboratorio/cadastros/unidade/_criaroueditarmodal.js',
            modalClass: 'CriarOuEditarUnidadeModal'
        });


        _$UnidadeTable.jtable({

            title: app.localize('Unidade'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _UnidadeService.paginar
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
                                    deleteUnidade(data.record);
                                });
                        }

                        return $span;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '15%'
                }
            }
        });

        function getUnidade(reload) {
            if (reload) {
                _$UnidadeTable.jtable('reload');
            } else {
                _$UnidadeTable.jtable('load', {
                    filtro: $('#UnidadeTableFilter').val()
                });
            }
        }

        function deleteUnidade(unidade) {

            abp.message.confirm(
                app.localize('DeleteWarning', unidade.Descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _UnidadeService.excluir(unidade)
                            .done(function () {
                                getUnidade(true);
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

        $('#CreateNewUnidadeButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarUnidadeParaExcelButton').click(function () {
            _UnidadeService
                .listarParaExcel({
                    filtro: $('#UnidadeTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetUnidadeButton, #RefreshUnidadeListButton').click(function (e) {
            e.preventDefault();
            getUnidade();
        });

        abp.event.on('app.CriarOuEditarUnidadeModalSaved', function () {
            getUnidade(true);
        });

        getUnidade();

        $('#UnidadeTableFilter').focus();
    });
})();