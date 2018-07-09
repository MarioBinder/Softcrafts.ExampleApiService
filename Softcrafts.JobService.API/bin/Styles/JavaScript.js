var logo = document.getElementById('logo');
logo.innerHTML = '<a href="https://semas-upg.wolff-dmm.de/APIs/">SEMAS Web APIs</a>';

var list = document.getElementsByClassName('collapseResource');
for (var i = 0; i < list.length; i++) {
    list[i].click();
}


