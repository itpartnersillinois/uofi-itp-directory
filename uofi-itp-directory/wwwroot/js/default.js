function alertOnScreen(msg) {
    var alertItem = document.querySelector('#update-alert');
    alertItem.innerHTML = msg;
    alertItem.classList.remove('hide');
    alertItem.classList.remove('fadeout');
    setInterval(function () { alertItem.classList.add('fadeout'); }, 2000);
    return true;
}