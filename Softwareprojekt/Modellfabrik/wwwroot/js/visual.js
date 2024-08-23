/**
 * @summary Diese Methode startet die Animation des Buttons.
 *
 * @param {boolean} on - Animation an oder aus
 */
function animation(on) {
    var styleElem = document.head.appendChild(document.createElement("style"));
    if (on) {                                                                                                       // ... wenn die Animation an geht also on gleich "true" ist
        styleElem.innerText = "#pulser {animation: pulse 1000ms cubic-bezier(0.9, 0.7, 0.5, 0.9) infinite}"; // ... dann wird die Pulsanimation gestarten
        InfoText.style.visibility = "hidden";
    } else {
        styleElem.innerText = "#pulser {animation: pulse 0ms cubic-bezier(0, 0, 0, 0) infinite}";            // ... ansonsten hört diese auf
    }
}

/**
 * @summary Diese Methode lässt den Bereich des Sprachverlaufs einblenden bzw. ausblenden.
 * Das geschieht je nach übergabe der Parameter.
 *
 * @param {boolean} boolean - Blendet den Sprachverlauf ein oder aus
 */
function hideSpeechHistoryPanel(boolean) {
    if (boolean) {                                              // ... wenn  "true" ist, dann wird der untere Bereich mit den Sprachverlauf ausgeblendet
        LowerPanel.style.visibility = "hidden";
        SpeechHistoryLabel.style.visibility = "hidden";
        SpeechHistoryTextBox.style.visibility = "hidden";
    } else {                                                   // ... sonst angezeigt und die folgenden Methode ausgeführt
        LowerPanel.style.visibility = "visible";
        SpeechHistoryLabel.style.visibility = "visible";
        toggleSpeechHistoryTextBox();
    }
}


/**
 * @summary Diese Methode lässt die Sprechblasen einblenden.
 * Je nach übergabe der Parameter, wird die erste, zweite oder keine angezeigt.

 * @param {boolean} showFirst - Zeigt die erste Sprechblase oder nicht
 * @param {boolean} showSecond - Zeigt die zweite Sprechblase oder nicht
 * @param {boolean} hideBoth - Lässt beide Sprechblasen verschwinden
 */
function showSpeechBubbles(showFirst, showSecond, hideBoth) {
    if (fullConnected()) { //nur wenn Internet und Brokerverbindung da ist
        if (showFirst) {                                            // ... wenn showFirst "true" ist, dann wird die Benutzer Sprechblase angezeigt
            SpeechBubbleMe.style.visibility = "visible";
            SpeechBubbleTextMe.style.visibility = "visible";
        } else if (showSecond) {                                    // ... und wwenn showSecond "true" ist, dann wird die Modellfabrik Sprechblase angezeigt
            SpeechBubbleFactory.style.visibility = "visible";
            SpeechBubbleFactoryText.style.visibility = "visible";
            toggleAnswerTimeText();
        }

        if (hideBoth) {                                             // ... wenn hideBoth "true" ist, dann werden die beide Sprechblasen ausgeblendet
            SpeechBubbleMe.style.visibility = "hidden";
            SpeechBubbleTextMe.style.visibility = "hidden";
            SpeechBubbleFactory.style.visibility = "hidden";
            SpeechBubbleFactoryText.style.visibility = "hidden";
            AnswerTimeLabel.style.visibility = "hidden";
            document.getElementById("text-factory").textContent = "";
            document.getElementById("text-me").textContent = "";
            InfoText.style.visibility = "visible";
        }
    }
}

/**
 * @summary Diese Methode sorgt dafür, dass die Zeit in der Modellfabrik Sprechblase angezeigt wird.
 */
function toggleAnswerTimeText() {
    if (slider(sessionStorage.getItem("timeSlider"))) {     // ... wenn  "true" ist, dann wird die Antwortzeit angezeigt
        AnswerTimeLabel.style.visibility = "visible";
    }
}

/**
 * @summary Die Methode wird ausgeführt, wenn man die Checkbox bezüglich des Sprachverlaufs drückt.
 * Dabei lässt Sie die Textarea mit den Sprachverlauf verschwinden.
 */
function toggleSpeechHistoryTextBox() {

    if (slider(sessionStorage.getItem("historySlider"))) {     // ... wenn "true" durch den Slider ist, dann wird der Sprachverlauf angezeigt
        SpeechHistoryTextBox.style.visibility = "visible";
        SpeechHistoryLabel.style.visibility = "visible";
    } else {                                                     // ... sonst ausgeblendet
        SpeechHistoryTextBox.style.visibility = "hidden";
        SpeechHistoryLabel.style.visibility = "hidden";
    }
}

/**
 * @summary Diese Methode überprüft wie der Slider gerade steht auf "true" oder "false".
 *
 * @param {boolean} sliderValue - Gibt an welcher Slider betätigt ist
 * @return Slider aktiviert oder nicht
 */
function slider(sliderValue) {
    return (sliderValue === "true" || sliderValue === null); //null -> wenn dasProgramm gestartet wird
}

/**
 * @summary Diese Methode ändert den Style vom Sprechhbutton und Text
 */
function afterConnectedHome() {
    if (sessionStorage.getItem("broker") === "true") {        // ... wenn broker "true" ist, dann werden folgende Styles geändert
        SpeechButton.style.backgroundColor = "dodgerblue";
        SpeechButton.disabled = false;
        InfoText.style.visibility = "visible";
    } else {                                                    // ... sonst werden diese Übernommen
        SpeechButton.style.backgroundColor = "gray";
        SpeechButton.disabled = true;
        InfoText.style.visibility = "hidden";
    }
}

/**
 * @summary Diese Methode ändert den Style vom Reconnecting-Button
 */
function afterConnectedSettings() {
    if (sessionStorage.getItem("broker") === "true") {       // ... wenn broker "true" ist, dann der Reconnecting-Button disabled
        ReconnectButton.disabled = true;

    } else {                                                    // ... sonst nicht
        ReconnectButton.disabled = false;
    }
}

/**
 * @summary Diese Methode ändert den Style vom den Buttons in der manuellen Steuerung 
 */
function afterConnectedHandControl() {
    if (sessionStorage.getItem("broker") === "true") {       // ... wenn broker "true" ist, folgenden Methode ausgeführt
        disableHandControlButtons(false);
    } else {                                                   //  ... sonst diese
        disableHandControlButtons(true);
    }
}

/**
 * @summary Diese Methode lässt alle Buttons in der manuellen Steuerung disabled.
 *
 * @param {boolean} disabled - Gibt an ob dies disabled sein sollen oder nicht
 */
function disableHandControlButtons(disabled) {
    for (let i = 0; i < HandControlButtons.length; i++) {
        HandControlButtons[i].disabled = disabled;
    }
}

/**
 * @summary Diese Methode zeigt, ob der Broker verbunden ist oder nicht.
 */
function brokerConnectStatus() {
    if (sessionStorage.getItem("broker") === "true") {       // ... wenn broker "true" ist, dann wird der Broker Name angezeigt
        BrokerConnectText.textContent = getBrokerIp();;
        BrokerConnectText.style.color = "green";
    } else {                                                   // ... sonst wird  "Disconnected" angezeigt
        BrokerConnectText.textContent = "Disconnected";
        BrokerConnectText.style.color = "red";
    }
}

/**
 * @summary Diese Methode zeigt, ob eine Internetverbindung besteht oder nicht.
 */
function internetConnectStatus(connected) {
    if (connected) {                                            // ... wenn connected "true" ist, dann wird "Connected" angezeigt
        InternetConnectText.textContent = `Connected`;
        InternetConnectText.style.color = "green";
    } else {                                                   // ... sonst wird "Disconnected" angezeigt
        InternetConnectText.textContent = `Disconnected`;
        InternetConnectText.style.color = "red";
    }
}

/**
 * @summary Zeigt einen Ladescreen
 * @param {number} seconds Anzeigedauer in Sekunden
 */
async function loadingScreen(seconds) {
    window.$("#cover").fadeIn(100);
    await delay(seconds);
    window.$("#cover").fadeOut(100);
}
