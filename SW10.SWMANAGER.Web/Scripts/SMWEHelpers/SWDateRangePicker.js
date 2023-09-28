var swCreateDateRangePickerOptions = function () {
    var options = {
        locale: {
            format: 'L',
            applyLabel: app.localize('Apply'),
            cancelLabel: app.localize('Cancel'),
            customRangeLabel: app.localize('CustomRange')
        },
        min: moment('1910-01-01'),
        minDate: moment('1910-01-01'),
        max: moment('2080-01-01'),
        maxDate: moment('2080-01-01'),
        ranges: {}
    };

    options.ranges[app.localize('Today')] = [moment().startOf('day'), moment().endOf('day')];
    options.ranges[app.localize('Yesterday')] = [moment().subtract(1, 'days').startOf('day'), moment().subtract(1, 'days').endOf('day')];
    options.ranges[app.localize('Last7Days')] = [moment().subtract(6, 'days').startOf('day'), moment().endOf('day')];
    options.ranges[app.localize('Last30Days')] = [moment().subtract(29, 'days').startOf('day'), moment().endOf('day')];
    options.ranges[app.localize('ThisMonth')] = [moment().startOf('month'), moment().endOf('month')];
    options.ranges[app.localize('LastMonth')] = [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')];

    return options;
};