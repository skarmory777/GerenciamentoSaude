
(function ($) {
    app.modals.CriarOuEditarPainelSenhaModal = function () {

        var _painelService = abp.services.app.painel;

        $(document).ready(function () {
            // CamposRequeridos();
        });


        var _modalManager;

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            //_$contaAdministrativaInformationsForm = _modalManager.getModal().find('form[name=contaAdministrativaInformationsForm]');
            //_$contaAdministrativaInformationsForm.validate();
        };



        this.save = function () {


           


            _$painelSenhaInformationsForm = _modalManager.getModal().find('form[name=PainelSenhaInformationsForm]');
            _$painelSenhaInformationsForm.validate();

            //_$contaAdministrativaInformationsForm.validate()
            //if (!_$contaAdministrativaInformationsForm.valid()) {
            //    return;
            //}

            var painelSenha = _$painelSenhaInformationsForm.serializeFormToObject();

          
            _modalManager.setBusy(true);
            _painelService.criarOuEditar(painelSenha)
                 .done(function (data) {
                     if (data.errors.length > 0) {
                         _ErrorModal.open({ erros: data.errors });
                     }
                     else {

                         abp.notify.info(app.localize('SavedSuccessfully'));
                         _modalManager.close();
                         abp.event.trigger('app.CriarOuEditarPainelModalSaved');
                         //location.reload();//seguindo o projeto pronto
                     }
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };




        var lista = [];

        $('#inserir').click(function (e) {
            e.preventDefault();
           




            var painelTipoLocalChamada = {};



            if ($('#tipoLocalChamadas').val() != '') {
                lista = JSON.parse($('#tipoLocalChamadas').val());
            }


            painelTipoLocalChamada.GridId = lista.length == 0 ? 1 : lista[lista.length - 1].GridId + 1;

            var tipoLocalChamada = $('#tipoLocalChamadaId').select2('data');
            if (tipoLocalChamada && tipoLocalChamada.length > 0) {

                painelTipoLocalChamada.TipoLocalChamadaDescricao = tipoLocalChamada[0].text;
            }

            painelTipoLocalChamada.TipoLocalChamdaId = $('#tipoLocalChamadaId').val();

            lista.push(painelTipoLocalChamada);


            _$tipoLocalChamadaTable.jtable('addRecord', {
                record: painelTipoLocalChamada
                , clientOnly: true
            });



            $('#tipoLocalChamadas').val(JSON.stringify(lista));

            $('#tipoLocalChamadaId').val('').trigger('change');
            $('#tipoLocalChamadaId').focus();

        });

        var _$tipoLocalChamadaTable = $('#tipoLocalChamadaTable');

        _$tipoLocalChamadaTable.jtable
            ({
                title: app.localize('Itens'),
                //paging: true,
                sorting: true,
                edit: false,
                create: false,
                multiSorting: true,

                fields:
                    {
                        GridId: {
                            key: true,
                            list: false
                        },
                        actions: {
                            title: app.localize('Actions'),
                            width: '7%',
                            sorting: false,
                            display: function (data) {
                                var $span = $('<span></span>');

                                $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                    .appendTo($span)
                                    .click(function (e) {
                                        e.preventDefault();
                                        deleteTipoLocalChamada(data.record);
                                    });
                                return $span;
                            }
                        },

                        TipoLocalChamadaDescricao: {
                            title: app.localize('TipoLocalChamada'),
                            width: '10%',
                            display: function (data) {
                                if (data.record.TipoLocalChamadaDescricao) {
                                    return data.record.TipoLocalChamadaDescricao;
                                }
                            }
                        }

                    }
            });


        function getTipoLocalChamadaTable(reload) {

           

            lista = JSON.parse($('#tipoLocalChamadas').val());

            var allRows = _$tipoLocalChamadaTable.find('.jtable-data-row')

            $.each(allRows, function () {
                var id = $(this).attr('data-record-key');

                _$tipoLocalChamadaTable.jtable('deleteRecord', { key: id, clientOnly: true });

            });

            for (var i = 0; i < lista.length; i++) {
                var item = lista[i];
                _$tipoLocalChamadaTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });
            }
        }

        getTipoLocalChamadaTable();

        function deleteTipoLocalChamada(tipoLocalChamada) {

           
            abp.message.confirm(
                app.localize('DeleteWarning', tipoLocalChamada.TipoLocalChamadaDescricao),
                function (isConfirmed) {
                    if (isConfirmed) {

                       

                        lista = JSON.parse($('#tipoLocalChamadas').val());

                        for (var i = 0; i < lista.length; i++) {
                            if (lista[i].GridId == tipoLocalChamada.GridId) {
                                lista.splice(i, 1);
                                $('#tipoLocalChamadas').val(JSON.stringify(lista));

                                _$tipoLocalChamadaTable.jtable('deleteRecord', {
                                    key: tipoLocalChamada.GridId
                                    , clientOnly: true
                                });

                                break;
                            }
                        }

                    }
                }
            );
        }


       

        selectSW('.selectTipolocalChamada', "/api/services/app/TipoLocalChamada/ListarTipoLocalChamadaDropdown");


    };
})(jQuery);