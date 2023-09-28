(function () {
	const parametrizacaoService = abp.services.app.parametrizacoes;
	const parametrizacoesIpService = abp.services.app.parametrizacoesIp;
	startLoader();
	pegaDados();
	loaderJtable();
	loadIpTable();
	$(".save-button").on("click", onSaveButton);
	
	$(".btn-save-parametrizacao-crud-ip").click(saveParametrizacaoCrudIp);


	$("#parametrizacao_crud_ip").mask('0ZZ.0ZZ.0ZZ.0ZZ', {
		translation: {
			'Z': {
				pattern: /[0-9]/, optional: true
			}
		}
	});

	
	function onSaveButton(e) {
		e.stopImmediatePropagation();
		$(this).buttonBusy(true);

		const formData = $(".parametrizacoesInformationsForm").serializeFormToObject();
		startLoader();
		parametrizacaoService.criarOuEditar(formData)
			.then(res => {
				abp.notify.success(app.localize('SavedSuccessfully'));
			})
			.always(() => {
				pegaDados();
				$(this).buttonBusy(false);
			})
	}

	function pegaDados() {
		parametrizacaoService.getParametrizacoes().then(data => {
			$("#id").val(data.id);
			$("#seguranca_habilitar_controle_de_ip").attr("checked", data.isHabilitaControleDeIp);
			$("#assistencial_habilitar_coleta_automatica").attr("checked", data.isHabilitaAssistencialColetaAutomatica);
			
			$('#parametrizacao_ip_table').jtable('load');
			stopLoader();
		});
	}

	function startLoader() {
		$(".parametrizacao-content").hide();
		$(".loader").show();
		$(".modal-footer").hide();
	}
	function stopLoader() {
		$(".loader").hide();
		$(".modal-footer").show();
		$(".parametrizacao-content").show();
	}

	function loadIpTable() {
		$(".parametrizacao_ip_table").jtable('load', null);
	}

	function saveParametrizacaoCrudIp(e) {
		const btn = $(this);
		btn.buttonBusy(true);

		parametrizacoesIpService.criarOuEditar({id:$("#parametrizacao_crud_ip_id").val(),  descricao:$("#parametrizacao_crud_ip_valor").val()}).then(res => {
			loadIpTable();
			resetParametrizacaoCrudIp();
			abp.notify.success(app.localize('SavedSuccessfully'));
		}).always(() =>{
			btn.buttonBusy(false);
		})
	}
	
	function editParametrizacaoCrudIp(row){
		$("#parametrizacao_crud_ip_id").val(row.id);
		$("#parametrizacao_crud_ip_valor").val(row.descricao);
		$("#parametrizacao_crud_ip_valor").focus();
	}
	
	function deleteParametrizacaoCrudIp (row) {
		if (row.id !== 0) {
			parametrizacoesIpService.excluir(row.id).then(() => {
				loadIpTable();
				abp.notify.success(app.localize('SuccessfullyDeleted'));
			})
		}
	}

	function resetParametrizacaoCrudIp () {
		$("#parametrizacao_crud_ip_id").val(0);
		$("#parametrizacao_crud_ip_valor").val(null);
	}
	
	function loaderJtable() {
		$(".parametrizacao_ip_table").jtable({
			title: app.localize('Ips'),
			paging: false,
			sorting: true,
			multiSorting: true,
			selecting: true,
			selectingCheckboxes: true,
			width:400,
			actions: {
				listAction: {
					method: parametrizacoesIpService.getIps
				}
			},
			fields: {
				id: { key: true, list: false },
				actions: {
					title: app.localize('Actions'),
					width: '20%',
					sorting: false,
					display: function (data) {
						var $span = $('<span></span>');
						$('<button class="btn btn-default btn-xs" type="button" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
							.appendTo($span).click(()=> {editParametrizacaoCrudIp(data.record)});

						$('<button class="btn btn-default btn-xs" type="button" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
							.appendTo($span).click(()=> deleteParametrizacaoCrudIp(data.record));
						return $span;
					}
				},
				descricao: {
					title: app.localize('Ip'),
					width: '80%',
					display: function (data) {
						return data.record.descricao;
					}
				},
			}
		})
	}
})();
