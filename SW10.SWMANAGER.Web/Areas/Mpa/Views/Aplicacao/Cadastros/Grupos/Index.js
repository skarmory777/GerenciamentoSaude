(function () {
    $(function () {
        var _$GruposTable = $('#GruposTable');
        var _gruposService = abp.services.app.grupo;
        var _$filterForm = $('#GruposFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Grupo.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Grupo.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Grupo.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Grupos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Grupos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarModal'
        });

        _$GruposTable.jtable({
            title: app.localize('Grupos'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _gruposService.listar
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
                                    if ((data.record.grupoReferenciaId != null) && (data.record.grupoReferenciaId != "") && (data.record.grupoReferenciaId != undefined)) {
                                        _createOrEditGrupoModal.open({ id: data.record.id });
                                    } else {
                                        _createOrEditModal.open({ id: data.record.id });
                                    };
                                });
                        }
                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteGrupos(data.record);
                                });
                        }
                        return $span;
                    }
                },
                //id: {
                //    title: app.localize('Codigo'),
                //    width: '8%'
                //},
                //grupoReferenciaId: {
                //    title: app.localize('GrupoReferenciaId'),
                //    width: '15%'
                //},
                //grupoReferenciaId: {
                //    title: app.localize('Principal'),
                //    width: '8%',
                //    listClass: 'Centralizado',
                //    display: function (data) {
                //        if (!data.record.grupoReferenciaId == 1) {
                //            return '<span class="label label-success" >' + app.localize('Yes') + '</span>';
                //        } else {
                //            return '<span class="label label-default" >' + app.localize('No') + '</span>';
                //        }
                //    }
                //},
                //sigla: {
                //    title: app.localize('Sigla'),
                //    width: '8%'
                //},
                codigo: {
                    title: app.localize('Codigo'),
                    width: '8%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '58%'
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

        function getGrupos(reload) {
            if (reload) {
                _$GruposTable.jtable('reload');
            } else {
                _$GruposTable.jtable('load', {
                    filtro: $('#GruposTableFilter').val()
                });
            }
        }

        function deleteGrupos(grupo) {

            abp.message.confirm(
                app.localize('DeleteWarning', grupo.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _gruposService.excluir(grupo)
                            .done(function () {
                                getGrupos(true);
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

        $('#CreateNewGrupoButton').click(function (e) {
            e.preventDefault();
            //debugger;;
            _createOrEditModal.open();
        });

        $('#ExportarGruposParaExcelButton').click(function () {
            _gruposService
                .listarParaExcel({
                    filtro: $('#GruposTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetGruposButton, #RefreshGruposListButton').click(function (e) {
            e.preventDefault();
            getGrupos();
        });

        abp.event.on('app.CriarOuEditarGrupoModalSaved', function () {
            getGrupos(true);
        });

        getGrupos();

        $('#GruposTableFilter').focus();
    });
})();