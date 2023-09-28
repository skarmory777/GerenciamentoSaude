(function () {
    $(function () {
        //var _$AtendimentosPacienteTable = $('#AtendimentosPacienteTable-' + localStorage["AtendimentoId"]);
        //var _atendimentosService = abp.services.app.atendimento;
        //var _$filterForm = $('#AtendimentosFilterForm-' + localStorage["AtendimentoId"]);

        //var _selectedDateRange = {
        //    startDate: moment().startOf('day'),
        //    endDate: moment().endOf('day')
        //};

        ///*
        // * Jtable assistencial - histório de atendimentos  
        // */
        //_$AtendimentosPacienteTable.jtable({

        //    title: app.localize('HistoricoAtendimentos'),
        //    paging: true,
        //    sorting: true,
        //    multiSorting: true,
        //    selecting: true, //Enable selecting
        //    multiselect: false, //Allow multiple selecting
        //    selectingCheckboxes: true, //Show checkboxes on first column

        //    actions: {
        //        listAction: {
        //            method: _AssistencialAtendimentosService.listarPorPaciente
        //        }
        //    },

        //    fields: {
        //        id: {
        //            key: true,
        //            list: false
        //        },
        //        dataAtendimento: {
        //            title: app.localize('Codigo'),
        //            width: '33%',
        //            display: function (data) {
        //                return moment(data.record.dataAtendimento).format('L LT');
        //            }
        //        },
        //        local: {
        //            title: app.localize('DataRegistro'),
        //            width: '33%'
        //        },
        //        usuario: {
        //            title: app.localize('Paciente'),
        //            width: '33%',
        //            display: function (data) {
        //                return data.record.creatorUserId;
        //            }
        //        },
        //        tipoGuia: {
        //            title: app.localize('TipoGuia'),
        //            width: '15%'
        //        },
        //        numeroGuia: {
        //            title: app.localize('NumeroGuia'),
        //            width: '10%'
        //        }
        //    },
        //    selectionChanged: function () {
        //        //Get all selected rows
        //        var $selectedRows = $('#list-historico-atendimento-' + localStorage["AtendimentoId"]).jtable('selectedRows');
        //        if ($selectedRows.length > 0) {
        //            //Show selected rows
        //            $selectedRows.each(function () {
        //                var record = $(this).data('record');
        //                _atendimentoModal.open(record.id);
        //            });
        //        }
        //    },
        //});

        //$('.dropdown').on('show.bs.dropdown', function () {

        //    //get the value (.dropdown-menu's width) and make it negative
        //    var length = parseInt($('.dropdown-menu').css('width'), 10) * -1;

        //    $('.dropdown-menu').css('left', length);
        //});
        ///*fim das funções anônimas de inicialização*/
        $('header-area-' + localStorage["AtendimentoId"]).load('mpa/assistenciais/headeratendimento/', { id: $('header-area-' + localStorage["AtendimentoId"]).data('id') });
        $('menu-modulo-' + localStorage["AtendimentoId"]).load('mpa/layout/SidebarTab/', { key: $('menu-modulo-' + localStorage["AtendimentoId"]).data('key'), menuName: $('menu-modulo-' + localStorage["AtendimentoId"]).data('menu-name'), currentPageName: $('menu-modulo-' + localStorage["AtendimentoId"]).data('current-page-name') });

        debugger;


        if ($.cookies('localChamada').val() !== null && $.cookies('localChamada').val() !== undefined) {
            $('#chamar-senha-' + localStorage["AtendimentoId"]).show();
        }
        else {
            $('#chamar-senha-' + localStorage["AtendimentoId"]).hide();
        }


        $('#chamar-senha-' + localStorage["AtendimentoId"]).on('click', function (e) {
            e.preventDefault();
            var _terminalSenhasService = abp.services.app.terminalSenhas;
            _terminalSenhasService.chamarSenha($('#tipo-local-chamada-id-' + localStorage["AtendimentoId"]).val(), $('#local-chamada-id-' + localStorage["AtendimentoId"]).val(), $('#senha-id-' + localStorage["AtendimentoId"]).val());
            $.cookie('localChamada', $('#local-chamada-id-' + localStorage["AtendimentoId"]).val());
        });
    });
})();