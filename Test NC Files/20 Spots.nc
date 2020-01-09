G0 X0 Y0

M8 ;Poliermittel ein, Spindel absenken
G4 P5; 10 Sekunden warten
M3 S1000;Spindel ein

G4 P5
G0 Y10
G4 P5
G0 Y20
G4 P5
G0 Y30
G4 P5
G0 Y40
G4 P5

G0 X10
G4 P5
G0 Y30
G4 P5
G0 Y20
G4 P5
G0 Y10
G4 P5
G0 Y0
G4 P5

G0 X20
G4 P5
G0 Y10
G4 P5
G0 Y20
G4 P5
G0 Y30
G4 P5
G0 Y40
G4 P5

G0 X20
G4 P5
G0 Y30
G4 P5
G0 Y20
G4 P5
G0 Y10
G4 P5
G0 Y0
G4 P5

M5 ;Spindel ausschalten
M9 ;Spindel abheben, Poliermittel aus

M30