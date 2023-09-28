var app = angular.module("formularioApp", [])
    .config(function ($logProvider) {
        $logProvider.debugEnabled(true);
    })
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
                result.ColConfigs.push(new self.criarCelula(linha, i, false, false))
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
                "Orientation": 'horizontal',
                "Properties": {},
                "PrependText": null,
                "AppendText": null,
                "Offset": null,
                "Size": null,
                "ColConfigReservadoId":null,
                // CAMPOS RESERVADOS - Preenchimento e SalvarTodos
                "Preenchimento": 1,
                "SalvarTodos": false // Null eh default, salva no Atendimento atual, 1 - Salva em todos os atendimentos
            };
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

        this.recriarCelula = function (colConfig, coluna, /*linha,*/ colspan, readonly) {
            if (colspan == undefined) {
                colspan = false;
            }

            if (!readonly) {
                readonly = false;
            }

            if (colConfig) {
                colConfig.Properties = self.formataProperties(colConfig)
            }
            console.log(colConfig);

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
                "ColConfigReservadoId": colConfig.ColConfigReservadoId,
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

        // Campos reservados    

        // Preenchimento
        var p1 = { value: 1, text: '1 - Campo em branco' };
        var p2 = { value: 2, text: '2 - Atendimento atual' };
        var p3 = { value: 3, text: '3 - Último lançamento' };
        var p4 = { value: 4, text: '4 - Último lançamento Atendimento Atual' };

        $scope.preenchimentos = [p1, p2, p3, p4];
        $scope.preenchimentoSelecionado = $scope.preenchimentos[0];

        var _formConfigService = abp.services.app.formConfig;
        var linhasFormReservado;
        var camposReservados = [];
        camposReservados.push({ id: 0, name: "Selecione o campo reservado" });

        $http.get("/Mpa/GeradorFormularios/ObterFormConfigReservado")
            .then(function (response) {
                linhasFormReservado = construtor.formataLinhas(response.data.Linhas);
                linhasFormReservado.forEach((item) => {
                    for (var i = 0; i < item.ColConfigs.length; i++) {
                        var novaCol = item.ColConfigs[i];
                        camposReservados.push({ id: novaCol.Id, name: novaCol.Name.toUpperCase() });
                    }
                });
                console.log(camposReservados);
                $scope.reservados = camposReservados;
            }, function () { });



        $scope.colConfigReservado = {};
        $scope.selecionadoInit = (id) => {
            return _.find($scope.reservados, (item) => item.id == id);
        };

        $scope.selecionado = (id) => {
            return _.any($scope.reservados, (item) => item.id == id);
        };

        $scope.onInit = (colConfigReservadoId) => {
            $scope.colConfigReservado = $scope.selecionadoInit(colConfigReservadoId);
        };

        $scope.editarCelulaReservada = function (celula, colConfigReservadoId) {
            var colConfigReservado = $scope.selecionadoInit(colConfigReservadoId);
            if (colConfigReservado == null || colConfigReservado.name == 'Selecione o campo reservado') {
                celula.Name = '';
                $scope.editarCelula(celula);
                return;
            }
            // ObterColConfigReservado

            $http.get("/Mpa/GeradorFormularios/ObterColConfigReservado?colDesejada=" + colConfigReservado.name)
                .then(function (response) {

                    celula.Label = response.data.Label;
                    celula.MultiOption = response.data.MultiOption;
                    celula.Name = response.data.Name;
                    celula.Placeholder = response.data.Placeholder;
                    celula.Type = response.data.Type;
                    celula.Preenchimento = response.data.Preenchimento;
                    celula.SalvarTodos = response.data.SalvarTodos;
                    celula.ColConfigReservadoId = colConfigReservado.id;
                    celula.ColConfigReservado = colConfigReservado;
                    celula.Properties = formataProperties(celula);

                    $scope.editType = celula;

                }, function () { });
        };


        $scope.montaClasses = function (totalCols, configLength, index, col) {
            var classNames = '';
            if (index != 0 && index != configLength) {
                classNames += ' col-config-column';
            }
            if (col.Offset) {
                classNames += ` col-md-offset-${col.Offset}`;
            }

            if (col.Size) {
                classNames += ` col-md-${col.Size}`;
            } else {
                classNames += ` col-md-${totalCols}`;
            }

            return classNames;
        };

        $('#fecha-modal').removeAttr("data-dismiss");
        $('#fecha-modal').on('click', function (e) {
            e.preventDefault();

            var inputsReservada = $('#editModal').find('input');
            var nameFuturo = inputsReservada.eq(2).val();
            //    console.log(nameFuturo);
            //if (camposReservados.indexOf(nameFuturo.toUpperCase()) > -1) {
            //    alert('Não é permitido criar um campo com "Name" contendo palavra reservada.');
            //} else {

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

            //if (camposReservados.indexOf(celula.Name) > -1) {
            //    alert('Não é permitido editar um campo reservado aqui.');
            //    return;
            //}

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

        var p1 = { value: 1, text: '1 - Campo em branco' };
        var p2 = { value: 2, text: '2 - Atendimento atual' };
        var p3 = { value: 3, text: '3 - Último lançamento' };
        var p4 = { value: 4, text: '4 - Último lançamento Atendimento Atual' };
        $scope.preenchimentos = [p1, p2, p3, p4];
        $scope.preenchimentoSelecionado = $scope.preenchimentos[0];

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
                .done(function () {
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
        
        $scope.fontSelecionada = function(font) {
            return $scope.fontSize === font;
        }
        
        $scope.selecionaFont = function (event) {
            $scope.fontSize = $(event.currentTarget).data("value");
        }

        $scope.preenchimentos = [p1, p2,p3, p4];
        $scope.preenchimentoSelecionado = $scope.preenchimentos[0];

        var _formConfigService = abp.services.app.formConfig;
        var linhasFormReservado;
        var camposReservados = [];
        camposReservados.push({ id: 0, name: "Selecione o campo reservado"});

        $scope.montaClasses = function (totalCols, configLength, index, col) {
            var classNames = '';
            if (index != 0 && index != configLength) {
                classNames += ' col-config-column';
            }
            if (col.Offset) {
                classNames += ` col-md-offset-${col.Offset}`;
            }

            if (col.Size) {
                classNames += ` col-md-${col.Size}`;
            } else {
                classNames += ` col-md-${totalCols}`;
            }

            return classNames;
        };

        $scope.colConfigReservado = {};
        $scope.selecionadoInit = (id) => {
            return _.find($scope.reservados, (item) => item.id == id);
        };
        
        $scope.selecionado = (id) => {
            return _.any($scope.reservados, (item) => item.id == id);
        };

        $scope.onInit = (colConfigReservadoId) => {
            $scope.colConfigReservado = $scope.selecionadoInit(colConfigReservadoId);
        };

        $scope.editarCelulaReservada = function (celula, colConfigReservadoId) {
            var colConfigReservado = $scope.selecionadoInit(colConfigReservadoId);
            if (colConfigReservado == null || colConfigReservado.name == 'Selecione o campo reservado') {
                celula.Name = '';
                $scope.editarCelula(celula);
                return;
            }
            // ObterColConfigReservado

            $http.get("/Mpa/GeradorFormularios/ObterColConfigReservado?colDesejada=" + colConfigReservado.name)
                .then(function (response) {

                    celula.Label = response.data.Label;
                    celula.MultiOption = response.data.MultiOption;
                    celula.Name = response.data.Name;
                    celula.Placeholder = response.data.Placeholder;
                    celula.Type = response.data.Type;
                    celula.Preenchimento = response.data.Preenchimento;
                    celula.SalvarTodos = response.data.SalvarTodos;
                    celula.ColConfigReservadoId = colConfigReservado.id;
                    celula.ColConfigReservado = colConfigReservado;
                    celula.Properties = formataProperties(celula);
                    $scope.editType = celula;

                }, function () { });
        };

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
            //    alert('Não é permitido editar um campo reservado aqui.');
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
            debugger;
            var _formConfigService = abp.services.app.formConfig;
            //console.log('scope: ' + $scope);

            var formConfig = {
                "Id": $scope.Id,
                "Nome": $scope.formName,
                "FontSize":$scope.fontSize,
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
                });
        };

        //Inicialização
        $http.get("/Mpa/GeradorFormularios/ObterFormConfig/" + $("#CloneId").val())
            .then(function (response) {

                $http.get("/Mpa/GeradorFormularios/ObterFormConfigReservado")
                    .then(function (responseConfigReservado) {
                        linhasFormReservado = construtor.formataLinhas(responseConfigReservado.data.Linhas);
                        linhasFormReservado.forEach((item) => {
                            for (var i = 0; i < item.ColConfigs.length; i++) {
                                var novaCol = item.ColConfigs[i];
                                camposReservados.push({ id: novaCol.Id, name: novaCol.Name.toUpperCase() });
                            }
                        });

                        //debugger;
                        $scope.Id = $("#CloneId").val();
                        $scope.formName = response.data.Nome;
                        $scope.fontSize = response.data.FontSize || "default";
                        // $scope.formsConfig = response.data.Linhas;
                        // //console.log(JSON.stringify(response.data));
                        $scope.reservados = camposReservados;
                        var linhas = construtor.formataLinhas(response.data.Linhas);
                        for (i = 0; i < linhas.length; i++) {
                            $scope.formsConfig.push(construtor.recriarLinha(linhas[i].Ordem, linhas[i].ColConfigs.length, linhas[i].Ordem, linhas[i]));
                        }

                        $scope.IsSistema = response.data.IsSistema;
                        $scope.IsDeleted = response.data.IsDeleted;
                        $scope.dados2.linhaAt = $scope.formsConfig.length + 1;


                    }, function () { });

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
        $scope.currentFormId = "Form_" + create_UUID();
        $scope.formsConfig = [];


        // Campos reservados
        var camposReservados = []; // ColConfigs
        
        setTimeout(() => {
            preencherDados();
        },0)

        // Fim - reservados
        $scope.gravarDados = function () {
            const formEl = $(`#${$scope.currentFormId}`)
            var habilitaAlteracaoLeito = formEl.find("#habilita-alteracao-leito").val()
            var leito = formEl.find("#leito-id").val()
            var leitoAtendimento = formEl.find("#atendimento-leito-id").val()

            if (habilitaAlteracaoLeito && leito != leitoAtendimento) {
                const confirmOptions = {
                    cancelButtonText: "Não, salvar sem alterar",
                    confirmButtonText: "Sim, alterar e salvar",
                    customClass: 'custom-swal-prontuario-eletronico',
                }
                abp.message.customConfirm("O Leito do atendimento mudou, você quer alterar o leito do formúlario?", "Alteração de leito", (confirm) => {
                    if (confirm) {
                        debugger
                        var nomeClasse = formEl.find('#nome-classe').val();
                        var service = abp.services.app[lowerFirstLetter(nomeClasse)]
                        if (service && service.alterarLeito) {
                            service.alterarLeito(formEl.find("#registro-classe-id").val(), leitoAtendimento, {
                                crossDomain: true,
                                xhrFields: { withCredentials: true }
                            }).done(function (data) {
                                $scope.gravarDadosAction()
                            })
                        }
                    } else {
                        $scope.gravarDadosAction()
                    }
                }, confirmOptions)
            } else {
                $scope.gravarDadosAction()
            }


            function lowerFirstLetter(string) {
                return string.charAt(0).toLowerCase() + string.slice(1);
            }

        }


        $scope.gravarDadosAction = function () {
            const formEl = $(`#${$scope.currentFormId}`)
            debugger
            var _formRespostaService = abp.services.app.formResposta;
            var _registroArquivoService = abp.services.app.registroArquivo;
            var formConfig = {
                "Id": formEl.find("#form-id").val(),
                "Nome": $scope.formName,
                "Linhas": construtor.formataSalvarLinhas($scope.formsConfig),
                "DataAlteracao": moment().format('L LT'),
                "IsSistema": $scope.IsSistema,
                "IsDeleted": $scope.IsDeleted,
                "CreatorUserId": $scope.CreatorUserId
            };

            var nomeClasse = formEl.find('#nome-classe').val();
            var registroClasseId = formEl.find('#registro-classe-id').val();

            var _url = '/api/services/app/' + nomeClasse;

            $('#btn-save').buttonBusy(true);
            _formRespostaService.criarOuEditar(formConfig, formEl.find("#dados-resposta-id").val(), nomeClasse, registroClasseId,
                {
                    crossDomain: true,
                    xhrFields: { withCredentials: true }
                })
                .done(function (data) {
                    abp.notify.success(app.localize('SavedSuccessfully'));
                    if (formEl.find('#nome-classe').length > 0 && formEl.find('#nome-classe').val() != '') {
                        $.ajax({
                            url: _url + '/Obter?id=' + formEl.find('#registro-classe-id').val(),
                            type: 'POST',
                            crossDomain: true,
                            xhrFields: { withCredentials: true },
                            success: function (response) {
                                var _result = response.result;
                                ////console.log(_result);
                                _result.formRespostaId = data;
                                _result.atendimento = null;
                                _result.prestador = null;
                                _result.unidadeOrganizacional = null;
                                $.ajax({
                                    type: 'POST',
                                    url: _url +
                                        '/AtualizarFormId?id=' +
                                        _result.id +
                                        '&respostaId=' +
                                        _result.formRespostaId,
                                    crossDomain: true,
                                    xhrFields: {
                                        withCredentials: true
                                    }
                                }).done(function () {
                                    var registroId = formEl.find('#registro-classe-id').val();
                                    _registroArquivoService.gravarImagemFormularioDinamico({
                                        registroId: registroId,
                                        operacaoId: sessionStorage["OperacaoId"],
                                        atendimentoId: localStorage["AtendimentoId"],
                                        formRespostaId: data
                                    }).done(function () {
                                        if (parent) {
                                            var iframe =
                                                $('#formulario-dinamico-area-' + localStorage["AtendimentoId"] + "-",
                                                    parent.document);
                                            var content = $('#conteudo-modulo-' + localStorage["AtendimentoId"],
                                                parent.document);
                                            var menu = $('#menu-modulo-' + localStorage["AtendimentoId"], parent.document);
                                            var button =
                                                $('#RefreshProntuariosEletronicosListButton-' +
                                                    localStorage["AtendimentoId"] +
                                                    '-' +
                                                    sessionStorage["OperacaoId"],
                                                    parent.document);

                                            if (updateTab()) {
                                                return;
                                            }

                                            if (iframe.length) {
                                                iframe.src = '';
                                                content.addClass('hidden');
                                                menu.removeClass('hidden');
                                                button.trigger('click');
                                            }

                                            var iframeContent = $("#formulario-dinamico-area22", parent.document);
                                            if (iframeContent.length) {
                                                var parentContent = iframeContent.parent();
                                                $("a[href='#" + parentContent.attr("id") + "']", parent.document).remove();
                                                parentContent.remove();
                                            }


                                        }
                                    });


                                });
                            }
                        });

                    } else {
                        abp.notify.success(app.localize('SavedSuccessfully'));
                        location.href = '/Mpa/GeradorFormularios/ListarDados/' + $("#form-id").val();
                        abp.event.trigger("CloseFormularioDinamico");
                    }


                })
                .always(function (data) {
                    $('#btn-save').buttonBusy(false);
                });
        };

        $scope.montaClasses = function (totalCols, configLength, index, col) {
            var classNames = '';
            if (index != 0 && index != configLength) {
                classNames += ' col-config-column';
            }
            if (col.Offset) {
                classNames += ` col-md-offset-${col.Offset}`;
            }

            if (col.Size) {
                classNames += ` col-md-${col.Size}`;
            } else {
                classNames += ` col-md-${totalCols}`;
            }

            return classNames;
        };

        function obterFormConfigData() {
            const formEl = $(`#${$scope.currentFormId}`)
            return {
                id: formEl.find("#form-id").val(),
                atendimentoId: formEl.find("#atendimento-id").val(),
                classeId: formEl.find("#registro-classe-id").val(),
                idResposta: formEl.find("#dados-resposta-id").val(),
            }
        }

        function preencherDados() {
            return obterCamposReservados()
                .then(obterDados)
                .then(onPreencherDadosComplete)


            function onPreencherDadosComplete() {
                formEl = $(`#${$scope.currentFormId}`)
                formEl.parents("#GeradorFormulariosTab").find(".loader").css("display", "none")
                formEl.find(".form-dinamico-content").css("display", "block")
                $timeout(function () {
                    DatesInNgForm();
                    $(".date-single-picker").mask("00/00/0000");
                }, 300);
            }
        }

        function obterCamposReservados() {
            return $http.get("/Mpa/GeradorFormularios/ObterFormConfigReservado").then(onObterCamposReservados)

            function onObterCamposReservados(response) {
                linhasFormReservado = construtor.formataLinhas(response.data.Linhas);
                var extrairColConfigs = function (item, index) {
                    for (var i = 0; i < item.ColConfigs.length; i++) {
                        var novaCol = item.ColConfigs[i];
                        camposReservados.push(novaCol);
                    }
                }

                linhasFormReservado.forEach(extrairColConfigs);
                $scope.reservados = camposReservados;

            }
        }

        function obterDados() {
            return $http.get("/Mpa/GeradorFormularios/ObterDados/" + obterFormConfigData().idResposta).then(onObterDados)

            function onObterDados(responseDados) {
                $scope.formName = responseDados.data.FormConfig.Nome;
                $scope.formsConfig = construtor.formataLinhas(responseDados.data.FormConfig.Linhas);
                $scope.formId = responseDados.data.FormConfig.Id;
                $scope.IsSistema = responseDados.data.IsSistema;
                $scope.IsDeleted = responseDados.data.IsDeleted;
                $scope.CreatorUserId = responseDados.data.CreatorUserId;
                if (!responseDados.data.IsPreenchido) {
                    return obterFormConfig()
                }

                return Promise.resolve(null)
            }
        }

        function obterFormConfig() {
            return $http.get(`/Mpa/GeradorFormularios/ObterFormConfig?${$.param(obterFormConfigData())}`).then(onObterFormConfig);

            function onObterFormConfig(responsePreenchido) {
                $scope.formsConfig = construtor.formataLinhas(responsePreenchido.data.Linhas);
                // tratando reservados
                //for (var x = 0; x < $scope.reservados.length; x++) {
                //    for (var y = 0; y < $scope.formsConfig.length; y++) {
                //        for (var z = 0; z < $scope.formsConfig[y].ColConfigs.length; z++) {
                //            if ($scope.formsConfig[y].ColConfigs[z].Name == $scope.reservados[x].Name) {
                //                $scope.formsConfig[y].ColConfigs[z].MultiOption = $scope.reservados[x].MultiOption;
                //            }
                //        }
                //    }
                //}
            }
        }
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
                linhasFormReservado = construtor.formataLinhas(response.data.Linhas);
                var extrairColConfigs = function (item, index) {
                    for (var i = 0; i < item.ColConfigs.length; i++) {
                        var novaCol = item.ColConfigs[i];
                        camposReservados.push(novaCol);
                    }
                }

                linhasFormReservado.forEach(extrairColConfigs);
                $scope.reservados = camposReservados;

                $http.get("/Mpa/GeradorFormularios/ObterDados/" + $("#dados-resposta-id").val())
                    .then(function (response) {

                        $scope.formName = response.data.FormConfig.Nome;
                        $scope.formsConfig = construtor.formataLinhas(response.data.FormConfig.Linhas);
                        debugger;
                        $scope.formId = response.data.FormConfig.Id;
                        $scope.IsSistema = response.data.IsSistema;
                        $scope.IsDeleted = response.data.IsDeleted;
                        $scope.CreatorUserId = response.data.CreatorUserId;
                        if (!response.data.IsPreenchido)
                            $http.get("/Mpa/GeradorFormularios/ObterFormConfig?id=" + $scope.formId + "&atendimentoId=" + localStorage["AtendimentoId"] + "&classeId=" + $("#registro-classe-id").val() + "&idResposta=" + $("#dados-resposta-id").val())
                                .then(function (responsePreenchido) {

                                    $scope.formsConfig = construtor.formataLinhas(responsePreenchido.data.Linhas);
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
                            });

                        $timeout(function () {
                            DatesInNgForm();
                            $(".date-single-picker").mask("00/00/0000");
                        }, 200);
                    });

            }, function () { });

        // Fim - reservados

        $scope.montaClasses = function (totalCols, configLength, index, col) {
            var classNames = '';
            if (index != 0 && index != configLength) {
                classNames += ' col-config-column';
            }
            if (col.Offset && (!_.isUndefined(col.Offset) || !_.isNull(col.Offset))) {
                classNames += ` col-md-offset-${col.Offset}`;
            }

            if (_.isUndefined(totalCols) || _.isNull(totalCols)) {
                totalCols = 12 / configLength;
            }
            if (col.Size && (!_.isUndefined(col.Size) || !_.isNull(col.Size))) {
                classNames += ` col-md-${col.Size}`;
            } else {
                classNames += ` col-md-${totalCols}`;
            }

            return classNames;
        };

        $scope.gravarDados = function () {
            var _formRespostaService = abp.services.app.formResposta;
            var _registroArquivoService = abp.services.app.registroArquivo;
            var formConfig = {
                Id: $scope.formId,
                Nome: $scope.formName,
                Linhas: construtor.formataSalvarLinhas($scope.formsConfig),
                "IsSistema": $scope.IsSistema,
                "IsDeleted": $scope.IsDeleted,
                "CreatorUserId": $scope.CreatorUserId
            };

            // cols
            //console.log('formsConfig:');
            //console.log($scope.formsConfig);

            var form = $('form[name=FormEdit]');
            var _url = '/api/services/app/' + $('#nome-classe').val();

            $('#btn-save').buttonBusy(true);
            _formRespostaService.criarOuEditar(formConfig, $("#dados-resposta-id").val(), $('#nome-classe').val(), $('#registro-classe-id').val(),
                {
                    crossDomain: true,
                    xhrFields: { withCredentials: true }
                })
                .done(function (data) {
                    var c = document.getElementById("divTable");
                    // var imagem = c.outerHTML;
                    var registroId = $('#registro-classe-id').val();
                    var formData = {
                        registroId: registroId,
                        operacaoId: sessionStorage["OperacaoId"],
                        atendimentoId: localStorage["AtendimentoId"],
                        formRespostaId: data
                    };


                    abp.notify.success(app.localize('SavedSuccessfully'));
                    if ($('#nome-classe').length > 0 && $('#nome-classe').val() != '') {
                        $.ajax({
                            url: _url + '/Obter?id=' + $('#registro-classe-id').val(),
                            type: 'POST',
                            crossDomain: true,
                            xhrFields: { withCredentials: true },
                            success: function (response) {
                                var _result = response.result;
                                ////console.log(_result);
                                _result.formRespostaId = data;
                                _result.atendimento = null;
                                _result.prestador = null;
                                _result.unidadeOrganizacional = null;
                                $.ajax({
                                    type: 'POST',
                                    url: _url +
                                        '/AtualizarFormId?id=' +
                                        _result.id +
                                        '&respostaId=' +
                                        _result.formRespostaId,
                                    crossDomain: true,
                                    xhrFields: { withCredentials: true }
                                }).done(function () {
                                    _registroArquivoService.gravarImagemFormularioDinamico({
                                        registroId: $('#registro-classe-id').val(),
                                        operacaoId: sessionStorage["OperacaoId"],
                                        atendimentoId: localStorage["AtendimentoId"],
                                        formRespostaId: data
                                    }).done(function () {
                                        if (parent) {
                                            var iframe =
                                                $('#formulario-dinamico-area-' + localStorage["AtendimentoId"] + "-",
                                                    parent.document);
                                            var content = $('#conteudo-modulo-' + localStorage["AtendimentoId"],
                                                parent.document);
                                            var menu = $('#menu-modulo-' + localStorage["AtendimentoId"],
                                                parent.document);
                                            var button =
                                                $('#RefreshProntuariosEletronicosListButton-' +
                                                    localStorage["AtendimentoId"] +
                                                    '-' +
                                                    sessionStorage["OperacaoId"],
                                                    parent.document);

                                            if (updateTab()) {
                                                return;
                                            }

                                            if (iframe.length) {

                                                iframe.src = '';
                                                content.addClass('hidden');
                                                menu.removeClass('hidden');
                                                button.trigger('click');
                                            }


                                            var iframeContent = $("#formulario-dinamico-area22", parent.document);
                                            if (iframeContent.length) {
                                                var parentContent = iframeContent.parent();
                                                $("a[href='#" + parentContent.attr("id") + "']", parent.document)
                                                    .remove();
                                                parentContent.remove();
                                            }
                                        }
                                    });
                                });
                            }
                        });
                    } else {
                        abp.notify.success(app.localize('SavedSuccessfully'));
                        location.href = '/Mpa/GeradorFormularios/ListarDados/' + $scope.formId;
                    }
                })
                .always(function (data) {

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


function updateTab() {

    var conteudo = $('#atendimento-' + sessionStorage['TargetConteudo'], parent.document);
    if (conteudo.length) {
        var href = $('#atendimento-' + sessionStorage['TargetConteudo'], parent.document).find("a").attr("href");

        conteudo.tab("show");
        parent.abp.event.trigger('reloadTab', { pagina: sessionStorage['TargetConteudo'] });
        $(href, parent.document).addClass("active");
        $("li[id^='atendimento-Prontuario-']", parent.document).each((index, item) => {
            $($(item).find("a").attr("href"), parent.document).remove();
            $(item).remove();
        });

        return true;
    }
    return false;
}

function DatesInNgForm() {
    var dateSingle = $('.date-single-picker');

    dateSingle.each(function (index) {
        var obj = $(this);
        ////console.log(obj.prop('id'));
        ////console.log(obj.val());

        var scope = angular.element($(obj)).scope();
        var objDateRange = obj.daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            //autoUpdateInput: false,
            //maxDate: new Date(),
            changeYear: true,
            //yearRange: 'c-10:c+10',
            showOn: "both",
            autoUpdateInput: false,
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR'
                    ? "DD/MM/YYYY"
                    : moment.locale().toUpperCase() === 'US'
                        ? "MM/DD/YYYY"
                        : "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Apply",
                "cancelLabel": "Clear",
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
        },
            function (start, end, label) {
                //console.log(start, end, label);
                var momentEnd = moment(end);
                var endValue = '';
                if (momentEnd.isValid()) {
                    endValue = moment(end).format('L');
                }
                obj.val(endValue);
                obj.trigger('input');
                obj.trigger('change');

                scope.$apply(function () {
                    scope.Col1.Value = endValue;
                });
            });

        if (scope.Col1.Value) {
            objDateRange.val(scope.Col1.Value);
        }
    }).on('cancel.daterangepicker', function (ev, picker) {
        obj.val('');

        scope.$apply(function () {
            scope.Col1.Value = '';
        });
    });

    var _selectedDateRange = {
        startDate: moment().add('-6', 'day').startOf('day'),
        endDate: moment().endOf('day')
    };

    var dateRange = $('.date-range-picker');
    dateRange.each(function (index) {
        var obj = $(this);
        ////console.log(obj.prop('id'));
        ////console.log(obj.val());
        var scope = angular.element($(obj)).scope();

        var objDateRange = obj.daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                //console.log(start, end, label);
                var momentStart = moment(start);
                var momentEnd = moment(end);
                var startValue = null;
                var endValue = null;

                if (momentStart.isValid()) {
                    startValue = momentStart.format('YYYY-MM-DDT00:00:00Z');;
                }

                if (momentStart.isValid()) {
                    endValue = momentEnd.format('YYYY-MM-DDT23:59:59.999Z');;
                }

                _selectedDateRange.startDate = startValue;
                _selectedDateRange.endDate = endValue;
                obj.val(startValue + ' - ' + endValue);
                obj.trigger('input');
                obj.trigger('change');

                scope.$apply(function () {
                    scope.Col1.Value = startDate + ' - ' + endValue;
                });
            });

        if (scope.Col1.Value) {
            var dates = scope.Col1.Value.split('-');
            if (dates.length) {
                var momentFirst = moment(dates[0], "YYYY-MM-DD");
                var firstValue = null;
                if (momentFirst.isValid()) {
                    firstValue = momentFirst.format("L");
                }

                var momentSecond = moment(dates[1], "YYYY-MM-DD");
                var secondValue = null;
                if (momentSecond.isValid()) {
                    secondValue = momentSecond.format("L");
                }

                objDateRange.val(firstValue + " - " + secondValue);
            } else {
                objDateRange.val(" - ");
            }
        }


        //obj.on('apply.daterangepicker', function (ev, picker) {
        //    $(this).val(picker.startDate.format('L') + ' - ' + picker.endDate.format('L'));
        //    //console.log($(this).val()).change();
        //});

    });
}