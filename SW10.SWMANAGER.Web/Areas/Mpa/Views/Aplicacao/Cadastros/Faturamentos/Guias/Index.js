(function () {
    $(function () {
        var _$GuiasTable = $('#GuiasTable');
        var _GuiasService = abp.services.app.faturamentoGuia;
        var _$filterForm = $('#GuiasFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Guias.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Guias.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Guias.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturamentoGuias/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Guias/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFaturamentoGuiaModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Guias/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        _$GuiasTable.jtable({

            title: app.localize('Guias'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _GuiasService.listar
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
                   //     if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                    //    }

                   //     if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteGuias(data.record);
                                });
                 //       }

                        return $span;
                    }
                }
                ,
                descricao: {
                    title: app.localize('Descricao'),
                    width: '20%'
                }
                ,
                isAmbulatorio: {
                    title: app.localize('Ambulatorio'),
                    width: '5%'
                }
                ,
                isInternacao: {
                    title: app.localize('Internacao'),
                    width: '5%'
                }
            }
        });

        function getGuias(reload) {
            if (reload) {
                _$GuiasTable.jtable('reload');
            } else {
                _$GuiasTable.jtable('load', {
                    filtro: $('#GuiasTableFilter').val()
                });
            }
        }

        function deleteGuias(guia) {

            abp.message.confirm(
                app.localize('DeleteWarning', guia.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _GuiasService.excluir(guia)
                            .done(function () {
                                getGuias(true);
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

        $('#CreateNewGuiaButton').click(function (e) {
            e.preventDefault();
            _createOrEditModal.open();
        });

        $('#ExportarGuiasParaExcelButton').click(function () {
            _GuiasService
                .listarParaExcel({
                    filtro: $('#GuiasTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetGuiasButton, #RefreshGuiasListButton').click(function (e) {
            e.preventDefault();
            getGuias();
        });

        abp.event.on('app.CriarOuEditarGuiaModalSaved', function () {
            getGuias(true);
        });

        getGuias();

        $('#GuiasTableFilter').focus();
    });
})();