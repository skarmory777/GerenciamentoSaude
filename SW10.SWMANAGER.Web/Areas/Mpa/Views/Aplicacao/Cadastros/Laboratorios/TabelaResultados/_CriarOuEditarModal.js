(function ($) {
    app.modals.CriarOuEditarTabelaResultadoModal = function () {
        var _tabelaresultadosService = abp.services.app.tabelaResultado;

        var _modalManager;
        var _$TabelaResultadosInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$TabelaResultadoInformationForm = _modalManager.getModal().find('form[name=TabelaResultadoInformationsForm]');
            _$TabelaResultadoInformationForm.validate();

        };

        //this.save = function () {
        //    if (!_$TabelaResultadoInformationForm.valid()) {
        //        return;
        //    }

        //    var TabelaResultado = _$TabelaResultadoInformationForm.serializeFormToObject();

        //    _modalManager.setBusy(true);

        //    _tabelaresultadosService.criarOuEditar(TabelaResultado)
        //        .done(function () {
        //            abp.notify.info(app.localize('SavedSuccessfully'));
        //            _modalManager.close();
        //            abp.event.trigger('app.CriarOuEditarTabelaResultadoModalSaved');
        //        })
        //        .always(function () {
        //            _modalManager.setBusy(false);
        //        });
        //};

        this.save = function () {
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

            if (lista.length > 0) {
                var itemProcessado = false;
                for (var i = 0; i < lista.length; i++) {
                    if (lista[i].IdGrid == form1.IdGrid) {
                        lista[i].Codigo = form1.Codigo;
                        lista[i].Descricao = form1.Descricao;
                        lista[i].TabelaId = form1.TabelaId;
                        lista[i].Id = form1.Id;
                        lista[i].IdGrid = form1.IdGrid;
                        itemProcessado = true;
                        break;
                    }
                }
                if (!itemProcessado) {
                    form1.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                    form1.TabelaId = $('#tabela-id').val();
                    lista.push(form1);
                }
            }
            else {
                form1.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                form1.TabelaId = $('#tabela-id').val();
                lista.push(form1);
            }
            $('#tabela-resultados').val(JSON.stringify(lista));
            abp.notify.info(app.localize('ListaAtualizada'));
            abp.event.trigger('app.CriarOuEditarTabelaResultadoModalSaved');
            _modalManager.close();
            //getExames();
            //novoRegistroExame();
        };

    };
})(jQuery);