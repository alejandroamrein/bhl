$(function () {
    $("#weiterButton").click(function () {
        window.location.href = $("#weiterButton").attr("href");
    });
    window.setTimeout(function () {
        $("#weiterButton").click();
    }, 5000);
});
