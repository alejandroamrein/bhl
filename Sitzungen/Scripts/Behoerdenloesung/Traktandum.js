$(function () {
    $(function() {
        $("#commentDatum").addClass("form-control");
    });
    $(document).on("click", "#commentSave", function () {
        var traktandId = $("#traktandId").val();
        var commentDatum = $("input[name='commentDatum']").val();
        var commentStatus = $('#commentStatus').val();
        var commentText = $('#commentText').text();
        var param = {
            traktandId: parseInt(traktandId),
            commentDatum: commentDatum,
            commentStatus: parseInt(commentStatus),
            commentText: commentText
        };
        $.ajax({
            url: '/Home/UpdateComment',
            type: 'get',
            data: param,
            //contentType: 'application/json; charset=utf-8',
            //dataType: 'json',
            success: function (data) {
                if (data.success) {
                    alert('ok');
                } else {
                    alert(data.error);
                }
            },
            error: function (data) {
                alert(JSON.stringify(data));
            }
        });
        return false;
    });
    $(document).on("click", ".downloadDatei", function () {
        var $tr = $(this).closest('tr');
        var id = $tr.data('id');
        //alert(id);
        var url = "/Home/GetFile/" + id;
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