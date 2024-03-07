function alertOnScreen(msg) {
    var alertItem = document.querySelector('#update-alert');
    var alertItemMessage = document.querySelector('#update-alert-message');
    alertItemMessage.innerHTML = msg;
    alertItem.className = 'fadein';
    return true;
}

function removeAlertOnScreen() {
    var alertItem = document.querySelector('#update-alert');
    alertItem.className = 'fadeout';
    setTimeout(function () { alertItem.className = ''; }, 1000);
    return true;
}