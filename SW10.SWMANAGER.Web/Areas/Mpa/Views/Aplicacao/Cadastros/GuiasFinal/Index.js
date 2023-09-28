(function () {
    $(function () {
        var _$GuiasTable = $('#GuiasTable');
        var _GuiasService = abp.services.app.guia;
        var _$filterForm = $('#GuiasFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.GuiaTipos.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.GuiaTipos.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.GuiaTipos.Delete')
        };

        // Criar ou editar
        var _criarModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/GuiasFinal/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/GuiasFinal/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarGuiaModal'
        });

        // Alterar coordenadas
        var _coordenadaModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/GuiasFinal/CoordenadaModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/GuiasFinal/_CoordenadaModal.js',
            modalClass: 'CoordenadaModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/GuiasNovoModelo/_PermissionsModal.js',
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
                    width: '7%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        // Editar campos
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _criarModal.open({ id: data.record.id });
                                });
                        }
                        // Posicionar campos
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Configurar') + '"><i class="	glyphicon glyphicon-hand-up"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _coordenadaModal.open({ id: data.record.id });
                                });
                        }
                        // Deletar
                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteGuias(data.record);
                                });
                        }

                        return $span;
                    }
                }
                ,
                descricao: {
                    title: app.localize('Descricao'),
                    width: '10%'
                }
                ,
                //complementar: {
                //    title: app.localize('Complementar'),
                //    width: '10%',
                //    display: function (data) {
                //        if (data.record.complementar) {
                //            return data.record.paciente.nomeCompleto;
                //        }
                //    }
                //}
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

        function deleteGuias(Guia) {
            abp.message.confirm(
                app.localize('DeleteWarning', Guia.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _GuiasService.excluir(Guia)
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

        $('#CreateNewGuiaButton').click(function () {
            _criarModal.open();
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

        abp.event.on('app.CriarGuiaModalSaved', function () {
            getGuias(true);
        });

        getGuias();

        $('#GuiasTableFilter').focus();
    });
})();