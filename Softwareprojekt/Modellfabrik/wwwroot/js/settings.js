/**
 * @summary Diese Methode sorgt dafür, dass die Stimme der Modellfabrik sich ändert, sobald der Switch Checkbox unter den Einstellungen betätigt wird.
 */
function changeVoice() {
    if (VoiceSlider.checked) {               // ... wenn die Checkbox betätigt ist also "true", dann wird die folgende Methode aufgerufen
        Male();
    } else {                           // ... ansonsten die andere Methode
        Female();
    }
    sessionStorage.setItem("voiceSlider", !VoiceSlider.checked);
}

/**
 * @summary Diese Methode sorgt dafür, dass der Sprachvertlauf der Modellfabrik angezeigt bzw. ausgeblended wird, sobald der Switch Checkbox unter den Einstellungen betätigt wird.
 */
function speechHistorySetting() {
    sessionStorage.setItem("historySlider", !SpeechHistorySlider.checked);
}

/**
 * @summary Diese Methode sorgt dafür, dass die Antwortzeit der Modellfabrik angezeigt bzw. ausgeblended wird, sobald der Switch Checkbox unter den Einstellungen betätigt wird.
 */
function answerTimeSetting() {
    sessionStorage.setItem("timeSlider", !AnswerTimeSlider.checked);
}

/**
 * @summary Diese Methode sorgt dafür, dass man mit dem Testbroker oder mit dem der Modellfabrik verbunden ist, sobald der Switch Checkbox unter den Einstellungen betätigt wird.
 */
function testBrokerSetting() {
    sessionStorage.setItem("testbroker", !TestBrokerSlider.checked);
    loadingScreen(3);
    tryConnectToBroker();
}

/**
 * @summary In der Methode wird über ein Ajax Befehl, die ChangeVoiceGender Methode vom Homecontroller aufgerufen,
 * und die Stimme auf eine Männliche geändert.
 */
function Male() {
    window.$.ajax({
        type: "POST",
        url: "/Settings/ChangeVoiceGender",
        data: { voiceName: "Male" },
        dataType: "json"
    });
}

/**
 * @summary In der Methode wird über ein Ajax Befehl, die ChangeVoiceGender Methode vom Homecontroller aufgerufen,
 * und die Stimme auf eine Weibliche geändert.
 */
function Female() {
    window.$.ajax({
        type: "POST",
        url: "/Settings/ChangeVoiceGender",
        data: { voiceName: "Female" },
        dataType: "json"
    });
}