
function BuildConfiguracaoPrescricaoItem() {
    let configuracaoPrescricaoItem = abp.services.app.configuracaoPrescricaoItem;
    let fieldName;
    let resData;
    let uuid;
    let methods = {
        doWork: (resDados) => {
            resData = resDados;
            var callbackComponents = fieldName == "divisaoId" ? methods.MontaDomDivisoes : methods.MontaDomPrescricaoItem;
            $("#configuracao-prescricao-item-tab").empty().html(methods.MontaDom(callbackComponents))
            methods.renderFields();
            //carregaDados().then(res => {
            //    $("#configuracao-prescricao-item-tab").empty().html(MontaDom(res))
            //})

        },
        doSubItemPrescricaoWork: (resDados) => {
            resData = resDados;
            var callbackComponents = fieldName == "divisaoId" ? methods.MontaDomDivisoes : methods.MontaDomPrescricaoItem;
            $("#configuracao-sub-prescricao-item-tab").empty().html(methods.MontaDom(callbackComponents))
            methods.renderFields();
            //carregaDados().then(res => {
            //    $("#configuracao-prescricao-item-tab").empty().html(MontaDom(res))
            //})

        },
        renderFields: () => {
            $(`.content-configuracao.content-${uuid}`).find("select.select2").each(function () {
                selectCOnfiguracaoPrescricao(`select[name="${$(this).attr("name")}"]`, $(this).data("servico"), eval($(this).data("filtro")) ? $(`${$(this).data("filtro")}`) : undefined, { multiple: $(this).data("multiple") });
                if ($(this).data("value") != null) {
                    let value = $(this).data("value");
                    if (typeof value === "string" && $(this).data("multiple") && value && value != null && value.indexOf(",") !== -1) {
                        value = JSON.parse(`[${value}]`);
                    }
                    setTimeout(() => {
                        if ($(this).data("multiple")) {
                            $(this).trigger("select2:selectByIds", value);
                        } if (!$(this).data("multiple")) {
                            $(this).trigger("select2:selectById", value);
                        }
                    }, 150)
                }
            })

            $(`.content-configuracao.content-${uuid}`).find("input.date").each(function () {
                datePicker($(this));
            });

            function datePicker($el) {
                $el.daterangepicker({
                    "singleDatePicker": true,
                    "showDropdowns": true,
                    autoUpdateInput: false,
                    //autoUpdateInput: false,
                    //maxDate: new Date(),
                    changeYear: true,
                    //yearRange: 'c-10:c+10',
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
                }, function (start) {
                    $el.val(start.format('DD/MM/YYYY'))
                });
            }

        },
        retornaDado: (campoId, campoName, resDado) => {
            if (!resDado || !_.some(resDado, (x) => x.configuracaoPrescricaoItemCampoId == campoId)) {
                return null;
            }

            var item = _.find(resDado, (x) => x.configuracaoPrescricaoItemCampoId == campoId);

            return item[campoName]
        },
        retornaId: (campoId, resDado) => methods.retornaDado(campoId, "id", resDado),
        MontaDom: (callbackComponents) => {
            uuid = create_UUID();
            let content = $(`<div class="row content-configuracao content-${uuid}" style="max-height: 600px;overflow-y: auto;"></div>`);
            return callbackComponents(content, uuid);
        },
        MontaDomDivisoes: (content) => {
            methods.montaComponente({
                campoId: configuracaoPropriedades.QtdPorHorario,
                campoName: "Quantidade Por Horário?",
                campoPadrao: { possuiValorPadrao: true, tipoCampoPadrao: "number", enable: false }
            }).appendTo(content);
            methods.montaComponente({
                campoId: configuracaoPropriedades.Unidade,
                campoName: "Unidade?",
                campoPadrao: { possuiValorPadrao: true, possuiOpcoes: true, enable: false, tipoCampoPadrao: "select2", servico: '/api/services/app/ProdutoUnidade/ListarUnidadeConsumoProdutoDropdown' }
            }).appendTo(content);
            methods.montaComponente({
                campoId: configuracaoPropriedades.ViaDeAplicacao,
                campoName: "Via de Aplicação?",
                campoPadrao: { possuiValorPadrao: true, possuiOpcoes: true, enable: false, tipoCampoPadrao: "select2", servico: '/api/services/app/VelocidadeInfusao/listarDropdown' }
            }).appendTo(content);
            methods.montaComponente({
                campoId: configuracaoPropriedades.FormaDeAplicacao,
                campoName: "Forma de Aplicação?",
                campoPadrao: {
                    enable: false,
                    possuiValorPadrao: true,
                    possuiOpcoes: true,
                    tipoCampoPadrao: "select2",
                    servico: '/api/services/app/FormaAplicacao/ListarDropdown',
                    filtroPadrao: `[name='${configuracaoPropriedades.ViaDeAplicacao}_select2_valor_padrao']`,
                    filtroOpcoes: `[name='${configuracaoPropriedades.ViaDeAplicacao}_select2_valor_opcoes']`
                }
            }).appendTo(content);
            methods.montaComponente({
                campoId: configuracaoPropriedades.Diluente,
                campoName: "Diluente?",
                campoPadrao: { possuiValorPadrao: true, tipoCampoPadrao: "number", enable: false }
            }).appendTo(content);
            methods.montaComponente({
                campoId: configuracaoPropriedades.Volume,
                campoName: "Volume?",
                campoPadrao: { possuiValorPadrao: true, tipoCampoPadrao: "number", enable: false }
            }).appendTo(content);
            methods.montaComponente({
                campoId: configuracaoPropriedades.Medico,
                campoName: "Médico?",
                campoPadrao: { possuiValorPadrao: false, tipoCampoPadrao: "number" }
            }).appendTo(content);

            methods.montaComponente({
                campoId: configuracaoPropriedades.Frequencia,
                campoName: "Frequência?",
                campoPadrao: { possuiValorPadrao: true, tipoCampoPadrao: "number", enable: false }
            }).appendTo(content);
            methods.montaComponente({
                campoId: configuracaoPropriedades.HoraInicial,
                campoName: "Hora Inícial?",
                campoPadrao: { possuiValorPadrao: false, tipoCampoPadrao: "time" }
            }).appendTo(content);
            methods.montaComponente({
                campoId: configuracaoPropriedades.DiaInicial,
                campoName: "Dia Inícial?",
                campoPadrao: { possuiValorPadrao: false, tipoCampoPadrao: "date" }
            }).appendTo(content);

            methods.montaComponente({
                campoId: configuracaoPropriedades.DiasProvaveisDeUso,
                campoName: "Dias Provaveis De Uso?",
                campoPadrao: { possuiValorPadrao: true, tipoCampoPadrao: "number" }
            }).appendTo(content);

            methods.montaComponente({
                campoId: configuracaoPropriedades.Observacao,
                campoName: "Observação?",
                campoPadrao: { possuiValorPadrao: true, tipoCampoPadrao: "textarea" }
            }).appendTo(content);
            return content;
        },
        MontaDomPrescricaoItem: (content) => {
            methods.montaComponente({
                campoId: configuracaoPropriedades.QtdPorHorario,
                campoName: "Quantidade Por Horário?",
                campoPadrao: { possuiValorPadrao: true, tipoCampoPadrao: "number", enable: true }
            }).appendTo(content);
            methods.montaComponente({
                campoId: configuracaoPropriedades.Unidade,
                campoName: "Unidade?",
                campoPadrao: {
                    possuiValorPadrao: true, possuiOpcoes: true, enable: true, tipoCampoPadrao: "select2",
                    servico: '/api/services/app/prescricaoItem/listarUnidadePorProdutoDropdown',
                    filtroPadrao: `[name='ProdutoId']`,
                    filtroOpcoes: `[name='ProdutoId']`,
                }
            }).appendTo(content);
            methods.montaComponente({
                campoId: configuracaoPropriedades.ViaDeAplicacao,
                campoName: "Via de Aplicação?",
                campoPadrao: { possuiValorPadrao: true, possuiOpcoes: true, enable: true, tipoCampoPadrao: "select2", servico: '/api/services/app/VelocidadeInfusao/ListarDropdown' }
            }).appendTo(content);
            methods.montaComponente({
                campoId: configuracaoPropriedades.FormaDeAplicacao,
                campoName: "Forma de Aplicação?",
                campoPadrao: {
                    enable: true,
                    possuiValorPadrao: true,
                    possuiOpcoes: true,
                    tipoCampoPadrao: "select2",
                    servico: '/api/services/app/FormaAplicacao/ListarDropdown',
                    filtroPadrao: `[name='${configuracaoPropriedades.ViaDeAplicacao}_select2_valor_padrao']`,
                    filtroOpcoes: `[name='${configuracaoPropriedades.ViaDeAplicacao}_select2_valor_opcoes']`
                }
            }).appendTo(content);
            methods.montaComponente({
                campoId: configuracaoPropriedades.Diluente,
                campoName: "Diluente?",
                campoPadrao: {
                    enable: true,
                    possuiValorPadrao: true,
                    possuiOpcoes: true,
                    tipoCampoPadrao: "select2",
                    servico: '/api/services/app/PrescricaoItem/listarDiluenteDropdown',
                    filtroPadrao: `[name='DivisaoId']`,
                    filtroOpcoes: `[name='DivisaoId']`,
                }
            }).appendTo(content);
            methods.montaComponente({
                campoId: configuracaoPropriedades.Volume,
                campoName: "Volume?",
                campoPadrao: { possuiValorPadrao: true, tipoCampoPadrao: "number", enable: true }
            }).appendTo(content);
            methods.montaComponente({
                campoId: configuracaoPropriedades.Medico,
                campoName: "Médico?",
                campoPadrao: { possuiValorPadrao: false, tipoCampoPadrao: "number" }
            }).appendTo(content);
            methods.montaComponente({
                campoId: configuracaoPropriedades.Frequencia,
                campoName: "Frequência?",
                campoPadrao: {
                    possuiValorPadrao: true,
                    possuiOpcoes: true,
                    tipoCampoPadrao: "select2",
                    servico: '/api/services/app/frequencia/listarDropdown',
                    filtroPadrao: `[name='${configuracaoPropriedades.Frequencia}_select2_valor_padrao']`,
                    filtroOpcoes: `[name='${configuracaoPropriedades.Frequencia}_select2_valor_opcoes']`
                }
            }).appendTo(content);
            methods.montaComponente({
                campoId: configuracaoPropriedades.HoraInicial,
                campoName: "Hora Inícial?",
                campoPadrao: { possuiValorPadrao: false, tipoCampoPadrao: "time" }
            }).appendTo(content);
            methods.montaComponente({
                campoId: configuracaoPropriedades.DiaInicial,
                campoName: "Dia Inícial?",
                campoPadrao: { possuiValorPadrao: false, tipoCampoPadrao: "date" }
            }).appendTo(content);

            methods.montaComponente({
                campoId: configuracaoPropriedades.DiasProvaveisDeUso,
                campoName: "Dias Provaveis De Uso?",
                campoPadrao: { possuiValorPadrao: true, tipoCampoPadrao: "number" }
            }).appendTo(content);

            methods.montaComponente({
                campoId: configuracaoPropriedades.Observacao,
                campoName: "Observação?",
                campoPadrao: { possuiValorPadrao: true, tipoCampoPadrao: "textarea" }
            }).appendTo(content);
            return content;
        },
        montaComponente: ({campoId, campoName, campoPadrao }) => {
            campoPadrao.possuiValorPadrao = campoPadrao.possuiValorPadrao ?? false;
            campoPadrao.enable = campoPadrao.enable ?? true;
            campoPadrao.possuiOpcoes = campoPadrao.possuiOpcoes ?? false;

            let renderValorPadrao = '';
            let renderOpcoes = '';
            let defaultValue = methods.retornaDado(campoId, "defaultValue", resData) ?? null;
            let options = JSON.parse(methods.retornaDado(campoId, "options", resData) ?? "[]");
            let id = methods.retornaId(campoId, resData) ?? 0;
            if (_.isArray(options) && options.length == 0) {
                options = null;
            }
            if (campoPadrao.tipoCampoPadrao == "select2" && campoPadrao.enable) {
                renderValorPadrao = `
                        <div class="col-md-3">                             
                            <div class="form-group">
                                <label style="font-size: 12px;">Valor Padrão </label>
                                <select name="${uuid}_${campoId}_${campoPadrao.tipoCampoPadrao}_valor_padrao" data-name="${campoId}_${campoPadrao.tipoCampoPadrao}_valor_padrao" class="form-control select2" style="width:100%;"
                                    data-servico="${campoPadrao.servico}" 
                                    data-multiple="false"
                                    data-id="${id}"
                                    data-campo-id="${campoId}" 
                                    data-tipo-campo="${campoPadrao.tipoCampoPadrao}"  
                                    data-filtro="${campoPadrao.filtroPadrao}"
                                    data-value="${defaultValue}"
                                    data-campo-name="defaultValue">
                                </select>
                            </div>
                        </div>`

                renderOpcoes = `
                        <div class="col-md-3">                             
                            <div class="form-group">
                                <label style="font-size: 12px;">Opções </label>
                                <select name="${uuid}_${campoId}_${campoPadrao.tipoCampoPadrao}_valor_opcoes" data-name="${campoId}_${campoPadrao.tipoCampoPadrao}_valor_opcoes" class="form-control select2" style="width:100%;"
                                    data-servico="${campoPadrao.servico}" 
                                    data-multiple="true"
                                    data-id="${id}"
                                    data-campo-id="${campoId}" 
                                    data-tipo-campo="${campoPadrao.tipoCampoPadrao}"
                                    data-filtro="${campoPadrao.filtroOpcoes}"
                                    data-value="${options}"
                                    data-campo-name="options">
                                </select>
                            </div>
                        </div>`
            }
            else if (campoPadrao.tipoCampoPadrao == "textarea" && campoPadrao.enable) {
                renderValorPadrao = `
                        <div class="col-md-3">                             
                            <div class="form-group">
                                <label style="font-size: 12px;">Valor Padrão </label>
                                <textarea name="${uuid}_${campoId}_${campoPadrao.tipoCampoPadrao}_valor_padrao" data-name="${campoId}_${campoPadrao.tipoCampoPadrao}_valor_padrao" id="${uuid}_valor-${campoId}" value="${defaultValue}" name="valor-padrao-${campoId}" class="form-control " type="${campoPadrao.tipoCampoPadrao}" 
                                    data-campo-id="${campoId}" 
                                    data-id="${id}"
                                    data-tipo-campo="${campoPadrao.tipoCampoPadrao}"
                                    data-campo-name="defaultValue">${defaultValue ?? ''}</textarea>
                            </div>
                        </div>`
            }
            else if (campoPadrao.tipoCampoPadrao == "date" && campoPadrao.enable) {
                renderValorPadrao = `
                        <div class="col-md-3">                             
                            <div class="form-group">
                                <label style="font-size: 12px;">Valor Padrão </label>
                                <input name="${uuid}_${campoId}_${campoPadrao.tipoCampoPadrao}_valor_padrao" data-name="${campoId}_${campoPadrao.tipoCampoPadrao}_valor_padrao" id="${uuid}_valor-${campoId}" value="${defaultValue}" name="valor-padrao-${campoId}" class="form-control date" type="text" 
                                    data-campo-id="${campoId}" 
                                    data-id="${id}"
                                    data-tipo-campo="${campoPadrao.tipoCampoPadrao}"
                                    data-campo-name="defaultValue"/>
                            </div>
                        </div>`
            }
            else if (campoPadrao.enable) {
                renderValorPadrao = `
                        <div class="col-md-3">                             
                            <div class="form-group">
                                <label style="font-size: 12px;">Valor Padrão </label>
                                <input name="${uuid}_${campoId}_${campoPadrao.tipoCampoPadrao}_valor_padrao" data-name="${campoId}_${campoPadrao.tipoCampoPadrao}_valor_padrao" id="${uuid}_valor-${campoId}" value="${defaultValue}" name="valor-padrao-${campoId}" class="form-control " type="${campoPadrao.tipoCampoPadrao}" 
                                    data-campo-id="${campoId}" 
                                    data-id="${id}"
                                    data-tipo-campo="${campoPadrao.tipoCampoPadrao}"
                                    data-campo-name="defaultValue"/>
                            </div>
                        </div>`
            }

            const renderSwitch = (campo, campoProp, campoName) => {
                var checked = methods.retornaDado(campoId, campoName, resData) ?? false;
                return `<div class="col-md-3">
                                <label style="font-size: 12px;">${campoProp} </label>
                                <div class="input-group">
                                    <div class="switch__container">
                                        <input id="${uuid}_${campo}-${campoId}" name="${uuid}_${campo}-${campoId}" data-name="${campo}-${campoId}" class="switch switch--shadow switch--shadow-xs" type="checkbox" ${checked ? `checked="checked"` : ""} 
                                        data-id="${id}"
                                        data-campo-id="${campoId}" 
                                        data-campo-name="${campoName}" />
                                        <label for="${uuid}_${campo}-${campoId}"></label>
                                    </div>
                                </div>
                            </div>`
            }
            return $(`<div class="col-md-6 configuracao-item-prescricao">
                        <div class="row">    
                            <div class="col-md-12">
                                <h5 class="font-weight-bold">${campoName}</h5>
                            </div>
                            ${renderSwitch("bloqueado", "Bloqueado?", "isBlock")}
                            ${renderSwitch("obrigatorio", "Obrigatório?", "isRequired")}
                            ${campoPadrao.possuiValorPadrao && campoPadrao.enable ? renderValorPadrao : ""}
                            ${campoPadrao.possuiOpcoes && campoPadrao.enable ? renderOpcoes : ""} 
                        </div>
                    </div>`)
        },
        MontaObj: (id) => {
            debugger;
            let obj = [];
            $(`.content-configuracao.content-${uuid}`).find("input, select, textarea").each(function () {
                let data = $(this).data();
                let alreadyExist = _.some(obj, (x) => x.configuracaoPrescricaoItemCampoId == data.campoId)
                let alreadyExistIndex = _.findIndex(obj, (x) => x.configuracaoPrescricaoItemCampoId == data.campoId)
                let objItem = {}

                if (alreadyExist) {
                    objItem = _.find(obj, (x) => x.configuracaoPrescricaoItemCampoId == data.campoId)
                } else {
                    objItem = {
                        id: data.id ?? 0,
                        configuracaoPrescricaoItemCampoId: data.campoId
                    }
                }
                if(fieldName == "subPrescricaoItemId") {
                    fieldName = "prescricaoItemId";
                }
                objItem[fieldName] = id;
                if (data.tipoCampo == "select2") {
                    if ($(this).data("multiple")) {
                        objItem[data.campoName] = JSON.stringify($(this).select2().val());
                    } else {
                        objItem[data.campoName] = $(this).select2().val();
                    }
                } else if ($(this).attr("type") == "checkbox") {
                    objItem[data.campoName] = $(this).is(':checked');
                } else {
                    objItem[data.campoName] = $(this).val();
                }

                if (!alreadyExist) {
                    obj.push(objItem);
                } else {
                    obj[alreadyExistIndex] = objItem;
                }

            })
            return obj
        }
    }

    function create_UUID(){
        var dt = new Date().getTime();
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            var r = (dt + Math.random()*16)%16 | 0;
            dt = Math.floor(dt/16);
            return (c=='x' ? r :(r&0x3|0x8)).toString(16);
        });
        return uuid;
    }

    let configuracaoPropriedades = {
        QtdPorHorario: 1,
        Unidade: 2,
        ViaDeAplicacao: 3,
        FormaDeAplicacao: 4,
        Diluente: 5,
        Volume: 6,
        Medico: 7,
        Frequencia: 8,
        HoraInicial: 9,
        DiaInicial: 10,
        DiasProvaveisDeUso: 11,
        Observacao: 12
    }
    let saveAction;
    return {
        renderDivisao: () => {
            fieldName = "divisaoId";
            saveAction = salvarDivisao;
            configuracaoPrescricaoItem.obterPorDivisao($("#id").val()).then(methods.doWork);
        },
        renderPrescricaoItem: () => {
            fieldName = "prescricaoItemId";
            saveAction = prescricaoItemId;
            configuracaoPrescricaoItem.obterPorPrescricaoItem($("#prescricao-item-id").val()).then(methods.doWork);
        },
        renderSubPrescricaoItem: () => {
            fieldName = "subPrescricaoItemId";
            saveAction = subPrescricaoItemId;
            configuracaoPrescricaoItem.obterPorPrescricaoItem($("#subPrescricaoItemId").val(),$("#prescricaoItemId").val()).then(methods.doSubItemPrescricaoWork);
        },
        save: (id) => {
            if (fieldName == "divisaoId") {
                return saveAction();
            }
            else if (fieldName == "prescricaoItemId") {
                return saveAction();
            }
            else if (fieldName == "subPrescricaoItemId") {
                return saveAction(id);
            }
            return Promise.resolve(null);
        }
    }
    
    function salvarDivisao() {
        return configuracaoPrescricaoItem.criarOuEditar(methods.MontaObj($("#id").val()));
    }
    
    function prescricaoItemId() {
        return configuracaoPrescricaoItem.criarOuEditar(methods.MontaObj($("#prescricao-item-id").val()));
    }
    
    function subPrescricaoItemId(id) {
        return configuracaoPrescricaoItem.criarOuEditar(methods.MontaObj(id));
    }

    function selectCOnfiguracaoPrescricao(classe, url, elementoFiltro, select2Options) {

        function sanitize(value) {
            if (!value || value == "" || value == "undefined" || value == "null" || value == "[]" || _.isUndefined(value) || _.isNull(value)) {
                return undefined
            }
            return value;
        }

        $(classe).css('width', '100%');
        $.fn.modal.Constructor.prototype.enforceFocus = function () { };

        function retornaJsonFilterId() {
            let defaultValue = sanitize($(classe).data("value"));
            let options = sanitize($(classe).data("options"));
            if (typeof defaultValue === "string") {
                if (defaultValue.indexOf(",") !== -1 && defaultValue.indexOf("[") === -1) {
                    defaultValue = JSON.parse("[" + defaultValue + "]");
                } else {
                    defaultValue = JSON.parse(defaultValue);
                }
            }
            if (typeof options === "string") {
                if (options.indexOf(",") !== -1 && options.indexOf("[") === -1) {
                    options = JSON.parse("[" + options + "]");
                } else {
                    options = JSON.parse(options);
                }
            }

            if (!_.isArray(defaultValue)) {
                defaultValue = [defaultValue];
            }

            return {
                id: defaultValue,
                options: options
            }
        }

        function retornaJsonFilter() {

            let options = sanitize($(classe).data("options"));

            if (typeof options === "string") {
                if (options.indexOf(",") !== -1 && options.indexOf("[") === -1) {
                    options = JSON.parse("[" + options + "]");
                } else {
                    options = JSON.parse(options);
                }
            }

            return {
                options: options
            }
        }

        function filtrar() {
            if (elementoFiltro) {
                var retorno = null;
                if (elementoFiltro.valor != undefined) {
                    retorno = elementoFiltro.valor;
                } else if ((elementoFiltro != undefined) && (elementoFiltro != null) && (elementoFiltro != 0) && (elementoFiltro != '0')) {
                    if (elementoFiltro.val()) {
                        retorno = elementoFiltro.val();
                    } else {
                        retorno = null;// elementoFiltro;
                    }
                } else if (elementoFiltro.val()) {
                    retorno = elementoFiltro.val();
                }
                return retorno;
            }
            else {
                return null;
            }
        }
        const ajax = {
            url: url,
            dataType: 'json',
            delay: 250,
            method: 'Post',

            data: function (params) {
                //   //console.log('data: ', params, (params.page == undefined));
                if (params.page == undefined)
                    params.page = '1';
                //   //console.log('data: ', params);

                let result = {
                    search: params.term,
                    page: params.page,
                    totalPorPagina: 10,
                    jsonFilter: JSON.stringify(retornaJsonFilter())
                };

                var filtro = filtrar();

                if (filtro && filtro !== "") {
                    result.filtro = filtro;
                    result.filtros = filtro;
                }

                return result;
            },
            processResults: function (data, params) {
                params.page = params.page || 1;

                return {
                    results: data.result.items,
                    pagination: {
                        more: (params.page * 10) < data.result.totalCount
                    }
                };
            },
            cache: true
        };
        const defaultSelect2Options = {
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax,
            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            minimumInputLength: 0
        };

        if (!_.isUndefined(select2Options) && _.isObject(select2Options)) {
            _.extend(defaultSelect2Options, select2Options);
        }

        $(classe).select2(defaultSelect2Options).on("select2:selectById", (event, id) => {
            $(classe).data("value", id);

            $.ajax({
                url: ajax.url,
                dataType: ajax.dataType,
                delay: ajax.delay,
                method: ajax.method,
                data: {
                    page: 1,
                    totalPorPagina: 50,
                    jsonFilter: JSON.stringify(retornaJsonFilterId())
                }
            }).then((res) => {
                if (res.result && res.result.items) {
                    var item = _.find(res.result.items, (x) => x.id == id);
                    if (item) {
                        //$(classe).select2('data', item);
                        $(`<option ></option`).val(item.id).text(item.text).attr('selected', 'selected').appendTo($(classe));

                        $(classe).trigger("change");
                    }
                }
            });
        });

        $(classe).select2(defaultSelect2Options).on("select2:selectByIds", (event, ...id) => {
            $(classe).data("value", id);

            $.ajax({
                url: ajax.url,
                dataType: ajax.dataType,
                delay: ajax.delay,
                method: ajax.method,
                data: {
                    page: 1,
                    totalPorPagina: 50,
                    jsonFilter: JSON.stringify(retornaJsonFilterId())
                }
            }).then((res) => {
                if (res.result && res.result.items) {
                    var items = _.filter(res.result.items, (x) => _.contains(id, x.id));
                    if (items) {
                        _.each(items, (item) => {
                            //$(classe).select2('data', item);
                            $(`<option ></option`).val(item.id).text(item.text).attr('selected', 'selected').appendTo($(classe));
                        });
                        $(classe).trigger("change");
                    }
                }
            });
        });

    }
}