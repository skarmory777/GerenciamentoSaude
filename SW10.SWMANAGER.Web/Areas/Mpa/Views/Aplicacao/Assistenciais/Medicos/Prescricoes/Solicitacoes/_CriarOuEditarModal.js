(function ($) {
    app.modals.solicitacoesModal = function () {
        const solicitacaoAntimicrobianoAppService = abp.services.app.solicitacaoAntimicrobiano
        let _modalManager = null
        const form = $("#solicitacaoAntimicrobianoForm")
        let validator = null
        
        $.validator.addMethod('valida-cultura', function(value, element) {
            const parent = $(element).parents(".tab-solicitacao");
            parent.find(".add-cultura").removeClass('has-error')
            debugger;
            if(value == 1) {
                let formSerialized = parseSolicitacaoAntimicrobianosItems(form.serializeFormToObject());
                const itemId = parent.data("id");
                const formItem = _.find(formSerialized, (item)=> item.prescricaoItemId == itemId);
                if(formItem) {
                    let checkItem = formItem['solicitacaoAntimicrobianosCulturas'];
                    const result =  checkItem !== null && checkItem !== undefined && checkItem.length !== 0;
                    if(!result) {
                        parent.find(".add-cultura").addClass('has-error')
                    }
                    return result;
                }
            }
            return true;
        },
        'Não foi inserida nenhuma cultura')
        this.init = function (modalManager) {
            _modalManager = modalManager
            
            setTimeout(function () {
                const btnFinish = $('<button></button>').text('Salvar')
                    .addClass('btn btn-info btn-save sw-btn-save').click(submit)
                // SmartWizard initialize
                // Smart Wizard
                $('#smartwizard').smartWizard({
                    selected: 0,
                    theme: 'arrows',
                    enableURLhash: false,
                    justified: true,
                    autoAdjustHeight: false,
                    cycleSteps: true,
                    backButtonSupport: true,
                    transition: {
                        animation: 'slide-horizontal', // Effect on navigation, none/fade/slide-horizontal/slide-vertical/slide-swing
                        speed: '400', // Transion animation speed
                        easing: '' // Transition animation easing. Not supported without a jQuery easing plugin
                    },
                    toolbarSettings: {
                        toolbarPosition: 'bottom', // both bottom
                        toolbarButtonPosition: 'right',
                        toolbarExtraButtons: [btnFinish]// both bottom
                    },
                    lang: { // Language variables for button
                        next: 'Próximo',
                        previous: 'Anterior'
                    },
                }).on("showStep", onShowStep);

                function onShowStep(e, anchorObject, stepNumber, stepDirection, stepPosition) {
                    $(".sw-btn-prev").hide();
                    $(".sw-btn-next").hide();
                    $(".sw-btn-save").hide();
                    if (stepPosition !== 'first') {
                        $(".sw-btn-prev").show();
                    }
                    if (stepPosition === 'last') {
                        $(".sw-btn-next").show();
                    }

                    if (stepPosition === 'last' || $('#smartwizard').data("smartWizard").pages.length === 1) {
                        $(".sw-btn-save").show();
                    }
                }
                $(".tab-content.tab-content-wizard").height($(".solicitacoes-content").height() - 130)
                $("#solicitacaoAntimicrobianoForm .container-fluid").height($(".tab-content.tab-content-wizard").height()  - 30)
                $("#solicitacaoAntimicrobianoForm .row-antibioticos").height($("#solicitacaoAntimicrobianoForm .container-fluid").height() -20)
                $("#solicitacaoAntimicrobianoForm .row-antibioticos-content").height($("#solicitacaoAntimicrobianoForm .container-fluid").height())

                createValidator();
            }, 300);

            $('#solicitacoesModal').addClass("fullscreen");
        }
        
        function createValidator() {
            if(form.data("validator")) {
                form.validate().destroy();
            }
            $.validator.setDefaults({
                ignore: ""
            })
            validator = $("#solicitacaoAntimicrobianoForm").validate({
                ignore: "",
                invalidHandler: function (e, validator) {
                    validateHandler(validator);
                }
            })
        }
        
        function submit(e) {
            saveSolicitacaoAntimicrobiano(e);
        }
        
        function saveSolicitacaoAntimicrobiano(event) {
            $(".sw-btn-save").buttonBusy(true)
            const validate = form.validate();
            if(_.keys(validate.settings.rules).length) {
                validate.settings.rules = {}
            }
            if (validate.form()) {
                let formSerialized = form.serializeFormToObject()
                return solicitacaoAntimicrobianoAppService.salvarSolicitacoes({ SolicitacaoAntimicrobianos: parseSolicitacaoAntimicrobianosItems(formSerialized)}).then(res => {
                    if (res.successo) {
                        _modalManager.close();
                        imprimir(res.ids, formSerialized["prescricao_id"]);
                    }
                }).always(() => {
                    $(".sw-btn-save").buttonBusy(false)    
                })
            } else {
                $(".sw-btn-save").buttonBusy(false)
            }
        }
        
        function validateHandler(validator) {
            console.log('a',validator);
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

        function imprimir(ids,prescricaoId) {
            let parameters = `ids=${ids}`;
            if (_.isArray(ids)) {
                parameters = ids.map(x => `ids=${x}`).join("&")
            }
            $.removeCookie("XSRF-TOKEN");
            printJS({
                printable: `/Mpa/AssistenciaisRelatorios/ImprimirSolicitacaoAntimicrobiano?${parameters}`,
                type: 'pdf',
                onPrintDialogClose: () => {
                    abp.event.trigger("liberarPrescricaoIndex", prescricaoId)
                    abp.event.trigger("liberarPrescricao", prescricaoId)
                }
            })
        }

        function parseSolicitacaoAntimicrobianosItems(formSerialized) {
            const prescricaoItemRespostaIds = $("[name=itemRespostaIds]").val().split(",")
            let solicitacaoAntimicrobianos = []
            const keys = Object.keys(formSerialized)

            _.forEach(prescricaoItemRespostaIds, (prescricaoItemRespostaId) => {
                prescricaoItemRespostaId = prescricaoItemRespostaId.trim();

                let prescricaoItemRespostaKeys = _.filter(keys, (item) => item.indexOf(`solicitacao_${prescricaoItemRespostaId.trim()}_`) !== -1)
                let data = {
                    "prescricaoItemId": formSerialized[`solicitacao_${prescricaoItemRespostaId}_prescricaoItemId`],
                    "atendimentoId": formSerialized["atendimento_id"],
                    "prescricaoId": formSerialized["prescricao_id"]
                }
                _.forEach(prescricaoItemRespostaKeys, (prescricaoItemRespostaKeysItem) => {
                    let nomePropriedade = prescricaoItemRespostaKeysItem.split(`solicitacao_${prescricaoItemRespostaId}_`)[1]
                    if (nomePropriedade.indexOf("cultura") !== -1) {
                        parseCultura(data, prescricaoItemRespostaKeysItem, prescricaoItemRespostaKeys, formSerialized)
                    } else if (nomePropriedade.indexOf("[]") !== -1 && nomePropriedade.indexOf("indicacao") !== -1) {
                        parseIndicacao(data, prescricaoItemRespostaKeysItem)
                    } else {
                        data[nomePropriedade] = formSerialized[prescricaoItemRespostaKeysItem]
                    }
                })
                solicitacaoAntimicrobianos.push(data)
            })

            return solicitacaoAntimicrobianos
        }

        function parseItems(formSerialized) {
            const prescricaoItemIds = $("[name=itemIds]").val().split(",")
            let solicitacaoAntimicrobianos = []
            const keys = Object.keys(formSerialized)
            _.forEach(prescricaoItemIds, (prescricaoItemId) => {
                prescricaoItemId = prescricaoItemId.trim()
                let prescricaoItemKeys = _.filter(keys, (item) => item.indexOf(`solicitacao_${prescricaoItemId.trim()}_`) !== -1)
                let data = {
                    "prescricaoItemId": prescricaoItemId,
                    "atendimentoId": formSerialized["atendimento_id"],
                    "prescricaoId": formSerialized["prescricao_id"]
                }
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
                } else {
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