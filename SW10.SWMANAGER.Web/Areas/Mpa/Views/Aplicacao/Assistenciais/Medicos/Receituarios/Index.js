(function () {
    $(function () {

        var _$ReceituariosTable = $('#ReceituariosTable-' + localStorage["AtendimentoId"]);
        var _receituariosService = abp.services.app.receituarioMedico;
        var _$filterForm = $('#ReceituariosFilterForm-' + localStorage["AtendimentoId"]);
        var _selectedDateRange = {
            startDate: moment(localStorage["DataAtendimento"]).startOf('day'), //moment().startOf('day'),
            endDate: moment().endOf('day')
        };

        console.log(_selectedDateRange);

        $('#date-range-' + localStorage["AtendimentoId"]).daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                console.log(_selectedDateRange);
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Receituario'),
            edit: abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Receituario'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Receituario')
        };
        
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Assistenciais/CriarOuEditarMedicoReceituario',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Receituarios/CriarOuEditarMedicoReceituario.js',
            modalClass: 'CriarOuEditarMedicoReceituarioModal'
        });

        abp.event.off('app.CriarOuEditarReceituariosModalSaved', onEventCriarOuEditarReceituariosModalSaved)
        abp.event.on('app.CriarOuEditarReceituariosModalSaved', onEventCriarOuEditarReceituariosModalSaved)

        function onEventCriarOuEditarReceituariosModalSaved(data) {
            if(data && data.id) {
                $.removeCookie("XSRF-TOKEN");
                printJS({
                    printable: '/Mpa/AssistenciaisRelatorios/ImprimirReceituarios?solicitacaoExameId=' + data.id, type: 'pdf',
                })
            }
        }

        $('#ReceituariosTable-' + localStorage["AtendimentoId"]).jtable({
            title: app.localize('Receituarios'),
            paging: true,
            sorting: true,
            multiSorting: true,
            multiSorting: true,
            selecting: true,
            selectingCheckboxes: true,
            actions: {
                listAction: {
                    method: _receituariosService.listarTodos
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '1%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        // Link da receita digital do paciente
                        $('<button class="btn btn-primary btn-xs" title="' + app.localize('CopiarLinkReceitaDigital') + '"><i class="fa fa-clone"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                linkReceitaDigitalPaciente(data.record.id);
                            });

                        return $span;
                    }
                },
                dataReceituario: {
                    title: app.localize('Data'),
                    width: '25%',
                    display: function (data) {
                        return moment(data.record.dataReceituario).format('L LT');
                    }
                },
                medico: {
                    title: app.localize('Medico'),
                    width: '15%',
                    display: function (data) {
                        return data.record.medico.sisPessoa.nomeCompleto;
                    }
                },
            },
            selectionChanged: function () {
                var $selectedRows = $('#ReceituariosTable-' + localStorage["AtendimentoId"]).jtable('selectedRows');

                if ($selectedRows.length > 0) {
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        abrirPDFPrescricao(record.id, localStorage["AtendimentoId"]);
                    });
                }
            },
            recordsLoaded: function (event, data) {

                $("div[id^='ReceituariosTable'] .jtable-main-container tr.jtable-data-row:first input[type=checkbox]").trigger('click');
            }
        });

        function abrirPDFPrescricao(receituarioId, atendimentoId) {
            App.startPageLoading({ animate: true }); document.querySelector('.loadingCommon').style.display = null;

            document.querySelector('#iframeReceitaPDF').src = '';

            _receituariosService.obterLinkMemedPDFPrescricao(receituarioId, atendimentoId).then(res => {
                var xhr = new XMLHttpRequest();

                xhr.open('GET', res);
                xhr.onreadystatechange = handler;
                xhr.responseType = 'blob';
                xhr.send();

                function handler() {
                    if (this.readyState === this.DONE) {
                        if (this.status === 200) {
                            // this.response is a Blob, because we set responseType above
                            var data_url = URL.createObjectURL(this.response);
                            document.querySelector('#iframeReceitaPDF').src = data_url;
                            App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
                        } 
                    }
                }
            }).always(() => {
                App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
            });
        }

        function linkReceitaDigitalPaciente(receituarioId) {
            App.startPageLoading({ animate: true }); document.querySelector('.loadingCommon').style.display = null;

            _receituariosService.obterLinkMemedReceitaDigitalPacientePrescricao(receituarioId, localStorage["AtendimentoId"]).then(res => {
                navigator.clipboard.writeText(res.link);

                App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
                swal({
                    title: app.localize('LinkReceitaDigitalCopiado'),
                    text: app.localize('CodigoDesbloqueio') + ': ' + res.digits
                });
            }).always(() => {
                App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
            });     
        }

        function getReceituarios() {
            const data = {
                atendimentoId: localStorage["AtendimentoId"],
                startDate: _selectedDateRange.startDate,
                endDate: _selectedDateRange.endDate
            }

            $('#ReceituariosTable-' + localStorage["AtendimentoId"]).jtable('load', data);
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
      
        $('#CreateNewReceituarioButton-' + localStorage["AtendimentoId"]).click(function (e) {
            e.preventDefault();
            const btn = $(this);
            btn.buttonBusy(true);
            _receituariosService.gerarNovoReceituarioMedico(localStorage["AtendimentoId"]).then(res => {
                //_createOrEditModal.open({ atendimentoId: localStorage["AtendimentoId"], receituarioId: res.id });

                var url = '/Mpa/Assistenciais/CriarOuEditarMedicoReceituario';
                url += '?atendimentoId=' + localStorage["AtendimentoId"];
                url += '&receituarioId=' + res.id;
                criarNewAba(sessionStorage["id"],
                    sessionStorage["dataRegistro"],
                    sessionStorage["codigoAtendimento"],
                    sessionStorage["paciente"],
                    url,
                    "Receituário");
            }).always(() => {
                btn.buttonBusy(false);
            });
        });

        $('#ExportarReceituariosParaExcelButton-' + localStorage["AtendimentoId"]).click(function () {
            _receituariosService
                .listarParaExcel({
                    filtro: $('#ReceituariosTableFilter-' + localStorage["AtendimentoId"]).val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetReceituariosButton-' + localStorage["AtendimentoId"]).on('click', function (e) {
            e.preventDefault();
            getReceituarios();
        });

        $('#RefreshReceituariosListButton-' + localStorage["AtendimentoId"]).on('click', function (e) {
            e.preventDefault();
            getReceituarios();
        });

        abp.event.on('app.CriarOuEditarReceituariosModalSaved', function () {
            getReceituarios();
        });

        getReceituarios();

        $('#ReceituariosTableFilter-' + localStorage["AtendimentoId"]).focus();

        function renderizarRelatorio(id) {
            var caminho = `/Mpa/AssistenciaisRelatorios/ImprimirReceituarioss%3FsolicitacaoExameId%3D` + id;
            var urlPath = window.location.href.split(window.location.pathname)[0].split("//")[1];
            $('#fVisualizar').attr('src', "//" + urlPath + "/libs/pdfjs/web/viewer.html?file=" + caminho + "&locale=pt-BR");
            $('#dvVisualizar').show();
        }
    });
})();