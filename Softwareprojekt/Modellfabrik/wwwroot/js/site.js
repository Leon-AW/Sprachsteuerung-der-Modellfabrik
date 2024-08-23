/**
 * @summary Initiert eine Zeitverzögerung bis die nächste Ausführung fortgesetzt wird
 *
 * @param {number} seconds - Setzt Zeit in s
 */
const delay = seconds => {
    return new Promise(
        resolve => setTimeout(resolve, seconds * 1000)

    );
};

/**
 * @summary In dieser Methode wird die Weboberfläche, je nachdem auf welchen Url-Pfad man ist, geladen. Dmit der Kontent auch nicht verloren geht.
 */
window.onload = function () {
    firstTimeStarted();
    connection();
    loadSettings();
};

/**
 * @summary Durch diese Methode werden alle Einstellungen der jewiligen Seite aufgerufen und geladen.
 */
function loadSettings() {
    brokerConnectStatus();
    const path = window.location.pathname;
    if (path === "/") {                         // ... wenn der Pfadname "/" entspricht, dann werden die folgenden Methoden ausgeführt
        onLoadHome();
    }

    if (path === "/Settings/Setting") {         // ... wenn er aber "/Settings/Setting" enstspricht, dann wird die folgende ausgeführt
        onLoadSettings();
    }

    if (path === "/Settings/HandControl") {     // ... oder sogar  "/Settings/HandControl" enstspricht, dann wird die folgende ausgeführt
        onLoadHandControl();
    }
}