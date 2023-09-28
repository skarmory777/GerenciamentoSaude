(function () {
    $(function () {
        var _$EquipamentosTable = $('#EquipamentosTable');
        var _EquipamentosService = abp.services.app.equipamento;
        var _$filterForm = $('#EquipamentosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.EquipamentoInterfaceamento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.EquipamentoInterfaceamento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.EquipamentoInterfaceamento.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Equipamentos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Equipamentos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarEquipamentoModal'
        });

        _$EquipamentosTable.jtable({

            title: app.localize('Equipamentos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _EquipamentosService.listar
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
                                    deleteEquipamentos(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '15%'
                },
                descricao: {
                    title: app.localize('Descrição'),
                    width: '15%'
                },
                tipoLayout : {
                    title: app.localize('Tipo Layout'),
                    width: '15%'
                },
                diretorioOrdem : {
                    title: app.localize('Ordem Diretório'),
                    width: '15%'
                },
                diretorioResultado : {
                    title: app.localize('Resultado Diretório'),
                    width: '15%'
                },
            }
        });

        function getEquipamentos(reload) {
            if (reload) {
                _$EquipamentosTable.jtable('reload');
            } else {
                _$EquipamentosTable.jtable('load', {
                    filtro: $('#EquipamentosTableFilter').val()
                });
            }
        }

        function deleteEquipamentos(Equipamento) {

            abp.message.confirm(
                app.localize('DeleteWarning', Equipamento.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _EquipamentosService.excluir(Equipamento)
                            .done(function () {
                                getEquipamentos(true);
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

        $('#CreateNewEquipamentoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarEquipamentosParaExcelButton').click(function () {
            _EquipamentosService
                .listarParaExcel({
                    filtro: $('#EquipamentosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetEquipamentosButton, #RefreshEquipamentosListButton').click(function (e) {
            e.preventDefault();
            getEquipamentos();
        });
        
        abp.event.on('app.CriarOuEditarEquipamentoModalSaved', function () {
            getEquipamentos(true);
        });

        getEquipamentos();

        $('#EquipamentosTableFilter').focus();


    });
})();