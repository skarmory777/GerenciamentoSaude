(function () {
    $(function () {
        var _$GrupoCentroCustoTable = $('#GruposCentroCustoTable');
        var _GrupoCentroCustoService = abp.services.app.grupoCentroCusto;
        var _$filterForm = $('#GruposCentroCustoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.GrupoCentroCustos.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.GrupoCentroCustos.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.GrupoCentroCustos.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/GruposCentroCusto/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/GruposCentroCusto/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarGruposCentroCustoModal'
        });

        _$GrupoCentroCustoTable.jtable({

            title: app.localize('GrupoCentroCusto'),
            paging: true,
            //sorting: true,
            //multiSorting: true,

            actions: {
                listAction: {
                    method: _GrupoCentroCustoService.listar
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
                                    deleteGrupoCentroCusto(data.record);
                                });
                        }

                        return $span;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '15%'
                }
                //,
                //tipoGrupoCentroCusto: {
                //    title: app.localize('Tipos de Grupo de Centro de Custo'),
                //    width: '15%',
                //    display: function (data) {
                //        return data.record.tipoGrupoCentroCustos.descricao
                //    }
                //}
            }
        });

        function getGrupoCentroCusto(reload) {
            if (reload) {
                _$GrupoCentroCustoTable.jtable('reload');
            } else {
                _$GrupoCentroCustoTable.jtable('load', {
                    filtro: $('#GrupoCentroCustoTableFilter').val()
                });
            }
        }

        function deleteGrupoCentroCusto(grupoCentroCusto) {

            abp.message.confirm(
                app.localize('DeleteWarning', grupoCentroCusto.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _GrupoCentroCustoService.excluir(grupoCentroCusto)
                            .done(function () {
                                getGrupoCentroCusto(true);
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

        $('#CreateNewGrupoCentroCustoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarGrupoCentroCustoParaExcelButton').click(function () {
            _GrupoCentroCustoService
                .listarParaExcel({
                    filtro: $('#GrupoCentroCustoTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetGrupoCentroCustoButton, #RefreshGrupoCentroCustoListButton').click(function (e) {
            e.preventDefault();
            getGrupoCentroCusto();
        });

        abp.event.on('app.CriarOuEditarGrupoCentroCustoModalSaved', function () {
            getGrupoCentroCusto(true);
        });

        getGrupoCentroCusto();

        $('#GrupoCentroCustoTableFilter').focus();
    });
})();