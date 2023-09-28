(function () {
    $(function () {
        var _$VisualAsaImportExportLogsTable = $('#VisualAsaImportExportLogsTable');
        var _VisualAsaImportExportLogsService = abp.services.app.visualAsaImportExportLog;
        var _$filterForm = $('#VisualAsaImportExportLogsFilterForm');

        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };

        $('#periodo').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });

        _$VisualAsaImportExportLogsTable.jtable({

            title: app.localize('VisualAsaImportExportLogs'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _VisualAsaImportExportLogsService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '20%',
                    display: function (data) {
                        return moment(data.record.creationTime).format("L LT");
                    }
                },
                tabela: {
                    title: app.localize('Tabela'),
                    width: '20%'
                },
                idSw: {
                    title: app.localize('IdSw'),
                    sorting: false,
                    width: '20%'
                },
                idAsa: {
                    title: app.localize('IdAsa'),
                    sorting: false,
                    width: '20%'
                },
                operacao: {
                    title: app.localize('Operacao'),
                    sorting: false,
                    width: '20%'
                }
            }
        });

        function getVisualAsaImportExportLogs(reload) {
            _$VisualAsaImportExportLogsTable.jtable('load', createRequestParams())
        }

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms, _selectedDateRange);
        }

        $('#ToggleService').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                method: 'post',
                url: '/mpa/visualasaimportexport/alternarservico',
                success: function (data) {
                    if (data == "Stopped") {
                        swal(app.localize("ServiceStopped"));
                    }
                    else {
                        swal(app.localize("ServiceStarted"));
                    }
                    location.reload();
                }
            });
        });

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

        $('#ExportarVisualAsaImportExportLogsParaExcelButton').click(function () {
            _VisualAsaImportExportLogsService
                .listarParaExcel({
                    filtro: $('#VisualAsaImportExportLogsTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetVisualAsaImportExportLogsButton, #RefreshVisualAsaImportExportLogsButton').click(function (e) {
            e.preventDefault();
            getVisualAsaImportExportLogs();
        });

        //abp.event.on('app.CriarOuEditarCidadeModalSaved', function () {
        //    getVisualAsaImportExportLogs();
        //});

        getVisualAsaImportExportLogs();

        $('#VisualAsaImportExportLogsTableFilter').focus();
    });
})();