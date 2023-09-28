(function () {
    $(function () {
        var _$AtestadosTable = $('#AtestadosTable');
        var _AtestadosService = abp.services.app.atestado;
        var _$filterForm = $('#AtestadosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Assistencial.AtestadoMedico.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Assistencial.AtestadoMedico.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Assistencial.AtestadoMedico.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Atestados/_CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Atestados/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarAtestadoModal'
        });

        _$AtestadosTable.jtable({

            title: app.localize('Atestado'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _AtestadosService.listar
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
                                    deleteAtestados(data.record);
                                });
                        }

                        return $span;
                    }
                },
                dataAtendimento: {
                    title: app.localize('DataAtendimento'),
                    width: '20%',
                    display: function (data) {
                        return moment(data.record.creationTime).format('L');
                    }
                },
                medico: {
                    title: app.localize('Medico'),
                    width: '20%',
                    display: function (data) {
                        return data.record.medico ? data.record.medico.nomeCompleto : "";
                    }
                },
                paciente: {
                    title: app.localize('Paciente'),
                    width: '20%',
                    display: function (data) {
                        return data.record.paciente ? data.record.paciente.nomeCompleto : "";
                    }
                },
                tipoAtestado: {
                    title: app.localize('TipoAtestado'),
                    width: '20%',
                    display: function (data) {
                        return data.record.tipoAtestado.descricao
                    }
                },
            }
        });

        function getAtestados(reload) {
            if (reload) {
                _$AtestadosTable.jtable('reload');
            } else {
                _$AtestadosTable.jtable('load', {
                    filtro: $('#AtestadosTableFilter').val()
                });
            }
        }

        function deleteAtestados(atestado) {

            abp.message.confirm(
                app.localize('DeleteWarning', atestado.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _AtestadosService.excluir(atestado)
                            .done(function () {
                                getAtestados(true);
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

        $('#CreateNewAtestadoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarAtestadosParaExcelButton').click(function () {
            _AtestadosService
                .listarParaExcel({
                    filtro: $('#AtestadosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetAtestadosButton, #RefreshAtestadosListButton').click(function (e) {
            e.preventDefault();
            getAtestados();
        });

        abp.event.on('app.CriarOuEditarAtestadoModalSaved', function () {
            getAtestados(true);
        });

        getAtestados();

        $('#AtestadosTableFilter').focus();
    });
})();