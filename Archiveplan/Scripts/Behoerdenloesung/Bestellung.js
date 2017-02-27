function bestellen() {
    var data = {};
    data.Anrede = $('#Anrede_I').val();
    data.Registraturplan = $('#Registraturplan_I').val();
    data.Name = $('#Name_I').val();
    data.Vorname = $('#Vorname_I').val();
    data.Verwaltungsname = $('#Verwaltungsname_I').val();
    data.EMail = $('#EMail_I').val();
    //var str = JSON.stringify(data);
    $.ajax({
        url: "/Home/Bestellen",
        type: "POST",
        data: "data=" + JSON.stringify(data),
        success: function (result) {
            alert(result);
        },
        error: function(xqr) {
            alert('error');
        }
    });
}
