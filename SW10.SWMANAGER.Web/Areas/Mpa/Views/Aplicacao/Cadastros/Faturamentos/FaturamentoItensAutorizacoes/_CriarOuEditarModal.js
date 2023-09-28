
(function ($) {
    $(function () {

        $(document).ready(function () {

            CamposRequeridos();
        });

        $('.modal-dialog').css('width', '1800px');

        $('#dataAutorizacaoId').on('load', function () {
            var d = new Date();
            var n = d.getDate();
            $('#DataAutorizacao').val(moment().format("L LT"));
        });

        //var _permissions = {
        //    create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Create'),
        //    edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Edit'),
        //    'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Delete')
        //};

        var _faturamentoItemAutorizacao = abp.services.app.faturamentoItemAutorizacao;

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });



        $('.close').on('click', function () {
            location.href = '/mpa/FaturamentoItensAutorizacoes';
        });

        $('.close-button').on('click', function () {
            location.href = '/mpa/FaturamentoItensAutorizacoes';
        });


        //$('#btnImprimir').on('click', function (e)
        //{
        //    _imprimirEntrada.open({ preMovimentoId: $('#id').val() });
        //});


        selectSW('.selectConvenio', "/api/services/app/convenio/ListarDropdown");
        selectSW('.selecFaturamentoGrupo', "/api/services/app/FaturamentoGrupo/ListarDropdown");
        selectSW('.selecFaturamentoSubGrupo', "/api/services/app/FaturamentoSubGrupo/ListarDropdown", $('#faturamentoGrupoId'));
        selectSWMultiplosFiltros('.selectFaturamentoItem', "/api/services/app/FaturamentoItem/ListarFaturamentoItemPorGrupoSubGrupoDropdown", ['faturamentoGrupoId', 'faturamentoSubGrupoId']);


        $('#faturamentoGrupoId').on("change", function () {
            selectSW('.selecFaturamentoSubGrupo', "/api/services/app/FaturamentoSubGrupo/ListarDropdown", $('#faturamentoGrupoId'));
            selectSWMultiplosFiltros('.selectFaturamentoItem', "/api/services/app/FaturamentoItem/ListarFaturamentoItemPorGrupoSubGrupoDropdown", ['faturamentoGrupoId', 'faturamentoSubGrupoId']);
        });


        $('#faturamentoSubGrupoId').on("change", function () {
            selectSWMultiplosFiltros('.selectFaturamentoItem', "/api/services/app/FaturamentoItem/ListarFaturamentoItemPorGrupoSubGrupoDropdown", ['faturamentoGrupoId', 'faturamentoSubGrupoId']);
        });



        $('#salvar').click(function (e) {
            e.preventDefault();

            var _$autorizacaoInformationsFormForm = $('form[name=autorizacaoInformationsForm');

            _$autorizacaoInformationsFormForm.validate();

            if (!_$autorizacaoInformationsFormForm.valid()) {
                return;
            }

            var faturamenetoItemAutorizacao = _$autorizacaoInformationsFormForm.serializeFormToObject();

            _faturamentoItemAutorizacao.criarOuEditar(faturamenetoItemAutorizacao)
            .done(function (data) {

                if (data.errors.length > 0) {
                    _ErrorModal.open({ erros: data.errors });
                }
                else {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    location.href = '/mpa/FaturamentoItensAutorizacoes';
                }
            })
        .always(function () {
            //  _modalManager.setBusy(false);
        });


        });

    });

})(jQuery);