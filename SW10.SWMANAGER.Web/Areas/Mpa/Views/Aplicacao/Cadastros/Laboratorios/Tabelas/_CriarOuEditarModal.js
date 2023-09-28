(function ($) {
    app.modals.CriarOuEditarTabelaModal = function () {
        var _tabelaService = abp.services.app.tabela;

        var _modalManager;
        var _$TabelasInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$TabelaInformationForm = _modalManager.getModal().find('form[name=TabelaInformationsForm]');
            _$TabelaInformationForm.validate();

            _$TabelaResultadoInformationForm = _modalManager.getModal().find('form[name=TabelaResultadoInformationsForm]');
            _$TabelaResultadoInformationForm.validate();

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        };

        this.save = function () {
            if (!_$TabelaInformationForm.valid()) {
                return;
            }
            var Tabela = _$TabelaInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);

            _tabelaService.criarOuEditar(Tabela)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarTabelaModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        //tabela resultado

        var _$TabelaResultadosTable = $('#TabelaResultadosTable');
        var _tabelaResultadoService = abp.services.app.tabelaResultado;
        var _$filterForm = $('#TabelaResultadosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.TabelaResultado.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.TabelaResultado.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.TabelaResultado.Delete')
        };

        var _createOrEditResultadoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TabelasResultados/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/TabelaResultados/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTabelaResultadoModal'
        });

        _$TabelaResultadosTable.jtable({

            title: app.localize('TabelaResultados'),
            sorting: true,
            paging: false,
            create: false,
            edit: false,
            multiSorting: true,

            //actions: {
            //    listAction: {
            //        method: listarResultados //_TabelaResultadosService.listar
            //    }
            //},

            fields: {
                Id: {
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
                                .click(function (e) {
                                    e.preventDefault();
                                    //_createOrEditResultadoModal.open({ id: data.record.id, tabelaId: $('#tabela-id').val() });
                                    editarRegistro(data.record.Id, data.record.IdGrid);
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deleteTabelaResultados(data.record);
                                });
                        }

                        return $span;
                    }
                },
                Codigo: {
                    title: app.localize('Código'),
                    width: '25%'
                },
                Descricao: {
                    title: app.localize('Descricao'),
                    width: '65%'
                },
            }
        });

        //function listarResultados() {
        //    var list = $('#tabela-resultados').val();
        //    res = _tabelaResultadoService.listarJson(JSON.parse(list));
        //    return res;
        //}

        function getTabelaResultados() {
            //_$TabelaResultadosTable.jtable('load', {
            //    filtro: $('#TabelaResultadosTableFilter').val(),
            //    principalId: $('#tabela-id').val()
            //});
            var lista = JSON.parse($('#tabela-resultados').val());
            for (var i = 0; i < lista.length; i++) {
                var item = lista[i];

                _$TabelaResultadosTable.jtable('addRecord', {
                    record: item,
                    clientOnly: true
                });
            }
        }

        function deleteTabelaResultados(TabelaResultado) {

            abp.message.confirm(
                app.localize('DeleteWarning', TabelaResultado.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TabelaResultadosService.excluir(TabelaResultado)
                            .done(function () {
                                getTabelaResultados();
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

        $('#CreateNewTabelaResultadoButton').click(function (e) {
            e.preventDefault();
            //_createOrEditResultadoModal.open({ tabelaId: $('#tabela-id').val() });

            novoRegistro();
            $('#exibir-sw-div-retratil-cadastro-resultado').trigger('click');
        });

        $('#ExportarTabelaResultadosParaExcelButton').click(function (e) {
            e.preventDefault()
            _TabelaResultadosService
                .listarParaExcel({
                    filtro: $('#TabelaResultadosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTabelaResultadosButton, #RefreshTabelaResultadosListButton').click(function (e) {
            e.preventDefault();
            getTabelaResultados();
        });

        abp.event.on('app.CriarOuEditarTabelaResultadoModalSaved', function () {
            getTabelaResultados();
        });

        $('#salvar-tabela-resultado').on('click', function (e) {
            e.preventDefault();
            salvarRegistro();
        });

        getTabelaResultados();

        $('#TabelaResultadosTableFilter').focus();

        function novoRegistro() {
            $('#codigo-tabela-resultado').val('');
            $('#descricao-tabela-resultado').val('');
            $('#id-tabela-resultado').val(0);
            $('#tabela-id-tabela-resultado').val($('#tabela-id').val());
            $('#id-grid-tabela-resultado').val('');

            $('#salvar-tabela-resultado i').removeClass('fa-check').addClass('fa-plus');
        }

        function editarRegistro(id, idGrid) {
            abp.ui.setBusy();
            var list = JSON.parse($('#tabela-resultados').val());
            var data;
            for (var i = 0; i < list.length; i++) {
                if (list[i].IdGrid == idGrid) {
                    data = list[i];
                    break;
                }
            }
            $('#codigo-tabela-resultado').val(data.Codigo);
            $('#descricao-tabela-resultado').val(data.Descricao);
            $('#id-tabela-resultado').val(data.Id);
            $('#tabela-id-tabela-resultado').val(data.TabelaId);
            $('#id-grid-tabela-resultado').val(data.IdGrid);
            $('#salvar-tabela-resultado i').removeClass('fa-plus').addClass('fa-check');

            abp.ui.clearBusy();

            $('#exibir-sw-div-retratil-cadastro-resultado').trigger('click');
        }

        function salvarRegistro() {
            var tabelaForm = _$TabelaInformationForm;
            var tabelaResultadoForm = _$TabelaResultadoInformationForm;
            var list = $('#tabela-resultados').val();
            tabelaResultadoForm.validate();

            if (!tabelaResultadoForm.valid()) {
                return;
            }

            tabelaForm.serializeFormToObject();
            var form1 = tabelaResultadoForm.serializeFormToObject();

            if (list != '') {
                var lista = JSON.parse(list);
            }
            else {
                var lista = [];
            }

            if (form1.IdGrid != null && form1.IdGrid != '' && form1.IdGrid != undefined) {
                //var itemProcessado = false;
                for (var i = 0; i < lista.length; i++) {
                    if (lista[i].IdGrid == form1.IdGrid) {
                        lista[i].Codigo = form1.Codigo;
                        lista[i].Descricao = form1.Descricao;
                        lista[i].TabelaId = form1.TabelaId;
                        lista[i].Id = form1.Id;
                        lista[i].IdGrid = form1.IdGrid;
                        //itemProcessado = true;

                        _$TabelaResultadosTable.jtable('updateRecord', {
                            record: lista[i],
                            clientOnly: true
                        });

                        break;
                    }
                }
                //if (!itemProcessado) {
                //    form1.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                //    form1.TabelaId = $('#tabela-id').val();
                //    lista.push(form1);
                //}
            }
            else {
                form1.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                form1.TabelaId = $('#tabela-id').val();
                lista.push(form1);

                _$TabelaResultadosTable.jtable('addRecord', {
                    record: form1,
                    clientOnly: true
                });

            }
            $('#tabela-resultados').val(JSON.stringify(lista));
            abp.notify.info(app.localize('ListaAtualizada'));
            //abp.event.trigger('app.CriarOuEditarTabelaResultadoModalSaved');
            novoRegistro();
        }

        $('#cancelar-tabela-resultado').on('click', function (e) {
            e.preventDefault();
            novoRegistro();
            $('#omitir-sw-div-retratil-cadastro-resultado').trigger('click');
        })

    };
})(jQuery);