(function ($) {
    app.modals.NovoTevMovimentoModal = function () {
        var _tevMovimentoService = abp.services.app.tevMovimento;
        var _modalManager;
        var $_tevMovimentoForm;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            $_tevMovimentoForm = $('form[name="TevMovimentoForm-' + localStorage["AtendimentoId"] + '"]');
            $_tevMovimentoForm.validate();
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '990px' });
            getTevMovimento();
            aplicarDateSingle();
            $('.select2').css('width', '100%');
        }

        this.save = function () {
            if (!$_tevMovimentoForm.valid()) {
                return;
            }
            var tevMovimento = $_tevMovimentoForm.serializeFormToObject();
            tevMovimento.AtendimentoId = localStorage["AtendimentoId"];
            _modalManager.setBusy(true);
            _tevMovimentoService.criarOuEditar(tevMovimento)
                  .done(function (data) {
                      abp.notify.success(app.localize('SavedSuccessfully'));
                      refreshFooter();
                      _modalManager.close();
                      abp.event.trigger('app.NovoTevMovimentoModalSaved');
                  })
                 .always(function () {
                     _modalManager.setBusy(false);
                 });
        }
        _$TevMovimentosTable = $('#tev-movimento-grid-' + localStorage["AtendimentoId"]);

        _$TevMovimentosTable.jtable({
            title: app.localize('TevMovimentos'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            selectingCheckboxes: true,
            multiSelecting: false,
            actions: {
                listAction: {
                    method: abp.services.app.tevMovimento.listar
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                data: {
                    title: app.localize('Data'),
                    width: '40%',
                    sorting: false,
                    display: function (data) {
                        return moment(data.record.data).format("L");
                    }
                },
                riscoId: {
                    title: app.localize('Risco'),
                    width: '60%',
                    display: function (data) {
                        return data.record.risco.descricao;
                    }
                },
            },
            selectionChanged: function () {
                //Get selected row
                var record = _$TevMovimentosTable.jtable('registroSelecionado');
                //$('#data-' + localStorage["AtendimentoId"]).val(moment(record.data).format("L"));
                $('#data-label-' + localStorage["AtendimentoId"]).val(moment(record.data).format("L"));
                $('#risco-id-' + localStorage["AtendimentoId"])
                    .append($('<option value="' + record.riscoId + '">' + record.risco.codigo + ' - ' + record.risco.descricao + '</option>'))
                    .val(record.riscoId)
                    .trigger('change');

                $('#observacao-' + localStorage["AtendimentoId"]).val(record.observacao);

            }
        });

        function getTevMovimento() {
            _$TevMovimentosTable.jtable('load', {
                principalId: localStorage["AtendimentoId"]
            })
        }

        abp.event.on('app.NovoTevMovimentoModalSaved', function () {
            getTevMovimento();
        });

        $('#risco-id-' + localStorage["AtendimentoId"]).select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            //language: 'pt-BR',
            ajax: {
                url: "/api/services/app/tevMovimento/ListarTevRiscoDropdown",
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page == undefined)
                        params.page = '1';
                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;
                    //console.log('data: ',data);
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

        });
        function refreshFooter() {
            $('#tev-movimento')
                .load('/mpa/assistenciais/_controlatev', { atendimentoId: localStorage["AtendimentoId"] });
        }
    };
})(jQuery);