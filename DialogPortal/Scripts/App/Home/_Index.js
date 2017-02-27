//var timeoutCounter = 5;
//var intervallId = 0;

//function onTimeout() {
//    $('#zurLoesung').val('Zur Behördenlösung');
//    if (timeoutCounter < 0) {
//        $('#zurLoesung').click();
//    } else {
//        intervallId = window.setTimeout(onTimeout, 1000);
//    }
//}

//$("#handyNummer").keyup(function (event) {
//    if (event.keyCode == 13) {
//        $("#anmelden").click();
//    }
//});

$(function () {
    $('#progress').hide();
    $('#zurLoesung').hide();
    $('#zurAdmin').hide();
    $('#responseMessage').hide();
    $('#postToUrl').hide();
    $('#datenbankDiv').hide();

    $('#zurLoesung').click(function () {
        //window.clearTimeout(intervallId);
        $('#postToUrl').click();
        return true;
    });

    $('#zurAdmin').click(function () {
        //window.clearTimeout(intervallId);
        window.location.href = "/Admin/Index";
    });

    $('#handyNummer').keydown(function () {
        $('#datenbankDiv').hide(1000);
        $('#datenbank').html("<option value='0'></option>");
    });

    $('#anmelden').click(function () {
        $('#progress').show();
        $('#responseMessage').text("");
        var data = {
            handyNummer: $('#handyNummer').val(),
            datenbankId: $('#datenbank').length == 0 ? 0 : parseInt($('#datenbank').val())
        };
        $.ajax({
            cache: false,
            url: "/Home/MIDRequest",
            type: "get",
            data: data,
            dataType: "json",
            success: function (result) {
                $('#progress').hide();
                if (result.erfolgreich) {
                    $('#token').val(result.token);
                    $('#hash').val(result.hash);
                    var $form = $('#token').closest('form');
                    $form.attr('action', result.url);
                    $('#responseMessage').text(result.responseMessage).css('color', 'green').show();
                    $('#zurLoesung').show();
                    //timeoutCounter = 5;
                    if (result.isAdmin) {
                        $('#zurAdmin').show();
                        //timeoutCounter = 10;
                    }
                    //window.setTimeout(onTimeout, 1000);
                } else {
                    if (result.datenbanken != null && result.datenbanken.length > 0) {
                        $('#datenbank').html("");
                        var i;
                        for (i = 0; i < result.datenbanken.length; i++) {
                            $('#datenbank').append("<option value='" + result.datenbanken[i].Key + "'>" + result.datenbanken[i].Value + "</option>")
                        }
                        $('#datenbankDiv').slideDown(1000);
                    }
                    $('#responseMessage').text(result.responseMessage).css('color', 'red').show();
                }
            },
            error: function (xhr) {
                //alert("ddd");
                $('#progress').hide();
                $('#responseMessage').text('Error: ' + xhr.responseText).css('color', 'red').show();
            }
        });
        return false;
    });
});
