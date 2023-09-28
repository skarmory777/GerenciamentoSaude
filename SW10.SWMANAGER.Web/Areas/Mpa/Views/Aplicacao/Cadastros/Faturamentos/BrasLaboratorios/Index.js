(function () {
    $(function () {
        var _$BrasLaboratoriosTable = $('#BrasLaboratoriosTable');
        var _BrasLaboratoriosService = abp.services.app.faturamentoBrasLaboratorio;
        var _$filterForm = $('#BrasLaboratoriosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.BrasLaboratorios.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.BrasLaboratorios.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.BrasLaboratorios.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturamentoBrasLaboratorios/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/BrasLaboratorios/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFaturamentoBrasLaboratorioModal'
        });

        _$BrasLaboratoriosTable.jtable({

            title: app.localize('BrasLaboratorios'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _BrasLaboratoriosService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '10%',
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
                                    deleteBrasLaboratorios(data.record);
                                });
                        }

                        return $span;
                    }
                }
                ,
                codigo: {
                    title: app.localize('Codigo'),
                    width: '15%'
                }
                ,
                descricao: {
                    title: app.localize('Descricao'),
                    width: '45%'
                }
            }
        });

        function getBrasLaboratorios(reload) {
            if (reload) {
                _$BrasLaboratoriosTable.jtable('reload');
            } else {
                _$BrasLaboratoriosTable.jtable('load', {
                    filtro: $('#BrasLaboratoriosTableFilter').val()
                });
            }
        }

        function deleteBrasLaboratorios(brasLaboratorio) {

            abp.message.confirm(
                app.localize('DeleteWarning', brasLaboratorio.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _BrasLaboratoriosService.excluir(brasLaboratorio)
                            .done(function () {
                                getBrasLaboratorios(true);
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

        $('#CreateNewBrasLaboratorioButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarBrasLaboratoriosParaExcelButton').click(function () {
            _BrasLaboratoriosService
                .listarParaExcel({
                    filtro: $('#BrasLaboratoriosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetBrasLaboratoriosButton, #RefreshBrasLaboratoriosListButton').click(function (e) {
            e.preventDefault();
            getBrasLaboratorios();
        });

        abp.event.on('app.CriarOuEditarBrasLaboratorioModalSaved', function () {
            getBrasLaboratorios(true);
        });

        getBrasLaboratorios();

        $('#BrasLaboratoriosTableFilter').focus();

        // Salvar BrasLaboratorio
        //function salvarBrasLaboratorio() {

        //    //var brasLaboratorioAppService = abp.

        //    _createOrEditModal.save();

        //    //var _$BrasLaboratorioInformationForm = $('form[name=BrasLaboratorioInformationsForm]');
        //    //var formData = _$BrasLaboratorioInformationForm.serialize();

        //    //var metodo = '/Mpa/FaturamentoBrasLaboratorios/SalvarBrasLaboratorio';
        //    //$.ajax({
        //    //    type: "POST",
        //    //    url: metodo,
        //    //    dataType: 'text',
        //    //    data: formData,
        //    //    success: function (result) {
        //    //        ////console.log('brasLaboratorio salvo');
        //    //        //$('#btn-fechar-modal').click();
        //    //        //$('#todas').click();
        //    //    },
        //    //    error: function (xhr, ajaxOptions, thrownError) {
        //    //        abp.notify.info(app.localize('Erro'));
        //    //    },
        //    //    beforeSend: function () {
        //    //    },
        //    //    complete: function () { }
        //    //});
        //}

    });
})();