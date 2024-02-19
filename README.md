# TDA572-Kvantum

## OpenTK
### What is OpenTK? 
"OpenTK is a library that provides high-speed access to native OpenGL, OpenCL, and OpenAL for .NET programs."

### How to install? (Visual Studio 2022)
1. Click Tools > Nuget Package Manager > Package Manager Console
2. In the Package Manager Console, write "Install-Package OpenTK" (without ""). Press enter, and it should install. 

## Info om ändringar till 3D grafik
### Bootstrap/Display/WindowOTK (kort beskrivning av första versionen)
Så det *brukade* fungera med Bootstrap och Display-klasserna:
Bootstrap kallade på Display.display() varje loop i game loopen. Display.display() uppdaterade information och ritade ut den.

Så det *nu* fungerar med OpenTK:
Bootstrap kallar window.Run() för att starta game loopen. I window-klassen sätts massa OpenGL-grejer som gör att vi kan rita ut grejer. Ytterligare OpenGL-grejer sätts i DisplayOpenTK, beroende på vilken form som ska skrivas ut. I första versionen är det bara en triangel. De exakta 3D-koordinaterna för vad och en av triangelns hörn sätts i spelklassen (“GameThreeDim”) och används för att kalla på DisplayOpenTK.drawTriangle(..).

### Vill ni läsa mer om OpenTK/OpenGL?
Jag har till stor del läst denna tutorialen: https://opentk.net/learn/chapter1/index.html 
Den kanske är oklar om man inte har använt OpenGL förut.

### Saker som kan fungera dåligt i 3d-graphics branchen
- Det kan hända att jag har introducerat weird dependencies mellan Bootstrap, WindowOTK, DisplayOpenTK och game-filerna.
- Det är också väldigt möjligt att jag har påverkat physicssystemet. Den har några funktioner som inväntar millisekunder på grejer och sånt. Har försökt bevara det, men vet inte om det blir rätt. Ser något fucked ut så är det nog det 👍
