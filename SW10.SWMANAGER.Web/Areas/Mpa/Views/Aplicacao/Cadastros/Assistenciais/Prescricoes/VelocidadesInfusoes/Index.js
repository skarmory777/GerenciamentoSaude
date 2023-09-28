(function () {
    $(function () {
        var _$VelocidadesInfusoesTable = $('#VelocidadesInfusoesTable');
        var _VelocidadesInfusoesService = abp.services.app.velocidadeInfusao;
        var _$filterForm = $('#VelocidadesInfusoesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.VelocidadeInfusao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.VelocidadeInfusao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.VelocidadeInfusao.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/VelocidadesInfusoes/CriarOuEditar',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/VelocidadesInfusoes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarVelocidadeInfusaoModal'
        });

        _$VelocidadesInfusoesTable.jtable({

            title: app.localize('VelocidadeInfusao'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _VelocidadesInfusoesService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '2%',
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
                                    deleteVelocidadesInfusoes(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '4%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '8%'
                },
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '2%',
                    display: function (data) {
                        return moment(data.record.creationTime).format('L');
                    }
                }
            }
        });

        function getVelocidadesInfusoes(reload) {
            if (reload) {
                _$VelocidadesInfusoesTable.jtable('reload');
            } else {
                _$VelocidadesInfusoesTable.jtable('load', {
                    filtro: $('#VelocidadesInfusoesTableFilter').val()
                });
            }
        }

        function deleteVelocidadesInfusoes(velocidadeInfusao) {

            abp.message.confirm(
                app.localize('DeleteWarning', velocidadeInfusao.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _VelocidadesInfusoesService.excluir(velocidadeInfusao)
                            .done(function () {
                                getVelocidadesInfusoes(true);
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

        $('#CreateNewVelocidadeInfusaoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarVelocidadesInfusoesParaExcelButton').click(function () {
            _VelocidadesInfusoesService
                .listarParaExcel({
                    filtro: $('#VelocidadesInfusoesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetVelocidadesInfusoesButton, #RefreshVelocidadesInfusoesListButton').click(function (e) {
            e.preventDefault();
            getVelocidadesInfusoes();
        });

        abp.event.on('app.CriarOuEditarVelocidadeInfusaoModalSaved', function () {
            getVelocidadesInfusoes(true);
        });

        getVelocidadesInfusoes();

        $('#VelocidadesInfusoesTableFilter').focus();
    });
})();