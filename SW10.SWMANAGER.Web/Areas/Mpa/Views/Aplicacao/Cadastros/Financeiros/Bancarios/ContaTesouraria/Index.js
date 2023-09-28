(function () {
    $(function () {
        var _$contaCorrenteFilterFormTable = $('#contaCorrenteTable');
        var _contaCorrenteService = abp.services.app.contaCorrente;
        var _$contaCorrenteFilterFormFilterForm = $('#contaCorrenteFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.Bancario.ContaTasouraria.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.Bancario.ContaTasouraria.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Financeiro.Bancario.ContaTasouraria.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ContaCorrente/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/Bancarios/ContaTesouraria/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarContaCorrente'
        });

        _$contaCorrenteFilterFormTable.jtable({

            title: app.localize('EditContaCorrente'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _contaCorrenteService.listar
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
                                    App.startPageLoading({ animate: true }); document.querySelector('.loadingCommon').style.display = null;
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {                                    
                                    deleteRegistro(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '10%'
                },

                descricao: {
                    title: app.localize('Descricao'),
                    width: '25%'
                },

                nomeGerente: {
                    title: app.localize('NomeGerente'),
                    width: '10%'
                },

                agencia: {
                    title: app.localize('Agencia'),
                    width: '10%',
                    display: function (data) {
                        if (data) {                            
                            return data.record.agencia.descricao || '';
                        }
                    }
                },

                limiteCredito: {
                    title: app.localize('LimiteCredito'),
                    width: '10%',
                    display: function (data) {
                        if (data) {
                            return formatarValor(data.record.limiteCredito);
                        }
                    }
                },

                dataAbertura: {
                    title: app.localize('DataAbertura'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.dataAbertura != null) {
                            return moment(data.record.dataAbertura).format('L')
                        }
                        else {
                            return '';
                        }
                    }
                }
            }
        });

        function getRegistros(reload) {
            if (reload) {
                _$contaCorrenteFilterFormTable.jtable('reload');
            } else {
                _$contaCorrenteFilterFormTable.jtable('load', {
                    filtro: $('#tableFilter').val()
                });
            }
        }

        function deleteRegistro(record) {

            abp.message.confirm(
                app.localize('DeleteWarning', record.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        App.startPageLoading({ animate: true }); document.querySelector('.loadingCommon').style.display = null;
                        _contaCorrenteService.excluir(record)
                            .done(function () {
                                App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
                                getRegistros(true);
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

        $('#CreateNewButton').click(function () {
            App.startPageLoading({ animate: true }); document.querySelector('.loadingCommon').style.display = null;
            _createOrEditModal.open();
        });


        $('#GetFeriadosButton, #RefreshFeriadosListButton').click(function (e) {
            e.preventDefault();
            getRegistros();
        });

        abp.event.on('app.CriarOuEditarFeriadoModalSaved', function () {
            getRegistros(true);
        });

        getRegistros();

        $('#tableFilter').focus();

    });
})();