$(function () {
    $('#progress').hide();
    $('#postToUrl').hide();
    if ($('#datenbankId').children().length == 1 && $('#datenbankId').val() == "0") {
        $('#datenbankDiv').hide();
    }

    $('#handyNummer').keydown(function () {
        $('#datenbankDiv').hide(1000);
        $('#datenbankId').html("<option value='0'></option>");
    });

    $('#zurLoesung').click(function() {
        $('#postToUrl').click();
        return true;
    });

    $('#zurAdmin').click(function() {
        window.location.href = "/Admin/Index";
    });

    $('#anmelden').click(function() {
        $('#progress').show();
        $('#responseMessage').text("");
        $('form1').submit();
    });
});