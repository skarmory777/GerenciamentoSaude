(function () {

    $(function () {

        //remover isso

        $('.modal-dialog').css('width', '1800px');

        var _$coletasTable = $('#coletasTable');
        var _resultadoService = abp.services.app.evolucaoResultados;

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Delete')
        };

        var _ModalVisualizarExamePorColeta = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/EvolucaoResultados/ModalVisualizarComparativoResultado',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Laboratorios/EvolucaoResultados/ListaExames.js',
            modalClass: 'ListaExamesModal',
            focusFunction: (_$modal) => { setTimeout(() => { _$modal.find('#comparativoEvolucaoResultadoTableFilter').get(0).focus(); }, 1) }
        });

        console.log(_ModalVisualizarExamePorColeta);

        _$coletasTable.jtable({

            title: app.localize('Confirmacao'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _resultadoService.listarNaoConferido
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },

                actions: {
                    title: app.localize('Actions'),
                    width: '2%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');

                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-binoculars"></i></button>')
                       .appendTo($span)
                       .click(function () {
                           _ModalVisualizarExamePorColeta.open({
                               id: data.record.id,
                               pacienteId: data.record.pacienteId,
                               atendimentoId: data.record.atendId,
                               codigoPaciente: data.record.Codigo,
                               nomePaciente: data.record.nomeCompleto
                           });
                           // location.href = 'GestaoLaudos/CriarOuEditarModal/' + data.record.id
                       });

                        return $span;
                    }
                },

                Codigo: {
                    title: app.localize('Codigo'),
                    width: '2%',
                    display: function (data) {
                        return data.record.codigoPaciente;
                    }
                },

                Paciente: {
                    title: app.localize('Paciente'),
                    width: '10%',
                    display: function (data) {
                        return data.record.nomeCompleto;
                    }
                }
            }
        });

        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };
        
        function getResultados(reload) {
            if (reload) {
                _$coletasTable.jtable('reload');
            } else {
                _$coletasTable.jtable('load', {
                    filtro: $('#ResultadosTableFilter').val(),
                });
            }
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
        
        $('#GetResultadosButton').click(function (e) {
            e.preventDefault();
            getResultados();
        });    

        getResultados();

        $('#ResultadosTableFilter').focus();
        
    });
})();