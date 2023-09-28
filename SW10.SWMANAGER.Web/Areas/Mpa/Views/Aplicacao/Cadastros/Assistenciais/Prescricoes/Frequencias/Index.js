(function () {
    $(function () {
        var _$FrequenciasTable = $('#FrequenciasTable');
        var _FrequenciasService = abp.services.app.frequencia;
        var _$filterForm = $('#FrequenciasFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.Frequencia.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.Frequencia.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.Frequencia.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Frequencias/CriarOuEditar',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/Frequencias/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFrequenciaModal'
        });

        _$FrequenciasTable.jtable({

            title: app.localize('Frequencia'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _FrequenciasService.listar
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
                                    deleteFrequencias(data.record);
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
                    width: '50%'
                },
                intervalo: {
                    title: app.localize('Intervalo'),
                    width: '30%'
                }
            }
        });

        function getFrequencias(reload) {
            if (reload) {
                _$FrequenciasTable.jtable('reload');
            } else {
                _$FrequenciasTable.jtable('load', {
                    filtro: $('#FrequenciasTableFilter').val()
                });
            }
        }

        function deleteFrequencias(frequencia) {

            abp.message.confirm(
                app.localize('DeleteWarning', frequencia.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _FrequenciasService.excluir(frequencia)
                            .done(function () {
                                getFrequencias(true);
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

        $('#CreateNewFrequenciaButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarFrequenciasParaExcelButton').click(function () {
            _FrequenciasService
                .listarParaExcel({
                    filtro: $('#FrequenciasTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetFrequenciasButton, #RefreshFrequenciasListButton').click(function (e) {
            e.preventDefault();
            getFrequencias();
        });

        abp.event.on('app.CriarOuEditarFrequenciaModalSaved', function () {
            getFrequencias(true);
        });

        getFrequencias();

        $('#FrequenciasTableFilter').focus();
    });
})();