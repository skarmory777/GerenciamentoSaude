function pageLoad() {
    try {
        var element = document.querySelector('table[id*=_fixedTable] > tbody > tr:last-child > td:last-child > div');
        if (element) {
            element.style.overflow = "visible";
        }
    } catch (e) {
        console.log(e);
    }
}