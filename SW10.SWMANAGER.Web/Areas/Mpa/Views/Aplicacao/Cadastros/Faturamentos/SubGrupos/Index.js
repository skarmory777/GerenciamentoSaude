(function () {
    $(function () {
        var _$SubGruposTable = $('#SubGruposTable');
        var _SubGruposService = abp.services.app.faturamentoSubGrupo;
        var _$filterForm = $('#SubGruposFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Grupos.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Grupos.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Grupos.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturamentoSubGrupos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/SubGrupos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFaturamentoSubGrupoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/SubGrupos/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        _$SubGruposTable.jtable({

            title: app.localize('SubGrupos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _SubGruposService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '30%',
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
                                    deleteSubGrupos(data.record);
                                });
                        }

                        return $span;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '70%'
                }
            }
        });

        function getSubGrupos(reload) {
            if (reload) {
                _$SubGruposTable.jtable('reload');
            } else {
                _$SubGruposTable.jtable('load', {
                    filtro: $('#SubGruposTableFilter').val()
                });
            }
        }

        function deleteSubGrupos(subGrupo) {

            abp.message.confirm(
                app.localize('DeleteWarning', subGrupo.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _SubGruposService.excluir(subGrupo)
                            .done(function () {
                                getSubGrupos(true);
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

        $('#CreateNewSubGrupoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarSubGruposParaExcelButton').click(function () {
            _SubGruposService
                .listarParaExcel({
                    filtro: $('#SubGruposTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetSubGruposButton, #RefreshSubGruposListButton').click(function (e) {
            e.preventDefault();
            getSubGrupos();
        });

        abp.event.on('app.CriarOuEditarSubGrupoModalSaved', function () {
            //getSubGrupos(true);
            //getSubGrupos(false);
            getSubGrupos();
        });

        getSubGrupos();

        $('#SubGruposTableFilter').focus();
    });
})();