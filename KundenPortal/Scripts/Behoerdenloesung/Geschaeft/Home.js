function onSuchen() {
    var s = $('#suchText_I').val();
    var url = _VD_ + '/Geschaeft/Home?suchText=' + s;
    window.location.href = url;
}

$(function () {
    $('#laden').hide(5000);
});
