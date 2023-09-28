(function () {
    $(function () {
        var _$IndicacoesTable = $('#IndicacoesTable');
        var _IndicacoesService = abp.services.app.indicacao;
        var _$filterForm = $('#IndicacoesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Indicacao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Indicacao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Indicacao.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Indicacoes/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Indicacoes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarIndicacaoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Indicacoes/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$IndicacoesTable.jtable({

            title: app.localize('Indicacoes'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _IndicacoesService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '33%',
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
                                    deleteIndicacoes(data.record);
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
                }
            }
        });

        function getIndicacoes(reload) {
            if (reload) {
                _$IndicacoesTable.jtable('reload');
            } else {
                _$IndicacoesTable.jtable('load', {
                    filtro: $('#IndicacoesTableFilter').val()
                });
            }
        }

        function deleteIndicacoes(Indicacao) {

            abp.message.confirm(
                app.localize('DeleteWarning', Indicacao.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _IndicacoesService.excluir(Indicacao)
                            .done(function () {
                                getIndicacoes(true);
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

        $('#CreateNewIndicacaoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarIndicacoesParaExcelButton').click(function () {
            _IndicacoesService
                .listarParaExcel({
                    filtro: $('#IndicacoesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetIndicacoesButton, #RefreshIndicacoesListButton').click(function (e) {
            e.preventDefault();
            getIndicacoes();
        });

        abp.event.on('app.CriarOuEditarIndicacaoModalSaved', function () {
            getIndicacoes(true);
        });

        getIndicacoes();

        $('#IndicacoesTableFilter').focus();
    });
})();