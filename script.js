var id, options;
var changed = 0;
var areas;

function success(pos) {
  var crd = pos.coords;

  var element = document.getElementById('coord');
    element.innerHTML = crd.latitude + ' ' + crd.longitude;

    var changedElement = document.getElementById('changed');
    changed = changed + 1;
    changedElement.innerHTML = changed;

    var color = "greenyellow";
    for (i = areas.length - 1; i >= 0; i--){
      item = areas[i];
      if (item['latN'] >= crd.latitude && item['longW'] <= crd.longitude && item['latS'] <= crd.latitude && item['longE'] >= crd.longitude){
        color = "red";
        break;
      }
    }

    document.body.style.backgroundColor = color;
}

function error(err) {
  console.warn('ERROR(' + err.code + '): ' + err.message);
  alert(err.message);
}

options = {
  enableHighAccuracy: true,
  timeout: 5000,
  maximumAge: 0
};

var noSleep = new NoSleep();
var toggleEl = document.querySelector("#toggle");
toggleEl.addEventListener('click', function() {
  noSleep.enable(); // keep the screen on!    
  toggleEl.style.visibility = "hidden";
  var xhttp = new XMLHttpRequest();
  xhttp.onreadystatechange = function() {
    if (this.readyState == 4 && this.status == 200) {
      areas = JSON.parse(this.responseText);
      document.body.style.backgroundColor = "greenyellow";
    }
  };
  xhttp.open('GET', 'areas.json', true);
  xhttp.send();
  id = navigator.geolocation.watchPosition(success, error, options);
}, false);

