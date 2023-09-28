(function () {
    $(function () {
        var _$BrasPrecosTable = $('#BrasPrecosTable');
        var _BrasPrecosService = abp.services.app.faturamentoBrasPreco;
        var _$filterForm = $('#BrasPrecosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.BrasPrecos.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.BrasPrecos.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.BrasPrecos.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturamentoBrasPrecos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/BrasPrecos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFaturamentoBrasPrecoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/BrasPrecos/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$BrasPrecosTable.jtable({

            title: app.localize('BrasPrecos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _BrasPrecosService.listar
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
                                    deleteBrasPrecos(data.record);
                                });
                        }

                        return $span;
                    }
                }
                ,
                preco: {
                    title: app.localize('Preco'),
                    width: '15%',

                    display: function (data) {
                        if (data.record.brasItem) {
                            return posicionarDireita(formatarValor(data.record.preco));
                        }
                    }
                }
                ,
                tipo: {
                    title: app.localize('Tipo'),
                    width: '15%'
                }
                ,
                codigoBrasTiss: {
                    title: app.localize('CodigoBrasTiss'),
                    width: '15%'
                }
                ,
                codigoBrasTuss: {
                    title: app.localize('CodigoBrasTuss'),
                    width: '15%'
                }
                ,
                item: {
                    title: app.localize('Item'),
                    width: '15%',
                    display: function (data) {

                        if (data.record.brasItem) {
                            return data.record.brasItem.descricao;
                        }

                    }
                }
                ,
                apresentacao: {
                    title: app.localize('Apresentacao'),
                    width: '15%',
                    display: function (data) {

                        if (data.record.brasApresentacao) {
                            return data.record.brasApresentacao.descricao;
                        }

                    }
                }
                ,
                laboratorio: {
                    title: app.localize('Laboratorio'),
                    width: '15%',
                    display: function (data) {

                        if (data.record.brasLaboratorio) {
                            return data.record.brasLaboratorio.descricao;
                        }

                    }
                }
            }
        });

        function getBrasPrecos(reload) {
            if (reload) {
                _$BrasPrecosTable.jtable('reload');
            } else {
                _$BrasPrecosTable.jtable('load', {
                    filtro: $('#BrasPrecosTableFilter').val()
                });
            }
        }

        function deleteBrasPrecos(brasPreco) {

            abp.message.confirm(
                app.localize('DeleteWarning', brasPreco.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _BrasPrecosService.excluir(brasPreco)
                            .done(function () {
                                getBrasPrecos(true);
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

        $('#CreateNewBrasPrecoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarBrasPrecosParaExcelButton').click(function () {
            _BrasPrecosService
                .listarParaExcel({
                    filtro: $('#BrasPrecosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetBrasPrecosButton, #RefreshBrasPrecosListButton').click(function (e) {
            e.preventDefault();
            getBrasPrecos();
        });

        abp.event.on('app.CriarOuEditarBrasPrecoModalSaved', function () {
            getBrasPrecos(true);
        });

        getBrasPrecos();

        $('#BrasPrecosTableFilter').focus();

        // Salvar BrasPreco
        //function salvarBrasPreco() {

        //    //var brasPrecoAppService = abp.

        //    _createOrEditModal.save();

        //    //var _$BrasPrecoInformationForm = $('form[name=BrasPrecoInformationsForm]');
        //    //var formData = _$BrasPrecoInformationForm.serialize();

        //    //var metodo = '/Mpa/FaturamentoBrasPrecos/SalvarBrasPreco';
        //    //$.ajax({
        //    //    type: "POST",
        //    //    url: metodo,
        //    //    dataType: 'text',
        //    //    data: formData,
        //    //    success: function (result) {
        //    //        ////console.log('brasPreco salvo');
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