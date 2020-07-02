var id, options;
var areas;

function success(pos) {
  var crd = pos.coords;

  var element = document.getElementById('velocity');
    element.innerHTML = Math.round(crd.speed * 3.6) + ' km/h';

    var color = "greenyellow";
    var maxSpeed = "";
    for (i = areas.length - 1; i >= 0; i--){
      item = areas[i];
      if (item['latN'] >= crd.latitude && item['longW'] <= crd.longitude && item['latS'] <= crd.latitude && item['longE'] >= crd.longitude){
        color = "red";
        if (item['MaxSpeed'] != null){
          maxSpeed = item['MaxSpeed'];
        }

        if (document.body.style.backgroundColor != "red"){
          document.getElementById('alarm-sound').play();
        }
        
        break;
      }
    }

    document.body.style.backgroundColor = color;
    document.getElementById('maxspeed').innerHTML = maxSpeed;
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
  document.getElementById('alarm-sound').load();
  if( document.getElementById('alarm-sound') ){
    document.getElementById('alarm-sound').pause();
  }
  noSleep = new NoSleep();
  noSleep.enable(); // keep the screen on!    
  toggleEl.style.visibility = "hidden";
  if (areas == null){
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function() {
      if (this.readyState == 4 && this.status == 200) {
        areas = JSON.parse(this.responseText);
        document.body.style.backgroundColor = "greenyellow";
      }
    };
    xhttp.open('GET', 'areas.json', true);
    xhttp.send();
  }
  
  id = navigator.geolocation.watchPosition(success, error, options);
}, false);

window.onblur = function(){
  document.body.style.backgroundColor = "yellow";
  toggleEl.style.visibility = "visible";
  noSleep.disable();
  navigator.geolocation.clearWatch(id);
}

