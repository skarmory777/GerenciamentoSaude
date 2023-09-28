(function ($) {

    app.modals.CriarOuEditarFaturamentoTabelaModal = function () {

        var _tabelasService = abp.services.app.faturamentoTabela;
        var _modalManager;
        var _$tabelaInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$tabelaInformationForm = _modalManager.getModal().find('form[name=FaturamentoTabelaInformationsForm]');
            _$tabelaInformationForm.validate({ ignore: "" });
            $('.modal-dialog').css('width', '1000px');
            $('.modal-dialog').css('top', '100px');

            //// PRECOS (ITENS TABELA)
            //var _$ItensTabelaTable = $('#ItensTabelaTable');
            //var _ItensTabelaService = abp.services.app.faturamentoItemTabela;
            //var _$filterForm = $('#ItensTabelaFilterForm');
            //var _permissions = {
            //    create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.ItensTabela.Create'),
            //    edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.ItensTabela.Edit'),
            //    'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.ItensTabela.Delete')
            //};
            //var _createOrEditModal = new app.ModalManager({
            //    viewUrl: abp.appPath + 'Mpa/FaturamentoItensTabela/CriarOuEditarModal',
            //    scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/ItensTabela/_CriarOuEditarModal.js',
            //    modalClass: 'CriarOuEditarFaturamentoItemTabelaModal'
            //});

            //function abrirModal() {
            //    _createOrEditModal.open();
            //}

            //_$ItensTabelaTable.jtable({

            //    title: app.localize('ItensTabela'),
            //    paging: true,
            //    pageSize: 10,
            //    sorting: true,
            //    multiSorting: true,

            //    actions: {
            //        listAction: {
            //            method: _ItensTabelaService.listarParaFatTabela
            //        }
            //    },

            //    fields: {
            //        id: {
            //            key: true,
            //            list: false
            //        },
            //        actions: {
            //            title: app.localize('Actions'),
            //            width: '5%',
            //            sorting: false,
            //            display: function (data) {
            //                var $span = $('<span></span>');
            //                if (_permissions.edit) {
            //                    $('<span class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></span>')
            //                        .appendTo($span)
            //                        .click(function () {

            //                            _createOrEditModal.open({ id: data.record.id, tabelaId: $('#id').val() });
            //                        });
            //                }

            //                if (_permissions.delete) {
            //                    $('<span class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></span>')
            //                        .appendTo($span)
            //                        .click(function () {
            //                            deleteItensTabela(data.record);
            //                        });
            //                }

            //                return $span;
            //            }
            //        }
            //        ,
            //        codigo: {
            //            title: app.localize('Codigo'),
            //            width: '5%'
            //        }
            //        ,
            //        descricao: {
            //            title: app.localize('Descricao'),
            //            width: '10%'
            //        }
            //        ,
            //        sisMoeda: {
            //            title: app.localize('Moeda'),
            //            width: '5%',
            //            display: function (data) {
            //                if (data.record.sisMoeda) {
            //                    return data.record.sisMoeda.descricao;
            //                }
            //            }
            //        }
            //        ,
            //        preco: {
            //            title: app.localize('Preco'),
            //            width: '5%'
            //        }
            //        ,
            //    }
            //});

            //$('#btn-get-precos').on('click', function (e) {
            //    e.preventDefault();
            //    getItensTabela();
            //});

            //function getItensTabela(reload) {
            //    if (reload) {
            //        _$ItensTabelaTable.jtable('reload');
            //    } else {
            //        _$ItensTabelaTable.jtable('load', {
            //            filtro: $('#precos-filter').val(),
            //            tabelaId: $('#id').val(),
            //            moedaId: $('#cbo-filtro-moeda').val()
            //        });
            //    }
            //}

            //function deleteItensTabela(itemTabela) {
            //    abp.message.confirm(
            //        app.localize('DeleteWarning', itemTabela.Nome),
            //        function (isConfirmed) {
            //            if (isConfirmed) {
            //                _ItensTabelaService.excluir(itemTabela)
            //                    .done(function () {
            //                        getItensTabela(true);
            //                        abp.notify.success(app.localize('SuccessfullyDeleted'));
            //                    });
            //            }
            //        }
            //    );
            //}

            //function createRequestParams() {
            //    var prms = {};
            //    _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            //    return $.extend(prms);
            //}

            //$('#ShowAdvancedFiltersSpan').click(function () {
            //    $('#ShowAdvancedFiltersSpan').hide();
            //    $('#HideAdvancedFiltersSpan').show();
            //    $('#AdvacedAuditFiltersArea').slideDown();
            //});

            //$('#HideAdvancedFiltersSpan').click(function () {
            //    $('#HideAdvancedFiltersSpan').hide();
            //    $('#ShowAdvancedFiltersSpan').show();
            //    $('#AdvacedAuditFiltersArea').slideUp();
            //});

            //$('#CreateNewItemTabelaButton').click(function (e) {
            //    e.preventDefault();
            //    var descricao = $('#descricao').val();
            //    if (descricao == '') {
            //        abp.notify.info(app.localize('DescricaoRequerida'));
            //    }
            //    else {
            //        //console.log('tabelaId: ' + $('#id').val());
            //        _createOrEditModal.open({ tabelaId: $('#id').val() });
            //    }
            //});

            ////$('#ExportarItensTabelaParaExcelButton').click(function () {
            ////    _ItensTabelaService
            ////        .listarParaExcel({
            ////            filtro: $('#ItensTabelaTableFilter').val(),
            ////            //sorting: $(''),
            ////            maxResultCount: $('span.jtable-page-size-change select').val()
            ////        })
            ////        .done(function (result) {
            ////            app.downloadTempFile(result);
            ////        });
            ////});

            //$('#GetItensTabelaButton, #RefreshItensTabelaListButton').click(function (e) {
            //    e.preventDefault();
            //    getItensTabela();
            //});

            //abp.event.on('app.CriarOuEditarItemTabelaModalSaved', function () {
            // //   debugger//modal TABELAS
            //    getItensTabela();
            //});

            //getItensTabela();

            //$('#ItensTabelaTableFilter').focus();
        };

        this.save = function () {
            if (!_$tabelaInformationForm.valid()) {
                return;
            }

            var tabela = _$tabelaInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _tabelasService.criarOuEditar(tabela)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarTabelaModalSaved');
                     //  this.init.getItensTabela();
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);