# Softwareentwicklungsprojekt: Sprachsteuerung einer Modellfabrik (PI Informatik)

* **Teammitglieder:**
	1. Ala Al-Khazzan
	2. Marc Zimmermann
	3. Leon Wagner
    4. Siwar Aouididi
* **Team:** 4
* **Semester:** WiSe 21/22

Mit Hilfe dieses Softwareentwicklungsprojekt soll die Kommunikation zwischen der Modellfabrik und einem User über eine Sprachsteuerung erfolgen.
Das Prinzip der Sprachsteuerung soll an einem Modell einer Fabrik umgesetzt werden. Hierfür soll die fischertechnik Modellfabrik verwendet werden. Für die Verarbeitung wird dabei ein RaspberryPi 3b verwendet.

## Voraussetzungen zur Nutzung der Software

- Eine Internet-Verbindung sollte vorhanden sein.

 Installiert sollte sein:
- Visual Studio 2019 
    - NuGet Pakete
        - Microsoft.CognitiveServices.Speech (Azure Speech SDK)
        - System.Device.Gpio (On/Off LED)
        - M2MqttDotnetCore (MQTT-Protokoll)
        - Newtonsoft.Json
        - NUnit
        - NUnit3TestAdapter
        - coverlet.collector
        - Microsoft.NET.Test.Sdk
        <br><br>
    - Workload:
        - ASP.NET Core
        <br><br>


- Eclipse Mosquitto Broker
    - unter https://mosquitto.org/download/ für den passenden Rechner installieren und weitere Schritte von unten "Installationshinweise und Anmerkung zum MQTT-Protokoll" befolgen.

## Installationshinweise für das Projekt

Die Installation sollte über Klonen oder laden des Repositorys auf Ihren lokalen Rechner erfolgen.
Dabei sollten keine Probleme auftauchen. Falls doch dann einmal im VS Installer ein neues Workload hinzufügen:
VS Installer -> Ändern -> "ASP.Net und Webentwicklung" installieren. Dieses Tool wird für das Arbeiten mit der Weboberfläche gebraucht.

## Installationshinweise und Anmerkung zum MQTT-Protokoll

Nach dem der Broker installiert wurde, sollten folgende Schritte ab "Install Steps" von 1-5 unter http://www.steves-internet-guide.com/install-mosquitto-broker/ befolgt werden.

Verbindung zur Modellfabrik:
Beim Starten des Programmes, versucht die Software zuerst eine Verbindung zur Modellfabrik herzustellen. Ist der Broker der Modellfabrik jedoch nicht erreichbar, erscheint eine Messagebox, die auf einen Verbindungsfehler zur Modellfabrik hinweist. Falls die Modellfabrik jedoch angeschlossen ist, sollte die Verbindung zwischen der Modellfabrik und dem Raspberry Pi überprüft werden. Um dann eine neue Verbindung zur Modellfabrik herzustellen, kann man im Reiter Einstellungen den Button "Reconnect" drücken.

Verbindung zum Testbroker:
Möchte man eine Verbindung zum MQTT Testbroker herstellen, kann man im Reiter Einstellungen den Slider bei Testbroker auf Yes setzen.

Ist die Verbindung hergestellt:
So können nun Sprachbefehle getätigt und die Software im vollen Umfang genutzt werden.

## Anmerkung zum Raspberry Pi
- Passwort des Raspberrypis: pi
- Username des Raspberrypis: pi
- Auf dem Raspberry Pi ist die Software Raspian OS als Betriebssystem installiert.
- Auf dem Rapberry Pi ist Eclipse Mosquitto Broker installiert: https://www.modius-techblog.de/smart-home/mqtt-broker-auf-dem-raspberry-pi/?cookie-state-change=1634236522455

- Auf dem Raspberry Pi ist .NET installiert:
  - .NET SDK (reflecting any global.json):
      - Version:   5.0.210
      - Commit:    3665e1a61c
      <br><br>

  - Runtime Environment:
      - OS Name:     raspbian
      - OS Version:  11
      - OS Platform: Linux
      - RID:         linux-arm
      - Base Path:   /home/pi/.dotnet/sdk/5.0.210/
      <br><br>

  - Host (useful for support):
      - Version: 5.0.13
      - Commit:  b3afe99225
      <br><br>

  - .NET SDKs installed:
      - 5.0.210 [/home/pi/.dotnet/sdk]
      <br><br>

  - .NET runtimes installed:
      - Microsoft.AspNetCore.App 5.0.13 [/home/pi/.dotnet/shared/Microsoft.AspNetCore.App]
      - Microsoft.NETCore.App 5.0.13 [/home/pi/.dotnet/shared/Microsoft.NETCore.App]
      <br><br>

## Voraussetzung zur Nutzung der Software im Raspberry Pi:

- Mikrofone und Lautsprecher am Raspberry anschließen.

- Die blaue LED auf PIN 26 installieren.

- Die rote LED auf PIN 13 installieren.

- Die grüne LED auf PIN 6 installieren.

## Nutzung der Software im Raspberry Pi:

Die Software wurde auf dem Raspberry Pi mit der Software [WinScp](https://winscp.net/eng/download.php) kopiert.

Um die Software zu benutzen, muss man sich erstmal auf dem Raspberry Pi einloggen. Dafür könnte man folgendes nutzen:

- Powershell oder PuTTY (Windows)
- Terminal (macOS)

Nach der Auswahl der Software den Befehl: "ssh pi@[RaspberrypiIPadresse]" in der Kommandozeile ausführen (Beispiel: ssh pi@192.168.0.105).
Dann das gültige Passwort: "pi" eingeben.

Danach muss man den Befehl "cd team4/Softwareprojekt/Modellfabrik" und anschließend "dotnet run  --urls http://*:5000" eingeben. 

Auf dem Fenster werden die folgenden Zeilen angezeigt:

"info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://[::]:5000
 info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
 info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
 info: Microsoft.Hosting.Lifetime[0]
      Content root path: /home/pi/team4/Softwareprojekt/Modellfabrik"

Danach kann der Nutzer einen beliebigen Browser öffnen und in der URL-Suchleiste: http://[RaspberrypiIPadresse]:5000 (Beispiel: http://192.168.0.105:5000) eingeben.

Nach der Ausführung wird man zur Weboberfläche geleitet, wo man die Software testen kann. 


## Verwendung der Software

Um die Software zu benutzen, muss man die .sln Datei unter /Softwareprojekt/Sprachsteuerung.sln erstmal öffnen.
Beim erstmaligen Öffnen der .sln Datei sollten Sie die aufkommenden Abfragen, die von Visual Studio kommen, mit
Ja, Ok oder Zulassen beantworten, ansonsten wird beim Ausführen des Programms der Webbrowser nicht geöffnet.
Ist die Software vollständig geladen, sollte Modellfabrik oder IIS Express oben als Startprojekt (Startknopf/Runknopf) festgelegt sein.
Ist es nicht der Fall, so führen Sie folgende Schritte aus:
Rechtsklick auf Modellfabrik im Projekmappen-Explorer -> Als Startprojekt festlegen
Falls ISS Express oben steht ändern Sie dies zu Modellfabrik: -> Dropdownknopf und wählen Modellfabrik aus

Nach Starten der Software wird man zur Weboberfläche geleitet, wo man die Software testen kann.
Nun können Sie in der Konsole die Kommunikation mit der Modellfabrik beobachten und dabei in Ihrem Browser mit der Modellfabrik interagieren.
Da empfehlen sich folgende Tests:
1. Den Button in der Mitte zur Aktivierung der Sprachsteuerung drücken. Dann einen der **gültigen Befehle** (siehe unten) aussprechen, sobald die Animation des Buttons kommt. Dann erhalten Sie auch eine verbale Rückmeldung der Modellfabrik.
Anschließend wird der Sprachverlauf im Dialogfenster (unten links) angezeigt.
2. Widerholen des ersten Tests, diesmal mit einem **ungültigen Befehl** (irgendwas sagen).
3. Sie können die oben genannten Tests auch über das **Erkennungswort: "Factory"** ansteuern. Dazu müssen Sie "Factory" sagen und wenn die Animation des Buttons zusehen ist, wieder einen Befehl einsprechen. **_Hinweis:_** Wenn das Schlüsselwort nicht direkt erkannt wird einfach noch mal klar und deutlich einsprechen, falls das nicht hilft und das Projekt mit der Kommandozeile gestartet wurde, dort rein klicken und einmal die Leertaste drücken. (Manchmal verliert das Programm den "Fokus", aber nur bekannt bei Nutzung mit der Kommandozeile)
4. Wenn man den Reiter "Manuelle Steuerung" angeklickt, kann man auch die Befehle darüber ansteuern.
5. Wenn man in das Mikrofon reinspricht, so leuchtet die blaue LED. Bei einem gültigen Befehl leuchtet die grüne und bei einem ungültigen die rote LED. Das kann man in den Videos bei Slack sehen (siehe Links).
6. Im Reiter "Einstellungen" kann man die Sprachstimme auf "Male" umstellen. Danach können Sprachbefehle mit männlicher Stimme getätigt werden.
7. Falls man sich den Sprachverlauf oder die Antwortzeit nicht anzeigen lassen möchte, kann man dies im Reiter "Einstellungen" mit dem jeweiligen Slider ändern.
8. Die Software ist ebenso bereit auf Internetverbindungsprobleme zu reagieren. Um das zu testen können Sie Ihr Gerät während der Sitzung auf Flugmodus stellen. Die Software wird mit einer Messagebox hinweisen, dass die Verbindung zum Internet und zum Broker abgebrochen ist. Sobald Sie den Flugmodus deaktivieren und eine Internetverbindung wieder haben, erscheint eine Messagebox, dass die Internetverbindung wieder da ist. Nun können Sie in den "Einstellungen" einen erneuten Verbindungsaufbau zum Broker aufbauen, indem man auf den Button "Reconnect" drückt. Wurde die Verbindung gefunden, so wird die Software wieder einsatzbereit sein.

## Gültige Befehle

**Bestelle ein rotes Werkstück.**

**Bestelle ein weißes Werkstück.**

**Bestelle ein blaues Werkstück.**

**Wie viele rote Werkstücke befinden sich im Lager?**

**Wie viele weiße Werkstücke befinden sich im Lager?**

**Wie viele blaue Werkstücke befinden sich im Lager?**

**Wie ist die Temperatur gerade?**

**Wie ist der Luftdruck gerade?**

**Wo befindet sich meine Bestellung gerade?**

## Links

https://app.slack.com/client/T02GYD7982D/C02GWEPMF52

https://versionone.f2.htw-berlin.de/VersionOne/Default.aspx 
