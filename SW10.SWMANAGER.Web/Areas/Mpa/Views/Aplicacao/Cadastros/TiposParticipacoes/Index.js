(function () {
    $(function () {
        var _$TiposParticipacoesTable = $('#TiposParticipacoesTable');
        var _TiposParticipacoesService = abp.services.app.tipoParticipacao;
        var _$filterForm = $('#TiposParticipacoesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoParticipacao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoParticipacao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoParticipacao.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TiposParticipacoes/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/TiposParticipacoes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTipoParticipacaoModal'
        });

        _$TiposParticipacoesTable.jtable({

            title: app.localize('TipoParticipacao'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TiposParticipacoesService.listar
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
                                    deleteTiposParticipacoes(data.record);
                                });
                        }

                        return $span;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '33%'
                },
                criacao: {
                    title: app.localize('Criacao'),
                    width: '33%',
                    display: function (data) {
                        return moment(data.record.criacao).format('L');
                    }
                }
               
            }
        });

        function getTiposParticipacoes(reload) {
            if (reload) {
                _$TiposParticipacoesTable.jtable('reload');
            } else {
                _$TiposParticipacoesTable.jtable('load', {
                    filtro: $('#TiposParticipacoesTableFilter').val()
                });
            }
        }

        function deleteTiposParticipacoes(tipoParticipacao) {

            abp.message.confirm(
                app.localize('DeleteWarning', tipoParticipacao.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TiposParticipacoesService.excluir(tipoParticipacao)
                            .done(function () {
                                getTiposParticipacoes(true);
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

        $('#CreateNewTipoParticipacaoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarTiposParticipacoesParaExcelButton').click(function () {
            _TiposParticipacoesService
                .listarParaExcel({
                    filtro: $('#TiposParticipacoesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTiposParticipacoesButton, #RefreshTiposParticipacoesListButton').click(function (e) {
            e.preventDefault();
            getTiposParticipacoes();
        });

        abp.event.on('app.CriarOuEditarTipoParticipacaoModalSaved', function () {
            getTiposParticipacoes(true);
        });

        getTiposParticipacoes();

        $('#TiposParticipacoesTableFilter').focus();
    });
})();