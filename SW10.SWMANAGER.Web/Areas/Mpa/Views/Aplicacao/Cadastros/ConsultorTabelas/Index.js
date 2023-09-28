(function () {
    $(function () {
        var _$ConsultorTabelasTable = $('#ConsultorTabelasTable');
        var _ConsultorTabelasService = abp.services.app.consultorTabela;
        var _$filterForm = $('#ConsultorTabelasFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Manutencao.Consultor.Tabela.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Manutencao.Consultor.Tabela.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Manutencao.Consultor.Tabela.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ConsultorTabelas/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ConsultorTabelas/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarConsultorTabelaModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ConsultorTabelas/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        _$ConsultorTabelasTable.jtable({

            title: app.localize('ConsultorTabela'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ConsultorTabelasService.listarTodos
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
                                    deleteConsultorTabelas(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '6%'
                },
                nome: {
                    title: app.localize('Nome'),
                    width: '14%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '30%'
                },
                itemMenu: {
                    title: app.localize('ItemMenu'),
                    width: '10%'
                },
                observacao: {
                    title: app.localize('Observacao'),
                    width: '30%'
                }
            }
        });

        function getConsultorTabelas(reload) {
            if (reload) {
                _$ConsultorTabelasTable.jtable('reload');
            } else {
                _$ConsultorTabelasTable.jtable('load', {
                    filtro: $('#ConsultorTabelasTableFilter').val()
                });
            }
        }

        function deleteConsultorTabelas(consultorTabela) {

            abp.message.confirm(
                app.localize('DeleteWarning', consultorTabela.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ConsultorTabelasService.excluir(consultorTabela)
                            .done(function () {
                                getConsultorTabelas(true);
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

        $('#CreateNewConsultorTabelaButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarConsultorTabelasParaExcelButton').click(function () {
            _ConsultorTabelasService
                .listarParaExcel({
                    filtro: $('#ConsultorTabelasTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetConsultorTabelasButton, #RefreshConsultorTabelasListButton').click(function (e) {
            e.preventDefault();
            getConsultorTabelas();
        });

        abp.event.on('app.CriarOuEditarConsultorTabelaModalSaved', function () {
            getConsultorTabelas(true);
        });

        getConsultorTabelas();

        $('#ConsultorTabelasTableFilter').focus();
    });
})();