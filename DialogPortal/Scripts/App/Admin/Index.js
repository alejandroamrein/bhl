$(function () {
    $("#form1").on("click", "button[class^='module']", function () {
        var $button = $(this);
        if ($button.hasClass('btn-danger')) {
            $button.addClass('btn-success');
            $button.removeClass('btn-danger');
        } else {
            $button.removeClass('btn-success');
            $button.addClass('btn-danger');
        }
        var $tr = $button.closest("tr");
        if ($tr.hasClass('unchanged')) {
            $tr.removeClass('unchanged');
            $tr.addClass('modified');
        }
        return false;
    });
    $("#addUser").click(function() {
        var str = "<tr class='added'>" +
            "<td><input class='handyNummer' type='text' style='width:100%' value='' /></td>" +
            "<td><input class='shortName' type='text' style='width:100%' value='' /></td>" +
            "<td><input class='vorname' type='text' style='width:100%' value='' /></td>" +
            "<td><input class='name' type='text' style='width:100%' value='' /></td>" +
            "<td>" + page.mandantModuleButtons + "</td>" +
            "<td><input class='admin' type='checkbox' class='admin' /></td>" +
            "<td><input class='block' type='checkbox' class='block' /></td>" +
            "<td><div title='Benutzer löschen' class='btn btn-default delete'><div class='glyphicon glyphicon-trash'></div></div></td>" +
            "</tr>";
        $(str).appendTo("#users");
    });
    $("#send").click(function() {
        var url = "/Admin/Send";
        var strData = formDataAsString();
        if (strData == null) {
            alert('Es gibt keine Änderungen. Es wurde kein Antrag erstellt.');
        }
        else {
            var data = {
                formData: strData
            };
            $.ajax({
                url: url,
                type: 'post',
                data: data,
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",  // Windows-1252   UTF-8   ISO-8859-1
                success: function (result) {
                    if (result.success) {
                        alert("Antrag-Nummer: " + result.antragNummer + "\nSie werden demnächst kontaktiert.");
                    } else {
                        alert("Fehler: " + result.errorMessage);
                    }
                },
                error: function () {
                    alert("Fehler bei der Übertragung des Antrages");
                }
            });
        }
    });
    $(document).on("click", ".delete", "", function() {
        var $button = $(this);
        var $td = $button.parent();
        var $tr = $td.parent();
        if ($tr.hasClass('added')) {
            $tr.remove();
        } else {
            $tr.addClass('deleted');
            $tr.hide(1000);
        }
    });
    $(document).on("change", "#users tbody tr input", "", function() {
        var $input = $(this);
        var $td = $input.parent();
        var $tr = $td.parent();
        if ($tr.hasClass('unchanged')) {
            $tr.removeClass('unchanged');
            $tr.addClass('modified');
        }
        if ($input.hasClass('handyNummer') && ($tr.hasClass('added') || $tr.hasClass('duplicated'))) {
            //alert($input.val());
            $tr.removeClass('duplicated');
            $tr.addClass('added');
            $('tr.unchanged,tr.modified', $tr.parent()).each(function () {
                var nummer = $('.handyNummer', $(this)).val();
                if (nummer == $input.val()) {
                    alert('Diese Nummer ist bereits vorhanden. Dieser Antrag wird nicht gespeichert.');
                    $tr.addClass('duplicated');
                    $tr.removeClass('added');
                }
            });
        }
    });
});
