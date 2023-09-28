(function () {
    $(function () {
        var _$TiposRespostasConfiguracoesTable = $('#TiposRespostasConfiguracoesTable');
        var _TiposRespostasConfiguracoesService = abp.services.app.tipoRespostaConfiguracao;
        var _$filterForm = $('#TiposRespostasConfiguracoesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoRespostaConfiguracao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoRespostaConfiguracao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoRespostaConfiguracao.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TiposRespostasConfiguracoes/_CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/TiposRespostasConfiguracoes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTipoRespostaConfiguracaoModal'
        });

        _$TiposRespostasConfiguracoesTable.jtable({
            title: app.localize('TipoRespostaConfiguracao'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _TiposRespostasConfiguracoesService.listar
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '20%',
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
                                    deleteTiposRespostasConfiguracoes(data.record);
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
            }
        });

        function getTiposRespostasConfiguracoes() {
            _$TiposRespostasConfiguracoesTable.jtable('load', {
                filtro: $('#TiposRespostasConfiguracoesTableFilter').val()
            });
        }

        function deleteTiposRespostasConfiguracoes(tipoRespostaConfiguracao) {
            abp.message.confirm(
                app.localize('DeleteWarning', tipoRespostaConfiguracao.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TiposRespostasConfiguracoesService.excluir(tipoRespostaConfiguracao)
                            .done(function () {
                                getTiposRespostasConfiguracoes(true);
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

        $('#CreateNewTipoRespostaConfiguracaoButton').click(function (e) {
            e.preventDefault();
            _createOrEditModal.open();
        });

        $('#ExportarTiposRespostasConfiguracoesParaExcelButton').click(function () {
            _TiposRespostasConfiguracoesService
                .listarParaExcel({
                    filtro: $('#TiposRespostasConfiguracoesTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTiposRespostasConfiguracoesButton, #RefreshTiposRespostasConfiguracoesListButton').click(function (e) {
            e.preventDefault();
            getTiposRespostasConfiguracoes();
        });

        abp.event.on('app.CriarOuEditarTipoRespostaConfiguracaoModalSaved', function () {
            getTiposRespostasConfiguracoes();
        });

        getTiposRespostasConfiguracoes();

        $('#TiposRespostasConfiguracoesTableFilter').focus();
    });
})();