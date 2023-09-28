/* Here, there are some custom plug-ins.
 * Developed for ASP.NET Iteration Zero (http://aspnetzero.com). */
(function ($) {
    if (!$) {
        return;
    }

    /* A simple jQuery plug-in to make a button busy. */
    $.fn.buttonBusy = function (isBusy) {
        return $(this).each(function () {
            var $button = $(this);
            var $icon = $button.find('i');
            var $buttonInnerSpan = $button.find('span');

            if (isBusy) {
                if ($button.hasClass('button-busy')) {
                    return;
                }

                $button.attr('disabled', 'disabled');

                //change icon
                if ($icon.length) {
                    $button.data('iconOriginalClasses', $icon.attr('class'));
                    $icon.removeClass();
                    $icon.addClass('fa fa-spin fa-spinner');
                }

                //change text
                if ($buttonInnerSpan.length && $button.attr('busy-text')) {
                    $button.data('buttonOriginalText', $buttonInnerSpan.html());
                    $buttonInnerSpan.html($button.attr('busy-text'));
                }

                $button.addClass('button-busy');
            } else {
                if (!$button.hasClass('button-busy')) {
                    return;
                }
                
                //enable button
                $button.removeAttr('disabled');

                //restore icon
                if ($icon.length && $button.data('iconOriginalClasses')) {
                    $icon.removeClass();
                    $icon.addClass($button.data('iconOriginalClasses'));
                }

                //restore text
                if ($buttonInnerSpan.length && $button.data('buttonOriginalText')) {
                    $buttonInnerSpan.html($button.data('buttonOriginalText'));
                }

                $button.removeClass('button-busy');
            }
        });
    };

    $.fn.serializeFormToObject = function() {
        //serialize to array
        var data = $(this).serializeArray();

        //add also disabled items
        $(':disabled[name]', this)
            .each(function() {
                //data.push({ name: this.name, value: $.trim(removerAcentos($(this).val())) });

                //var item = { name: this.name, value: $.trim($(this).val()) };
                //if ($(this).data("mask"))
                //{
                //    item.value = $.trim($(this).cleanVal());
                //}
                data.push({ name: this.name, value: $.trim($(this).val()) });
            });

        //map to object
        var obj = {};
        //data.map(function (x) { obj[x.name] = $.trim(removerAcentos(x.value)); });
        data.map(function (x) { obj[x.name] = $.trim(x.value); });

        return obj;
    };

    //$.fn.serializeArray = function () {
    //    /// <summary>
    //    ///     Encode a set of form elements as an array of names and values.
    //    /// </summary>
    //    /// <returns type="Array" />
    //    var rselectTextarea = /select|textarea/i;
    //    var rinput = /text|hidden|password|search/i;
    //    var rCRLF = /\r?\n/g;

    //    return this.map(function () {
    //        return this.elements ? jQuery.makeArray(this.elements) : this;
    //    })
	//	.filter(function () {
	//	    return this.name && !this.disabled &&
	//			(this.checked || rselectTextarea.test(this.nodeName) ||
	//				rinput.test(this.type));
	//	})
	//	.map(function (i, elem) {
	//	    var val = null;
	//	    if (jQuery(this).data("mask"))
	//	    {
	//	        debugger;
	//	        val = jQuery.trim(jQuery(this).cleanVal());
	//	    }
	//	    else {
	//	        val = jQuery(this).val();
	//	    }

	//	    return val == null ?
	//			null :
	//			jQuery.isArray(val) ?
	//				jQuery.map(val, function (val, i) {
	//				    return { name: elem.name, value: val.replace(rCRLF, "\r\n") };
	//				}) :
	//				{ name: elem.name, value: val.replace(rCRLF, "\r\n") };
	//	}).get();
    //};

})(jQuery);