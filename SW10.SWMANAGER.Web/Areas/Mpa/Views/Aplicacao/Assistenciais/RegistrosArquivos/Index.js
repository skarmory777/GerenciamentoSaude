(function ($) {
    app.modals.ListarRegistroArquivos = function () {
        let _modalManager;
        const registroArquivoAppService = abp.services.app.registroArquivo;
        const _$registrosArquivosTable = $('#registrosArquivosTable');
        this.init = function (modalManager) {
            _modalManager = modalManager;
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            
            getRegistrosArquivos();
        }

        $('.modal-dialog').css('width', '90vw').css('height', '80vh');
        selectSW("#FiltroOperacao",'/api/services/app/RegistroArquivo/ListarRegistroTabelas');
        $("#FiltroOperacao").on("change",(e) => {
            e.stopImmediatePropagation();
            getRegistrosArquivos();
        })
        let _selectedDateRange = {
            startDate: moment().add(-90, "days").startOf('day'),
            endDate: moment().endOf('day')
        };
        $('#FiltroIntervaloDatas').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
                getRegistrosArquivos();
            });
        $('#RefreshPepAssistencialAtendimentosButton').on("click",(e) => {
            e.stopImmediatePropagation();
            getRegistrosArquivos();
        })
        _$registrosArquivosTable.jtable({

            title: app.localize('Imagens'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: false, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column

            actions: {
                listAction: {
                    method: registroArquivoAppService.listarPorAtendimento,
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                OperacaoDescricao: {
                    title: app.localize('Operacao'),
                    width: '10%',
                    display: function (data) {
                        return data.record.operacaoDescricao;
                    }
                },
                data: {
                    title: app.localize('Data'),
                    width: '10%',
                    display: function (data) {
                        return moment(data.record.dataRegistro).format('L HH:mm');
                    }
                },
            },
            recordsLoaded : function (event, data) {
                $('#registrosArquivosTable .jtable-main-container tr.jtable-data-row:first input[type=checkbox]').trigger('click');
            },

            selectionChanged: function () {
                var $selectedRows = _$registrosArquivosTable.jtable('selectedRows');
                if ($selectedRows.length > 0) {
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        exibirImagemRegistroArquivo(record);
                    });
                }
            }
        })
        function getRegistrosArquivos() {
            const data = { 
                atendimentoId: $('#atendimentoDoRegistroId').val(),
                operacaoId : $("#FiltroOperacao").val(),
                startDate: _selectedDateRange.startDate,
                endDate: _selectedDateRange.endDate
            }
            
            _$registrosArquivosTable.jtable('load', data);
        }
        function exibirImagemRegistroArquivo(record) {
           
            if (record.isPDF) {
                $('#imagemRegistroArquivo').hide();
                $('#divPDF').show();
                
                caminho = "/Mpa/RegistroArquivo/VisualizarPorId?id=" + record.registroId;

                PDFObject.embed(caminho, "#divPDF", {});
            }
            else
            {
                $('#divPDF').hide();
                $('#imagemRegistroArquivo').show();

                var caminho = "/mpa/Assistenciais/VisualizarImagemRegistroArquivo/" + record.registroId;

                $.ajax({
                    url: caminho,
                    type: 'POST',

                    beforeSend: function () {
                        abp.ui.setBusy('#divImg');
                    },

                    success: function (data) {
                    }
                });
            }
        }
    };
})(jQuery);