function onUpdateTask() {
    window['gvGesTasksGridView'].UpdateEdit();
}

function onCancelTask() {
    window['gvGesTasksGridView'].CancelEdit();
}

function onUploadFile() {
    $('#ucFolderId').val($('#hidFolderId').val());
    pcUploadFile.Show();
}

function onAddGesBemerkung() {
    // Link to open the dialog
    dialogGesArtId.SetSelectedIndex(-1);
    dialogGesSachbearbeiterId.SetSelectedIndex(-1);
    $("#dialogGesDatum_I").val("");
    $("#dialogGesText").val("");
    //$("#dialog").dialog("open");
    pcGesNeueBemerkung.Show();
}

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

//function onAddBemerkung(TbAFGAufgabe_id) {
//    // Link to open the dialog
//    $("#dialog").data("TbAFGAufgabe_id", TbAFGAufgabe_id);
//    $("#dialog-text").val("");
//    $("#dialog").dialog("open");
//}

function doAddGesBemerkung() {
    var datum = $("#dialogGesDatum_I").val();
    var artId = $("#dialogGesArtId_VI").val();
    var sachbearbeiterId = $("#dialogGesSachbearbeiterId_VI").val();
    var text = $("#dialogGesText").val();
    if (text.length > 0) {
        var data = {
            datum: datum,
            artId: parseInt(artId),
            sachbearbeiterId: parseInt(sachbearbeiterId),
            bemerkung: text
        };
        $.ajax({
            url: _VD_ + "/Geschaeft/AddGesBemerkung",
            data: data,
            success: function (result) {
                if (result.length > 0) {
                    alert(result);
                } else {
                    pcGesNeueBemerkung.Hide();
                    gvGeschaeftBemerkungen.Refresh();
                }
            },
            error: function () {
                alert('error');
            }
        });
    }
}

function doAddAfgBemerkung() {
    var datum = $("#dialogAfgDatum_I").val();
    var artId = $("#dialogAfgArtId_VI").val();
    var sachbearbeiterId = $("#dialogAfgSachbearbeiterId_VI").val();
    var text = $("#dialogAfgText").val();
    if (text.length > 0) {
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
                url: _VD_ + "/Geschaeft/TaskAddBemerkung",
                data: data,
                success: function(result) {
                    if (result.length > 0) {
                        alert(result);
                    } else {
                        pcAfgNeueBemerkung.Hide();
                        var grid = window["gvBemerkungen_" + tbAufgabeid];
                        grid.Refresh();
                    }
                },
                error: function() {
                    alert('error');
                }
            });
        }
    }
}

function onAnzeigen(id) {
    $.ajax({
        url: _VD_ + "/Geschaeft/TraktandumAnzeigenViewPartial/" + id,
        cache: false,
        success: function (data) {
            $('#popupContent').html(data);
        },
        error: function (jqXHR) {
            $('#popupContent').html('Error loading data (' + jqXHR.responseText + ')');
        }
    });
        
    pcProtokoll.Show();    
    //$('#popupContent').load(_VD_ + "/Geschaeft/TraktandumAnzeigenViewPartial/" + id);
    //$('#popupContent').html("<div>CONTENIDO</div><div><b>"+id+"</b></div>");
}

function onHerunterladen(id) {
    var url = _VD_ + "/Sitzungen/GetFile/" + id;
    window.location = url;
    return false;
}

$(function () {

    $('#laden').hide(10000);

    $("#uploadFiles").fileinput({
        uploadUrl: _VD_ + "/Geschaeft/UploadFile", // server upload action
        uploadAsync: true,
        maxFileCount: 5,

        language: "de",
        //initialPreview: [],
        //overwriteInitial: false,
        //maxFileSize: 100,
        initialCaption: "Dokument auswählen",
        //previewFileType: "text",  // image
        //browseClass: "btn btn-success",
        //browseLabel: "Dokument auswählen",
        //browseIcon: '<i class="glyphicon glyphicon-picture"></i>',
        //removeClass: "btn btn-danger",
        //removeLabel: "Löschen",
        //removeIcon: '<i class="glyphicon glyphicon-trash"></i>',
        //uploadClass: "btn btn-info",
        //uploadLabel: "Hochladen",
        //uploadIcon: '<i class="glyphicon glyphicon-upload"></i>',
        //allowedFileTypes: ["image", "video"],
        allowedFileExtensions: ["doc", "docx", "pdf", "xls", "xlsx"],
        previewClass: "bg-warning"
        //previewFileType: "docx"
    });

    //$("#dialog").dialog({
    //    autoOpen: false,
    //    modal: true,
    //    width: 600,
    //    height: 350,
    //    buttons: [
    //        {
    //            text: "Speichern",
    //            click: function () {
    //                //alert('onAddBemerkung(' + TbAFGAufgabe_id + ')');
    //                var datum = $("#dialog-datum_I").val();
    //                var text = $("#dialog-text").val();
    //                var tbAufgabeid = $("#dialog").data("TbAFGAufgabe_id");
    //                if (text.length > 0) {
    //                    var data = {
    //                        TbAFGAufgabe_id: tbAufgabeid,
    //                        datum: datum,
    //                        bemerkung: text
    //                    };
    //                    $.ajax({
    //                        url: "/Geschaeft/MyTasksAddBemerkung",
    //                        data: data,
    //                        success: function (result) {
    //                            if (result.length > 0) {
    //                                alert(result);
    //                            } else {
    //                                $("#dialog").dialog("close");
    //                                var grid = window["gvBemerkungen_" + tbAufgabeid];
    //                                grid.Refresh();
    //                            }
    //                        },
    //                        error: function () {
    //                            alert('error');
    //                        }
    //                    });
    //                }
    //            }
    //        },
    //        {
    //            text: "Abbrechen",
    //            click: function () {
    //                $("#dialog").dialog("close");
    //            }
    //        }
    //    ]
    //});

});
