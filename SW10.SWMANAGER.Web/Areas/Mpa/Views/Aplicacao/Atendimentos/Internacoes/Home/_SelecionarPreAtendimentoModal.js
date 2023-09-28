(function ($) {
    app.modals.SelecionarPreAtendimentoModal = function () {

        var preAtendimento = [];

        var _divisaoService = abp.services.app.divisao;
        var _kitExameItensService = abp.services.app.kitExameItem;

        var _AtendimentosService = abp.services.app.atendimento;

        var _$PreAtendimentosTable = $('#PreAtendimentosTable');

        var _modalManager;

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Atendimento.PreAtendimento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Atendimento.PreAtendimento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Atendimento.PreAtendimento.Delete')
        };

        this.init = function (modalManager) {
            _modalManager = modalManager;
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        };

        this.save = function () {

           //criar o objeto do tipo SolicitacaoExameItem
            if (preAtendimento.length > 0) {
                _modalManager.setBusy(true);

                   for (var i = 0; i < preAtendimento.length; i++) {

                    preAtendimento[i].isAmbulatorioEmergencia = 0;
                    preAtendimento[i].isInternacao = 1;
                    preAtendimento[i].isHomeCare = 0;
                    preAtendimento[i].isPreatendimento = 0;
                    //preAtendimento[i].dataRegistro =  moment().format('L');
                    
                    //_AtendimentosService.criarOuEditar(preAtendimento[i]);

                    //carregra os dados do preAtendimento na aba de edição de internação
                    window.editarPreAtendimento(preAtendimento[i]);
                   
                }
                _modalManager.close();
            }

            else {
                abp.notify.warn(app.localize('SelecioneLista'));
                }
            //carregra o grib de atendimento
            window.carregaAtendimento();
        };

        function getPre(reload) {
            //_$PreAtendimentosTable.jtable('load');
            if (reload) {
                _$PreAtendimentosTable.jtable('reload');
            } else {
                _$PreAtendimentosTable.jtable('load', {
                    filtro: $('#PreAtendimentosFilter').val()
                });
            }
        }

        _$PreAtendimentosTable.jtable({
            title: app.localize('PreAtendimentos'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting'                                                                                                                                                                                                                                                                                                                                                                                                                
            //multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column
            actions: {
                listAction: {
                    method: _AtendimentosService.listarFiltroPreAtendimento
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                }
                ,
                tipoAtendimento: {
                    title: app.localize('TipoAtendimento'),
                    width: '7%',
                    display: function (data) {
                        if (data.record.atendimentoTipo) {
                            return data.record.atendimentoTipo.descricao;
                        }
                    }
                }
                ,
                paciente: {
                    title: app.localize('Paciente'),
                    width: '9%',
                    display: function (data) {
                        if (data.record.paciente) {
                            return data.record.paciente.nomeCompleto;
                        }
                    }
                }
                ,
                dataRegistro: {
                    title: app.localize('DataPreAtendimento'),
                    width: '3%',
                    display: function (data) {
                        return moment(data.record.dataPreatendimento).format('L');
                    }
                }
                ,
                observacao: {
                    title: app.localize('Observacao'),
                    width: '9%'
                    ,
                    display: function (data) {
                        if (data.record) {
                            return data.record.observacao;
                        }
                    }
                }
            },
             selectionChanged: function () {
                    //Get all selected rows
                    var $selectedRows = $('#PreAtendimentosTable').jtable('selectedRows');
                    if ($selectedRows.length > 0) {
                        //Show selected rows
                        var list = [];
                        var i = 0;
                        $selectedRows.each(function () {
                            var record = $(this).data('record');
                            list[i] = record;
                            i++;
                        })
                        preAtendimento = [];
                        preAtendimento = list;
                    }
                }
            
        });

        getPre();

        $('#GetPre').click(function (e) {
            e.preventDefault();
            getPre();
        });
    };
})(jQuery);