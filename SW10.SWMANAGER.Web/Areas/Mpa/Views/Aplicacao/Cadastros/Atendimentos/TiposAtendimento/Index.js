(function () {
    $(function () {
        var _$TiposAtendimentoTable = $('#TiposAtendimentoTable');
        var _TiposAtendimentoService = abp.services.app.tipoAtendimento;
        var _$filterForm = $('#TiposAtendimentoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.TipoAtendimento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.TipoAtendimento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.TipoAtendimento.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TiposAtendimento/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Atendimentos/TiposAtendimento/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTipoAtendimentoModal'
        });

        _$TiposAtendimentoTable.jtable({

            title: app.localize('TipoAtendimento'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TiposAtendimentoService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '10%',
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
                                    deleteTiposAtendimento(data.record);
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
                isInternacao: {
                    title: app.localize('IsInternacao'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.isInternacao) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                },
                isAmbulatorioEmergencia: {
                    title: app.localize('IsAmbulatorioEmergencia'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.isAmbulatorioEmergencia) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                }
            }
        });

        function getTiposAtendimento(reload) {
            if (reload) {
                _$TiposAtendimentoTable.jtable('reload');
            } else {
                _$TiposAtendimentoTable.jtable('load', {
                    filtro: $('#TiposAtendimentoTableFilter').val()
                });
            }
        }

        function deleteTiposAtendimento(tipoAtendimento) {

            abp.message.confirm(
                app.localize('DeleteWarning', tipoAtendimento.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TiposAtendimentoService.excluir(tipoAtendimento)
                            .done(function () {
                                getTiposAtendimento(true);
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

        $('#CreateNewTipoAtendimentoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarTiposAtendimentoParaExcelButton').click(function () {
            _TiposAtendimentoService
                .listarParaExcel({
                    filtro: $('#TiposAtendimentoTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTiposAtendimentoButton, #RefreshTiposAtendimentoListButton').click(function (e) {
            e.preventDefault();
            getTiposAtendimento();
        });

        abp.event.on('app.CriarOuEditarTipoAtendimentoModalSaved', function () {
            getTiposAtendimento(true);
        });

        getTiposAtendimento();

        $('#TiposAtendimentoTableFilter').focus();
    });
})();