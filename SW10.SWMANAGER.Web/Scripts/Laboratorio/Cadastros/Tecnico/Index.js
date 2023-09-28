(function () {
    $(function () {
        var _$TecnicoTable = $('#TecnicoTable');
        var _TecnicoService = abp.services.app.tecnico;
        var _$filterForm = $('#TecnicoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Tecnico.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Tecnico.Edit'),
            delete: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Tecnico.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Laboratorio/Cadastros/TecnicoCriarOuEditarModal',
            scriptUrl: abp.appPath + 'scripts/laboratorio/cadastros/tecnico/_criaroueditarmodal.js',
            modalClass: 'CriarOuEditarTecnicoModal'
        });


        _$TecnicoTable.jtable({

            title: app.localize('Tecnico'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TecnicoService.paginar
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
                                    deleteTecnico(data.record);
                                });
                        }

                        return $span;
                    }
                },
                nome: {
                    title: app.localize('Nome'),
                    width: '15%'
                }
            }
        });

        function getTecnico(reload) {
            if (reload) {
                _$TecnicoTable.jtable('reload');
            } else {
                _$TecnicoTable.jtable('load', {
                    filtro: $('#TecnicoTableFilter').val()
                });
            }
        }

        function deleteTecnico(tecnico) {

            abp.message.confirm(
                app.localize('DeleteWarning', tecnico.Descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TecnicoService.excluir(tecnico)
                            .done(function () {
                                getTecnico(true);
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

        $('#ExportarTecnicoParaExcelButton').click(function () {
            _TecnicoService
                .listarParaExcel({
                    filtro: $('#TecnicoTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTecnicoButton, #RefreshTecnicoListButton').click(function (e) {
            e.preventDefault();
            getTecnico();
        });

        abp.event.on('app.CriarOuEditarTecnicoModalSaved', function () {
            getTecnico(true);
        });

        getTecnico();

        $('#TecnicoTableFilter').focus();
    });
})();