var app = angular.module("formularioApp", [])
    .service("construtor", ["tipos", function (tipos) {
        var self = this;

        this.criarLinha = function (linha, numCol, Ordem) {
            if (!Ordem) {
                Ordem = linha;
            }
            if (numCol === undefined) {
                numCol = 1;
            }
            if (linha === undefined) {
                throw "Informe o numero da linha";
            }
            if (numCol > 12) {
                numCol = 12;
            } else if (numCol < 1) {
                numCol = 1;
            }

            var result = {
                "Id": 0,
                "ColConfigs": [],
                "Ordem": Ordem,
                "TotCols": 12
            };

            for (var i = 0; i < numCol; i++) {
                result.ColConfigs.push(new self.criarCelula(linha, i, false, false));
            }
            result.TotCols = Math.round(12 / numCol);

            return result;
        };

        this.criarCelula = function (linha, coluna, colspan, readonly) {
            if (colspan == undefined) {
                colspan = false;
            }

            if (!readonly) {
                readonly = false;
            }

            return {
                "Id": "col_id_" + linha + "_" + coluna,
                "Name": "col_name_" + linha + "_" + coluna,
                "Label": "Campo " + linha + "_" + coluna,
                "Placeholder": "col_placeholder_" + linha + "_" + coluna,
                "Value": null,
                "Type": tipos.text,
                "Colspan": colspan,
                "Readonly": readonly,
                "MultiOption": [],
                "properties": {},

                // CAMPOS RESERVADOS - Preenchimento e SalvarTodos
                "Preenchimento": 1,
                "SalvarTodos": false // Null eh default, salva no Atendimento atual, 1 - Salva em todos os atendimentos
            };
        }

        this.formataLinhas = function (linhas) {
            linhas = JSON.parse(JSON.stringify(linhas));
            _.forEach(linhas, (item) => {
                _.forEach(item.ColConfigs, (colConfigItem) => {
                    colConfigItem.Properties = this.formataProperties(colConfigItem)
                })
            })
            return linhas
        }

        this.formataProperties = (colConfigItem) => {
            return this.formataJSON(colConfigItem.Properties);
        }

        this.formataJSON = (value) => {
            if (value && value !== "{}" && value !== "\"{}\"" && typeof value === "string") {
                return self.formataJSON(JSON.parse(value));
            } else if (value && typeof value === "object") {
                return value;
            }
            return {}
        }

        this.formataSalvarLinhas = (linhas) => {
            linhas = JSON.parse(JSON.stringify(linhas));
            _.forEach(linhas, (item) => {
                _.forEach(item.ColConfigs, (colConfigItem) => {
                    if (colConfigItem.Properties) {
                        colConfigItem.Properties = JSON.stringify(colConfigItem.Properties);
                    }
                    if (colConfigItem.properties) {
                        colConfigItem.properties = JSON.stringify(colConfigItem.properties);
                    }
                })
            })
            return linhas
        }

        // Recriar
        this.recriarLinha = function (linha, numCol, Ordem, item) {
            if (!Ordem) {
                Ordem = linha;
            }
            if (numCol === undefined) {
                numCol = 1;
            }
            if (linha === undefined) {
                throw "Informe o numero da linha";
            }

            if (numCol > 12) {
                numCol = 12;
            } else if (numCol < 1) {
                numCol = 1;
            }

            var result = {
                //"Id": 0,
                "Id": item.Id,
                "ColConfigs": [],
                "Ordem": Ordem,
                "TotCols": 12
            };

            for (var j = 0; j < item.ColConfigs.length; j++) {
                result.ColConfigs.push(new self.recriarCelula(item.ColConfigs[j], j, false, false));
            }
            result.TotCols = Math.round(12 / numCol);

            //console.log('result: ' + JSON.stringify(result));

            return result;
        };

        this.recriarCelula = function (colConfig, coluna, /*linha,*/ colspan, readonly) {
            if (colspan == undefined) {
                colspan = false;
            }

            if (!readonly) {
                readonly = false;
            }

            if (colConfig.Properties && typeof colConfig.Properties == "string") {
                colConfig.Properties = JSON.parse(colConfig.Properties)
            } else {
                colConfig.Properties = {}
            }

            return {
                //    "Id": id,
                "Id": colConfig.Id,
                //   "Name": name,
                "Name": colConfig.Name,
                "Label": colConfig.Label,
                "Placeholder": colConfig.Placeholder,
                "Value": colConfig.Value,
                "Type": colConfig.Type,
                "Colspan": colConfig.Colspan,
                "Readonly": colConfig.Readonly,
                "MultiOption": colConfig.MultiOption,
                "Orientation": colConfig.Orientation || "horizontal",
                "PrependText": colConfig.PrependText,
                "AppendText": colConfig.AppendText,
                "Properties": colConfig.Properties,
                "Offset": colConfig.Offset,
                "Size": colConfig.Size,
                // CAMPOS RESERVADOS - Preenchimento
                "Preenchimento": colConfig.Preenchimento,
                "SalvarTodos": colConfig.SalvarTodos

            };
        }

    }])

    .constant("tipos", {
        "text": "text",
        "email": "email",
        "checkbox": "checkbox",
        "datetimesingle": "datetimesingle",
        "datetimerange": "datetimerange",
        "radio": "radio",
        "select": "select",
        "range": "range",
        "textarea": "textarea"
    })

    .controller("formularioCtrl", ["$scope", "$http", "construtor", "tipos", function ($scope, $http, construtor, tipos) {
        $('#myModal').modal({ show: false });

        // Bloqueando esc
        //$(document).keypress(function (e) {
        //    debugger
        //    if (e.keyCode === 27) {
        //        event.preventDefault(); //prevent default if it is body
        //    }
        //});
        // Fim - bloqueando esc

        // Campos reservados    

        // Preenchimento
        $scope.preenchimentos = [1, 2, 3];
        $scope.preenchimentoSelecionado = $scope.preenchimentos[0];

        var _formConfigService = abp.services.app.formConfig;
        var linhasFormReservado;
        var camposReservados = [];
        camposReservados.push("Selecione");

        $http.get("/Mpa/GeradorFormularios/ObterFormConfigReservado")
            .then(function (response) {
                linhasFormReservado = response.data.Linhas;
                var extrairColConfigs = function (item, index) {
                    for (var i = 0; i < item.ColConfigs.length; i++) {
                        var novaCol = item.ColConfigs[i];
                        camposReservados.push(novaCol.Name.toUpperCase());
                    }
                }
                linhasFormReservado.forEach(extrairColConfigs);
            }, function () { });

        $scope.reservados = camposReservados;
        $scope.selecionado = camposReservados[0];
        $scope.editarCelulaReservada = function (celula, name) {

            if (name == 'Selecione') {
                celula.Name = '';
                $scope.editarCelula(celula);
                return;
            }

            // ObterColConfigReservado

            $http.get("/Mpa/GeradorFormularios/ObterColConfigReservado?colDesejada=" + name)
                .then(function (response) {
                    celula.Label = response.data.Label;
                    celula.MultiOption = response.data.MultiOption;
                    celula.Name = response.data.Name;
                    celula.Placeholder = response.data.Placeholder;
                    celula.Type = response.data.Type;
                    celula.Preenchimento = response.data.Preenchimento;
                    celula.SalvarTodos = response.data.SalvarTodos;
                    celula.Properties = formataProperties(celula);

                    $scope.editType = celula;

                }, function () { });

            var inputsReservada = $('#editModal').find('input');
        }

        $('#fecha-modal').removeAttr("data-dismiss");
        $('#fecha-modal').on('click', function (e) {
            e.preventDefault();

            var inputsReservada = $('#editModal').find('input');
            var nameFuturo = inputsReservada.eq(2).val();
            //    console.log(nameFuturo);
            //if (camposReservados.indexOf(nameFuturo.toUpperCase()) > -1) {
            //    alert('Não é permitido criar um campo com "Name" contendo palavra reservada.');
            //} else {
            //    $('#editModal').modal('toggle');
            //}

            $('#editModal').modal('toggle');
        });
        // Fim - campos reservados

        $scope.editType = {};
        $scope.formName = "Form_1";
        $scope.formsConfig = [];
        $scope.typeOption = [];
        $scope.dados = { "NumLinha": 1 };
        $scope.dados2 = {
            "linhaAt": 1,
            "NumColunas": 1
        };

        $scope.editarCelula = function (celula) {

            if (camposReservados.indexOf(celula.Name) > -1) {
                alert('Não é permitido editar um campo reservado aqui.');
                return;
            }

            $scope.editType = celula;
            $('#editModal').modal('toggle');
        }

        $scope.addMultiOption = function (celula) {
            if (celula.MultiOption) {
                celula.MultiOption.push({ Opcao: "Nova Opção" });
            }
        }

        $scope.addLinha = function (numCol) {
            $scope.formsConfig.push(construtor.criarLinha($scope.formsConfig.length, numCol, $scope.dados2.linhaAt));
            $scope.dados2.linhaAt = $scope.formsConfig.length + 1;
        }

        $scope.removerLinha = function (item) {
            var index = $scope.formsConfig.indexOf(item)
            $scope.formsConfig.splice(index, 1);
            $scope.dados2.linhaAt = $scope.formsConfig.length + 1;
        }

        $scope.tempArray = [];
        $scope.addLinhaAt = function (linha, NumColunas) {
            //     debugger
            var diferenca = linha - $scope.formsConfig.length;
            if (diferenca > 1) {
                linha = $scope.formsConfig.length + 1;
            }

            if ($scope.formsConfig.length < 1) {
                $scope.addLinha(NumColunas);
                return;
            } else if (linha > $scope.formsConfig.length) {
                $scope.addLinha(NumColunas);
                return;
            }

            function pushTempArray(i) {
                $scope.tempArray.push(i);
            };
            function zerarTempArray() {
                $scope.tempArray = [];
            };
            function getTempArray() {
                return $scope.tempArray;
            };

            $scope.formsConfig.forEach(reconstroi);

            function reconstroi(item, index) {

                if (index < linha - 1) {
                    pushTempArray(item);
                }
                else if (index == linha - 1) {
                    pushTempArray(construtor.criarLinha(linha - 1, NumColunas, index));
                    pushTempArray(construtor.recriarLinha(linha + 1, NumColunas, index, item));
                }
                else {
                    pushTempArray(construtor.recriarLinha(linha + 1, NumColunas, index, item));
                }

                item.Ordem = index;
            };

            $scope.formsConfig = getTempArray().slice(0);

            zerarTempArray();
            $scope.dados2.linhaAt = $scope.formsConfig.length + 1;
        }

        $scope.tempArray2 = [];
        $scope.removerLinhaAt = function (elemento, linha) {

            var index = $scope.formsConfig.indexOf(elemento)
            $scope.formsConfig.splice(index, 1);

            function pushTempArray2(i) {
                $scope.tempArray2.push(i);
            };
            function zerarTempArray2() {
                $scope.tempArray2 = [];
            };
            function getTempArray2() {
                return $scope.tempArray2;
            };

            $scope.formsConfig.forEach(reconstroi2);

            function reconstroi2(item, index) {

                if (index <= linha) {
                    pushTempArray2(construtor.recriarLinha(index, item.ColConfigs.length, index, item));
                }
                else {
                    pushTempArray2(construtor.recriarLinha(index - 1, item.ColConfigs.length, index, item));
                }

                linha = linha + 1;
            };

            $scope.formsConfig = getTempArray2().slice(0);
            zerarTempArray2();
            $scope.dados2.linhaAt = $scope.formsConfig.length + 1;
        }

        $scope.gravarConfig = function () {

            var _formConfigService = abp.services.app.formConfig;

            var formConfig = {
                "Id": "0",
                "Nome": $scope.formName,
                "Linhas": construtor.formataSalvarLinhas($scope.formsConfig),
                "DataAlteracao": moment().format('L LT'),
                "IsSistema": "0",
                "IsDeleted": "0"
            };

            $('#btn-save').buttonBusy(true);

            _formConfigService.criarOuEditar(formConfig)
                .done(function () {
                    abp.notify.success(app.localize('SavedSuccessfully'));
                    window.location.href = "/Mpa/GeradorFormularios/Index";
                })
                .always(function () {
                    $('#btn-save').buttonBusy(false);
                });
        }

        //Inicialização
        $scope.typeOption.push(tipos.text);
        $scope.typeOption.push(tipos.select);
        $scope.typeOption.push(tipos.radio);
        $scope.typeOption.push(tipos.email);
        $scope.typeOption.push(tipos.checkbox);
        $scope.typeOption.push(tipos.datetimesingle);
        $scope.typeOption.push(tipos.datetimerange);
        $scope.typeOption.push(tipos.range);
        $scope.typeOption.push(tipos.textarea);
        //End Inicialização

    }])

    .controller("editarFormularioCtrl", ["$scope", "$http", "construtor", "tipos", function ($scope, $http, construtor, tipos) {
        $('#myModal').modal({ show: false });

        $scope.editType = {};
        $scope.formName = "Form_1";
        $scope.formId = 0;
        $scope.formsConfig = [];
        $scope.typeOption = [];
        $scope.dados = { "NumLinha": 1 };

        $scope.editarCelula = function (celula) {
            $scope.editType = celula;
            $('#editModal').modal('toggle');
        }

        $scope.removerLinha = function (item) {
            var index = $scope.formsConfig.indexOf(item)
            $scope.formsConfig.splice(index, 1);
        }

        $scope.addMultiOption = function (celula) {
            if (celula.MultiOption) {
                celula.MultiOption.push({ Opcao: "Nova Opção" });
            }
        }

        $scope.addLinha = function (numCol) {
            $scope.formsConfig.push(construtor.criarLinha($scope.formsConfig.length, numCol));
        }

        $scope.gravarConfig = function () {
            //   debugger
            var _formConfigService = abp.services.app.formConfig;

            var formConfig = {
                "Id": "0",
                "Nome": $scope.formName,
                "Linhas": construtor.formataSalvarLinhas($scope.formsConfig),
                "DataAlteracao": moment().format('L LT'),
                "IsSistema": "0",
                "IsDeleted": "0"
            };

            $('#btn-save').buttonBusy(true);

            _formConfigService.criarOuEditar(formConfig)
                .done(function () {
                    abp.notify.success(app.localize('SavedSuccessfully'));
                    window.location.href = "/Mpa/GeradorFormularios/Index";
                })
                .always(function () {
                    $('#btn-save').buttonBusy(false);
                });
        }

        //Inicialização
        $scope.typeOption.push(tipos.text);
        $scope.typeOption.push(tipos.select);
        $scope.typeOption.push(tipos.radio);
        $scope.typeOption.push(tipos.email);
        $scope.typeOption.push(tipos.checkbox);
        $scope.typeOption.push(tipos.datetimesingle);
        $scope.typeOption.push(tipos.datetimerange);
        $scope.typeOption.push(tipos.range);
        $scope.typeOption.push(tipos.textarea);
        //End Inicialização

    }])

    .controller("editarFormularioCtrlRod", ["$scope", "$http", "construtor", "tipos", function ($scope, $http, construtor, tipos) {
        $('#myModal').modal({ show: false });

        // Campos reservados    

        // Preenchimento

        var p1 = { value: 1, text: '1 - Campo em branco' };
        var p2 = { value: 2, text: '2 - Atendimento atual' };
        var p3 = { value: 3, text: '3 - Último lançamento' };
        var p4 = { value: 4, text: '4 - Último lançamento Atendimento Atual' };

        $scope.preenchimentos = [p1, p2, p3 ,p4];
        $scope.preenchimentoSelecionado = $scope.preenchimentos[0];

        var _formConfigService = abp.services.app.formConfig;
        var linhasFormReservado;
        var camposReservados = [];
        camposReservados.push("Selecione");

        $http.get("/Mpa/GeradorFormularios/ObterFormConfigReservado")
            .then(function (response) {
                linhasFormReservado = response.data.Linhas;
                var extrairColConfigs = function (item, index) {
                    for (var i = 0; i < item.ColConfigs.length; i++) {
                        var novaCol = item.ColConfigs[i];
                        camposReservados.push(novaCol.Name.toUpperCase());
                    }
                }
                linhasFormReservado.forEach(extrairColConfigs);
                $scope.reservados = camposReservados;
                $scope.selecionado = camposReservados[0];
            }, function () { });


        $scope.editarCelulaReservada = function (celula, name) {

            if (name == 'Selecione') {
                celula.Name = '';
                $scope.editarCelula(celula);
                return;
            }

            // ObterColConfigReservado

            $http.get("/Mpa/GeradorFormularios/ObterColConfigReservado?colDesejada=" + name)
                .then(function (response) {

                    celula.Label = response.data.Label;
                    celula.MultiOption = response.data.MultiOption;
                    celula.Name = response.data.Name;
                    celula.Placeholder = response.data.Placeholder;
                    celula.Type = response.data.Type;
                    celula.Preenchimento = response.data.Preenchimento;
                    celula.SalvarTodos = response.data.SalvarTodos;
                    celula.Properties = formataProperties(celula);
                    $scope.editType = celula;

                }, function () { });

            var inputsReservada = $('#editModal').find('input');
        }

        $('#fecha-modal').removeAttr("data-dismiss");
        $('#fecha-modal').on('click', function (e) {
            e.preventDefault();

            var inputsReservada = $('#editModal').find('input');
            var nameFuturo = inputsReservada.eq(2).val();
            //    console.log(nameFuturo);
            //if (camposReservados.indexOf(nameFuturo.toUpperCase()) > -1) {
            //    alert('Não é permitido criar um campo com "Name" contendo palavra reservada.');
            //} else {
            //    $('#editModal').modal('toggle');
            //}
            $('#editModal').modal('toggle');
        });
        // Fim - campos reservados

        $scope.editType = {};
        $scope.formName = "";
        $scope.formsConfig = [];
        $scope.typeOption = [];
        $scope.dados = { "NumLinha": 1 };
        $scope.dados2 = {
            "linhaAt": 1,
            "NumColunas": 1
        };

        $scope.editarCelula = function (celula) {

            //if (camposReservados.indexOf(celula.Name) > -1) {
            //    alert('Não é permitido editar um campo reservado.');
            //    return;
            //}

            $scope.editType = celula;

            // Preenchimento (campos reservados)

            // Implementar aqui, conversao de radio buttons de preenchimento para valores int:
            // 1 - Vazio, 2 - Registro atual, 3 - Ultimo lancamento


            $('#editModal').modal('toggle');
        }

        $scope.addMultiOption = function (celula) {
            if (celula.MultiOption) {
                celula.MultiOption.push({ Opcao: "Nova Opção" });
            }
        }

        $scope.addLinha = function (numCol) {
            $scope.formsConfig.push(construtor.criarLinha($scope.formsConfig.length, numCol));
            $scope.dados2.linhaAt = $scope.formsConfig.length + 1;
            ajustarOrdensProto();
        }

        $scope.removerLinha = function (item) {
            var index = $scope.formsConfig.indexOf(item)
            $scope.formsConfig.splice(index, 1);
            $scope.dados2.linhaAt = $scope.formsConfig.length + 1;
            ajustarOrdensProto();
        }

        $scope.tempArray = [];
        $scope.addLinhaAt = function (linha, numColuna) {
            //    debugger


            var diferenca = linha - $scope.formsConfig.length;

            if (diferenca > 1) {
                linha = $scope.formsConfig.length + 1;
            }
            if ($scope.formsConfig.length < 1) {
                $scope.addLinha(numColuna);
                ajustarOrdensProto();
                return;
            } else if (linha > $scope.formsConfig.length) {
                $scope.addLinha(numColuna);
                ajustarOrdensProto();
                return;
            }

            function pushTempArray(i) {
                $scope.tempArray.push(i);
            };
            function zerarTempArray() {
                $scope.tempArray = [];
            };
            function getTempArray() {
                return $scope.tempArray;
            };

            $scope.formsConfig.forEach(reconstroi);

            function reconstroi(item, index) {
                item.Ordem = index;
                ////console.log('item: ' + JSON.stringify(item));

                if (index < linha - 1) {
                    pushTempArray(item);
                }
                else if (index == linha - 1) {
                    pushTempArray(construtor.criarLinha(linha, numColuna, 0));
                    pushTempArray(construtor.recriarLinha(linha + 1, item.ColConfigs.length, 0, item));
                }
                else {
                    pushTempArray(construtor.recriarLinha(linha + 1, item.ColConfigs.length, 0, item));
                }
            };

            $scope.formsConfig = getTempArray().slice(0);
            zerarTempArray();
            $scope.dados2.linhaAt = $scope.formsConfig.length + 1;
            ajustarOrdensProto();
        }

        $scope.tempArray2 = [];
        $scope.removerLinhaAt = function (elemento, linha) {

            var index = $scope.formsConfig.indexOf(elemento)
            $scope.formsConfig.splice(index, 1);

            function pushTempArray2(i) {
                $scope.tempArray2.push(i);
            };
            function zerarTempArray2() {
                $scope.tempArray2 = [];
            };
            function getTempArray2() {
                return $scope.tempArray2;
            };

            $scope.formsConfig.forEach(reconstroi2);

            function reconstroi2(item, index) {
                item.Ordem = index;
                if (index <= linha) {
                    pushTempArray2(construtor.recriarLinha(index, item.ColConfigs.length, 0, item));
                }
                else {
                    pushTempArray2(construtor.recriarLinha(index - 1, item.ColConfigs.length, 0, item));
                }

                linha = linha + 1;
            };

            $scope.formsConfig = getTempArray2().slice(0);
            zerarTempArray2();
            $scope.dados2.linhaAt = $scope.formsConfig.length + 1;
            ajustarOrdensProto();
        }


        function ajustarOrdensProto() {
            $scope.formsConfig.forEach(ajustarOrdens);

            //console.log(JSON.stringify($scope.formsConfig));
        }

        function ajustarOrdens(item, index) {
            item.Ordem = index;
        }

        $scope.gravarConfig = function () {

            var _formConfigService = abp.services.app.formConfig;

            var formConfig = {
                "Id": $scope.Id,
                "Nome": $scope.formName,
                "Linhas": construtor.formataSalvarLinhas($scope.formsConfig),
                "DataAlteracao": moment().format('L LT'),
                "IsSistema": "0",
                "IsDeleted": "0",
                "Descricao": "RESERVADO"
            };

            $('#btn-save').buttonBusy(true);

            _formConfigService.criarOuEditar(formConfig)
                .done(function () {
                    abp.notify.success(app.localize('SavedSuccessfully'));
                    window.location.href = "/Mpa/GeradorFormularios/Index";
                });
        }

        //Inicialização
        $http.get("/Mpa/GeradorFormularios/ObterFormConfig/" + $("#CloneId").val())
            .then(function (response) {
                $scope.Id = $("#CloneId").val();
                $scope.formName = response.data.Nome;
                // $scope.formsConfig = response.data.Linhas;
                // //console.log(JSON.stringify(response.data));
                debugger;
                for (i = 0; i < response.data.Linhas.length; i++) {
                    $scope.formsConfig.push(construtor.recriarLinha(response.data.Linhas[i].Ordem, response.data.Linhas[i].ColConfigs.length, response.data.Linhas[i].Ordem, response.data.Linhas[i]));
                }
                console.log($scope.formsConfig);

                $scope.IsSistema = response.data.IsSistema;
                $scope.IsDeleted = response.data.IsDeleted;
                $scope.dados2.linhaAt = $scope.formsConfig.length + 1;
            }, function () { });

        $scope.typeOption.push(tipos.text);
        $scope.typeOption.push(tipos.select);
        $scope.typeOption.push(tipos.radio);
        $scope.typeOption.push(tipos.email);
        $scope.typeOption.push(tipos.checkbox);
        $scope.typeOption.push(tipos.datetimesingle);
        $scope.typeOption.push(tipos.datetimerange);
        $scope.typeOption.push(tipos.range);
        $scope.typeOption.push(tipos.textarea);
        //End Inicialização
    }])

    .controller("clonarFormularioCtrl", ["$scope", "$http", "construtor", "tipos", function ($scope, $http, construtor, tipos) {
        $('#myModal').modal({ show: false });

        $scope.editType = {};
        $scope.formName = "";
        $scope.formsConfig = [];
        $scope.typeOption = [];
        $scope.dados = { "NumLinha": 1 };
        $scope.dados2 = { "linhaAt": 1 };

        $scope.editarCelula = function (celula) {
            $scope.editType = celula;
            $('#editModal').modal('toggle');
        }

        $scope.addMultiOption = function (celula) {
            if (celula.MultiOption) {
                celula.MultiOption.push({ Opcao: "Nova Opção" });
            }
        }

        $scope.addLinha = function (numCol) {
            $scope.formsConfig.push(construtor.criarLinha($scope.formsConfig.length, numCol));
            $scope.dados2.linhaAt = $scope.formsConfig.length + 1;
        }

        $scope.removerLinha = function (item) {
            var index = $scope.formsConfig.indexOf(item)
            $scope.formsConfig.splice(index, 1);
            $scope.dados2.linhaAt = $scope.formsConfig.length + 1;
        }

        $scope.tempArray = [];
        $scope.addLinhaAt = function (linha, numLinha) {

            var diferenca = linha - $scope.formsConfig.length;
            if (diferenca > 1) {
                linha = $scope.formsConfig.length + 1;
            }

            if ($scope.formsConfig.length < 1) {
                $scope.addLinha(numLinha);
                return;
            } else if (linha > $scope.formsConfig.length) {
                $scope.addLinha(numLinha);
                return;
            }

            function pushTempArray(i) {
                $scope.tempArray.push(i);
            };
            function zerarTempArray() {
                $scope.tempArray = [];
            };
            function getTempArray() {
                return $scope.tempArray;
            };

            $scope.formsConfig.forEach(reconstroi);

            //function reconstroi(item, index) {
            //    //  //console.log('index: ' + index + ' - linha: ' + linha);

            //    if (index < linha - 1) {
            //        pushTempArray(item);
            //    }
            //    else if (index == linha - 1) {
            //        pushTempArray(construtor.criarLinha(linha - 1, numLinha, 0));
            //        pushTempArray(construtor.recriarLinha(linha + 1, numLinha, 0, item));
            //    }
            //    else {
            //        pushTempArray(construtor.recriarLinha(linha + 1, numLinha, 0, item));
            //    }
            //};

            function reconstroi(item, index) {
                //console.log('item: ' +  JSON.stringify(item));
                //      item.ColConfigs.length
                if (index < linha - 1) {
                    pushTempArray(item);
                }
                else if (index == linha - 1) {
                    pushTempArray(construtor.criarLinha(linha - 1, numLinha, 0));
                    pushTempArray(construtor.recriarLinha(linha + 1, item.ColConfigs.length, 0, item));
                }
                else {
                    pushTempArray(construtor.recriarLinha(linha + 1, item.ColConfigs.length, 0, item));
                }
            };

            $scope.formsConfig = getTempArray().slice(0);
            zerarTempArray();
            $scope.dados2.linhaAt = $scope.formsConfig.length + 1;
        }

        $scope.tempArray2 = [];
        $scope.removerLinhaAt = function (elemento, linha) {

            var index = $scope.formsConfig.indexOf(elemento)
            $scope.formsConfig.splice(index, 1);

            function pushTempArray2(i) {
                $scope.tempArray2.push(i);
            };
            function zerarTempArray2() {
                $scope.tempArray2 = [];
            };
            function getTempArray2() {
                return $scope.tempArray2;
            };

            $scope.formsConfig.forEach(reconstroi2);

            function reconstroi2(item, index) {

                if (index <= linha) {
                    pushTempArray2(construtor.recriarLinha(index, item.ColConfigs.length, 0, item));
                }
                else {
                    pushTempArray2(construtor.recriarLinha(index - 1, item.ColConfigs.length, 0, item));
                }

                linha = linha + 1;
            };

            $scope.formsConfig = getTempArray2().slice(0);
            zerarTempArray2();
            $scope.dados2.linhaAt = $scope.formsConfig.length + 1;
        }

        $scope.gravarConfig = function () {

            var _formConfigService = abp.services.app.formConfig;

            var formConfig = {

                "Nome": $scope.formName,
                "Linhas": construtor.formataSalvarLinhas($scope.formsConfig),
                "DataAlteracao": moment().format('L LT'),
                "IsSistema": "0",
                "IsDeleted": "0"
            };

            $('#btn-save').buttonBusy(true);

            _formConfigService.criarOuEditar(formConfig)
                .done(function () {
                    abp.notify.success(app.localize('SavedSuccessfully'));
                    window.location.href = "/Mpa/GeradorFormularios/Index";
                })
                .always(function () {
                    $('#btn-save').buttonBusy(false);
                });
        }

        //Inicialização
        $http.get("/Mpa/GeradorFormularios/ObterCloneFormConfig/" + $("#CloneId").val())
            .then(function (response) {
                $scope.Id = response.data.Id;
                $scope.formName = response.data.Nome;

                //      $scope.formsConfig = response.data.Linhas;
                ////console.log(JSON.stringify(response.data.Linhas));

                for (i = 0; i < response.data.Linhas.length; i++) {

                    //console.log(JSON.stringify(response.data.Linhas[i]));

                    $scope.formsConfig.push(construtor.recriarLinha(i, response.data.Linhas[i].ColConfigs.length, response.data.Linhas[i].ColConfigs.Ordem, response.data.Linhas[i]));
                    //        pushTempArray2(construtor.recriarLinha(index, item.ColConfigs.length, 0, item));
                }

                $scope.IsSistema = response.data.IsSistema;
                $scope.IsDeleted = response.data.IsDeleted;
                $scope.dados2.linhaAt = $scope.formsConfig.length + 1;
            }, function () { });

        $scope.typeOption.push(tipos.text);
        $scope.typeOption.push(tipos.select);
        $scope.typeOption.push(tipos.radio);
        $scope.typeOption.push(tipos.email);
        $scope.typeOption.push(tipos.checkbox);
        $scope.typeOption.push(tipos.datetimesingle);
        $scope.typeOption.push(tipos.datetimerange);
        $scope.typeOption.push(tipos.range);
        $scope.typeOption.push(tipos.textarea);
        //End Inicialização
    }])

    .controller("preencherCtrl", ["$scope", "$http", "construtor", "tipos", "$timeout", function ($scope, $http, construtor, tipos, $timeout) {

        $scope.formName = "Form_1";
        $scope.formsConfig = [];

        // Campos reservados
        var camposReservados = []; // ColConfigs

        $http.get("/Mpa/GeradorFormularios/ObterFormConfigReservado")
            .then(function (response) {
                linhasFormReservado = response.data.Linhas;

                var extrairColConfigs = function (item, index) {
                    for (var i = 0; i < item.ColConfigs.length; i++) {
                        var novaCol = item.ColConfigs[i];

                        camposReservados.push(novaCol);
                    }
                }

                linhasFormReservado.forEach(extrairColConfigs);
                console.log(camposReservados);

            }, function () { });

        $scope.reservados = camposReservados;

        // Fim - reservados

        $http.get("/Mpa/GeradorFormularios/ObterFormConfig/" + $("#form-id").val()).then(function (response) {

            $scope.Id = response.data.Id;
            $scope.formName = response.data.Nome;
            $scope.formsConfig = response.data.Linhas;
            $scope.IsSistema = response.data.IsSistema;
            $scope.IsDeleted = response.data.IsDeleted;
            $scope.CreatorUserId = response.data.CreatorUserId;

            console.log('Cols:');
            console.log($scope.formsConfig.ColConfigs);

            // tratando reservados

            for (var x = 0; x < $scope.reservados.length; x++) {
                for (var y = 0; y < $scope.formsConfig.length; y++) {
                    for (var z = 0; z < $scope.formsConfig[y].ColConfigs.length; z++) {
                        if ($scope.formsConfig[y].ColConfigs[z].Name == $scope.reservados[x].Name) {
                            $scope.formsConfig[y].ColConfigs[z].MultiOption = $scope.reservados[x].MultiOption;
                        }
                    }
                }
            }

            // Fim - tratando reservados

            $timeout(function () {
                newForm();
            }, 200);
        }, function () { });

        $scope.gravarDados = function () {
            var _formRespostaService = abp.services.app.formResposta;
            var formConfig = {
                "Id": $("#form-id").val(),
                "Nome": $scope.formName,
                "Linhas": $scope.formsConfig,
                "DataAlteracao": moment().format('L LT'),
                "IsSistema": $scope.IsSistema,
                "IsDeleted": $scope.IsDeleted,
                "CreatorUserId": $scope.CreatorUserId
            };
            var nomeClasse = $('#nome-classe').val();
            var registroClasseId = $('#registro-classe-id').val();

            var _url = '/api/services/app/' + $('#nome-classe').val();

            $('#btn-save').buttonBusy(true);
            _formRespostaService.criarOuEditar(formConfig, 0, $('#nome-classe').val(), $('#registro-classe-id').val())
                .done(function (data) {
                    //     
                    ////console.log(nomeClasse);
                    if ($('#nome-classe').length > 0 && $('#nome-classe').val() != '') {
                        $.ajax({
                            url: _url + '/Obter?id=' + $('#registro-classe-id').val(),
                            type: 'POST',
                            success: function (response) {
                                var _result = response.result;
                                ////console.log(_result);
                                _result.formRespostaId = data;
                                _result.atendimento = null;
                                _result.prestador = null;
                                _result.unidadeOrganizacional = null;
                                $.ajax({
                                    type: 'POST',
                                    url: _url + '/AtualizarFormId?id=' + _result.id + '&respostaId=' + _result.formRespostaId,
                                    success: function () {
                                        abp.notify.success(app.localize('SavedSuccessfully'));
                                        //window.self.close();
                                        var iframe = parent.document.getElementById('formulario-dinamico-area-' + localStorage["AtendimentoId"]);
                                        var content = parent.document.getElementById('conteudo-modulo-' + localStorage["AtendimentoId"]);
                                        var menu = parent.document.getElementById('menu-modulo-' + localStorage["AtendimentoId"]);
                                        if (iframe) {
                                            iframe.src = '';
                                            content.classList.add('hidden');
                                            menu.classList.remove("hidden");
                                        }
                                    }
                                })
                            }
                        });
                    } else {
                        abp.notify.success(app.localize('SavedSuccessfully'));
                        location.href = '/Mpa/GeradorFormularios/ListarDados/' + $("#form-id").val();
                    }
                })
                .always(function () {
                    $('#btn-save').buttonBusy(false);
                });
        };
    }])

    .controller("editarPreenchimentoCtrl", ["$scope", "$http", "construtor", "tipos", "$timeout", function ($scope, $http, construtor, tipos, $timeout) {

        var _self = this;

        $scope.formId = 0;
        $scope.formName = "Form_1";
        $scope.formsConfig = [];

        var formAppService = abp.services.app.formConfig;

        // Atendimento
        $scope.atendimentoId = $('#atendimento-id').val();
        // Fim - Atendimento


        // Campos reservados

        // Reiterando campos com valor alterado (reservados)
        _self.reiterar = function (y, z, data) {
            //       _self.$scope.formsConfig[y].ColConfigs[z].Value = data;
            $scope.formsConfig[y].ColConfigs[z].Value = data;
            $scope.$apply();
        }
        // Fim - Reiterando campos...

        var camposReservados = []; // ColConfigs

        $http.get("/Mpa/GeradorFormularios/ObterFormConfigReservado")
            .then(function (response) {
                //     debugger
                linhasFormReservado = response.data.Linhas;

                var extrairColConfigs = function (item, index) {
                    for (var i = 0; i < item.ColConfigs.length; i++) {
                        var novaCol = item.ColConfigs[i];
                        camposReservados.push(novaCol);
                    }
                }

                linhasFormReservado.forEach(extrairColConfigs);
                console.log(camposReservados);
                $scope.reservados = camposReservados;

                $http.get("/Mpa/GeradorFormularios/ObterDados/" + $("#dados-resposta-id").val())
                    .then(function (response) {

                        $scope.formName = response.data.FormConfig.Nome;
                        $scope.formsConfig = response.data.FormConfig.Linhas;
                        $scope.formId = response.data.FormConfig.Id;
                        $scope.IsSistema = response.data.IsSistema;
                        $scope.IsDeleted = response.data.IsDeleted;
                        $scope.CreatorUserId = response.data.CreatorUserId;

                        // tratando reservados, preenchimento e 'salvarTodos'
                        for (let x = 0; x < $scope.reservados.length; x++) {
                            for (let y = 0; y < $scope.formsConfig.length; y++) {
                                for (let z = 0; z < $scope.formsConfig[y].ColConfigs.length; z++) {
                                    //      debugger
                                    if ($scope.formsConfig[y].ColConfigs[z].Name == $scope.reservados[x].Name) {
                                        $scope.formsConfig[y].ColConfigs[z].MultiOption = $scope.reservados[x].MultiOption;

                                        var preenchimento = $scope.reservados[x].Preenchimento;
                                        $scope.formsConfig[y].ColConfigs[z].Preenchimento = preenchimento;
                                        $scope.formsConfig[y].ColConfigs[z].SalvarTodos = $scope.reservados[x].SalvarTodos;

                                        // Carregar em branco
                                        if (preenchimento == 1) {
                                            $scope.formsConfig[y].ColConfigs[z].Value = '';
                                        }
                                        // Acrregar do atendimento atual
                                        else if (preenchimento == 2) {
                                            // default
                                        }
                                        // Carregar do ultimo lancamento
                                        else if (preenchimento == 3) {
                                            var colConfigName = $scope.formsConfig[y].ColConfigs[z].Name;
                                            var atendimentoId = $scope.atendimentoId;

                                            formAppService.obterValorUltimoLancamento(colConfigName, atendimentoId)

                                                .done(function (data) {
                                                    //    $scope.formsConfig[y].ColConfigs[z].Value = data;

                                                    _self.reiterar(y, z, data);

                                                });


                                        }
                                    }
                                }
                            }
                        }
                        // Fim - tratando reservados

                        $timeout(function () {
                            editForm();
                        }, 200);
                    });

            }, function () { });

        // Fim - reservados



        $scope.gravarDados = function () {
            var _formRespostaService = abp.services.app.formResposta;
            var formConfig = {
                Id: $scope.formId,
                Nome: $scope.formName,
                Linhas: $scope.formsConfig,
                "IsSistema": $scope.IsSistema,
                "IsDeleted": $scope.IsDeleted,
                "CreatorUserId": $scope.CreatorUserId
            };

            // cols
            console.log('formsConfig:');
            console.log($scope.formsConfig);

            var form = $('form[name=FormEdit]');
            var result = form.serializeFormToObject();
            ////console.log('Form: ' + form);
            ////console.log('Form Serializado: ' + JSON.stringify(result));
            var _url = '/api/services/app/' + $('#nome-classe').val();
            ////console.log(formConfig);
            $('#btn-save').buttonBusy(true);
            _formRespostaService.criarOuEditar(formConfig, $("#dados-resposta-id").val(), $('#nome-classe').val(), $('#registro-classe-id').val())
                .done(function (data) {
                    if ($('#nome-classe').length > 0 && $('#nome-classe').val() != '') {
                        $.ajax({
                            url: _url + '/Obter?id=' + $('#registro-classe-id').val(),
                            type: 'POST',
                            success: function (response) {
                                var _result = response.result;
                                ////console.log(_result);
                                _result.formRespostaId = data;
                                _result.atendimento = null;
                                _result.prestador = null;
                                _result.unidadeOrganizacional = null;
                                $.ajax({
                                    type: 'POST',
                                    url: _url + '/AtualizarFormId?id=' + _result.id + '&respostaId=' + _result.formRespostaId,
                                    success: function () {
                                        abp.notify.success(app.localize('SavedSuccessfully'));
                                        //window.self.close();
                                        var iframe = parent.document.getElementById('formulario-dinamico-area-' + localStorage["AtendimentoId"]);
                                        var content = parent.document.getElementById('conteudo-modulo-' + localStorage["AtendimentoId"]);
                                        var menu = parent.document.getElementById('menu-modulo-' + localStorage["AtendimentoId"]);
                                        if (iframe) {
                                            iframe.src = '';
                                            content.classList.add('hidden');
                                            menu.classList.remove("hidden");
                                        }
                                    }
                                })
                            }
                        });
                    } else {
                        abp.notify.success(app.localize('SavedSuccessfully'));
                        location.href = '/Mpa/GeradorFormularios/ListarDados/' + $scope.formId;
                    }
                })
                .always(function () {
                    $('#btn-save').buttonBusy(false);
                });
        }
    }])

    .controller("detalharPreenchimentoCtrl", ["$scope", "$http", "construtor", "tipos", function ($scope, $http, construtor, tipos) {
        $scope.formId = 0;
        $scope.formName = "Form_1";
        $scope.formsConfig = [];
        $scope.ReadOnly = true;

        $http.get("/Mpa/GeradorFormularios/ObterDados/" + $("#dadosRespostaId").val())
            .then(function (response) {
                $scope.formName = response.data.Nome;
                $scope.formsConfig = response.data.FormConfig.Linhas;
                $scope.formId = response.data.FormConfig.Id;
                $scope.IsSistema = response.data.IsSistema;
                $scope.IsDeleted = response.data.IsDeleted;
                $scope.CreatorUserId = response.data.CreatorUserId;
            });
    }]);

