(function () {
    $(function () {
        var _$UnidadesTable = $('#UnidadesTable');
        var _UnidadesService = abp.services.app.unidade;
        var _$filterForm = $('#UnidadesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Unidade.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Unidade.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Unidade.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Unidades/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Unidades/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarModal'
        });

        var _createOrEditUnidadeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Unidades/CriarOuEditarUnidadeModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Unidades/_CriarOuEditarUnidadeModal.js',
            modalClass: 'CriarOuEditarUnidadeModal'
        });

        _$UnidadesTable.jtable({
            title: app.localize('Unidades'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _UnidadesService.listar
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
                                    if ((data.record.unidadeReferenciaId != null) && (data.record.unidadeReferenciaId != "") && (data.record.unidadeReferenciaId != undefined)) {
                                        _createOrEditUnidadeModal.open({ id: data.record.id });
                                    } else {
                                        _createOrEditModal.open({ id: data.record.id });
                                    };
                                });
                        }
                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteUnidades(data.record);
                                });
                        }
                        return $span;
                    }
                },
                //id: {
                //    title: app.localize('Codigo'),
                //    width: '8%'
                //},
                //unidadeReferenciaId: {
                //    title: app.localize('UnidadeReferenciaId'),
                //    width: '15%'
                //},
                //unidadeReferenciaId: {
                //    title: app.localize('Principal'),
                //    width: '8%',
                //    listClass: 'Centralizado',
                //    display: function (data) {
                //        if (!data.record.unidadeReferenciaId == 1) {
                //            return '<span class="label label-success" >' + app.localize('Yes') + '</span>';
                //        } else {
                //            return '<span class="label label-default" >' + app.localize('No') + '</span>';
                //        }
                //    }
                //},
                sigla: {
                    title: app.localize('Sigla'),
                    width: '8%'
                },
                fator: {
                    title: app.localize('Fator'),
                    width: '8%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '66%'
                },
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '10%',
                    display: function (data) {
                        return moment(data.record.creationTime).format('L');
                    }
                }
            }
        });

        function getUnidades(reload) {
            if (reload) {
                _$UnidadesTable.jtable('reload');
            } else {
                _$UnidadesTable.jtable('load', {
                    filtro: $('#UnidadesTableFilter').val()
                });
            }
        }

        function deleteUnidades(unidade) {

            abp.message.confirm(
                app.localize('DeleteWarning', unidade.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _UnidadesService.excluir(unidade)
                            .done(function () {
                                getUnidades(true);
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

        $('#CreateNewUnidadeButton').click(function (e) {
            e.preventDefault();
            //debugger;;
            _createOrEditModal.open();
        });

        $('#ExportarUnidadesParaExcelButton').click(function () {
            _UnidadesService
                .listarParaExcel({
                    filtro: $('#UnidadesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetUnidadesButton, #RefreshUnidadesListButton').click(function (e) {
            e.preventDefault();
            getUnidades();
        });

        abp.event.on('app.CriarOuEditarUnidadeModalSaved', function () {
            getUnidades(true);
        });

        getUnidades();

        $('#UnidadesTableFilter').focus();
    });
})();