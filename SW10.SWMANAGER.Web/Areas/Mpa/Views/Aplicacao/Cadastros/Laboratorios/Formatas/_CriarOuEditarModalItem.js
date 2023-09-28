(function ($) {
    app.modals.CriarOuEditarFormataItemModal = function () {
        var _formataItemsService = abp.services.app.formataItem;

        var _modalManager;
        var _$FormataItemsInformationForm = null;

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$FormataItemInformationForm = _modalManager.getModal().find('form[name=FormataItemInformationsForm]');
            _$FormataItemInformationForm.validate();

            $('.select2').css('width', '100%');
        };

        this.save = function () {
            if (!_$FormataItemInformationForm.valid()) {
                return;
            }
            var list = $('#formata-itens').val();

            var FormataItem = _$FormataItemInformationForm.serializeFormToObject();
            var form1 = FormataItem;

            if (list != '') {
                var lista = JSON.parse(list);
            }
            else {
                var lista = [];
            }

            if (lista.length > 0) {
                var itemProcessado = false;
                for (var i = 0; i < lista.length; i++) {
                    if (lista[i].IdGrid == form1.IdGrid) {

                        lista[i].Codigo = form1.Codigo;
                        lista[i].Descricao = form1.Descricao;
                        lista[i].FormataId = form1.FormataId;
                        lista[i].Id = form1.Id;
                        lista[i].IdGrid = form1.IdGrid;
                        lista[i].ItemResultadoId = form1.ItemResultadoId;
                        lista[i].Ordem = form1.Ordem;
                        lista[i].OrdemRegistro = form1.OrdemRegistro;
                        lista[i].Formula = form1.Formula;
                        lista[i].IsBi = form1.IsBi;
                        lista[i].IsRefExame = form1.IsRefExame;
                        lista[i].LaboratorioUnidadeId = form1.LaboratorioUnidadeId;
                        lista[i].TipoResultadoId = form1.TipoResultadoId;

                        itemProcessado = true;
                        break;
                    }
                }
                if (!itemProcessado) {
                    form1.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                    form1.FormataId = $('#formata-id').val();
                    lista.push(form1);
                }
            }
            else {
                form1.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                form1.FormataId = $('#formata-id').val();
                lista.push(form1);
            }
            $('#formata-itens').val(JSON.stringify(lista));
            abp.notify.info(app.localize('ListaAtualizada'));
            abp.event.trigger('app.CriarOuEditarFormataItemModalSaved');
            _modalManager.close();

            //_modalManager.setBusy(true);


            ////FormataItem.FormataId = ('#id').val();

            //_formataItemsService.criarOuEditar(FormataItem)
            //    .done(function (result) {

            //        if (result.errors.length > 0) {
            //            _ErrorModal.open({ erros: result.errors });
            //        }
            //        else {

            //            abp.notify.info(app.localize('SavedSuccessfully'));
            //            _modalManager.close();
            //            abp.event.trigger('app.CriarOuEditarFormataItemModalSaved');
            //        }
            //    })
            //    .always(function () {
            //        _modalManager.setBusy(false);
            //    });
        };

        $('#item-resultado-id').select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: '/api/services/app/itemresultado/listardropdown',
                dataType: 'json',
                delay: 250,
                method: 'Post',

                data: function (params) {
                    if (params.page == undefined)
                        params.page = '1';

                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10,
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: data.result.items,
                        pagination: {
                            more: (params.page * 10) < data.result.totalCount
                        }
                    };
                },
                cache: true
            },
            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            minimumInputLength: 0
        }).on('change', function () {
            if ($(this).val() != null && $(this).val() != '' && $(this).val() != undefined) {
                preencherItemResultado($(this).val());
            }
        });

        function preencherItemResultado(id) {
            $.ajax({
                url: '/api/services/app/itemresultado/obter?id=' + id,
                //data: { id: id },
                method: 'POST',
                success: function (data) {
                    var record = data.result;
                },
            })
        }

        function novoRegistro() {
            $('#codigo-tabela-resultado').val('');
            $('#descricao-tabela-resultado').val('');
            $('#id-tabela-resultado').val(0);
            $('#tabela-id-tabela-resultado').val($('#tabela-id').val());
            $('#id-grid-tabela-resultado').val('');

            $('#salvar-formata-item i').removeClass('fa-check').addClass('fa-plus');

            $('#exibir-sw-div-retratil-formata-item').trigger('click');
        }

        function editarRegistro(id, idGrid) {
            abp.ui.setBusy();
            var list = JSON.parse($('#formata-itens').val());
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

            $('#salvar-formata-item i').removeClass('fa-plus').addClass('fa-check');

            abp.ui.clearBusy();

            $('#exibir-sw-div-retratil-formata-item').trigger('click');
        }

    };
})(jQuery);