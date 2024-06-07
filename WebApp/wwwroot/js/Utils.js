/* Funcionalidad para calcular el digito de verificacion desde un nit*/
function CalcularDigitoVerificacion(myNit) {
    try {
        var vpri,
            x,
            y,
            z;

        // Se limpia el Nit
        myNit = myNit.replace(/\s/g, ""); // Espacios
        myNit = myNit.replace(/,/g, ""); // Comas
        myNit = myNit.replace(/\./g, ""); // Puntos
        myNit = myNit.replace(/-/g, ""); // Guiones

        // Se valida el nit
        if (isNaN(myNit)) {
            console.log("El nit/cédula '" + myNit + "' no es válido(a).");
            return "0";
        };

        // Procedimiento
        vpri = new Array(16);
        z = myNit.length;

        vpri[1] = 3;
        vpri[2] = 7;
        vpri[3] = 13;
        vpri[4] = 17;
        vpri[5] = 19;
        vpri[6] = 23;
        vpri[7] = 29;
        vpri[8] = 37;
        vpri[9] = 41;
        vpri[10] = 43;
        vpri[11] = 47;
        vpri[12] = 53;
        vpri[13] = 59;
        vpri[14] = 67;
        vpri[15] = 71;

        x = 0;
        y = 0;
        for (var i = 0; i < z; i++) {
            y = (myNit.substr(i, 1));
            // console.log ( y + "x" + vpri[z-i] + ":" ) ;

            x += (y * vpri[z - i]);
            // console.log ( x ) ;
        }

        y = x % 11;
        // console.log ( y ) ;

        return (y > 1) ? 11 - y : y;
    } catch (e) {
        return "0";
    }

}

function DiasEntreFechas(f1, f2) {

    var day_as_milliseconds = 86400000;
    var diff_in_millisenconds = f2 - f1;
    var diff_in_days = diff_in_millisenconds / day_as_milliseconds;
    return Math.trunc(diff_in_days);
}

/* Funcionalidad para calcular la edad desde una fecha*/
function ObtenerEdadCompleta(fromDate, toDate) {

    if (fromDate == null || fromDate == undefined)
        return { edad: 0, meses: 0, dias: 0, textoEdad: '' };

    if (!(fromDate instanceof Date)) {
        fromDate = new Date(fromDate);
    }

    var fecha_hoy = new Date();
    if (toDate != undefined) {
        if (!(toDate instanceof Date)) {
            toDate = new Date(toDate);
        }
        fecha_hoy = toDate;
    }

    var a = moment(fecha_hoy);
    var b = moment(fromDate);

    var years = a.diff(b, 'year');
    b.add(years, 'years');

    var months = a.diff(b, 'months');
    b.add(months, 'months');

    var days = a.diff(b, 'days');


    var textEdad = "";
    if (years <= 0)
        textEdad = "";
    else if (years == 1)
        textEdad = years + " año";
    else
        textEdad = years + " años";

    var textMes = "";
    if (months <= 0)
        textMes = "";
    else if (months == 1)
        textMes = months + " mes";
    else
        textMes = months + " meses";

    var textDias = "";
    if (days <= 0)
        textDias = "";
    else if (days == 1)
        textDias = days + " día";
    else
        textDias = days + " días";

    var textoTotal = textEdad + " " + textMes + " " + textDias;

    var result = { edad: years, meses: months, dias: days, textoEdad: textoTotal };
    return result;

}


/* Funcionalidad para realizar pantalla completa del navegador*/
var isFullscreen = false;
function CambiarPantallaCompleta() {
    var elem = document.documentElement;

    if (!isFullscreen) {
        if (elem.requestFullscreen) {
            elem.requestFullscreen();
        } else if (elem.mozRequestFullScreen) { /* Firefox */
            elem.mozRequestFullScreen();
        } else if (elem.webkitRequestFullscreen) { /* Chrome, Safari & Opera */
            elem.webkitRequestFullscreen();
        } else if (elem.msRequestFullscreen) { /* IE/Edge */
            elem.msRequestFullscreen();
        }
        isFullscreen = !isFullscreen;
    } else {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitExitFullscreen) {
            document.webkitExitFullscreen();
        } else if (document.msExitFullscreen) {
            document.msExitFullscreen();
        }
        isFullscreen = !isFullscreen;
    }
}

/* Borra storage de las grillas */
function BorrarStorageGrids() {
    var objstorage = Object.keys(localStorage).filter(key => key.startsWith('SiisoGridStorage'));
    for (var i = 0; i < objstorage.length; i++) {
        localStorage.removeItem(objstorage[i]);
    }
}

/* Agrega dias a una fecha */
function AgregarDiasAFecha(fecha, nroDias) {
    var result = new Date(fecha);
    result.setDate(result.getDate() + (nroDias - 1));
    return result;
}


/* Para localizacion geografica se obtiene coordenadas*/
var SiisoGeoLocalizacion = { Coordenadas: "0,0", Error: false, MensajeError: "" };
function SiisoRunGeoLocalizacion() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(SiisoSetGeoLocalizacion, SiisoSetErrorGeoLocalizacion);
    } else {
        alert("La Geolocalización no es soportada por este navegador (" + platform.description + "). Por favor use otro navegador para el correcto funcionamiento del aplicativo.");
    }
}

function SiisoSetGeoLocalizacion(position) {
    SiisoGeoLocalizacion.Coordenadas = position.coords.latitude + "," + position.coords.longitude;
}

function SiisoSetErrorGeoLocalizacion(error) {
    SiisoGeoLocalizacion.Error = true;
    switch (error.code) {
        case error.PERMISSION_DENIED:
            SiisoGeoLocalizacion.MensajeError = ("El usuario denego el permiso de ubicacion.");
            alert("El usuario denego el permiso de ubicacion. Por favor hablitarlo para el correcto funcionamiento del aplicativo.");
            break;
        case error.POSITION_UNAVAILABLE:
            SiisoGeoLocalizacion.MensajeError = ("La información de ubicación no está disponible.");
            console.log("La información de ubicación no está disponible.");
            break;
        case error.TIMEOUT:
            SiisoGeoLocalizacion.MensajeError = ("Se agotó el tiempo de espera de la solicitud para obtener la ubicación del usuario.");
            console.log("Se agotó el tiempo de espera de la solicitud para obtener la ubicación del usuario.");
            break;
        case error.UNKNOWN_ERROR:
            SiisoGeoLocalizacion.MensajeError = ("Un error desconocido ocurrió al obtener la ubicación.");
            console.log("Un error desconocido ocurrió al obtener la ubicación.");
            break;
    }
}

function SiisoGetMensajeGeoLocalizacion() {
    if (SiisoGeoLocalizacion.Error)
        return SiisoGeoLocalizacion.MensajeError;
    else
        return "https://www.google.com/maps/search/?api=1&query=" + SiisoGeoLocalizacion.Coordenadas;
}
/**********************************************************************************/

/* Funcion para verificar respuesta con el server cada 5 segundos */
var SiisoPingStorage = "SiisoPingLog";
var SiisoPingTimeOut = 2;
var isSendeingPingLog = false;
var saveLogPingServer = false;

function PingServersIfErrorTimeOut() {

    var servers = [
        (location.origin + "/GetResponseFromServer"),
        "https://pokeapi.co/api/v2/pokemon/ditto",
        "https://api.zippopotam.us/us/33162",
        "https://www.datos.gov.co/resource/xdk5-pm3f.json/?municipio=Cali"
    ];
    servers.forEach(server => {
        $.ajax({
            url: (server),
            type: 'GET',
            cache: false,
            timeout: (SiisoPingTimeOut * 1000),
            complete: function (xhr) {
                var logs = JSON.parse(localStorage.getItem(SiisoPingStorage) || "[]");
                if (xhr.status == 0) {
                    logs.push(GetLogStatusMessage(xhr, server));
                    localStorage.setItem(SiisoPingStorage, JSON.stringify(logs));
                }
                else {
                    if (logs.length >= 50 && !isSendeingPingLog && saveLogPingServer)
                        SendLogPingServer(logs);
                }
            }
        });
    });
}

function GetLogStatusMessage(xhr, uri) {
    var net = "OK";
    if (xhr.status == 0)
        net = "BAD"

    return net +
        ";" + uri +
        ";" + SiisoPingTimeOut + " seg" +
        ";" + xhr.status +
        ";" + xhr.statusText +
        ";" + moment(new Date()).format("YYYY-MM-DD;HH:mm:ss") +
        ";" + platform.description +
        ";" + SiisoGetMensajeGeoLocalizacion();

}

function SendLogPingServer(logs) {
    isSendeingPingLog = true;
    $.ajax({
        url: (location.origin + "/SaveLogFromClient"),
        type: 'POST',
        data: { logs: logs, type: 1 },
        success: function () {
            localStorage.removeItem(SiisoPingStorage);
            isSendeingPingLog = false;
        },
        error: function (xhr) {
            setTimeout(() => { SendLogPingServer(logs); }, 2000);
        }
    });
}

//setInterval(PingServersIfErrorTimeOut, 5000);
/******************************************************************/

var imageAddr = "https://http2.mlstatic.com/D_NQ_NP974268-MLA41149104135_032020-F.jpg";
var downloadSize = 53479; //bytes
var speedLimitKB = 200;
var SiisoSpeedTestStorage = "SiisoSpeedTestStorage";
var isSendingLogSpeedTest = false;
var saveLogSpeedTest = false;

function InitiateSpeedDetection() {
    var logSpeedTest = [];
    var ls = localStorage.getItem(SiisoSpeedTestStorage);
    if (!ls) {
        localStorage.setItem(SiisoSpeedTestStorage, "[]");
    } else {
        logSpeedTest = JSON.parse(localStorage.getItem(SiisoSpeedTestStorage) || "[]");
    }
    MeasureConnectionSpeed(logSpeedTest);
};

function MeasureConnectionSpeed(logSpeedTest) {
    var startTime, endTime;
    var download = new Image();
    download.onload = function () {
        endTime = (new Date()).getTime();
        showResults();
    }

    download.onerror = function (err, msg) {
        logs.push(GetLogSpeedTestMessage("Invalid image, or error downloading"));
    }

    startTime = (new Date()).getTime();
    var cacheBuster = "?nnn=" + startTime;
    download.src = imageAddr + cacheBuster;

    function showResults() {
        var duration = (endTime - startTime) / 1000;
        var bitsLoaded = downloadSize * 8;
        var speedBps = (bitsLoaded / duration).toFixed(2);
        var speedKbps = (speedBps / 1024).toFixed(2);
        if (speedKbps <= speedLimitKB)
            DevExpress.ui.notify(`Su conexion de internet esta presentando lentitud. (${speedKbps} Kbps)`, "warning", 1000);
        if (logSpeedTest.length >= 500 && !isSendingLogSpeedTest && saveLogSpeedTest)
            SendLogSpeedTestServer(logSpeedTest);
        //if (logSpeedTest.length % 10 == 0)
        //    console.info(`Tiempo promedio descarga de 0.5 MG: ${speedKbps} Kbps`);

        logSpeedTest.push(GetLogSpeedTestMessage(speedKbps));
        localStorage.setItem(SiisoSpeedTestStorage, JSON.stringify(logSpeedTest));
    }
}

function GetLogSpeedTestMessage(speed) {
    return moment(new Date()).format("YYYY-MM-DD;HH:mm:ss") +
        ";" + platform.description +
        ";" + downloadSize +
        ";" + speedLimitKB +
        ";" + speed;
}

function SendLogSpeedTestServer(logs) {
    isSendingLogSpeedTest = true;
    $.ajax({
        url: (location.origin + "/SaveLogFromClient"),
        type: 'POST',
        data: { logs: logs, type: 2 },
        success: function () {
            localStorage.removeItem(SiisoSpeedTestStorage);
            localStorage = [];
            isSendingLogSpeedTest = false;
        },
        error: function (xhr) {
            setTimeout(() => { SendLogSpeedTestServer(logs); }, 2000);
        }
    });
}

setInterval(InitiateSpeedDetection, 2000);