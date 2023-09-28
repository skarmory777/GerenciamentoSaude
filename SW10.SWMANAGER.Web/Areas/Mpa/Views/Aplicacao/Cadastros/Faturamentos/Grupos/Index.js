(function () {
    $(function () {
        var _$GruposTable = $('#GruposTable');
        var _GruposService = abp.services.app.faturamentoGrupo;
        var _$filterForm = $('#GruposFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Grupos.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Grupos.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Grupos.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturamentoGrupos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Grupos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFaturamentoGrupoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Grupos/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        _$GruposTable.jtable({

            title: app.localize('Grupos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _GruposService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '5%',
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
                                    deleteGrupos(data.record);
                                });
                        }

                        return $span;
                    }
                }
                ,
                descricao: {
                    title: app.localize('Descricao'),
                    width: '20%'
                }
                ,
                tipo: {
                    title: app.localize('Tipo'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.tipoGrupo) {
                            return data.record.tipoGrupo.descricao;
                        }
                    }
                }
                ,
                isAtivo: {
                    title: app.localize('IsAtivo'),
                    width: '5%'
                }
                ,
                isPacote: {
                    title: app.localize('IsPacote'),
                    width: '5%'
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
                        _GruposService.excluir(grupo)
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
            _createOrEditModal.open();
        });

        $('#ExportarGruposParaExcelButton').click(function () {
            _GruposService
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