(function () {
    app.modals.AlterarTipoLocalChamadaModal = function () {
        var _$senhasTable = $('#senhasTable');
        var _localChamadasService = abp.services.app.localChamadas;

        _$senhasTable.jtable({
            title: app.localize('TiposLocalChamadas'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _localChamadasService.listarSenhasNaoChamadasIndex
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
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                editarFilar(data.record);



                                //if ((data.record.grupoReferenciaId != null) && (data.record.grupoReferenciaId != "") && (data.record.grupoReferenciaId != undefined)) {
                                //    _createOrEditGrupoModal.open({ id: data.record.id });
                                //} else {
                                //    _createOrEditModal.open({ id: data.record.id });
                                //};
                            });

                        return $span;
                    }
                },



                tipoLocalChamada: {
                    title: app.localize('TipoLocalChamada'),
                    width: '40%'
                },

                numeroSenha: {
                    title: app.localize('Senha'),
                    width: '15%'
                },
                nomePaciente: {
                    title: app.localize('Paciente'),
                    width: '45%'
                },

            }
        });

        function getSenhas(reload) {
            if (reload) {
                _$senhasTable.jtable('reload');
            } else {
                _$senhasTable.jtable('load', {
                    tipoLocalChamadaId: $('#tipoLocalChamadaAuteracaoId').val()
                });
            }
        }

        getSenhas();


        $('#tipoLocalChamadaAuteracaoId').change(function (e) {
            e.preventDefault();
            getSenhas();
        });

        function editarFilar(record) {

            $('#senhaMovId').val(record.senhaMovimentoId);

            $('#tipoLocalChamadaNovoId')
                 .append($("<option>") 
                 .val(record.tipoLocalChamadaId) 
                 .text(record.tipoLocalChamada)
            ) 
            .val(record.tipoLocalChamadaId) 
            .trigger("change");

        }

        selectSW('.selectTipoLocalChamadaAuteracao', "/api/services/app/TipoLocalChamada/ListarTipoLocalChamadaDropdown");

        $('#inserir').on('click', function (e) {
            e.preventDefault();
            _localChamadasService.alterarTipoLocalChamadaSenha($('#senhaMovId').val(), $('#tipoLocalChamadaNovoId').val());

            $('#tipoLocalChamadaNovoId').val(null).trigger("change");

            getSenhas();

        });
    };
})(jQuery);
