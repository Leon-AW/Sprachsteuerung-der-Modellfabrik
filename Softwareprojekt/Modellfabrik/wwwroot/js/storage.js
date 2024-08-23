/**
 * @summary Hat sich vorher den VoiceSlider-Status gemerkt und lädt diesen bei erneutem Laden der Settings-Seite wieder
 */
function onLoadSettings() {
    historySlider();
    answerTimeSlider();
    voiceSlider();
    testBrokerSlider();
    afterConnectedSettings();
}

/**
 * @summary Hat sich vorher den VoiceSlider-Status gemerkt und lädt diesen bei erneutem Laden der Handcontroll-Seite wieder
 */
function onLoadHandControl() {
    afterConnectedHandControl();
}

/**
 * @summary Wird ausgeführt wenn die Startseite neugeladen wird
 */
function onLoadHome() {
    writeSpeechHistory();
    toggleSpeechHistoryTextBox(); //toggled die Textbox entsprechend der Einstellung
    afterConnectedHome();
    recognizeKey();
}

/**
 * @summary Wird beim erstmaligen Starten ausgeführt
 *
 * @return Ob es zum ersten mal gestartet ist oder nicht
 */
async function firstTimeStarted() {
    if (sessionStorage.getItem("start") === "true") {               // ... wenn start "true" ist, return false
        return false;
    } else {
        sessionStorage.setItem("start", true);                      // ... sonst true
        sessionStorage.setItem("internet", true);
        loadingScreen(6);
        return true;
    }
}

/**
 * @summary Hat sich den Status der Voice-Checkbox gemerkt und ruft diesen wieder ab
 */
function voiceSlider() {
    if (sessionStorage.getItem("voiceSlider") === "false") {        // ... wenn voiceSlider "false" ist, dann ist checked false
        VoiceSlider.checked = false;
    } else {                                                        // ... sonst true
        VoiceSlider.checked = true; 
    }
}

/**
 * @summary Hat sich den Status der AnswerTime-Checkbox gemerkt und ruft diesen wieder ab
 */
function answerTimeSlider() {
    if (sessionStorage.getItem("timeSlider") === "false") {        // ... wenn timeSlider "false" ist, dann ist checked false
        AnswerTimeSlider.checked = false;
    } else {                                                        // ... sonst true
        AnswerTimeSlider.checked = true;
    }
}

/**
 * @summary Hat sich den Status der SpeechHistory-Checkbox gemerkt und ruft diesen wieder ab
 */
function historySlider() {
    if (sessionStorage.getItem("historySlider") === "false") {        // ... wenn historySlider "false" ist, dann ist checked false
        SpeechHistorySlider.checked = false;
    } else {                                                        // ... sonst true
        SpeechHistorySlider.checked = true;
    }
}

/**
 * @summary Hat sich den Status der TestBroker-Checkbox gemerkt und ruft diesen wieder ab
 */
function testBrokerSlider() {
    if (sessionStorage.getItem("testbroker") === "true") {        // ... wenn testbroker "true" ist, dann ist checked true
        TestBrokerSlider.checked = true;
    } else {                                                        // ... sonst false
        TestBrokerSlider.checked = false;
    }
}