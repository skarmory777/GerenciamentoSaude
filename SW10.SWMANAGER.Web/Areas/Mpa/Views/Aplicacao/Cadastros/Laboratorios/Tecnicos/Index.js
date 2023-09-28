(function () {
    $(function () {
        var _$TecnicosTable = $('#TecnicosTable');
        var _TecnicosService = abp.services.app.tecnico;
        var _$filterForm = $('#TecnicosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Tecnico.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Tecnico.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Tecnico.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Tecnicos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Tecnicos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTecnicoModal'
        });

        _$TecnicosTable.jtable({

            title: app.localize('Tecnicos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TecnicosService.listar
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
                                    deleteTecnicos(data.record);
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
                regConselho: {
                    title: app.localize('Registro Conselho'),
                    width: '15%'
                },
            }
        });

        function getTecnicos(reload) {
            if (reload) {
                _$TecnicosTable.jtable('reload');
            } else {
                _$TecnicosTable.jtable('load', {
                    filtro: $('#TecnicosTableFilter').val()
                });
            }
        }

        function deleteTecnicos(Tecnico) {

            abp.message.confirm(
                app.localize('DeleteWarning', Tecnico.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TecnicosService.excluir(Tecnico)
                            .done(function () {
                                getTecnicos(true);
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

        $('#CreateNewTecnicoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarTecnicosParaExcelButton').click(function () {
            _TecnicosService
                .listarParaExcel({
                    filtro: $('#TecnicosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTecnicosButton, #RefreshTecnicosListButton').click(function (e) {
            e.preventDefault();
            getTecnicos();
        });

        abp.event.on('app.CriarOuEditarTecnicoModalSaved', function () {
            getTecnicos(true);
        });

        getTecnicos();

        $('#TecnicosTableFilter').focus();


    });
})();