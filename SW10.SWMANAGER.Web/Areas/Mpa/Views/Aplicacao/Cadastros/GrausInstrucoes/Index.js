(function () {
    $(function () {
        var _$GrausInstrucoesTable = $('#GrausInstrucoesTable');
        var _GrausInstrucoesService = abp.services.app.grauInstrucao;
        var _$filterForm = $('#GrausInstrucoesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.GrauInstrucao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.GrauInstrucao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.GrauInstrucao.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/GrausInstrucoes/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/GrausInstrucoes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarGrauInstrucaoModal'
        });

        _$GrausInstrucoesTable.jtable({

            title: app.localize('GrauInstrucao'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _GrausInstrucoesService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '33%',
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
                                    deleteGrausInstrucoes(data.record);
                                });
                        }

                        return $span;
                    }
                },
                diaMesAno: {
                    title: app.localize('DataGrauInstrucao'),
                    width: '33%',
                    display: function (data) {
                        return moment(data.record.diaMesAno).format('L');
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '33%'
                }
            }
        });

        function getGrausInstrucoes(reload) {
            if (reload) {
                _$GrausInstrucoesTable.jtable('reload');
            } else {
                _$GrausInstrucoesTable.jtable('load', {
                    filtro: $('#GrausInstrucoesTableFilter').val()
                });
            }
        }

        function deleteGrausInstrucoes(grauInstrucao) {

            abp.message.confirm(
                app.localize('DeleteWarning', grauInstrucao.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _GrausInstrucoesService.excluir(grauInstrucao)
                            .done(function () {
                                getGrausInstrucoes(true);
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

        $('#CreateNewGrauInstrucaoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarGrausInstrucoesParaExcelButton').click(function () {
            _GrausInstrucoesService
                .listarParaExcel({
                    filtro: $('#GrausInstrucoesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetGrausInstrucoesButton, #RefreshGrausInstrucoesListButton').click(function (e) {
            e.preventDefault();
            getGrausInstrucoes();
        });

        abp.event.on('app.CriarOuEditarGrauInstrucaoModalSaved', function () {
            getGrausInstrucoes(true);
        });

        getGrausInstrucoes();

        $('#GrausInstrucoesTableFilter').focus();
    });
})();