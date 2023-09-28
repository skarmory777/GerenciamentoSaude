const dateCellRender = (format) => {
    return (data) => {
        if (data.value !== undefined && data.value !== null) {
            const momentObj = moment(data.value)
            if (momentObj.isValid()) {
                return momentObj.format(format)
            }
        }
        return "";
    }
}

const numericCellRender = (format) => {
    return (data) => {
        debugger
        if (data.value !== undefined && data.value !== null) {
            const momentObj = numeral(data.value)
            if (momentObj.value != null) {
                return momentObj.format(format)
            }
        }
        return "";
    }
}
numeral.locale("pt-br");

const agGridHelper = {

    cellRenderer: {
        date: dateCellRender("DD/MM/YYYY HH:mm:ss"),
        float: numericCellRender("0,0.00"),
        integer: numericCellRender("0,0"),
        perc: numericCellRender("0.00%"),
        money: numericCellRender("$0,0.00"),
        dateOptions: {
            dateTime: dateCellRender("DD/MM/YYYY HH:mm:ss"),
            dateOnly: dateCellRender("DD/MM/YYYY"),
            dateCustom: dateCellRender
        },
        floatOptions: {
            umaCasas: numericCellRender("0,0.0"),
            duasCasas: numericCellRender("0,0.00"),
            tresCasas: numericCellRender("0,0.000"),
            quatroCasas: numericCellRender("0,0.0000"),
            cincoCasas: numericCellRender("0,0.00000"),
        },
        percOptions: {
            umaCasas: numericCellRender("0.0%"),
            duasCasas: numericCellRender("0.00%"),
            tresCasas: numericCellRender("0.000%"),
            quatroCasas: numericCellRender("0.0000%"),
            cincoCasas: numericCellRender("0.00000%"),
        },
        moneyOptions: {
            tresCasas: numericCellRender("$0,0.000"),
            quatroCasas: numericCellRender("$0,0.0000"),
            cincoCasas: numericCellRender("$0,0.00000")
        }
    }
}