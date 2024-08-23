/**
 * @summary  Startet das Hören auf ein Schlüsselwort
 */
async function recognizeKey() {
    if (fullConnected()) { //Wenn der Broker und Internet connected ist...
        stopKeyRecognition(); //...dann aktives Schlüsselwort lauschen beenden
        loadingScreen(1);
        await delay(1); //1s warten -> sonst Fehlermeldung im SpeechManager --> vorallem vor Ort in der Modellfabrik
        startKeyRecognition(); //Schlüsselworterkennung starten
    }
}

/**
 * @summary In der Methode wird über ein Ajax Befehl, die ActivateKeyRecognition Methode vom Homecontroller aufgerufen, die das anhören des Schlüsselwortes startet.
 */
function startKeyRecognition() {
    sessionStorage.setItem("buttonPressed", false);

    window.$.ajax({
        type: "POST",
        url: "Home/ActivateKeyRecognition",
        content: "application/json;",
        dataType: "dataType",
        data: { activate: true },
        complete: function () {                                            // ... sobald der Ajax Befehl beendet ist 
            if (sessionStorage.getItem("buttonPressed") === "false") {      // ... und wenn der Button nicht gedrückt ist, dann wird die folgende Methode aufgerufen
                callSpeechToText();
            }
        }
    });
}

/**
 * @summary In der Methode wird über ein Ajax Befehl, die ActivateKeyRecognition Methode vom Homecontroller aufgerufen, die das anhören des Schlüsselwortes stoppt.
*/
function stopKeyRecognition() {
    window.$.ajax({
        type: "POST",
        url: "Home/ActivateKeyRecognition",
        content: "application/json;",
        dataType: "dataType",
        data: { activate: false }
    });
}

/**
 * @summary  Methode setzt den Wert  von buttonPressed auf true, falls der Button gedrückt wird.
 */
function buttonPressed() {
    sessionStorage.setItem("buttonPressed", true);
    callSpeechToText();
}

/**
 * @summary In der Methode werden die untenstehenden Methoden aufgerufen und über ein Ajax Befehl, die CallSpeechInput Methode vom Homecontroller aufgeruft.
 */
function callSpeechToText() {
    stopKeyRecognition();
    animation(true);
    hideSpeechHistoryPanel(true);
    SpeechButton.disabled = true;

    window.$.ajax({
        type: "POST",
        url: "Home/CallSpeechInput",
        content: "application/json;",
        dataType: "json",
        success: function (recognizedText) {                    // ... bei erfolgreichen Ajax Befehl, werden die folgenden Methoden ausgeführt
            if (recognizedText.getText.length !== 0) {
                showSpeechBubbles(true, false, false);
                SpeechBubbleTextMe.textContent = recognizedText.getText;     // ... und die Benutzer Frage in die jeweilige Sprechblase angezeigt
            }
            animation(false);
        },
        complete: function () {                                // ... bei beenden des Ajax Befehls, wird folgende Methode ausgeführt
            callTextToSpeech();
        }
    });
}

/**
 * @summary Die Methode setzt die Schriftfarbe entsprechend der Antwort von der Modellfabrik, ob diese gültig oder nicht ist.
 */
function setFactoryTextColor() {
    window.$.ajax({
        type: "POST",
        url: "Home/IsTextValid",
        contentType: "application/json",
        dataType: "json",
        success: function (valid) {                    // ... bei erfolgreichen Ajax Befehl, wird folgende Verzweigung geprüft
            if (valid) {                               // .. wenn valid "ture" ist (Text ist validiert)
                SpeechBubbleFactoryText.style.color = "green";     // ... dann wird ide Schriftfabrbe auf Grün gesetzt
            }
            else {
                SpeechBubbleFactoryText.style.color = "red";       // ... ansonsten auf rot
            }
        }
    });
}

/**
 * @summary In der Methode wird über ein Ajax Befehl, die CallSpeechOutput Methode vom Homecontroller aufgeruft.
*/
function callTextToSpeech() {
    window.$.ajax({
        type: "POST",
        url: "Home/CallSpeechOutput",
        content: "application/json;",
        dataType: "json",
        success: function (response) {      // ... bei erfolgreichen Ajax Befehl, werden die folgenden Methoden ausgeführt
            setFactoryTextColor();
            getAnswerTime();
            showSpeechBubbles(false, true, false);
            SpeechBubbleFactoryText.textContent = response.getText;     // ... und die Modellfabrik Antwort in die jeweilige Sprechblase angezeigt
        },
        complete: async function () {       // ... bei beenden des Ajax Befehls, werden die folgenden anderen Methoden nach einem delay von vier Sekunden ausgeführt
            await delay(4);
            writeSpeechHistory();
            SpeechButton.disabled = false;
            hideSpeechHistoryPanel(false);
            showSpeechBubbles(false, false, true);
            startKeyRecognition();
        }
    });
}

/**
 * @summary In der Methode wird über ein Ajax Befehl, die GetHistory Methode vom Homecontroller aufgerufen.
*/
function writeSpeechHistory() {
    window.$.ajax({
        type: "POST",
        url: "Home/GetHistory",
        content: "application/json;",
        dataType: "json",
        success: function (history) {                  // ... bei erfolgreichen Ajax Befehl, wird der Sprachverlauf in der Textbox aufgerufen
            SpeechHistoryTextBox.textContent = history.getText;
        }
    });
}

/**
 * @summary In der Methode wird über ein Ajax Befehl, die GetTime Methode vom Homecontroller aufgerufen.
*/
function getAnswerTime() {
    window.$.ajax({
        type: "POST",
        url: "Home/GetTime",
        content: "application/json;",
        dataType: "json",
        success: function (time) {
            // ... bei erfolgreichen Ajax Befehl, wird die Antwortzeit in der Antwortsprechblase angezeigt
            if (parseFloat(time) < 3) {                               // .. wenn valid "ture" ist (Text ist validiert)
                AnswerTimeLabel.style.color = "green";
                AnswerTimeLabel.textContent = `Antwortzeit: ${time}s`;     // ... dann wird ide Schriftfabrbe auf Grün gesetzt
            }
            else {
                AnswerTimeLabel.textContent = `(bad-connection) Antwortzeit: ${time}s`;
                AnswerTimeLabel.style.color = "red";       // ... ansonsten auf rot
            }
        }
    });
}