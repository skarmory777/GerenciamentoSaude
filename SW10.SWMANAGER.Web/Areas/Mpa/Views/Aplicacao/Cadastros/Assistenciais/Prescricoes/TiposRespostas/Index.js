(function () {
    $(function () {
        var _$TiposRespostasTable = $('#TiposRespostasTable');
        var _TiposRespostasService = abp.services.app.tipoResposta;
        var _$filterForm = $('#TiposRespostasFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoResposta.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoResposta.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.TipoResposta.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TiposRespostas/_CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/TiposRespostas/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTipoRespostaModal'
        });

        _$TiposRespostasTable.jtable({

            title: app.localize('TipoResposta'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TiposRespostasService.listar
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
                                    deleteTiposRespostas(data.record);
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

        function getTiposRespostas() {
            //if (reload) {
            //    _$TiposRespostasTable.jtable('reload');
            //} else {
            _$TiposRespostasTable.jtable('load', {
                filtro: $('#TiposRespostasTableFilter').val()
            });
            //}
        }

        function deleteTiposRespostas(tipoResposta) {
            abp.message.confirm(
                app.localize('DeleteWarning', tipoResposta.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TiposRespostasService.excluir(tipoResposta)
                            .done(function () {
                                getTiposRespostas(true);
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

        $('#CreateNewTipoRespostaButton').click(function (e) {
            e.preventDefault();
            _createOrEditModal.open();
        });

        $('#ExportarTiposRespostasParaExcelButton').click(function () {
            _TiposRespostasService
                .listarParaExcel({
                    filtro: $('#TiposRespostasTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTiposRespostasButton, #RefreshTiposRespostasListButton').click(function (e) {
            e.preventDefault();
            getTiposRespostas();
        });

        abp.event.on('app.CriarOuEditarTipoRespostaModalSaved', function () {
            getTiposRespostas();
        });

        getTiposRespostas();

        $('#TiposRespostasTableFilter').focus();
    });
})();