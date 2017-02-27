$(function () {

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-left",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    //$("#speichern").hide();

    $("#kommentarHead").click(function () {
        $("#kommentarTable").toggle(1000);
    });

    $("#commentDatum").addClass("form-control");

    $(document).on("click", "#commentSave", function () {
        //$("#speichern").show();

        var traktandId = $("#traktandId").val();
        var commentDatum = $("input[name='commentDatum']").val();
        var commentStatus = $('#commentStatus').val();
        //var commentText = $('#commentText').text(); // IE
        //alert(commentText);
        var commentText = $('#commentText').val(); // IE Firefox Chrome Safari
        //alert(commentValue);
        //var commentVertraulich = $('#commentVertraulich').is(":checked");
        var commentVertraulich = $('#commentVertraulich').val();
        var param = {
            traktandId: parseInt(traktandId),
            commentDatum: commentDatum,
            commentStatus: parseInt(commentStatus),
            commentText: commentText,
            commentVertraulich: commentVertraulich
        };
        $.ajax({
            url: _VD_ + '/Sitzungen/UpdateComment',
            type: 'get',
            data: param,
            //contentType: 'application/json; charset=utf-8',
            //dataType: 'json',
            success: function (data) {
                //$("#speichern").hide(3000);
                if (data.success) {
                    toastr["success"]("Kommentar erfolgreich gespeichert", "Speichern");
                    $('#bemerkungDiv').addClass('hat-bemerkung');
                    // alert('ok');
                    //$("<tr><td>" + data.stellungNahmeDatum + "</td><td>" + data.stellungNahmeUser + "</td><td>" + data.bemerkungen + "</td><td><input type='checkbox' " + (data.vertraulich ? "checked='checked'" : "") + " /></td><td>" + data.status + "</td></tr>").appendTo("#kommentarTable");
                } else {
                    toastr["error"]("Kommentar wurde nicht gespeichert (" + data.error + ")", "Speichern");
                    //alert(data.error);
                }
            },
            error: function (data) {
                toastr["error"]("Kommentar wurde nicht gespeichert", "Speichern");
                //$("#speichern").hide(3000);
                //alert(JSON.stringify(data));
            }
        });
        return false;
    });
    $(document).on("click", ".downloadDatei", function () {
        var $tr = $(this).closest('tr');
        var id = $tr.data('id');
        //alert(id);
        var url = _VD_ + "/Sitzungen/GetFile/" + id;
        window.location = url;
        return false;
    });

    //$(document).on("click", "#loadItemsButton", function () {
    //    myViewModel.loadItems();
    //    return false;
    //});
    //$(document).on("click", ".dialog-item", function () {
    //    var id = $(this).attr("data-id");
    //    myViewModel.loadItem(id);
    //    return false;
    //});

    //var myViewModel = {
    //    saveComment: function (id) {
    //        $.ajax({
    //            url: '/api/Administration/' + id,
    //            type: 'put',
    //            data: '=' + myViewModel.currentItem.Titel(),
    //            //contentType: 'application/json; charset=utf-8',
    //            //dataType: 'json',
    //            success: function (data) {
    //                alert('ok');
    //            },
    //            error: function (data) {
    //                alert(JSON.stringify(data));
    //            }
    //        });
    //    },
    //    loadItems: function () {
    //        $.ajax({
    //            url: '/api/Administration',
    //            type: 'get',
    //            success: function (data) {
    //                myViewModel.items(data);
    //            },
    //            error: function (data) {
    //                alert('err');
    //            }
    //        });
    //    },
    //    loadItem: function (id) {
    //        $.ajax({
    //            url: '/api/Administration/' + id,
    //            type: 'get',
    //            success: function (data) {
    //                myViewModel.currentItem.TraktandId(data.TraktandId);
    //                myViewModel.currentItem.Stellungnahme(data.Stellungnahme);
    //                myViewModel.currentItem.Titel(data.Titel);
    //                myViewModel.currentItem.Datum(data.Datum);
    //                myViewModel.currentItem.Status(data.Status);
    //            },
    //            error: function (data) {
    //                alert('err');
    //            }
    //        });
    //    },
    //    currentItem: {
    //        TraktandId: ko.observable(0),
    //        Stellungnahme: ko.observable('Wird geladen...'),
    //        Titel: ko.observable('Wird geladen...'),
    //        Datum: ko.observable('Wird geladen...'),
    //        Status: ko.observable('Wird geladen...')
    //},
    //    items: ko.observable([
    //        { TraktandId: 1, Titel: "Wird geladen..." }
    //    ])
    //};
    //ko.applyBindings(myViewModel);
    //myViewModel.loadItems();
});