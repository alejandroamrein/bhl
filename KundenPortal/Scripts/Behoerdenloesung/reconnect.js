//(function () {
//var timeout = 20;
//var msTimeOut = (20 * 60000) - 30000;
var msTimeOut = 60000; // jede Minute
var count = 0;
var maxInt = 1000000;
function Reconnect() {
    count++;
    var img = new Image(1, 1);
    img.src = _VD_ + '/Home/Reconnect/' + count;
    if (count >= maxInt) {
        count = 0;
    }
}
window.setInterval('Reconnect()', msTimeOut);
//})();