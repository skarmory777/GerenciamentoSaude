(function ($) {
    app.modals.solicitacaoAntimicrobianoModal = function () {
        const form = $("#solicitacaoAntimicrobianoForm")

        const solicitacaoAntimicrobianoAppService = abp.services.app.solicitacaoAntimicrobiano
        let _modalManager = null
        this.init = function (modalManager) {
            _modalManager = modalManager
            $('.modal-dialog').css({ 'min-width': '95%', 'min-height': '650px' })

            $(".save-button").click(submit)

            $(".add-cultura").click(addCultura)


            $('.checkbox-infeccao').on('change', function () {
                $(this).parents(".grupo-checkbox-infeccao").find(".checkbox-infeccao").not(this).prop('checked', false)
            })

            $('.add-cultura-date').each(function () {
                const el = $(this)
                el.daterangepicker({
                    "singleDatePicker": true,
                    "showDropdowns": true,
                    autoUpdateInput: false,
                    maxDate: new Date(),
                    changeYear: true,
                    yearRange: 'c-10:c+10',
                    showOn: "both",
                    "locale": {
                        "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
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
                }, (selDate) => el.val(selDate.format('L')).addClass('form-control edited'))
            })


            $(".add-cultura-tipo").select2({ width: '300px' })
        }

        function randomizeInteger(min, max) {
            if (max == null) {
                max = (min == null ? Number.MAX_SAFE_INTEGER : min)
                min = 0
            }

            min = Math.ceil(min) // inclusive min
            max = Math.floor(max) // exclusive max

            if (min > max - 1) {
                throw new Error("Incorrect arguments.")
            }

            return min + Math.floor((max - min) * Math.random())
        }

        function addCultura(event) {
            const el = $(event.currentTarget)
            const resultPanel = el.parents(".resultado-panel")
            const navCulturas = resultPanel.find(".navCulturas")
            const tabsCulturas = resultPanel.find(".tabsCulturas")
            const tiposResultados = JSON.parse($("[name=TipoResultados]").val())
            const tiposCulturas = JSON.parse($("[name=TipoCulturas]").val())

            //let rules = {}
            //resultPanel.find(".form-add").find("input").each(function () {
            //    rules[$(this).attr("name")] = { required: true }
            //})


            //const validator = $('#solicitacaoAntimicrobianoForm').validate({ rules });
            //debugger

            //var isValid = true;
            //resultPanel.find(".form-add").find("input").each(function () {
            //    if (!isValid || !validator.element(this)) {
            //        isValid = false;
            //    }
            //})

            //if (!isValid) {
            //    return;
            //}

            const culturaId = el.data('id') + '_' + randomizeInteger() + '_nova_cultura'
            const date = resultPanel.find('.add-cultura-date').val() || moment().format("DD/MM/YYYY")
            const tipoId = resultPanel.find('.add-cultura-tipo').val() || ''
            let findTipo = _.find(tiposCulturas, (item) => item.Id == tipoId)
            const tipo = findTipo != undefined && findTipo != null ? findTipo.Descricao : ''
            el.buttonBusy(true)
            let templateNav = `
                <li role="presentation" class="${culturaId}_li">
                    <a href="#${culturaId}" role="tab" data-toggle="tab" class="nav-item">
                        <span>${date} -  ${tipo}</span>
                        <button type="button" class="close text-danger ${culturaId}_close_btn" style="margin-left: 10px;margin-top: 5px;" aria-label="Close">
                          <span aria-hidden="true" class="text-danger">&times;</span>
                        </button>
                    </a>                   
                </li>`
            let templateResultados = ``
            _.forEach(tiposResultados, (item) => {
                templateResultados += `
                    <div class="col-md-4">
                        <div class="form-group">
                            <input type="checkbox" name="${culturaId}_resultado[]" value="${item.Id}" class="form-control checkbox-inline" />
                            <label>${item.Descricao}</label>
                        </div>
                    </div>`
            })

            let templateTabCultura = `
                <div role="tabpanel" class="tab-pane col-md-12" id="${culturaId}">
                    <input type="hidden" name="idCultura" value="${culturaId}" />
                    <input type="hidden" name="${culturaId}_dataCultura" value="${moment(date, 'DD/MM/YYYY').toISOString()}" />
                    <input type="hidden" name="${culturaId}_tipoId" value="${tipoId}" />
                    <div class="row">
                        <div class="form-group">
                            <label class="col-md-12 text-bold">
                                Resultado:
                            </label>
                        </div>
                    </div>
                    ${templateResultados}
                    <div class="col-sm-12">
                        <div class="form-group">
                            <label for="justificativa" class="text-bold">Outros Resultados:</label>
                            <textarea name="${culturaId}_outrosResultados" class="form-control input-sm" rows="4"></textarea>
                        </div>
                    </div>
                </div>`

            navCulturas.append(templateNav)
            tabsCulturas.append(templateTabCultura)

            resultPanel.find(`[href="#${culturaId}"]`).tab('show')
            resultPanel.find('.add-cultura-date').val(null).trigger("change")
            resultPanel.find('.add-cultura-tipo').val(null).trigger("change")

            $(`.${culturaId}_close_btn`).click(function (event) {
                event.stopImmediatePropagation()
                navCulturas.find(`.${culturaId}_li`).remove()
                tabsCulturas.find(`#${culturaId}`).remove()
                if (resultPanel.find("a.nav-item").length) {
                    $(resultPanel.find("a.nav-item")[0]).tab('show')
                }
            })

            el.buttonBusy(false)
        }

        function submit(event) {
            $(".save-button").buttonBusy(true)
            $.validator.setDefaults({
                ignore: ""
            })

            jQuery('#solicitacaoAntimicrobianoForm').validate({
                invalidHandler: function (e, validator) {
                    $("a.nav-item.hasError").removeClass("hasError")
                    if (validator.errorList.length) {
                        const firstElement = $(validator.errorList[0].element)
                        $(validator.errorList).each(function (index, errorListItem) {
                            $(errorListItem.element).parents(".tab-pane").each(function (index, el) {
                                let navItem = $(`a[href="#${$(el).attr("id")}"]`)
                                navItem.addClass("hasError")
                                if ($(el) === firstElement) {
                                    navItem.tab('show')
                                }
                            })
                        })

                        $(validator.errorList[0].element).parents(".tab-pane").each(function (index, el) {
                            $(`a[href="#${$(el).attr("id")}"]`).tab('show')
                            $(validator.errorList[0].element).focus()
                        })
                    }
                }
            })
            if (form.valid()) {
                var formSerialized = form.serializeFormToObject()
                return solicitacaoAntimicrobianoAppService.salvarSolicitacoes({ SolicitacaoAntimicrobianos: parseItems(formSerialized) }).then(res => {
                    if (res.successo == true) {
                        _modalManager.close()
                        imprimir(res.ids);
                    }
                })
            } else {
                $(".save-button").buttonBusy(false)
            }

            function imprimir(ids) {
                let parameters = `ids=${ids}`;
                if (_.isArray(ids)) {
                    parameters = ids.map(x => `ids=${x}`).join("&")
                }
                $.removeCookie("XSRF-TOKEN");
                printJS({
                    printable: `/Mpa/AssistenciaisRelatorios/ImprimirSolicitacaoAntimicrobiano?${parameters}`,
                    type: 'pdf',
                    onPrintDialogClose: () => {
                        abp.event.trigger("liberarPrescricao", formSerialized["prescricao_id"])
                    }
                })
            }
        }

        function parseItems(formSerialized) {
            const prescricaoItemIds = $("[name=itemIds]").val().split(",")
            let solicitacaoAntimicrobianos = []
            const keys = Object.keys(formSerialized)
            _.forEach(prescricaoItemIds, (prescricaoItemId) => {
                prescricaoItemId = prescricaoItemId.trim()
                let prescricaoItemKeys = _.filter(keys, (item) => item.indexOf(`solicitacao_${prescricaoItemId.trim()}_`) !== -1)
                let data = { "prescricaoItemId": prescricaoItemId, "atendimentoId": formSerialized["atendimento_id"], "prescricaoId": formSerialized["prescricao_id"] }
                _.forEach(prescricaoItemKeys, (prescricaoItemKeysItem) => {
                    let nomePropriedade = prescricaoItemKeysItem.split(`solicitacao_${prescricaoItemId}_`)[1]
                    if (nomePropriedade.indexOf("cultura") !== -1) {
                        parseCultura(data, prescricaoItemKeysItem, prescricaoItemKeys, formSerialized)
                    } else if (nomePropriedade.indexOf("[]") !== -1 && nomePropriedade.indexOf("indicacao") !== -1) {
                        parseIndicacao(data, prescricaoItemKeysItem)
                    } else {
                        data[nomePropriedade] = formSerialized[prescricaoItemKeysItem]
                    }
                })
                solicitacaoAntimicrobianos.push(data)
            })
            return solicitacaoAntimicrobianos
        }

        function parseIndicacao(data, prescricaoItemKeysItem) {
            if (!data['solicitacaoAntimicrobianosIndicacoes']) {
                data['solicitacaoAntimicrobianosIndicacoes'] = []
            }
            $(`[name='${prescricaoItemKeysItem}']:checkbox:checked`).each(function (i) {
                let dataItem = {}
                dataItem['tipoSolicitacaoAntimicrobianosIndicacaoId'] = $(this).val()
                data['solicitacaoAntimicrobianosIndicacoes'].push(dataItem)
            })

        }
        function parseCultura(data, prescricaoItemKeysItem, prescricaoItemKeys, formSerialized) {
            if (!data['solicitacaoAntimicrobianosCulturas']) {
                data['solicitacaoAntimicrobianosCulturas'] = []
            }
            let solicitacaoAntimicrobianosCulturas = data['solicitacaoAntimicrobianosCulturas']
            if (prescricaoItemKeysItem.indexOf("_nova_cultura") !== -1) {
                let tmpKeys = prescricaoItemKeysItem.replace(`solicitacao_${formSerialized['itemIds']}_`, '')
                tmpKeys = tmpKeys.split("_nova_cultura_")
                const key = tmpKeys[0]
                const propriedade = tmpKeys[1]
                let actualItem = _.find(solicitacaoAntimicrobianosCulturas, (item) => item.key == key)
                let existItem = true

                if (actualItem == null || actualItem == undefined) {
                    actualItem = {}
                    existItem = false
                }

                if (propriedade.indexOf("resultado[]") !== -1) {
                    parseResultado(actualItem, prescricaoItemKeysItem)
                }
                else {
                    actualItem[propriedade] = formSerialized[prescricaoItemKeysItem]
                }

                if (!existItem) {
                    actualItem.key = key
                    solicitacaoAntimicrobianosCulturas.push(actualItem)
                }
            }
            data['solicitacaoAntimicrobianosCulturas'] = solicitacaoAntimicrobianosCulturas
        }

        function parseResultado(data, prescricaoItemKeysItem) {
            if (!data['solicitacaoAntimicrobianosResultados']) {
                data['solicitacaoAntimicrobianosResultados'] = []
            }
            $(`[name='${prescricaoItemKeysItem}']:checkbox:checked`).each(function (i) {
                var dataItem = {}
                dataItem['tipoSolicitacaoAntimicrobianosResultadoId'] = $(this).val()
                data['solicitacaoAntimicrobianosResultados'].push(dataItem)
            })
        }
    }
})(jQuery)