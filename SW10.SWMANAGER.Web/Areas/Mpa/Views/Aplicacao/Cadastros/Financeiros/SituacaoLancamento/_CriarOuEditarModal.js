(function ($) {
    app.modals.CriarOuEditarSituacaoLancamentoModal = function () {



        var _situacaoLancamentoService = abp.services.app.situacaoLancamento;

        var _modalManager;
        var _$situacaoLancamentoInformationsForm = null;


        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$situacaoLancamentoInformationsForm = _modalManager.getModal().find('form[name=situacaoLancamentoInformationsForm]');
            _$situacaoLancamentoInformationsForm.validate();


           

        };

        this.save = function () {


           
            //if (!_$FormaPagamentoInformationsForm.valid()) {
            //    return;
            //}

            var situacaoLancamento = _$situacaoLancamentoInformationsForm.serializeFormToObject();


            _modalManager.setBusy(true);
            _situacaoLancamentoService.criarOuEditar(situacaoLancamento)
                 .done(function (data) {
                     if (data.errors.length > 0) {
                         _ErrorModal.open({ erros: data.errors });
                     }
                     else {

                         abp.notify.info(app.localize('SavedSuccessfully'));
                         _modalManager.close();
                         abp.event.trigger('app.CriarOuEditarFeriadoModalSaved');
                         //location.reload();//seguindo o projeto pronto
                     }
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        //$('.minhacor').minicolors({
        //    control: $('.minhacor').attr('data-control') || 'hue',
        //    defaultValue: $('.minhacor').attr('data-defaultValue') || '',
        //    format: $('.minhacor').attr('data-format') || 'hex',
        //    keywords: $('.minhacor').attr('data-keywords') || '',
        //    inline: $('.minhacor').attr('data-inline') === 'true',
        //    letterCase: $('.minhacor').attr('data-letterCase') || 'lowercase',
        //    opacity: $('.minhacor').attr('data-opacity'),
        //    position: $('.minhacor').attr('data-position') || 'bottom left',
        //    swatches: $('.minhacor').attr('data-swatches') ? $(this).attr('data-swatches').split('|') : [],
        //    change: function (value, opacity) {
        //        if (!value) return;
        //        if (opacity) value += ', ' + opacity;
        //        if (typeof console === 'object') {
        //            //console.log(value);
        //        }
        //        swatches: $('.minhacor').addClass('edited')
        //    },
        //    theme: 'bootstrap'
        //}).addClass('edited');

        //$('.minhacor').each(function () {
        //    $(this).minicolors({
        //        control: $(this).attr('data-control') || 'hue',
        //        defaultValue: $(this).attr('data-defaultValue') || '',
        //        format: $(this).attr('data-format') || 'hex',
        //        keywords: $(this).attr('data-keywords') || '',
        //        inline: $(this).attr('data-inline') === 'true',
        //        letterCase: $(this).attr('data-letterCase') || 'lowercase',
        //        opacity: $(this).attr('data-opacity'),
        //        position: $(this).attr('data-position') || 'bottom left',
        //        swatches: $(this).attr('data-swatches') ? $(this).attr('data-swatches').split('|') : [],
        //        change: function (value, opacity) {
        //            if (!value) return;
        //            if (opacity) value += ', ' + opacity;
        //            if (typeof console === 'object') {
        //                //console.log(value);
        //            }
        //            swatches: $(this).addClass('edited')
        //        },
        //        theme: 'bootstrap'
        //    }).addClass('edited');
        //});

       

       //var setting  = {
       //     defaults: {
       //         animationSpeed: 50,
       //         animationEasing: 'swing',
       //         change: null,
       //         changeDelay: 0,
       //         control: 'hue',
       //         defaultValue: '',
       //         format: 'hex',
       //         hide: null,
       //         hideSpeed: 100,
       //         inline: false,
       //         keywords: '',
       //         letterCase: 'lowercase',
       //         opacity: false,
       //         position: 'bottom left',
       //         show: null,
       //         showSpeed: 100,
       //         theme: 'default',
       //         swatches: []
       //     }
       // };




        //$('#corLancamentoLetra').minicolors({
        //    defaults: {
        //        animationSpeed: 50,
        //        animationEasing: 'swing',
        //        change: null,
        //        changeDelay: 0,
        //        control: 'hue',
        //        defaultValue: '',
        //        format: 'hex',
        //        hide: null,
        //        hideSpeed: 100,
        //        inline: false,
        //        keywords: '',
        //        letterCase: 'lowercase',
        //        opacity: false,
        //        position: 'bottom left',
        //        show: null,
        //        showSpeed: 100,
        //        theme: 'default',
        //        swatches: []
        //    }
        //});


        //$('#corLancamentoFundo').minicolors({
        //    defaults: {
        //        animationSpeed: 50,
        //        animationEasing: 'swing',
        //        change: null,
        //        changeDelay: 0,
        //        control: 'hue',
        //        defaultValue: '',
        //        format: 'hex',
        //        hide: null,
        //        hideSpeed: 100,
        //        inline: false,
        //        keywords: '',
        //        letterCase: 'lowercase',
        //        opacity: false,
        //        position: 'bottom left',
        //        show: null,
        //        showSpeed: 100,
        //        theme: 'default',
        //        swatches: []
        //    }
        //});

        

        $('.minhacor').minicolors({
            defaults: {
                animationSpeed: 50,
                animationEasing: 'swing',
                change: null,
                changeDelay: 0,
                control: 'hue',
                defaultValue: '',
                format: 'hex',
                hide: null,
                hideSpeed: 100,
                inline: false,
                keywords: '',
                letterCase: 'lowercase',
                opacity: false,
                position: 'bottom left',
                show: null,
                showSpeed: 100,
                theme: 'default',
                swatches: []
            }
        });


    };
})(jQuery);