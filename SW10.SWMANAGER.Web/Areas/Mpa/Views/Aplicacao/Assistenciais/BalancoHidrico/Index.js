(function () {
    $(function () {
        const _balancoHidricoService = abp.services.app.balancoHidrico;
        
        const graficoOptions = criarGraficoOptions();

        const numberTempMaskTemplate = {
            mask: 'num',
            blocks: {
                num: {
                    mask: Number,
                    thousandsSeparator: '.',
                    scale: 1,	// digits after decimal
                    signed: false, // allow negative
                    normalizeZeros: true,  // appends or removes zeros at ends
                    radix: ',',  // fractional delimiter
                    padFractionalZeros: true,  // if true, then pads zeros at end to the length of scale
                    allowDecimal: true
                }
            },
        };

        const numberMaskTemplate = {
            mask: 'num',
            blocks: {
                num: {
                    mask: Number,
                    thousandsSeparator: '.',
                    scale: 0,	// digits after decimal
                    signed: false, // allow negative
                    normalizeZeros: true,  // appends or removes zeros at ends
                    radix: ',',  // fractional delimiter
                    padFractionalZeros: false,  // if true, then pads zeros at end to the length of scale
                    allowDecimal: false
                }
            },
        };

        const numberSolucaoMaskTemplate = {
            mask: 'num',
            blocks: {
                num: {
                    mask: Number,
                    thousandsSeparator: '.',
                    scale: 2,	// digits after decimal
                    signed: false, // allow negative
                    normalizeZeros: true,  // appends or removes zeros at ends
                    radix: ',',  // fractional delimiter
                    padFractionalZeros: true,  // if true, then pads zeros at end to the length of scale
                    allowDecimal: true
                }
            },
        };

        let maskFields = {};

        $('.balancoDate').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            maxDate: new Date(),
            autoUpdateInput: true,
            changeYear: false,
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR'
                    ? "DD/MM/YYYY"
                    : moment.locale().toUpperCase() === 'US'
                        ? "MM/DD/YYYY"
                        : "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Apply",
                "cancelLabel": "Cancel",
                "fromLabel": "From",
                "toLabel": "To",
                "customRangeLabel": "Custom",
                "daysOfWeek": [
                    app.localize('Dom'),
                    app.localize('Seg'),
                    app.localize('Ter'),
                    app.localize('Qua'),
                    app.localize('Qui'),
                    app.localize('Sex'),
                    app.localize('Sab')
                ],
                "monthNames": [
                    app.localize("Jan"),
                    app.localize("Fev"),
                    app.localize("Mar"),
                    app.localize("Abr"),
                    app.localize("Mai"),
                    app.localize("Jun"),
                    app.localize("Jul"),
                    app.localize("Ago"),
                    app.localize("Set"),
                    app.localize("Out"),
                    app.localize("Nov"),
                    app.localize("Dez"),
                ],
                "firstDay": 0
            }
        }, callbackDatePicker);

        hotkeys.filter = function(){ return true; }
        
        hotkeys('ctrl+up, ctrl+down, ctrl+left, ctrl+right, enter', function (event, handler){
            const el = $(event.target || event.srcElement);
            if(!el.hasClass("navegavel")){
                return;
            }
            event.stopImmediatePropagation();
            
            let matriz = [];
            let inputs =  [];
            let matrizLimit = 0;
            if(el.parents(".conteudo-legendas-solucoes").length) {
                matrizLimit = 6;
                inputs = _.sortBy($(".conteudo-legendas-solucoes").find(":input:not([type=hidden])").map(function() {
                        const el = $(this);
                        return {index: el.data("index"), name: el.attr("name")}
                    }),'index');
                
            } else if (el.parents(".conteudo-balanco-hidrico").length) {
                matrizLimit = 30;
                inputs = _.sortBy($(".conteudo-balanco-hidrico").find("input").map(function() {
                    const el = $(this);
                    return {index: el.data("index"), name: el.attr("name")}
                }),'index');
            }

            _.forEach(inputs, (item) => {
                if(matriz.length === 0) {
                    matriz.push([]);
                }
                let currentMatrizItem = matriz[matriz.length -1];
                if(currentMatrizItem.length === matrizLimit)
                {
                    matriz.push([]);
                    currentMatrizItem = matriz[matriz.length -1];
                }
                currentMatrizItem.push(item);
            })
            let currentPosition = el.data("index");
            let newInput;
            let newPosition;
            switch (handler.key) {
                case 'ctrl+up':
                    newPosition = currentPosition -matrizLimit;
                    doInputChange(newPosition,cbMenos);
                    break;
               
                case 'ctrl+down':
                    newPosition = currentPosition +matrizLimit;
                    doInputChange(newPosition,cbMais);
                    break;
                case 'ctrl+left':
                    newPosition = currentPosition -1;
                    doInputChange(newPosition,cbMenos);
                    break;
                case 'ctrl+right':
                    newPosition = currentPosition +1;
                    doInputChange(newPosition,cbMais);
                    break;
                case 'enter':
                    newPosition = currentPosition +matrizLimit;
                    doInputChange(newPosition,cbEnter);
                    break;
            }
            function cbMais (position) { return position +1 }
            
            function cbMenos (position) { return position -1 }

            function cbEnter (position) { return position + matrizLimit }
            
            function doInputChange(newPosition, callback) {
                newInput = getInput(newPosition);
                if(newInput) {
                    newInput = $(`[name='${newInput.name}']`);
                    if(!newInput.length) return;

                    if(newInput.attr("type") === "hidden") {
                        return doInputChange(callback(newPosition),callback)
                    }
                    else {
                        changeInput(newInput);
                    }
                }
            }
            function getInput(newPosition) {
                if(_.findIndex(inputs,(obj) => obj.index === newPosition) === -1) {
                    return;
                }
                let newEl;
                _.forEach(matriz, (linha) => {
                    _.forEach(linha, (obj) => {
                        if(obj.index === newPosition) {
                            newEl = obj;
                            return;
                        }
                    })
                })
                return newEl;
            }
            function changeInput(newEl) {
                newEl.focus();
                $('html, body').animate({ scrollTop: newEl.offset().top }, 500);
            }
        });

        $("#chooseDate").on("click", () => {$('.balancoDate').show()});


        $("#previousDate").on("click", function () {
            let date = moment($(".balancoDate").data("defaultValue"), "DD/MM/YYYY");
            date = date.subtract(1, 'days');
            $(".balancoDate").data('daterangepicker').setStartDate(date.format("DD/MM/YYYY"));
            callbackDatePicker(date);
        });
        
        $(".btn-conferir").on("click",conferir);
        $(".btn-conferirTotal").on("click",conferirTotal);
        
        $(".btn-desConferir").on("click",desconferir);

        $(".btn-desConferirTotal").on("click",desconferirTotal);
        
        $("#nextDate").on("click", function () {
            let date = moment($(".balancoDate").data("defaultValue"), "DD/MM/YYYY");
            date = date.add(1, 'days');

            $(".balancoDate").data('daterangepicker').setStartDate(date.format("DD/MM/YYYY"));
            callbackDatePicker(date);
        });

        var defaultDate = moment($(".balancoDate").data("defaultValue"), "DD/MM/YYYY ");
        ajusteTamanhoBalanco();

        callbackDatePicker(defaultDate);

        function callbackDatePicker(start) {
            date = moment(start);
            var previous = date.clone().subtract(1, 'days');
            $('.balancoDate span').html(date.format('DD/MM/YYYY'));
            $(".balancoDate").data("defaultValue", date.format("DD/MM/YYYY"));
            $("#bl24").html(previous.format('DD/MM/YYYY'));

            setTimeout(() => { callAjax(start); }, 100);
        }

        function callAjax(start) {
            $(".balanco .container-content").addClass("hidden");
            $(".balanco .loader").removeClass("hidden");
            
            $.ajax({
                cache: false,
                async: true,
                url: `/Mpa/Assistenciais/BalancoHidricoPartial/`,
                method: 'post',
                data: {
                    id: $("#atendimentoId").val(),
                    date: start.toISOString()
                }
            }).done((data) => { onPartial(data, start) });
        }

        function balancoHidricoCopiarSolucoesAjax(start, copiarSolucoes) {
            $(".balanco .container-content").addClass("hidden");
            $(".balanco .loader").removeClass("hidden");
            $.ajax({
                cache: false,
                async: true,
                url: `/Mpa/Assistenciais/BalancoHidricoCopiarSolucoes/`,
                method: 'post',
                data: { id: $("#atendimentoId").val(), date: start.toISOString(), copiarSolucoes: copiarSolucoes}
            }).done((data) => {
                debugger;
                callAjax(start);
             });
        }
        
        function onClickBtnTodosGraficos(e) {
            e.stopImmediatePropagation();
            const tipoGrafico = 'todos';
            let modal = `<div class="modal fade modal-${tipoGrafico}" role="dialog">
                  <div class="modal-dialog" style="min-width: 95vw">
                    <!-- Modal content-->
                    <div class="modal-content">
                      <div class="modal-body">
                        <div class="row">
                            <div class="col-md-4"> <div class="grafico-todos-t" style="width: 100%; height: 300px"></div> </div>
                            <div class="col-md-4"> <div class="grafico-todos-p" style="width: 100%; height: 300px"></div> </div>
                            <div class="col-md-4"> <div class="grafico-todos-r" style="width: 100%; height: 300px"></div> </div>
                            <div class="col-md-12"> <div class="grafico-todos-pa" style="width: 100%; height: 300px"></div> </div>
                            <div class="col-md-4"> <div class="grafico-todos-spo2" style="width: 100%; height: 300px"></div> </div>
                            <div class="col-md-4"> <div class="grafico-todos-eva" style="width: 100%; height: 300px"></div> </div>
                            <div class="col-md-4"> <div class="grafico-todos-hgt" style="width: 100%; height: 300px"></div> </div>
                        </div>
                      </div>
                    </div>
                  </div>`;
            $(".conteudo-balanco-hidrico").append(modal);

            $(`.modal-${tipoGrafico}`).modal();

            $(`.modal-${tipoGrafico}`).on('shown.bs.modal', function (e) {
                setTimeout(() => {
                    createChart('t')
                    createChart('p')
                    createChart('r')
                    createChart('spo2')
                    createChart('pa')
                    createChart('eva')
                    createChart('hgt')
                    
                    function createChart(tipoGrafico) {
                        let options = {};
                        let chartDom = $(`.grafico-todos-${tipoGrafico}`)[0];
                        let myChart = echarts.init(chartDom);

                        if(tipoGrafico == 'pad' || tipoGrafico == 'pas') {
                            options = graficoOptions.pa();
                        } else if(_.isFunction(graficoOptions[tipoGrafico])) {
                            options = graficoOptions[tipoGrafico](jsonData(tipoGrafico));
                        }
                        options && myChart.setOption(options);
                    }
                },0)
            })

            $(`.modal-${tipoGrafico}`).on('hidden.bs.modal', function (e) {
                $(`.modal-${tipoGrafico}`).remove();
            })
        }

        function onClickBtnGrafico (e) {
            e.stopImmediatePropagation();
            const tipoGrafico = $(e.currentTarget).data("grafico");
            let modal = `<div class="modal fade modal-${tipoGrafico}" role="dialog">
                  <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                      <div class="modal-body">
                        <div class="row">
                            <div class="grafico-${tipoGrafico}" style="width: 100%; min-height: 400px"></div>
                        </div>
                      </div>
                    </div>
                  </div>`;
            $(".conteudo-balanco-hidrico").append(modal);

            $(`.modal-${tipoGrafico}`).modal();

            $(`.modal-${tipoGrafico}`).on('shown.bs.modal', function (e) {
                setTimeout(() => {
                    let options = {};
                    let chartDom = $(`.grafico-${tipoGrafico}`)[0];
                    let myChart = echarts.init(chartDom);

                    if(tipoGrafico == 'pad' || tipoGrafico == 'pas') {
                        options = graficoOptions.pa();
                    } else if(_.isFunction(graficoOptions[tipoGrafico])) {
                        options = graficoOptions[tipoGrafico](jsonData(tipoGrafico));
                    }
                    options && myChart.setOption(options);
                
                },0)
            })
            
            $(`.modal-${tipoGrafico}`).on('hidden.bs.modal', function (e) {
                $(`.modal-${tipoGrafico}`).remove();
            })
        }

        function changeEvacuacoes() {
            $(this).parents(".grupo-checkbox-evacuacao").find(".checkbox-evacuacao").not(this).prop('checked', false)
        }

        function onPartial(data, start) {
            $(".balanco .container-content").html(data);
            botoesConferencia();
            $(".balanco .loader").addClass("hidden")
            $(".balanco .container-content").removeClass("hidden");

            $('.checkbox-evacuacao').off('change', changeEvacuacoes);
            $('.checkbox-evacuacao').on('change', changeEvacuacoes);
            
            $("#conteudo-sinais-vitais").html($("#conteudo-sinais-vitais-data").html());

            $(".sinais-vitais-summario-item .btn-grafico").off("click",onClickBtnGrafico)
            $(".sinais-vitais-summario-item .btn-grafico").on("click",onClickBtnGrafico)
            
            ajusteTabela();
            
            setTimeout(function () {
                $("#summario_iv").html($("#iv").val());
                $("#summario_diur").html($("#diur").val());
                $("#summario_iEvo").html($("#iEvo").val());
                $("#summario_sEd").html($("#sEd").val());
                $("#summario_enteral").html($("#enteral").val());
                $("#summario_dreno").html($("#dreno").val());
                $("#summario_dreno2").html($("#dreno2").val());
                $("#summario_hd").html($("#hd").val());
                $("#summario_tpIntro").html($("#tpIntro").val());
                $("#summario_tpEli").html($("#tpEli").val());
                $("#summario_tg").html($("#tg").val());
                $("#summario_balancoCumulativo").html($("#balancoCumulativo").val());
                $("#summario_balancoAtual").html($("#balancoAtual").val());
                
                $("#summario_atualizado").html($("#atualizado").val());

                $("#altura").html($("#inputAltura").val());
                $("#peso").html($("#inputPeso").val());
                if ($("#balancoHidricoId").val() == "0" && $("#isDataAtual").val() == "1" && $("#balancoAnteriorId").val() !== "0") {
                    copiarLegendasESolucoes(start);
                }
                criaCamposNumericos()
            }, 100);
            
            function criaCamposNumericos() {
                maskFields = {};
                $(".tableBalanco input").each(function() {
                    const campo = $(this);
                    if(campo.data("solucao") == true) {
                        maskFields[campo.attr("name")] = IMask(campo[0], numberSolucaoMaskTemplate);
                    } else {
                        maskFields[campo.attr("name")] = IMask(campo[0], campo.data("field") == "t" ? numberTempMaskTemplate : numberMaskTemplate);
                    }
                });
            }
        }
        
        function botoesConferencia() {
            const conferirTurno = $("#checkConferirTurno").val();
            const conferirTotal = $("#checkConferirTotal").val();
            
            if(conferirTurno == "true") {
                $(".btn-conferir").removeClass("hidden");
            } else {
                $(".btn-conferir").addClass("hidden");
            }

            if(conferirTotal == "true") {
                $(".btn-conferirTotal").removeClass("hidden");
            } else {
                $(".btn-conferirTotal").addClass("hidden");
            }

            const conferidoTotal = $("#conferidoTotal").val();
            const desConferencia = $("#checkEnableDesConferencia").val();

            if(conferidoTotal != "true" && desConferencia =="true") {
                $(".btn-desConferir").removeClass("hidden");
            } else {
                $(".btn-desConferir").addClass("hidden");
            }

            if(conferidoTotal == "true" && desConferencia =="true") {
                $(".btn-desConferirTotal").removeClass("hidden");
            } else {
                $(".btn-desConferirTotal").addClass("hidden");
            }
        }
        
        function desconferir() {
            if($("#balancoHidricoId").val() == 0) {
                abp.message.info("Você precisa salvar o balanço hídrico antes de desconferir");
                return;
            }

            abp.message.confirmHtml("", app.localize('Deseja desconferir e desbloquear o balanço hídrico do plantão atual?'),onConferirConfirm );

            function onConferirConfirm(isConfirmed) {
                if (isConfirmed) {
                    $("button.confirm").buttonBusy(true);
                    _balancoHidricoService.desconferir($("#balancoHidricoId").val())
                        .then(res => { atualizaBalanco(); })
                        .always(() => { $("button.confirm").buttonBusy(false);})
                }
            }
        }

        function desconferirTotal() {
            if($("#balancoHidricoId").val() == 0) {
                abp.message.info("Você precisa salvar o balanço hídrico antes de desconferir");
                return;
            }

            abp.message.confirmHtml("", app.localize('Deseja desconferir e desbloquear o balanço hídrico todo?'),onConferirConfirm );

            function onConferirConfirm(isConfirmed) {
                if (isConfirmed) {
                    $("button.confirm").buttonBusy(true);
                    _balancoHidricoService.desconferir($("#balancoHidricoId").val())
                        .then(res => { atualizaBalanco(); })
                        .always(() => { $("button.confirm").buttonBusy(false);})
                }
            }
        }
        
        function conferir() {
            
            if($("#balancoHidricoId").val() == 0) {
                abp.message.info("Você precisa salvar o balanço hídrico antes de conferir");
                return;
            }
            
            abp.message.confirmHtml("", app.localize('Deseja conferir e bloquear o balanço hídrico do plantão atual?'),onConferirConfirm );

            function onConferirConfirm(isConfirmed) {
                if (isConfirmed) {
                    $("button.confirm").buttonBusy(true);
                    _balancoHidricoService.conferir($("#balancoHidricoId").val())
                        .then(res => { atualizaBalanco(); })
                        .always(() => { $("button.confirm").buttonBusy(false);})
                }
            }
        }

        function conferirTotal() {
            if($("#balancoHidricoId").val() == 0) {
                abp.message.info("Você precisa salvar o balanço hídrico antes de conferir");
                return;
            }
            abp.message.confirmHtml("", app.localize('Deseja conferir e bloquear o balanço hídrico todo?'),onConferirConfirm );

            function onConferirConfirm(isConfirmed) {
                if (isConfirmed) {
                    $("button.confirm").buttonBusy(true);
                    _balancoHidricoService.conferir($("#balancoHidricoId").val())
                        .then(res => { atualizaBalanco(); })
                        .always(() => { $("button.confirm").buttonBusy(false);})
                }
            }
        }

        function copiarLegendasESolucoes(start) {
            abp.message.confirm(
                `Deseja copiar as legendas e soluções do último balanco hidríco existente?`,
                function (isConfirmed) {
                    if (isConfirmed) {
                        balancoHidricoCopiarSolucoesAjax(start, true);
                    }
                }
            );
        }

        function ajusteTabela() {
            $('[data-toggle="tooltip"]').tooltip({ container: 'body' });
            
            var $table = $('table.tableBalanco');
            $table.floatThead({
                "scrollContainer": true
            });
            $('.scroller').css("height",$(window).height() - $('.scroller').offset().top)
            $table.floatThead('reflow');

            var hora = moment().hours();

            $('table.tableBalanco > tbody > tr[data-tg="false"][data-tp="false"]').each(function () {
                var tr = $(this);
                var trHora = tr.data("hora");

                if (trHora === hora + ":00") {
                    tr.addClass("table-balanco-active");
                    $("table.tableBalanco > tbody").get(0)
                        .scrollBy({ top: tr.offsetParent().offset().top, left: 0, behavior: 'smooth' });
                    //element.scrollBy({ top: 100, left: 0, behavior: 'smooth' });
                    return;
                }
                tr.removeClass("table-balanco-active");
            });
            
            ajustaVertical('manha');
            ajustaVertical('noite');
            ajustaVertical('total');
            
            function ajustaVertical(tipo) {
                const el = `table.tableBalanco .vertical-table-column-${tipo}`
                $(el).height($(el).height());
                if(tipo != "total") {
                    $(el).css("position", "absolute");
                    $(el).css("padding-top", ($(el).height() - getTextWidth($(el).find('.user-name').html())) / 2)
                }
            }
            function getTextWidth(text, font) {
                // re-use canvas object for better performance
                var canvas = getTextWidth.canvas || (getTextWidth.canvas = document.createElement("canvas"));
                var context = canvas.getContext("2d");
                context.font = font;
                var metrics = context.measureText(text);
                return metrics.width;
            }

        }

        function ajusteTamanhoBalanco() {

            setTimeout(function () {
                var stick = $('.sticky-top-balanco');
                stick.css("width", $(".balanco .container-content").width() + 30);
            }, 30);
        }


        $(window).resize(function () {
            ajusteTamanhoBalanco();
        });

        $('.sidebar-toggler').on('click', function (e) {
            ajusteTamanhoBalanco();
        });

        $('.expandBtn').on('click', function (e) {
            const balancoDiv = $('.balanco')
            if(balancoDiv.hasClass('fullscreen')) {
                balancoDiv.removeClass('fullscreen')
            } else {
                balancoDiv.addClass('fullscreen')
            }
            
            setTimeout(() => { ajusteTabela()},0);
        })
        $('.saveBtn').on('click', function (e) {
                var that = $(this);
                that.attr("disabled", "disabled");
                var balancoHidricoModel = {
                    id: $("#balancoHidricoId").val(),
                    atendimentoId: $("#atendimentoId").val(),
                    dataBalancoHidrico: $(".balancoDate").data("defaultValue"),
                    horaIntervalo: $("#horaIntervalo").val(),
                    balancoHidricoItems: [],
                    balancoHidricoSolucoes: [],
                    altura: $("#altura").val(),
                    peso: $("#peso").val(),
                    Conferido12: $("#Conferido12").val(),
                    Conferido24: $("#Conferido24").val(),
                    evacuacoes: $(".checkbox-evacuacao:checked").val(),
                    aspecto: $("#aspecto").val(),
                    
                    // Manhã
                    conferidoManha: $("#conferidoManha").val(),
                    dtConferidoManha: $("#dtConferidoManha").val(),
                    conferidoManhaUserId: $("#conferidoManhaUserId").val(),
                    // Noite
                    conferidoNoite: $("#conferidoNoite").val(),
                    dtConferidoNoite: $("#dtConferidoNoite").val(),
                    conferidoNoiteUserId: $("#conferidoNoiteUserId").val(),
                    // Total
                    conferidoTotal: $("#conferidoTotal").val(),
                    dtConferidoTotal: $("#dtConferidoTotal").val(),
                    conferidoTotalUserId: $("#conferidoTotalUserId").val(),

                    // Manhã
                    desConferidoManha: $("#desConferidoManha").val(),
                    dtDesConferidoManha: $("#dtDesConferidoManha").val(),
                    desConferidoManhaUserId: $("#desConferidoManhaUserId").val(),
                    // Noite
                    conferidoNoite: $("#conferidoNoite").val(),
                    dtConferidoNoite: $("#dtConferidoNoite").val(),
                    conferidoNoiteUserId: $("#conferidoNoiteUserId").val(),
                    // Total
                    desConferidoNoite: $("#desConferidoNoite").val(),
                    dtDesConferidoNoite: $("#dtDesConferidoNoite").val(),
                    desConferidoNoiteUserId: $("#desConferidoNoiteUserId").val(),
                };

                $("input[name^='solucao_']").each(function () {
                    const solItem = $(this);
                    const mdl = {
                        id: solItem.data("id"),
                        "indiceSolucao": solItem.attr("name").replace("solucao_", ""),
                        valor: solItem.val(),
                        "balancoHidricoId": balancoHidricoModel.id,
                        "atendimentoId": balancoHidricoModel.atendimentoId
                    };

                    balancoHidricoModel.balancoHidricoSolucoes.push(mdl);
                });
                function getData(item, index ,field,isParcial, isGeral, isTransp) {
                    return retornaValorMaskField(maskFields,getItem(item,index,field,isParcial,isGeral,isTransp).attr("name"))
                }
                
                function getItem(item, index ,field,isParcial, isGeral,isTransp) {
                    const el = getName(isParcial,isGeral,isTransp);
                    return item.find(el);

                    function getName(isParcial, isGeral, isTransp) {
                        if(isParcial) {
                            return `input[name='${field}_${index}_tp']`;
                        }
                        if(isGeral) {
                            return `input[name='${field}_${index}_tg']`;
                        }
                        if(isTransp) {
                            return `input[name='${field}_${index}_transp']`;
                        }
                        return `input[name='${field}_${index}']`;
                    }
                }
                $(".tableBalanco tbody tr").each(function () {
                    const hourItem = $(this);
                    const index = hourItem.data("index");
                    const totalParcial = hourItem.data("tp") || false;
                    const totalGeral = hourItem.data("tg") || false;
                    const totalTransp = hourItem.data("transp") || false;
                    let balancoHidricoItem = {
                        "balancoHidricoId": balancoHidricoModel.id,
                        "atendimentoId": balancoHidricoModel.atendimentoId,
                        id: hourItem.data("id"),
                        hora: hourItem.data("hora"),
                        totalParcial: totalParcial,
                        totalGeral: totalGeral,
                        totalTransporte: totalTransp,
                        sinaisVitais: {
                            id: hourItem.data("sinaisId"),
                            temperatura: getData(hourItem,index,'t',totalParcial,totalGeral,totalTransp),
                            pulso: getData(hourItem,index,'p',totalParcial, totalGeral,totalTransp),
                            respiracao: getData(hourItem,index,'r',totalParcial,totalGeral,totalTransp),
                            "spo2":getData(hourItem,index,'spo2',totalParcial,totalGeral,totalTransp),
                            "ins": getData(hourItem,index,'ins',totalParcial,totalGeral,totalTransp),
                            "pressaoSistolica": getData(hourItem,index,'ps',totalParcial,totalGeral,totalTransp),
                            "pressaoDiastolica":getData(hourItem,index,'pd',totalParcial,totalGeral,totalTransp),
                            "pressaoVenosaCentral":getData(hourItem,index,'pvc',totalParcial,totalGeral,totalTransp),
                            "escalaDeDor": getData(hourItem,index,'eva',totalParcial,totalGeral,totalTransp),
                            "hemoglucoteste":getData(hourItem,index,'hgt',totalParcial,totalGeral,totalTransp),
                            "pressaoIntracraniana" :getData(hourItem, index,'pic',totalParcial,totalGeral,totalTransp),
                        },
                        endovenosos: [],
                        "sangueDerivados": getData(hourItem,index,'sangueDerivados',totalParcial,totalGeral,totalTransp),
                        "enteral": getData(hourItem,index,'enteral',totalParcial,totalGeral,totalTransp),
                        "ingestVoSne": getData(hourItem,index,'ingesta',totalParcial,totalGeral,totalTransp),
                        "diurese": getData(hourItem,index,'diurese',totalParcial,totalGeral,totalTransp),
                        "hd": getData(hourItem,index,'hd',totalParcial,totalGeral,totalTransp),
                        "dreno": getData(hourItem,index,'dreno',totalParcial,totalGeral,totalTransp),
                        "dreno2": getData(hourItem,index,'dreno_2',totalParcial,totalGeral,totalTransp),
                        "irrigacaodeEntrada" :getData(hourItem,index,'ie',totalParcial,totalGeral,totalTransp),
                        "irrigacaodeSaida" :getData(hourItem,index,'is',totalParcial,totalGeral,totalTransp),
                    };

                    balancoHidricoModel.balancoHidricoSolucoes.forEach((solucao) => {
                        let item = getItem(hourItem,index, `end_${solucao["indiceSolucao"]}`, totalParcial,totalGeral, totalTransp);
                        //let item = hourItem.data("tp") ? hourItem.find("input[name='end_" + solucao["indiceSolucao"] + "_" + index + "_tp']") :  hourItem.find("input[name='end_" + solucao["indiceSolucao"] + "_" + index + "']");
                        let mdlSolucoes = {
                            balancoHidricoItemId: balancoHidricoItem.id,
                            id: item.data("id"),
                            valor: retornaValorMaskField(maskFields,item.attr("name")),
                            "IndiceSolucao": solucao["indiceSolucao"]
                        };
                        balancoHidricoItem.endovenosos.push(mdlSolucoes);
                    });

                    balancoHidricoModel.balancoHidricoItems.push(balancoHidricoItem);
                });
                _balancoHidricoService.upSertBalancoHidricoAsync(balancoHidricoModel).done(function (res) {
                    that.removeAttr("disabled");
                    atualizaBalanco();

                }).fail(function () {
                    that.removeAttr("disabled");
                });
            });
        
        $(".btn-sinais-vitais-todos-graficos").on('click', onClickBtnTodosGraficos);
        
        function atualizaBalanco() {
            let defaultDate = moment($(".balancoDate").data("defaultValue"), "DD/MM/YYYY");
            abp.notify.success(app.localize('SavedSuccessfully'));
            setTimeout(function () {
                callAjax(defaultDate);
            }, 500);
        }

        $('.exportBtn').on('click', function (e) {
            const defaultDate = moment($("#balancoHidricoData").val(), "DD/MM/YYYY");

            const url = "/Mpa/Assistenciais/BalancoHidricoRelatorio?id=" + $("#atendimentoId").val() + "&date=" + defaultDate.toISOString();
            window.open(url, "_blank");
        });
        
        function criarGraficoOptions() {
            return {
                't': grafico("Temperatura",'{value} °C'),
                'p': grafico("Pulso"),
                'r': grafico("Respiração"),
                'spo2': grafico("Saturação do oxigênio no sangue"),
                'eva': grafico("Escala de Dor"),
                'hgt': grafico("Hemoglucoteste"),
                'pa': graficoPa
            }
        }
        
        function baseGrafico(jsonData, title, formatter) {
            if(!jsonData) {
                return;
            }
            
            formatter = formatter ?? '{value}';
            return {
                title: { text: title },
                toolbox: {
                    show: true,
                    feature: {
                        dataZoom: { yAxisIndex: 'none' },
                        dataView: { readOnly: false },
                        magicType: { type: ['line', 'bar'] },
                        restore: {},
                        saveAsImage: {}
                    }
                },
                tooltip: { trigger: 'axis' },
                xAxis: { type: 'category', boundaryGap: false,  data: jsonData.map(x=> x.Hora) },
                yAxis: { type: 'value', boundaryGap: ['20%', '20%'], axisLabel: { formatter: formatter } },
                series: [{
                    data: jsonData.map(x=> x.Number),
                    type: 'line',
                    markPoint: { data: [ {type: 'max', name: 'Maxima'}, {type: 'min', name: 'Minima'} ] },
                    markLine: { data: [ {type: 'average', name: 'Media'} ] }
                }]
            };
        }
        function grafico(title, formatter) {
            return function(jsonData) {
                return baseGrafico(jsonData, title,formatter);
            }
        }

        function jsonData(tipo) {
            const data = $(`#input_hidden_${tipo}`).val();
            let jsonData = undefined;
            try {
                jsonData = JSON.parse(data);
            }
            catch (e) {
                return;
            }

            return  _.filter(jsonData, (item) => item.Number);
        }
        
        function graficoPa() {
            let pasData = jsonData("pas")
            let padData = jsonData("pad")
            
            let pasHoras = pasData ? pasData.map(x=> x.Hora) : []
            let padHoras = padData ? padData.map(x=> x.Hora) : []
            
            let horas = _.unique(padHoras.concat(pasHoras))
            
            let labels = []
            let pasSeries = []
            let padSeries = []
            _.forEach(horas, (itemHora) => {
                labels.push(itemHora)
                let padHora = _.find(padData, (itemData) => itemData.Hora === itemHora)
                let pasHora = _.find(pasData, (itemData) => itemData.Hora === itemHora)
                
                padSeries.push(padHora ? padHora.Number: null)
                pasSeries.push(pasHora ? pasHora.Number: null)
            })

            return {
                title: { text: "Pressão arterial" },
                toolbox: {
                    show: true,
                    feature: {
                        dataZoom: { yAxisIndex: 'none' },
                        dataView: {readOnly: false},
                        magicType: {type: ['line', 'bar']},
                        restore: {},
                        saveAsImage: {}
                    }
                },
                legend: { data: ["PAS","PAD"] },
                tooltip: { trigger: 'axis' },
                xAxis: { type: 'category', boundaryGap: false, data: labels },
                yAxis: { type: 'value', boundaryGap: ['20%', '20%'] },
                series: [
                    { name:"PAS", data: pasSeries, type: 'line', markPoint: { data: [{type: 'max', name: 'Maxima'}, {type: 'min', name: 'Minima'} ]} }, 
                    { name:"PAD", data: padSeries, type: 'line', markPoint: { data: [{type: 'max', name: 'Maxima'}, {type: 'min', name: 'Minima'} ]} }
                ]
            };
        }

        function retornaValorMaskField(maskFields, input) {
            if(!maskFields[input]) {
                return undefined;
            }
            
            const el = maskFields[input].unmaskedValue;
            if (el === "" || el === null || el === undefined) {
                return undefined;
            }
            return el;
        }
    });
})();

