(function () {
    $(function () {
        var _$ModelosPrescricaoTable = $('#ModelosPrescricaoTable');
        var _modeloPrescricaoService = abp.services.app.modeloPrescricao;
        var _$filterForm = $('#ModelosAtestadosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.ModeloPrescricao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.ModeloPrescricao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.ModeloPrescricao.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ModelosPrescricaos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Prescricoes/Index.js',
            modalClass: 'FullModal'
        });

        _$ModelosPrescricaoTable.jtable({

            title: app.localize('ModeloPrescricao'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _modeloPrescricaoService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '20%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _createOrEditModal.open({ id: data.record.id });
                                   // location.href = `Assistenciais/MedicoPrescricao/${data.record.id}`;
                                });
                        }
                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteModelosAtestados(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '20%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '30%',
                   
                }
            }
        });

        function getModelosPrescricao(reload) {
            if (reload) {
                _$ModelosPrescricaoTable.jtable('reload');
            } else {
                _$ModelosPrescricaoTable.jtable('load', {
                    filtro: $('#ModelosAtestadosTableFilter').val()
                });
            }
        }

        function deleteModelosAtestados(modeloAtestado) {

            abp.message.confirm(
                app.localize('DeleteWarning', modeloAtestado.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ModelosAtestadosService.excluir(modeloAtestado)
                            .done(function () {
                                getModelosAtestados(true);
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

        $('#CreateNewModeloAtestadoButton').click(function () {
            debugger;

            sessionStorage["id"] = 12475;
            sessionStorage["dataRegistro"] = '2000-01-01';
            sessionStorage["codigoAtendimento"] = '00133850';
            sessionStorage["paciente"] = 'teste teste';



            _createOrEditModal.open();
        });

        $('#ExportarModelosAtestadosParaExcelButton').click(function () {
            _ModelosAtestadosService
                .listarParaExcel({
                    filtro: $('#ModelosAtestadosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetModelosAtestadosButton, #RefreshModelosAtestadosListButton').click(function (e) {
            e.preventDefault();
            getModelosAtestados();
        });

        abp.event.on('app.CriarOuEditarModeloAtestadoModalSaved', function () {
            getModelosAtestados(true);
        });

        getModelosPrescricao();

        $('#ModelosAtestadosTableFilter').focus();
    });
})();