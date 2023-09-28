(function () {
    $(function () {
        $(document).ready(function () {
            if ($('#tipo-rel').val() != 1) {
                $('.div-detalhado').show();
            }
            else {
                $('.div-detalhado').hide();
            }
        });

        $('#tipo-atendimento').change(function (e) {
            e.preventDefault();
            debugger;
            var unidade = '';
            if ($('#tipo-atendimento').val() == 1) {
                unidade = "ambEmr"
            } else if ($('#tipo-atendimento').val() == 2) {
                unidade = "inter"
            } else {
                unidade = ""
            }

            selectSW('#unidade-organizacional-id', "/api/services/app/UnidadeOrganizacional/ListarDropdownPorUsuarioTipoAtendimento", { valor: unidade });
        });


        $('#tipo-rel').on('change', function () {
            if ($('#tipo-rel').val() != 1) {
                $('.div-detalhado').show();
            }
            else {
                $('.div-detalhado').hide();
            }
        });
        var _$filterForm = $('#AtendimentoFilterForm');

        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };
        //_$filterForm.find('.date-range-picker').daterangepicker(
        $('#periodo').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms, _selectedDateRange);
        }

        $("#btnVisualizar").click(function () {
            exibirRelatorio();
        });

        function exibirRelatorio() {
            createRequestParams();
            /*var caminho = $('#tipo-rel').val() < 4 ? '/mpa/atendimentorelatorio/VisualizarRptAtendimentoResumidoPDF' : '/mpa/atendimentorelatorio/VisualizarRptAtendimentoDetalhadoPDF';*/
            var caminho = '/mpa/atendimentorelatorio/MapaDiaSintatico';
            var vData = $('#periodo').val().split(' - ');
            var dataIni = vData[0];
            var dataFim = vData[1];
            $.ajax({
                url: caminho,
                method: 'post',
                //data: {
                //    EmpresaId: $('#empresausuario-id').val() != null ? $('#empresausuario-id').val() : 0,
                //    StartDate: dataIni, //_selectedDateRange.startDate.format('L'),
                //    EndDate: dataFim, //_selectedDateRange.endDate.format('L'),
                //    MedicoId: $('#medico-id').val() != null ? $('#medico-id').val() : 0,
                //    EspecialidadeId: $('#especialidade-id').val() != null ? $('#especialidade-id').val() : 0,
                //    ConvenioId: $('#convenio-id').val() != null ? $('#convenio-id').val() : 0,
                //    PacienteId: $('#paciente-id').val() != null ? $('#paciente-id').val() : 0,
                //    UnidadeOrganizacionalId: $('#unidade-organizacional-id').val() != null ? $('#unidade-organizacional-id').val() : 0,
                //    TipoAtendimento: $('#tipo-atendimento').val() != null ? $('#tipo-atendimento').val() : 0,
                //    TipoRel: $('#tipo-rel').val(),
                //    TipoPeriodo: $('#tipo-periodo').val(),
                //    Filtrado: montaFiltros()
                //},
                data: {
                    UnidadeOrganizacionalId: $('#unidade-organizacional-id').val() != null ? $('#unidade-organizacional-id').val() : 0,
                    StatusId: $('input[name="rdStatusLeito"]:checked').val() != null ? $('input[name="rdStatusLeito"]:checked').val() : 0,
                },
                cache: false,
                async: false,
                beforeSend: function () {
                    abp.ui.setBusy();
                },
                complete: function () {
                    abp.ui.clearBusy();
                },
                error: function () {
                    abp.ui.clearBusy();
                },
                fail: function () {
                    abp.ui.clearBusy();
                },
                success: function (data) {
                    var path = data;
                    var urlPath = window.location.href.split(window.location.pathname)[0].split("//")[1];
                    $('#fVisualizar').attr('src', "//" + urlPath + "/libs/pdfjs/web/viewer.html?file=" + path + "&locale=pt-BR");
                    $('#dvVisualizar').show();
                }
            });
        }

        function montaFiltros() {
            var filtro = "";

            filtro = $('#empresausuario-id').val() != null ? filtro + " Empresa " + $('#empresausuario-id').select2('data')[0].text + ";" : filtro;
            filtro = $('#unidade-organizacional-id').val() != null ? filtro + " Unidade " + $('#unidade-organizacional-id').select2('data')[0].text + ";" : filtro;
            filtro = $('#medico-id').val() != null ? filtro + " Médico " + $('#medico-id').select2('data')[0].text + ";" : filtro;
            filtro = $('#especialidade-id').val() != null ? filtro + " Especialidade " + $('#especialidade-id').select2('data')[0].text + ";" : filtro;
            filtro = $('#convenio-id').val() != null ? filtro + " Convênio " + $('#convenio-id').select2('data')[0].text + ";" : filtro;
            filtro = $('#paciente-id').val() != null ? filtro + " Paciente " + $('#paciente-id').select2('data')[0].text + ";" : filtro;

            return filtro != "" ? "Filtrado por : " + filtro : "Nenhum filtro selecionado";

        }

        /*//Exportar arquivos Word, PDF, Excel
        $(".btnExportar").click(function () {
            window.open(urlExportar + montarParam() + "&formato=" + $(this).data("formato"));
        });


    grupo.change(function () {
        carregar(urlGrupoClasse + "/" + $(this).val(), classe);
    });

    classe.change(function () {
        carregar(urlGrupoSubClasse + "/" + $(this).val(), $("#SubClasse"));
    });

    function carregar(url, destino) {
        $.get(url, function (result) {
            if (result) {
                destino.html("");
                for (var i in result) {
                    destino.append(new Option(result[i].Text, result[i].Value));
                }
            }
        });
    }

    function montarParam() {
        return "?GrupoProduto=" + grupo.val() + "&Classe=" + classe.val() + "&SubClasse=" + subClasse.val() + "&EhMovimentacao=" + "@Model.EhMovimentacao.ToString()";
    }*/



        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAtendimentosFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAtendimentosFiltersArea').slideUp();
        });

        $('body').addClass('page-sidebar-closed');

        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');

        $('.select2').css('width', '100%');

        aplicarSelect2Padrao();
        aplicarDateRange();

        selectSW('.selectEmpresa', '/api/services/app/empresa/ListarDropdownPorUsuario');
        
    });
})();