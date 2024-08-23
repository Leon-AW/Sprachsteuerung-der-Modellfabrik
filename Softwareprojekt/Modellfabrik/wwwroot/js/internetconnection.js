/**
 * @summary Checkt die Internetconnection
 */
function checkInternetConnection() {
    window.$.ajax({
        type: "POST",
        url: "/Connection/CheckInternetConnection",
        contentType: "application/json",
        dataType: "json",
        success: function (valid) {
            handleInternetConnection(valid);
        }
    });
}

/**
 * @summary Führt Schritte nach Überprüfung der Verbindung zum Internet aus
 * @param {any} valid
 */
function handleInternetConnection(valid) {
    internetConnectStatus(valid);
    if (valid) {                        //Wenn Internet da...
        showSwalInternetConnect();      //success message zeigen
        if (sessionStorage.getItem("broker") === null) { //Am Anfang ist broker null, d.h. das wird nur einmal ausgeführt
            tryConnectToBroker(); //zum Broker verbinden
        }
        sessionStorage.setItem("internet", true); //internet auf true setzen
    } else {
        noInternet(); //sonst kein Internet
    }
}

/**
 * @summary Wird ausgeführt wenn keine Verbindung zum Internet besteht
 */
function noInternet() {
    if (sessionStorage.getItem("internet") === "true") { //Wenn vorher das Internet da war (sonst würde es immer im Loop den alert zeigen)
        disconnectBrokerWhenConnected();
        sessionStorage.setItem("broker", false);
        sessionStorage.setItem("internet", false);
        loadSettings();
        showSwalInternetDisconnect();
    }
};

/**
 * @summary wird gezeigt wenn Internet disconnected ist
 */
function showSwalInternetDisconnect() {
    window.Swal.fire({
        icon: 'error',
        title: 'Es besteht keine Internetverbindung.',
        text: 'Bitte überprüfen Sie Netzwerkkabel, Modem und Router oder stellen Sie die Verbindung zum Wlan erneut her!',
        confirmButtonColor: '#1D2865'
    }).then((result) => {
        if (result.isConfirmed) {
            brokerConnectStatus();
            showSwalBrokerDisconnect(getBrokerIp());
        }
    });
}

/**
 * @summary wird gezeigt wenn wieder eine Verbindung zum Internet besteht
 */
function showSwalInternetConnect() {
    if (sessionStorage.getItem("internet") === "false") {
        window.Swal.fire({
            icon: 'success',
            title: 'Es besteht eine Internetverbindung.',
            confirmButtonColor: '#1D2865'
        });
    }
}