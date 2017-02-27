function Logout() {
    alert("Zeit ist abgelaufen");
    document.location.href = _VD_ + "/Home/LogOff";
}
window.setInterval('Logout()', _TIMEOUT_);