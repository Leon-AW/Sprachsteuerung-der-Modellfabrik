/**
 * @summary checkt alle 5 Sekunden die Verbindung von Internet und bedingt auch Broker
 */
async function connection() {
    checkInternetConnection();
    if (sessionStorage.getItem("internet") === "true" && sessionStorage.getItem("tryConnect") === false) { //Wenn Internet da ist und sich grade nicht versucht wird zu connecten
        checkBrokerConnection(getBrokerIp()); //dann Broker Connection checken
    }
    await delay(5); //5 Sekunden warten
    connection();
}

/**
 * @summary Versucht sich zum Broker zu connecten
 */
async function tryConnectToBroker() {
    sessionStorage.setItem("tryConnect", true); //tryConnect wird "belegt"
    const ip = getBrokerIp(); //ip wird ermittelt

    disconnectBrokerWhenConnected();

    await delay(0.5); //0,5s warten

    connectToBroker(ip);

    await delay(2); //2s warten

    checkBrokerConnection(ip); //checkt ob er sich verbinden konnte

    sessionStorage.setItem("tryConnect", false); //tryConnect wird wieder freigegeben
}

/**
 * @summary es gibt nur 2 IPs - Testbroker Mosquitto und Modellfabrik
 * @return ip des Brokers
 */
function getBrokerIp() {
    var ip;
    if (sessionStorage.getItem("testbroker") === "true") {
        ip = "test.mosquitto.org";
    } else {
        ip = "192.168.0.10";
    }

    return ip;
}


/**
 * @summary Disconnected den Broker wenn er verbunden ist (ConnectionController)
 */
function disconnectBrokerWhenConnected() {
    if (sessionStorage.getItem("broker") === "true") {
        window.$.ajax({
            type: "POST",
            url: "/Connection/DisconnectBroker",
            dataType: "json"
        });
    }
}

/**
 * @summary  Verbindet sich zum Broker (ConnectionController)
 * @param {string} ip Broker IP
 */
function connectToBroker(ip) {
    window.$.ajax({
        type: "POST",
        url: "/Connection/ConnectToBroker",
        data: { brokerIp: ip },
        dataType: "json"
    });
}

/**
 * @summary Wenn man sich das erste mal mit dem Mosquitto-Broker verbindet dann werden Testdaten gepublisht
 * @param {string} ip Broker IP
 */
function testPublish(ip) {
    if (ip === "test.mosquitto.org" && sessionStorage.getItem("firstPub") === null && sessionStorage.getItem("broker") === "true") {
        sessionStorage.setItem("firstPub", true);
        window.$.ajax({
            type: "POST",
            url: "/Connection/TestPublish",
            dataType: "json"
        });
    }
}

/**
 * @summary  Prüft ob es eine Verbindung zu einem Broker gibt
 * @param {string} ip Broker IP
 */
function checkBrokerConnection(ip) {
    window.$.ajax({
        type: "POST",
        url: "/Connection/IsConnectedToBroker",
        contentType: "application/json",
        dataType: "json",
        success: function (valid) {
            handleBrokerConnection(valid, ip); //Wenn die Methode aus dem ConnectionController erfolgreich ausgeführt wurde dann wird handleBrokerConnection aufgerufen
        }
    });
}

/**
 * @summary Führt Schritte nach Überprüfung der Verbindung zum Broker aus
 * @param {boolean} connected Ob der Broker connected ist oder nicht
 * @param {string} ip Broker IP
 */
function handleBrokerConnection(connected, ip) {
    changeConnectionState(connected, ip);
    if (connected) { //wenn connected
        showSwalBrokerConnect(ip); //zeige success message
    } else {
        showSwalBrokerDisconnect(ip); //zeige error message
    }
    window.$("#cover").fadeOut(100); //Ladescreenausblenden falls einer da ist
}

/**
 * @summary zeigt success message
 * @param {string} ip Broker IP
 */
function showSwalBrokerConnect(ip) {
    window.Swal.fire({
        icon: 'success',
        title: 'Erfolgreich zum Broker ' + ip + ' verbunden!',
        confirmButtonColor: '#1D2865'
    });
    brokerConnectStatus(); //visual.js --> StatusText oben rechts
}

/**
 * @summary zeigt error message
 * @param {string} ip Broker IP
 */
function showSwalBrokerDisconnect(ip) {
    window.Swal.fire({
        icon: 'error',
        title: 'Konnte sich nicht zum Broker ' + ip + ' verbinden!',
        text: 'Broker oder Einstellungen prüfen!',
        confirmButtonColor: '#1D2865'
    });
}

/**
 * @summary Wenn der Connection-Status des Brokers sich ändert werden entsprechende Items gespeichert und die GUI angepasst
 * @param {boolean} connected Ob connected oder nicht
 * @param {string} ip Broker IP
 */
function changeConnectionState(connected, ip) {
    if (String(connected) !== sessionStorage.getItem("broker")) { //Wenn aktueller Connectstatus ungleich vorherigem
        sessionStorage.setItem("broker", connected); //Item (connected) wird auf true oder false gesetzt, je nach dem ob eine Verbindung besteht
        try { ReconnectButton.disabled = connected; } catch (e) {/*Nichts soll passieren*/ } //Für den Fall, dass auf einer anderen Seite als der Settingseite ausgeführt wird
        loadSettings(); //settings anwenden
        testPublish(ip); //Wenn noch keine Testdaten an mosquitto gesendet wurden
    }
}

/**
 * Wenn eine Verbindung zu Internet und Broker besteht dann wird true zurückgegeben
 * @return Broker und Internet verbunden, dann true
 */
function fullConnected() {
    return sessionStorage.getItem("broker") === "true" && sessionStorage.getItem("internet") === "true";
}