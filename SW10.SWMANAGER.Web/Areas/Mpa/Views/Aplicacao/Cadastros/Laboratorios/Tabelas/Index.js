(function () {
    $(function () {
        var _$TabelasTable = $('#TabelasTable');
        var _TabelasService = abp.services.app.tabela;
        var _$filterForm = $('#TabelasFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Tabela.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Tabela.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Tabela.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Tabelas/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Tabelas/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTabelaModal'
        });

        _$TabelasTable.jtable({

            title: app.localize('Tabelas'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TabelasService.listar
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
                                    deleteTabelas(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Código'),
                    width: '15%'
                },
                descricao: {
                    title: app.localize('Nome'),
                    width: '15%'
                },
            }
        });

        function getTabelas(reload) {
            if (reload) {
                _$TabelasTable.jtable('reload');
            } else {
                _$TabelasTable.jtable('load', {
                    filtro: $('#TabelasTableFilter').val()
                });
            }
        }

        function deleteTabelas(Tabela) {

            abp.message.confirm(
                app.localize('DeleteWarning', Tabela.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TabelasService.excluir(Tabela)
                            .done(function () {
                                getTabelas(true);
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

        $('#CreateNewTabelaButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarTabelasParaExcelButton').click(function () {
            _TabelasService
                .listarParaExcel({
                    filtro: $('#TabelasTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTabelasButton, #RefreshTabelasListButton').click(function (e) {
            e.preventDefault();
            getTabelas();
        });

        abp.event.on('app.CriarOuEditarTabelaModalSaved', function () {
            getTabelas(true);
        });

        getTabelas();

        $('#TabelasTableFilter').focus();


    });
})();