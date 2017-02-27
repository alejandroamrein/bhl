function onAddAfgBemerkung(TbAFGAufgabe_id) {
    // Link to open the dialog
    dialogAfgArtId.SetSelectedIndex(-1);
    dialogAfgSachbearbeiterId.SetSelectedIndex(-1);
    $("#dialogAfgDatum_I").val("");
    $("#dialogAfgText").val("");
    $("#dialogAfgId").val(TbAFGAufgabe_id);
    //$("#dialog").dialog("open");
    pcAfgNeueBemerkung.Show();
}

function doAddAfgBemerkung() {
    var datum = $("#dialogAfgDatum_I").val();
    var artId = $("#dialogAfgArtId_VI").val();
    var sachbearbeiterId = $("#dialogAfgSachbearbeiterId_VI").val();
    var text = $("#dialogAfgText").val();
    if (text.length > 0) {
        var datum = $("#dialogAfgDatum_I").val();
        var text = $("#dialogAfgText").val();
        var tbAufgabeid = $("#dialogAfgId").val();
        if (text.length > 0) {
            var data = {
                TbAFGAufgabe_id: tbAufgabeid,
                artId: parseInt(artId),
                sachbearbeiterId: parseInt(sachbearbeiterId),
                datum: datum,
                bemerkung: text
            };
            $.ajax({
                url: _VD_ + "/Tasks/MyTasksAddBemerkung",
                data: data,
                success: function (result) {
                    if (result.length > 0) {
                        alert(result);
                    } else {
                        pcAfgNeueBemerkung.Hide();
                        var grid = window["gvBemerkungen_" + tbAufgabeid];
                        grid.Refresh();
                    }
                },
                error: function () {
                    alert('error');
                }
            });
        }
    }
}

function onAktualisieren() {
    var s = $('#filter_VI').val();
    var url = _VD_ + '/Tasks/Home?filter=' + s;
    window.location.href = url;
}