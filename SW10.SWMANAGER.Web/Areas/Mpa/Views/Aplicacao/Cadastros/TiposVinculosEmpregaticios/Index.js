(function () {
    $(function () {
        var _$TiposVinculosEmpregaticiosTable = $('#TiposVinculosEmpregaticiosTable');
        var _TiposVinculosEmpregaticiosService = abp.services.app.tipoVinculoEmpregaticio;
        var _$filterForm = $('#TiposVinculosEmpregaticiosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoVinculoEmpregativo.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoVinculoEmpregativo.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoVinculoEmpregativo.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TiposVinculosEmpregaticios/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/TiposVinculosEmpregaticios/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTipoVinculoEmpregaticioModal'
        });

        _$TiposVinculosEmpregaticiosTable.jtable({

            title: app.localize('TipoVinculoEmpregaticio'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TiposVinculosEmpregaticiosService.listar
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
                                    deleteTiposVinculosEmpregaticios(data.record);
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

        function getTiposVinculosEmpregaticios(reload) {
            if (reload) {
                _$TiposVinculosEmpregaticiosTable.jtable('reload');
            } else {
                _$TiposVinculosEmpregaticiosTable.jtable('load', {
                    filtro: $('#TiposVinculosEmpregaticiosTableFilter').val()
                });
            }
        }

        function deleteTiposVinculosEmpregaticios(tipoVinculoEmpregaticio) {

            abp.message.confirm(
                app.localize('DeleteWarning', tipoVinculoEmpregaticio.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TiposVinculosEmpregaticiosService.excluir(tipoVinculoEmpregaticio)
                            .done(function () {
                                getTiposVinculosEmpregaticios(true);
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

        $('#CreateNewTipoVinculoEmpregaticioButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarTiposVinculosEmpregaticiosParaExcelButton').click(function () {
            _TiposVinculosEmpregaticiosService
                .listarParaExcel({
                    filtro: $('#TiposVinculosEmpregaticiosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTiposVinculosEmpregaticiosButton, #RefreshTiposVinculosEmpregaticiosListButton').click(function (e) {
            e.preventDefault();
            getTiposVinculosEmpregaticios();
        });

        abp.event.on('app.CriarOuEditarTipoVinculoEmpregaticioModalSaved', function () {
            getTiposVinculosEmpregaticios(true);
        });

        getTiposVinculosEmpregaticios();

        $('#TiposVinculosEmpregaticiosTableFilter').focus();
    });
})();