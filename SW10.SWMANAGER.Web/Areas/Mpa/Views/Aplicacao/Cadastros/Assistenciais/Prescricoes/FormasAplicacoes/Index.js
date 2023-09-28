(function () {
    $(function () {
        var _$FormasAplicacoesTable = $('#FormasAplicacoesTable');
        var _FormasAplicacoesService = abp.services.app.formaAplicacao;
        var _$filterForm = $('#FormasAplicacoesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormaAplicacao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormaAplicacao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.FormaAplicacao.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FormasAplicacoes/CriarOuEditar',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/FormasAplicacoes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFormaAplicacaoModal'
        });

        _$FormasAplicacoesTable.jtable({

            title: app.localize('FormaAplicacao'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _FormasAplicacoesService.listar
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
                                    deleteFormasAplicacoes(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '4%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '8%'
                },
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '2%',
                    display: function (data) {
                        return moment(data.record.creationTime).format('L');
                    }
                }
            }
        });

        function getFormasAplicacoes(reload) {
            if (reload) {
                _$FormasAplicacoesTable.jtable('reload');
            } else {
                _$FormasAplicacoesTable.jtable('load', {
                    filtro: $('#FormasAplicacoesTableFilter').val()
                });
            }
        }

        function deleteFormasAplicacoes(formaAplicacao) {

            abp.message.confirm(
                app.localize('DeleteWarning', formaAplicacao.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _FormasAplicacoesService.excluir(formaAplicacao)
                            .done(function () {
                                getFormasAplicacoes(true);
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

        $('#CreateNewFormaAplicacaoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarFormasAplicacoesParaExcelButton').click(function () {
            _FormasAplicacoesService
                .listarParaExcel({
                    filtro: $('#FormasAplicacoesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetFormasAplicacoesButton, #RefreshFormasAplicacoesListButton').click(function (e) {
            e.preventDefault();
            getFormasAplicacoes();
        });

        abp.event.on('app.CriarOuEditarFormaAplicacaoModalSaved', function () {
            getFormasAplicacoes(true);
        });

        getFormasAplicacoes();

        $('#FormasAplicacoesTableFilter').focus();
    });
})();