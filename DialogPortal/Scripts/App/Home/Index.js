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

//var _callInProgress = false;

var requestId = "";

$(function () {
    $('#progress').hide();
    $('#progress2').hide();
    $('#zurLoesung').hide();
    $('#zurAdmin').hide();
    $('#responseMessage').hide();
    $('#postToUrl').hide();
    $('#datenbankDiv').hide();

    $('#zurLoesung').click(function () {
        //window.clearTimeout(intervallId);
        $('#progress2').show();
        $('#postToUrl').click();
        return true;
    });

    $('#zurAdmin').click(function () {
        //window.clearTimeout(intervallId);
        window.location.href = "/Admin/Index";
    });

    $('#handyNummer').keydown(function () {
        $('#datenbankDiv').hide(animVelocity);
        $('#datenbank').html("<option value='0'></option>");
        $('#zurLoesung').hide(animVelocity);
        $('#zurAdmin').hide(animVelocity);
        $('#responseMessage').hide(animVelocity);
        $('#anmelden').show(animVelocity);
    });

    $('#anmelden').click(function () {
        //if (_callInProgress) {
        //    return;
        //}
        $('#progress').show(animVelocity);
        $('#responseMessage').text("");
        $('#anmelden').hide(animVelocity);

        var data = {
            requestId: requestId,
            handyNummer: $('#handyNummer').val(),
            datenbankId: $('#datenbank').length == 0 ? 0 : parseInt($('#datenbank').val())
        };
        //_callInProgress = true;
        $.ajax({
            async: true,
            timeout: 60000,
            cache: false,
            url: "/Home/MIDRequest",
            type: "get",
            data: data,
            dataType: "json",
            success: function (result) {
                if (result.status == 'selectdb') {
                    if (result.datenbanken != null && result.datenbanken.length > 0) {
                        $('#datenbank').html("");
                        var i;
                        for (i = 0; i < result.datenbanken.length; i++) {
                            $('#datenbank').append("<option value='" + result.datenbanken[i].Key + "'>" + result.datenbanken[i].Value + "</option>")
                        }
                        $('#datenbankDiv').slideDown(animVelocity);
                        $('#progress').hide(animVelocity);
                        $('#anmelden').show(animVelocity, function () {
                            $('#anmelden').focus();
                        });
                    }

                } else if (result.status == 'pending') {
                    requestId = result.requestId;
                    window.setTimeout(function () { $('#anmelden').click(); }, loginPollMs);

                } else if (result.status == "error") {
                    $('#responseMessage').text(result.responseMessage).css('color', 'red').show(animVelocity);
                    $('#progress').hide(animVelocity);

                } else if (result.status == "ready") {
                    if (result.erfolgreich) {
                        $('#token').val(result.token);
                        $('#hash').val(result.hash);
                        $('#url').val(result.url);
                        //var $form = $('#token').closest('form');
                        //$form.attr('action', result.url);
                        $('#responseMessage').text(result.responseMessage).css('color', 'green').show(animVelocity);
                        $('#zurLoesung').show(animVelocity);
                        $('#zurLoesung').focus();
                        if (result.isAdmin) {
                            $('#zurAdmin').show(animVelocity);
                        }
                    } else {
                        $('#responseMessage').text(result.responseMessage).css('color', 'red').show(animVelocity);
                    }
                    $('#progress').hide(animVelocity);
                    requestId = "";
                }
            },
            error: function (xhr) {
                //_callInProgress = false;
                $('#progress').hide(animVelocity);
                $('#responseMessage').text('Error: ' + xhr.reason).css('color', 'red').show(animVelocity);
            }
        });
        return false;
    });
});
