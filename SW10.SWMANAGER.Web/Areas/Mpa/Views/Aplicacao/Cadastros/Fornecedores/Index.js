(function () {
    $(function () {
        var _$FornecedoresTable = $('#FornecedoresTable');
        var _FornecedoresService = abp.services.app.fornecedor;

        var _$filterForm = $('#FornecedoresFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Fornecedor.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Fornecedor.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Fornecedor.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Fornecedores/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Fornecedores/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFornecedorModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Fornecedores/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        _$FornecedoresTable.jtable({

            title: app.localize('Fornecedores'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _FornecedoresService.listarFornecedores
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
                                    deleteFornecedores(data.record);
                                });
                        }

                        return $span;
                    }
                },

                //FisicaJuridica: {
                //    title: app.localize('FisicaJuridica'),
                //    width: '5%',
                //    display: function (data) {
                //        if (data.record.fisicaJuridica == "F") {
                //            return "Física";
                //        }
                //        else if (data.record.fisicaJuridica == "J") {
                //            return "Júridica";
                //        }
                //    }
                //},

                nomeFornecedor: {
                    title: app.localize('Nome'),
                    width: '60%',
                    display: function (data) {
                        let $span = $('<span class="flex"></span>');
                        if (data.record) {
                            if (data.record.fisicaJuridica == "F") {
                                if (data.record.nomeCompleto) {
                                    $span.append(
                                        `<span style="width:100%;text-align:left" >
                                            <p class="nome-title-desc">Nome:</p>
                                            <p class="nome-title">${data.record.nomeCompleto}</p>
                                        </span>`);
                                }
                            }
                            else if (data.record.fisicaJuridica == "J") {
                                if (data.record.nomeFantasia) {
                                    $span.append(
                                        `<span  style="width:50%" >
                                            <p class="nome-title-desc">Nome Fantasia:</p>
                                            <p class="nome-title">${data.record.nomeFantasia}</p>
                                        </span>`);
                                }
                                if (data.record.razaoSocial) {
                                    $span.append(
                                        `<span  style="width:50%">
                                            <p class="nome-title-desc">Razão Social:</p>
                                            <p class="nome-title">${data.record.razaoSocial}</p>
                                        </span>`);
                                }
                            }
                        }
                        return $span;
                    }
                },
                CPFCNPJ: {
                    title: app.localize('CPFCNPJ'),
                    width: '32%',
                    display: function (data) {
                        let $span = $('<span class="flex"></span>');
                        if (data.record.fisicaJuridica == "F") {
                            if (data.record.cpf) {
                                $span.append(
                                    `<span style="padding: 0px 10px;width: 100%;text-align:left">
                                        <p class="nome-title-desc">CPF:</p>
                                        <p class="nome-title">${data.record.cpf}</p>
                                    </span>`);
                            }
                        }
                        else if (data.record.fisicaJuridica == "J") {
                            if (data.record.cnpj) {
                                $span.append(
                                    `<span style="padding: 0px 10px;width: 100%;">
                                        <p class="nome-title-desc">CNPJ:</p>
                                        <p class="nome-title">${data.record.cnpj}</p>
                                    </span>`);
                            }
                            else {
                                $span.append(`<span style="padding: 0px 10px;width: 30%;">&nbsp;</span>`);
                            }

                            //if (data.record.inscricaoEstadual) {
                            //    $span.append(
                            //        `<span style="padding: 0px 10px;width: 30%;">
                            //            <p class="nome-title-desc">Inscricao Estadual:</p>
                            //            <p class="nome-title">${data.record.inscricaoEstadual}</p>
                            //        </span>`);
                            //}
                            //else {
                            //    $span.append(`<span style="padding: 0px 10px;width: 30%;">&nbsp;</span>`);
                            //}

                            //if (data.record.inscricaoMunicipal) {
                            //    $span.append(
                            //        `<span style="padding: 0px 10px;width: 30%;">
                            //            <p class="nome-title-desc">Inscricao Municipal:</p>
                            //            <p class="nome-title">${data.record.inscricaoMunicipal}</p>
                            //        </span>`);
                            //} else {
                            //    $span.append(`<span style="padding: 0px 10px;width: 30%;">&nbsp;</span>`);
                            //}

                            
                        }
                        return $span;
                    }
                }
            }
        });

        function getFornecedores(reload) {
            if (reload) {
                _$FornecedoresTable.jtable('reload');
            } else {
                _$FornecedoresTable.jtable('load', {
                    filtro: $('#FornecedoresTableFilter').val()
                });
            }
        }

        function deleteFornecedores(Fornecedor) {

            abp.message.confirm(
                app.localize('DeleteWarning', Fornecedor.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _FornecedoresService.excluir(Fornecedor)
                            .done(function () {
                                getFornecedores(true);
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

        $('#CreateNewFornecedorButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarFornecedoresParaExcelButton').click(function () {
            _FornecedoresService
                .listarParaExcel({
                    filtro: $('#FornecedoresTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetFornecedoresButton, #RefreshFornecedoresListButton').click(function (e) {
            e.preventDefault();
            getFornecedores();
        });

        abp.event.on('app.CriarOuEditarFornecedorModalSaved', function () {
            getFornecedores(true);
        });

        getFornecedores();

        $('#FornecedoresTableFilter').focus();
    });
})();