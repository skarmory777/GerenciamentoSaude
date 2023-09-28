(function () {
    $(function () {
        const contaAppService = abp.services.app.conta;

        let container = null;
        const _selectedDateRangeResumoContaItem = {
            startDate: undefined,
            endDate: undefined,
            autoUpdateInput: false,
        };

        abp.event.on("ContaMedica::CarregaContaMedica", carregarContaMedica);

        abp.event.on("ContaMedica::DownloadContaMedica", downloadContaMedica);


        const pickerOptions = app.createDateRangePickerOptions();
        $('#impressaoContaFilterData').daterangepicker(
            $.extend(true, pickerOptions, _selectedDateRangeResumoContaItem),
            function (start, end, label) {
                _selectedDateRangeResumoContaItem.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRangeResumoContaItem.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');

                //gridItemsOptions.refresh();
            })
            .on('apply.daterangepicker', function (ev, picker) {
                $(this).val(picker.startDate.format('DD/MM/YYYY') + ' - ' + picker.endDate.format('DD/MM/YYYY'));
            }).on('cancel.daterangepicker', function (ev, picker) {
                _selectedDateRangeResumoContaItem.startDate = undefined;
                _selectedDateRangeResumoContaItem.endDate = undefined;
                $(this).val('');
            });

        selectSWWithDefaultValue($("#impressaoContaFilterLocalUtilizacao"), '/api/services/app/UnidadeOrganizacional/ListarDropdown', undefined, { multiple:true});
        selectSWWithDefaultValue($("#impressaoContaFilterTerceirizado"), "/api/services/app/terceirizado/ListarDropdown", undefined, { multiple: true });
        selectSWWithDefaultValue($("#impressaoContaFilterCentroCusto"), '/api/services/app/CentroCusto/ListarDropdownCodigoCentroCusto', undefined, { multiple: true });
        selectSWWithDefaultValue($("#impressaoContaFilterTurno"), "/api/services/app/Turno/ListarDropdown", undefined, { multiple: true });
        selectSWWithDefaultValue($("#impressaoContaFilterGrupo"), "/api/services/app/FaturamentoGrupo/ListarDropdown", undefined, { multiple:true});
        


        $(".btn-impressao-conta-medica-filtro").on("click", function (event) {
            const btn = $(this);
            btn.buttonBusy(true);
            abp.event.trigger("ContaMedica::CarregaContaMedica", {tipo:btn.data("tipo")});
            setTimeout(() => {
                btn.buttonBusy(false)
            }, 300);
        })

        $(".btn-imprimir-conta-medica-filtro").on("click", function (event) {
            const btn = $(this);
            btn.buttonBusy(true);
            abp.event.trigger("ContaMedica::DownloadContaMedica", { tipo: btn.data("tipo") });
            setTimeout(() => {
                btn.buttonBusy(false)
            }, 300);
        })

        function getContaMedicaId() {
            return container.find("input[name='impressaoContaContaMedicaId']").val() || 0
        }

        function downloadContaMedica(event) {
            const btn = $(this);
            btn.buttonBusy(true);
            const { tipo } = event;
            container = $(`#impressaoContaTipo_${tipo}`);
            const data = {
                id: getContaMedicaId(),
                atendimentoId: container.find("[name='impressaoContaAtendimentoId']").val(),
                contaMedicaId: getContaMedicaId(),
                dataInicial: _selectedDateRangeResumoContaItem.startDate,
                dataFinal: _selectedDateRangeResumoContaItem.endDate,
                grupoIds: container.find("#impressaoContaFilterGrupo").val(),
                centroDeCustoIds: container.find("#impressaoContaFilterCentroCusto").val(),
                localUtilizacaoIds: container.find("#impressaoContaFilterLocalUtilizacao").val(),
                terceirizadoIds: container.find("#impressaoContaFilterTerceirizado").val(),
                turnoIds: container.find("#impressaoContaFilterTurno").val()
            };

            console.log(data);

            printJS({
                printable: `/Mpa/FaturarAtendimento/ImprimirConta?${$.param(data)}`,
                type: 'pdf',
                onPrintDialogClose: () => {
                    btn.buttonBusy(false)
                }
            })
        }

        function carregarContaMedica(event) {
            const { tipo } = event;
            container = $(`#impressaoContaTipo_${tipo}`);

            resumoContaMedica = [];
            loaderContaMedica();

            const data = {
                id: getContaMedicaId(),
                dataInicial: _selectedDateRangeResumoContaItem.startDate,
                dataFinal: _selectedDateRangeResumoContaItem.endDate,
                grupoIds: container.find("#impressaoContaFilterGrupo").val(),
                centroDeCustoIds: container.find("#impressaoContaFilterCentroCusto").val(),
                localUtilizacaoIds: container.find("#impressaoContaFilterLocalUtilizacao").val(),
                terceirizadoIds: container.find("#impressaoContaFilterTerceirizado").val(),
                turnoIds: container.find("#impressaoContaFilterTurno").val()
            }

            const service = getService(tipo)
            if (_.isFunction(service)) {
                service(data).then(res => {
                    resumoContaMedica = montarResumoConta(res.returnObject.resumoContaGrupo);
                }).then(() => {
                    criarContaMedica(tipo);
                })
            }
            

            function montarResumoConta(resumoConta) {
                let result = [];
                _.forEach(resumoConta, (x) => {
                    result.push(criarGrupo(x.grupoId, x.grupoDescricao, x.grupoOrdem, x.resumoContaSubGrupo));
                })

                return result;
            }

            function criarGrupo(grupoId, grupoDescricao, grupoOrdem, resumoContaSubGrupo) {
                let resultGrupo = {
                    grupoId,
                    grupoDescricao,
                    grupoOrdem,
                    resumoContaItem: []
                }

                _.forEach(resumoContaSubGrupo, (resumoContaSubGrupoItem) => {
                    _.forEach(resumoContaSubGrupoItem.resumoContaItem, (resumoContaItem) => {
                        resultGrupo.resumoContaItem.push(criarItemResumoContaMedica(
                            resumoContaSubGrupoItem.subGrupoId,
                            resumoContaSubGrupoItem.subGrupoDescricao,
                            {
                                codigo: resumoContaItem.codigo,
                                codAmb: resumoContaItem.codAmb,
                                codCbhpm: resumoContaItem.codCbhpm,
                                codTuss: resumoContaItem.codTuss
                            },
                            resumoContaItem.dataInicial,
                            resumoContaItem.dataFinal,
                            resumoContaItem.resumoDetalhamento != null ? resumoContaItem.resumoDetalhamento.honorarios : null,
                            resumoContaItem.fatItemDescricao,
                            resumoContaItem.fatItemDescricaoTuss,
                            resumoContaItem.fatContaItemDescricao,
                            resumoContaItem.fatContaItemCodigo,
                            resumoContaItem.qtde,
                            resumoContaItem.resumoDetalhamento != null ? resumoContaItem.resumoDetalhamento.preco : null,
                            resumoContaItem.resumoDetalhamento != null && resumoContaItem.resumoDetalhamento.moeda != null ? resumoContaItem.resumoDetalhamento.moeda.codigo : null,
                            resumoContaItem.resumoDetalhamento != null ? resumoContaItem.resumoDetalhamento.valor : null,
                            resumoContaItem.resumoDetalhamento != null ? resumoContaItem.resumoDetalhamento.percentual : null,
                            resumoContaItem.resumoDetalhamento != null ? resumoContaItem.resumoDetalhamento.valorTotal : null,
                            resumoContaItem.resumoDetalhamento != null ? resumoContaItem.resumoDetalhamento.coch : null,
                            resumoContaItem.resumoDetalhamento != null ? resumoContaItem.resumoDetalhamento.hmch : null,
                            resumoContaItem.resumoDetalhamento != null ? resumoContaItem.resumoDetalhamento.valorCOCH : null,
                            resumoContaItem.resumoDetalhamento != null ? resumoContaItem.resumoDetalhamento.valorHMCH : null,
                            resumoContaItem.resumoDetalhamento != null ? resumoContaItem.resumoDetalhamento.metragemFilme : null,
                            resumoContaItem.resumoDetalhamento != null ? resumoContaItem.resumoDetalhamento.valorFilme : null,
                            resumoContaItem.observacao
                        ))
                    })
                })

                return resultGrupo;
            }
        }

        function getService(tipo) {
            return contaAppService[`resumoConta${tipo}`];
        }

        function loaderContaMedica() {
            const resumoConta = $(".resumo-conta");
            resumoConta.empty();
            const loader = `<div class="loader" style="">
               <img src="/libs/spinner.io/Spinner.svg">
               <p class="loading">Carregando<span>.</span><span>.</span><span>.</span></p>
            </div>`;
            resumoConta.append(loader);
        }

        function criarContaMedica(tipo) {
            if (tipo == "Aberta") {
                criarContaMedicaAberta()
            }
            else {
                criarContaMedicaFechada()
            }
        }

        const formataReal = (valor) => {
            return numeral(valor).format('$0,0.00')
        }
        const getPrecoUnit = (subItem) => {

            if (subItem.moeda == "R$") {
                return numeral(subItem.precoUnit).format('$0,0.00')
            }
            else {
                return `${numeral(subItem.precoUnit).format('0,0.00')} ${subItem.moeda ?? ""}`
            }
        }

        function criarContaMedicaAberta() {
            const renderHeader = (item, context) => {
                let result =  `<div class="row"><h3 class="text-center bold" style="height: 50px;line-height: 50px;"> ${item.grupoDescricao || ''}</h3> </div> 
                `;
                //<div class="row header"> <div class="col-md-12 text-left"> <h6 class="bold">${item.subTitle}</h6> </div> </div>
                content.append(result);
            }
            const renderSubHeader = (item, context) => {
                let result = `<div class="row"><h4 class="text-center bold" style="height: 50px;line-height: 50px;color:#337ab7"> ${item.subGrupoDescricao || ''}</h4> </div>`;
                //<div class="row header"> <div class="col-md-12 text-left"> <h6 class="bold">${item.subTitle}</h6> </div> </div>
                content.append(result);
            }

            

            const renderHonorario = (dadosProfissonalSaude, profissionalValor, isCredenciado, tipoProfissional) => {
                var resultRenderHonorario = "";
                if (!dadosProfissonalSaude) {
                    return resultRenderHonorario;
                }
                resultRenderHonorario += `
                    <div class="row line-data">
                        <div class="col-md-1 col-md-offset-2 text-left">
                            ${sanitize(tipoProfissional)}
                        </div>
                        <div class="col-md-8 text-left">
                            ${sanitize(dadosProfissonalSaude.conselho)}: ${sanitize(dadosProfissonalSaude.numeroConselho)} - CPF: ${sanitize(dadosProfissonalSaude.cpf)} - ${sanitize(dadosProfissonalSaude.nome)}
                        </div>
                        <div class="col-md-1 text-right">
                            ${(isCredenciado? "": numeral(profissionalValor).format('$0,0.00'))}
                        </div>
                    </div>`;

                function sanitize(value) {
                    return !value ? "" : value;
                }
                return resultRenderHonorario;
            }

            const renderSubItemHonorarios = (honorarios) => {
                let resultHonorarios = "";

                if (!honorarios) {
                    return resultHonorarios;
                }
                
                if (honorarios.hasMedico) {
                    resultHonorarios += renderHonorario(honorarios.dadosMedico, honorarios.medicoValor, honorarios.medicoIsCredenciado,  'Médico');
                }

                if (honorarios.hasAuxiliar1) {
                    resultHonorarios += renderHonorario(honorarios.dadosAuxiliar1, honorarios.auxiliar1Valor, honorarios.auxiliar1IsCredenciado, '1° Auxiliar');
                }

                if (honorarios.hasAuxiliar2) {
                    resultHonorarios += renderHonorario(honorarios.dadosAuxiliar2, honorarios.auxiliar2Valor, honorarios.auxiliar2IsCredenciado, '2° Auxiliar');
                }

                if (honorarios.hasAuxiliar3) {
                    resultHonorarios += renderHonorario(honorarios.dadosAuxiliar3, honorarios.auxiliar3Valor, honorarios.auxiliar3IsCredenciado, '3° Auxiliar');
                }

                if (honorarios.hasInstrumentador) {
                    resultHonorarios += renderHonorario(honorarios.dadosInstrumentador, honorarios.instrumentadorValor, honorarios.instrumentadorIsCredenciado, 'Instrumentador');
                }

                if (honorarios.hasAnestesista) {
                    resultHonorarios += renderHonorario(honorarios.dadosAnestesista, honorarios.anestesistaValor, honorarios.anestesistaIsCredenciado, 'Anestesista');
                }

                return resultHonorarios;
            }

            const renderSubItem =(subItem, content) => {
                let result = `
                <div class="row line-data">
                 <!--<div class="col-md-2 text-left">
                        <button type="button" class="btn btn-danger"><i class="fas fa-trash"></i></button>
                        <button type="button" class="btn btn-secondary"><i class="fas fa-edit"></i></button>
                        <button type="button" class="btn btn-info"><i class="fas fa-info-circle"></i></button>
                        <button type="button" class="btn btn-primary"><i class="fas fa-layer-group"></i></button>
                    </div>-->
                    <div class="col-md-2 text-center">
                        <div class="row">
                            <div class="col-md-6">
                               <b>Código:</b> ${(subItem.cod != null ? subItem.cod.codigo ?? "" : '')}
                            </div>
                            <div class="col-md-6">
                               <b>Tuss:</b> ${(subItem.cod != null ? subItem.cod.codTuss ?? "" : '')}
                            </div>
                        </div>
                        <div class="row">
                            ${(subItem.dataInicial != null && moment(subItem.dataInicial).isValid() ? moment(subItem.dataInicial).format("DD/MM/YYYY"): "-")}
                        </div>
                    </div>
                    <div class="col-md-5 text-left">
                        <div class="row">
                           <b>Tuss:</b> ${subItem.descricaoTuss ?? ""}
                        </div>
                       <div class="row">
                         <b>Descrição:</b> ${subItem.descricao ?? ""}
                        </div>
                        
                    </div>
                    <div class="col-md-1 text-right">
                        ${numeral(subItem.qtd).format('0,0')}
                    </div>
                    <div class="col-md-1 text-right">
                        ${getPrecoUnit(subItem)}
                    </div>
                    <div class="col-md-1 text-right">
                        ${numeral(subItem.valorTotal).format('$0,0.00')}
                    </div>
                    <div class="col-md-1 text-right">
                         ${numeral(subItem.percent).format('0%')}
                    </div>
                    <div class="col-md-1 text-right">
                        ${numeral(subItem.total).format('$0,0.00')}
                    </div>
                </div>`
                if(subItem.subTitle) {
                    result += `
                    <div class="row line-data">
                        <div class="col-md-1 text-left">
                            ${subItem.subTitle.cod}
                        </div>
                        <div class="col-md-11 text-left">
                           - ${subItem.subTitle.text}
                        </div>
                    </div>`;
                }
                
                result += renderSubItemHonorarios(subItem.honorarios);

                if (subItem.valorFilme && subItem.valorFilme != 0) {
                    result += `
                    <div class="row line-data">
                        <div class="col-md-2 text-left" style="margin-left:10px">
                           Filme (${subItem.metragemFilme})
                        </div>
                        <div class="col-md-10 text-right" style="margin-left:-10px">
                           ${formataReal(subItem.valorFilme)}
                        </div>
                    </div>`;
                }
                if (subItem.valorCOCH && subItem.valorCOCH != 0) {
                    result += `
                    <div class="row line-data">
                        <div class="col-md-2 text-left" style="margin-left:10px">
                           COCH (${subItem.coch})
                        </div>
                        <div class="col-md-10 text-right" style="margin-left:-10px">
                           ${formataReal(subItem.valorCOCH)}
                        </div>
                    </div>`;
                }
                if (subItem.valorHMCH && subItem.valorHMCH != 0) {
                    result += `
                    <div class="row line-data">
                        <div class="col-md-2 text-left" style="margin-left:10px">
                           HMCH (${subItem.hmch})
                        </div>
                        <div class="col-md-10 text-right" style="margin-left:-10px">
                           ${formataReal(subItem.valorHMCH)}
                        </div>
                    </div>`;
                }
                if (subItem.observacao && subItem.observacao != '' && subItem.observacao != null) {
                    result += `
                    <div class="row line-data">
                        <div class="col-md-12 text-left" style="margin-left:10px">
                           <b>${subItem.observacao}</b>
                        </div>
                    </div>`;
                }

                content.append(result);

                
            }

            

            const renderResumo = (item, content) => {
                const total = sumBy(item.resumoContaItem, (sumItem) => sumItem.total);
                if (!item.grupoDescricao || item.grupoDescricao == '') {
                    return;
                }
                const totalLabel = `Total de ${item.grupoDescricao}`;
                let result = `
                <div class="row resumo">
                    <div class="col-md-11 text-left">
                        <h5 class="bold"> ${totalLabel}</h5>
                    </div>
                    <div class="col-md-1 text-right" style="border-top: solid">
                        <h5 class="bold valor"> ${numeral(total).format('$0,0.00')}</h5>
                    </div>
                </div>
                `;
                content.append(result);
            }

            const renderSubTotal = (items, content) => {
                const total = sumBy(items, (sumItem) => sumItem.total);
                if (!items[0].subGrupoDescricao || items[0].subGrupoDescricao == '') {
                    return;
                }
                const totalLabel = `Total de ${items[0].subGrupoDescricao}`;
                let result = `
                 <div class="row total">
                    <div class="col-md-11 text-left">
                        <h5 class="bold" style="color:#337ab7"> ${totalLabel}</h5>
                    </div>
                    <div class="col-md-1 text-right" style="border-top: solid">
                        <h5 class="bold valor" style="color:#337ab7"> ${numeral(total).format('$0,0.00')}</h5>
                    </div>
                </div>
                `;
                content.append(result);
            }

            const renderTotal = (item, content) => {
                const total = sumBy(item.resumoContaItem,(sumItem) => sumItem.total);
                let result = `
                 <div class="row total">
                    <div class="col-md-11 text-left">
                        <h4 class="bold"> Total de ${item.grupoDescricao}</h4>
                    </div>
                    <div class="col-md-1 text-right" style="border-top: solid">
                        <h4 class="bold valor"> ${numeral(total).format('$0,0.00')}</h4>
                    </div>
                </div>
                `;
                content.append(result);
            }

            const renderTotalPeriodo = (resumoContaMedica, content) => {
                let total = 0;

                _.forEach(resumoContaMedica, (item)=> {
                    total += sumBy(item.resumoContaItem,(sumItem) => sumItem.total);
                })

                let result = `
                 <div class="row total" style="margin: 50px 0" >
                    <div class="col-md-11 text-left">
                        <h3 class="bold"> TOTAL DO PERIODO</h3>
                    </div>
                    <div class="col-md-1 text-right" style="border-top: solid">
                        <h3 class="bold valor"> ${numeral(total).format('$0,0.00')}</h3>
                    </div>
                </div>
                `;
                content.append(result);
            }

            const sumBy = (arr, func) => arr.reduce((acc, item) => acc + func(item), 0)

            let content = $(`<div class="custom-scrollbar" style='height: 600px;overflow: scroll;top: 10px;position: relative;padding-right: 11px;background-color: white;'></div>`);
            let resumoConta = container.find(".resumo-conta");
            resumoConta.empty();

            resumoConta.append(
                `<nav class="navbar navbar-default navbar-el" style="margin-bottom: 0px">
                    <div class="container-fluid">
                        <div class="row" style="min-height: 50px;line-height: 50px;">
                        <!--<div class="col-md-2 text-left">
                            <h5>Ações</h5>
                        </div>-->
                        <div class="col-md-7 text-center">
                            <h5>Item</h5>
                        </div>
                        <div class="col-md-1 text-right">
                            <h5>Qtde</h5>
                        </div>
                        <div class="col-md-1 text-right">
                            <h5>Preco Unitario</h5>
                        </div>
                        <div class="col-md-1 text-right">
                            <h5>Valor Total</h5>
                        </div>
                        <div class="col-md-1 text-right">
                            <h5>Percentual</h5>
                        </div>
                        <div class="col-md-1 text-right">
                            <h5>Total</h5>
                        </div>
                    </div>
                    </div>
                </nav>`)
            console.log(resumoContaMedica);
            
            _.forEach(_.sortBy(resumoContaMedica, (item) => item.grupoOrdem),(item) => {
                renderHeader(item, content);
                _.forEach(_.groupBy(_.sortBy(item.resumoContaItem, (x) => x.subGrupoId), (x) => x.subGrupoId), (resumoGrupo) => {
                    if (!resumoGrupo.length) {
                        return;
                    }
                    renderSubHeader(resumoGrupo[0], content);
                    _.forEach(resumoGrupo, (resumoContaItem) => {
                        renderSubItem(resumoContaItem, content)
                    });
                    renderSubTotal(resumoGrupo, content);
                });
                /*renderResumo(item, content);*/
                renderTotal(item, content);
            })

            renderTotalPeriodo(resumoContaMedica,content);

            resumoConta.append(content);
        }


        function criarContaMedicaFechada() {
            const renderHeader = (item, context) => {
                let result = `<div class="row"><h3 class="text-center bold" style="height: 50px;line-height: 50px;"> ${item.grupoDescricao || ''}</h3> </div> 
                `;
                //<div class="row header"> <div class="col-md-12 text-left"> <h6 class="bold">${item.subTitle}</h6> </div> </div>
                content.append(result);
            }
            const renderSubHeader = (item, context) => {
                let result = `<div class="row"><h4 class="text-center bold" style="height: 50px;line-height: 50px;color:#337ab7"> ${item.subGrupoDescricao || ''}</h4> </div>`;
                //<div class="row header"> <div class="col-md-12 text-left"> <h6 class="bold">${item.subTitle}</h6> </div> </div>
                content.append(result);
            }

            const getPrecoUnit = (subItem) => {
                if (subItem.valorCOCH && subItem.valorCOCH > 0) {
                    return numeral(subItem.valorCOCH).format('0,0.00') + " CH";
                }
                else if (subItem.valorHMCH && subItem.valorHMCH > 0) {
                    return numeral(subItem.valorCOCH).format('0,0.00') + " CH";
                }
                return numeral(subItem.precoUnit).format('$0,0.00')
            }

            const renderHonorario = (dadosProfissonalSaude, profissionalValor, isCredenciado, tipoProfissional) => {
                var resultRenderHonorario = "";
                if (!dadosProfissonalSaude) {
                    return resultRenderHonorario;
                }
                resultRenderHonorario += `
                    <div class="row line-data">
                        <div class="col-md-1 col-md-offset-2 text-left">
                            ${sanitize(tipoProfissional)}
                        </div>
                        <div class="col-md-8 text-left">
                            ${sanitize(dadosProfissonalSaude.conselho)}: ${sanitize(dadosProfissonalSaude.numeroConselho)} - CPF: ${sanitize(dadosProfissonalSaude.cpf)} - ${sanitize(dadosProfissonalSaude.nome)}
                        </div>
                        <div class="col-md-1 text-right">
                            ${(isCredenciado ? "" : numeral(profissionalValor).format('$0,0.00'))}
                        </div>
                    </div>`;

                function sanitize(value) {
                    return !value ? "" : value;
                }
                return resultRenderHonorario;
            }

            const renderSubItemHonorarios = (honorarios) => {
                let resultHonorarios = "";

                if (!honorarios) {
                    return resultHonorarios;
                }

                if (honorarios.hasMedico) {
                    resultHonorarios += renderHonorario(honorarios.dadosMedico, honorarios.medicoValor, honorarios.medicoIsCredenciado, 'Médico');
                }

                if (honorarios.hasAuxiliar1) {
                    resultHonorarios += renderHonorario(honorarios.dadosAuxiliar1, honorarios.auxiliar1Valor, honorarios.auxiliar1IsCredenciado, '1° Auxiliar');
                }

                if (honorarios.hasAuxiliar2) {
                    resultHonorarios += renderHonorario(honorarios.dadosAuxiliar2, honorarios.auxiliar2Valor, honorarios.auxiliar2IsCredenciado, '2° Auxiliar');
                }

                if (honorarios.hasAuxiliar3) {
                    resultHonorarios += renderHonorario(honorarios.dadosAuxiliar3, honorarios.auxiliar3Valor, honorarios.auxiliar3IsCredenciado, '3° Auxiliar');
                }

                if (honorarios.hasInstrumentador) {
                    resultHonorarios += renderHonorario(honorarios.dadosInstrumentador, honorarios.instrumentadorValor, honorarios.instrumentadorIsCredenciado, 'Instrumentador');
                }

                if (honorarios.hasAnestesista) {
                    resultHonorarios += renderHonorario(honorarios.dadosAnestesista, honorarios.anestesistaValor, honorarios.anestesistaIsCredenciado, 'Anestesista');
                }

                return resultHonorarios;
            }

            const renderSubItem = (subItem, content) => {
                let result = `
                <div class="row line-data">
                 <!--<div class="col-md-2 text-left">
                        <button type="button" class="btn btn-danger"><i class="fas fa-trash"></i></button>
                        <button type="button" class="btn btn-secondary"><i class="fas fa-edit"></i></button>
                        <button type="button" class="btn btn-info"><i class="fas fa-info-circle"></i></button>
                        <button type="button" class="btn btn-primary"><i class="fas fa-layer-group"></i></button>
                    </div>-->
                    <div class="col-md-2 text-center">
                        ${(subItem.contaItemCodigo || '')}
                    </div>
                    <div class="col-md-5 text-left">
                         ${(subItem.contaItemDescricao || '')}
                    </div>
                    <div class="col-md-1 text-right">
                        ${numeral(subItem.qtd).format('0,0')}
                    </div>
                    <div class="col-md-1 text-right">
                        ${getPrecoUnit(subItem)}
                    </div>
                    <div class="col-md-1 text-right">
                        ${numeral(subItem.valorTotal).format('$0,0.00')}
                    </div>
                    <div class="col-md-1 text-right">
                         ${numeral(subItem.percent).format('0%')}
                    </div>
                    <div class="col-md-1 text-right">
                        ${numeral(subItem.total).format('$0,0.00')}
                    </div>
                </div>`
                if (subItem.subTitle) {
                    result += `
                    <div class="row line-data">
                        <div class="col-md-1 text-left">
                            ${subItem.subTitle.cod}
                        </div>
                        <div class="col-md-11 text-left">
                           - ${subItem.subTitle.text}
                        </div>
                    </div>`;
                }

                if (subItem.valorFilme && subItem.valorFilme != 0) {
                    result += `
                    <div class="row line-data">
                        <div class="col-md-2 text-left" style="margin-left:10px">
                           Filme (${subItem.metragemFilme})
                        </div>
                        <div class="col-md-10 text-right" style="margin-left:-10px">
                           ${formataReal(subItem.valorFilme)}
                        </div>
                    </div>`;
                }
                if (subItem.valorCOCH && subItem.valorCOCH != 0) {
                    result += `
                    <div class="row line-data">
                        <div class="col-md-2 text-left" style="margin-left:10px">
                           COCH (${subItem.coch})
                        </div>
                        <div class="col-md-10 text-right" style="margin-left:-10px">
                           ${formataReal(subItem.valorCOCH)}
                        </div>
                    </div>`;
                }
                if (subItem.valorHMCH && subItem.valorHMCH != 0) {
                    result += `
                    <div class="row line-data">
                        <div class="col-md-2 text-left" style="margin-left:10px">
                           HMCH (${subItem.hmch})
                        </div>
                        <div class="col-md-10 text-right" style="margin-left:-10px">
                           ${formataReal(subItem.valorHMCH)}
                        </div>
                    </div>`;
                }
                if (subItem.observacao && subItem.observacao != '' && subItem.observacao != null) {
                    result += `
                    <div class="row line-data">
                        <div class="col-md-12 text-left" style="margin-left:10px">
                           <b>${subItem.observacao}</b>
                        </div>
                    </div>`;
                }

                result += renderSubItemHonorarios(subItem.honorarios);
                content.append(result);


            }



            const renderResumo = (item, content) => {
                const total = sumBy(item.resumoContaItem, (sumItem) => sumItem.total);
                if (!item.grupoDescricao || item.grupoDescricao == '') {
                    return;
                }
                const totalLabel = `Total de ${item.grupoDescricao}`;
                let result = `
                <div class="row resumo">
                    <div class="col-md-11 text-left">
                        <h5 class="bold"> ${totalLabel}</h5>
                    </div>
                    <div class="col-md-1 text-right" style="border-top: solid">
                        <h5 class="bold valor"> ${numeral(total).format('$0,0.00')}</h5>
                    </div>
                </div>
                `;
                content.append(result);
            }

            const renderSubTotal = (items, content) => {
                const total = sumBy(items, (sumItem) => sumItem.total);
                if (!items[0].subGrupoDescricao || items[0].subGrupoDescricao == '') {
                    return;
                }
                const totalLabel = `Total de ${items[0].subGrupoDescricao}`;
                let result = `
                 <div class="row total">
                    <div class="col-md-11 text-left">
                        <h5 class="bold" style="color:#337ab7"> ${totalLabel}</h5>
                    </div>
                    <div class="col-md-1 text-right" style="border-top: solid">
                        <h5 class="bold valor" style="color:#337ab7"> ${numeral(total).format('$0,0.00')}</h5>
                    </div>
                </div>
                `;
                content.append(result);
            }

            const renderTotal = (item, content) => {
                const total = sumBy(item.resumoContaItem, (sumItem) => sumItem.total);
                let result = `
                 <div class="row total">
                    <div class="col-md-11 text-left">
                        <h4 class="bold"> Total de ${item.grupoDescricao}</h4>
                    </div>
                    <div class="col-md-1 text-right" style="border-top: solid">
                        <h4 class="bold valor"> ${numeral(total).format('$0,0.00')}</h4>
                    </div>
                </div>
                `;
                content.append(result);
            }

            const renderTotalPeriodo = (resumoContaMedica, content) => {
                let total = 0;

                _.forEach(resumoContaMedica, (item) => {
                    total += sumBy(item.resumoContaItem, (sumItem) => sumItem.total);
                })

                let result = `
                 <div class="row total" style="margin: 50px 0" >
                    <div class="col-md-11 text-left">
                        <h3 class="bold"> TOTAL DO PERIODO</h3>
                    </div>
                    <div class="col-md-1 text-right" style="border-top: solid">
                        <h3 class="bold valor"> ${numeral(total).format('$0,0.00')}</h3>
                    </div>
                </div>
                `;
                content.append(result);
            }

            const sumBy = (arr, func) => arr.reduce((acc, item) => acc + func(item), 0)

            let content = $(`<div class="custom-scrollbar" style='height: 600px;overflow: scroll;top: 10px;position: relative;padding-right: 11px;background-color: white;'></div>`);
            let resumoConta = container.find(".resumo-conta");
            resumoConta.empty();

            resumoConta.append(
                `<nav class="navbar navbar-default navbar-el" style="margin-bottom: 0px">
                    <div class="container-fluid">
                        <div class="row" style="min-height: 50px;line-height: 50px;">
                        <!--<div class="col-md-2 text-left">
                            <h5>Ações</h5>
                        </div>-->
                        
                        <div class="col-md-2 text-center">
                            <h5>Código</h5>
                        </div>
                        <div class="col-md-5 text-left">
                            <h5>Descrição</h5>
                        </div>
                        <div class="col-md-1 text-right">
                            <h5>Qtde</h5>
                        </div>
                        <div class="col-md-1 text-right">
                            <h5>Preco Unitario</h5>
                        </div>
                        <div class="col-md-1 text-right">
                            <h5>Valor Total</h5>
                        </div>
                        <div class="col-md-1 text-right">
                            <h5>Percentual</h5>
                        </div>
                        <div class="col-md-1 text-right">
                            <h5>Total</h5>
                        </div>
                    </div>
                    </div>
                </nav>`)

            _.forEach(_.sortBy(resumoContaMedica, (item) => item.grupoOrdem), (item) => {
                renderHeader(item, content);
                _.forEach(_.groupBy(_.sortBy(item.resumoContaItem, (x) => x.subGrupoId), (x) => x.subGrupoId), (resumoGrupo) => {
                    if (!resumoGrupo.length) {
                        return;
                    }
                    renderSubHeader(resumoGrupo[0], content);
                    _.forEach(resumoGrupo, (resumoContaItem) => {
                        renderSubItem(resumoContaItem, content)
                    });
                    renderSubTotal(resumoGrupo, content);
                });
                /*renderResumo(item, content);*/
                renderTotal(item, content);
            })

            renderTotalPeriodo(resumoContaMedica, content);

            resumoConta.append(content);
        }

        function criarItemResumoContaMedica(
            subGrupoId,
            subGrupoDescricao,
            cod,
            dataInicial,
            dataFinal,
            honorarios,
            descricao,
            descricaoTuss,
            contaItemDescricao,
            contaItemCodigo,
            qtd,
            precoUnit,
            moeda,
            valorTotal,
            percent,total,
            coch,
            hmch, valorCOCH, valorHMCH,
            metragemFilme, valorFilme,
            observacao)
        {
            return {
                subGrupoId,
                subGrupoDescricao,
                cod,
                dataInicial,
                dataFinal,
                honorarios,
                descricao,
                descricaoTuss,
                contaItemDescricao,
                contaItemCodigo,
                qtd,
                precoUnit,
                moeda,
                valorTotal,
                percent,
                total,
                coch,
                hmch,
                valorCOCH,
                valorHMCH,
                metragemFilme,
                valorFilme,
                observacao
            }
        }
    })
})()