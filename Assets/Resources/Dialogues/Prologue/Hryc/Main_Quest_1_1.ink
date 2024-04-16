INCLUDE ../../globals.ink

-> MainQuest1_1

=== MainQuest1_1 ===
#speaker:Hryc #portrait:hryc_neutral
Ha, ha! Myślałeś że ze mną wygrasz?

#speaker:Wit #portrait:wit_angry
Ej… nie miałem szans. 

#speaker:Hryc #portrait:hryc_neutral
Twoja strata, nie moja wina że akurat niosłeś batiuszce Aleksandrowi skrzynię.

#speaker:Wit #portrait:wit_angry
Ja przynajmniej coś robię, a nie lenię się cały dzień na sianie.

#speaker:Hryc #portrait:hryc_neutral
Wylegiwanie się jest bardzo męczące…

#speaker:Wit #portrait:wit_angry
Gdybym nie miał zajętych rąk to bym wygrał.

#speaker:Hryc #portrait:hryc_neutral
Skoro tak twierdzisz…

#speaker:Wit #portrait:wit_smirk
To może ścigajmy się do zwalonego pniaka przy jeziorze.

#speaker:Hryc #portrait:hryc_neutral
A co będę miał gdy już wygram?

* [Udowodnisz że jesteś ode mnie lepszy.]
    #speaker:Hryc #portrait:hryc_neutral
    Oczywiście że jestem. Przygotuj się na przegraną.
    ~race_reward = "satisfaction"
    
* [Oddam ci mój scyzoryk.]
    #speaker:Hryc #portrait:hryc_neutral
    Długo się nim w takim razie nie nacieszysz
    ~race_reward = "knife"
    
* [A co gdy ja wygram?]
    #speaker:Hryc #portrait:hryc_neutral
    Nie licz na to, ale oddam ci moją procę.
    ~race_reward = "slingshot"
- 
#speaker:Hryc #portrait:hryc_neutral
Gotowy?

* [Złap mnie jeśli potrafisz]
    -> HrycDelay
* [(Czekaj)]
    #speaker:Hryc #portrait:hryc_neutral
    3..
-
* [Złap mnie jeśli potrafisz]
    -> HrycDelay
* [(Czekaj)]
    #speaker:Hryc #portrait:hryc_neutral
    2..   
-
* [Złap mnie jeśli potrafisz]
    -> HrycDelay
* [(Czekaj)]
    #speaker:Wit #portrait:wit_angry
    Ej!
-
-> END

=== HrycDelay ===
#speaker:Hryc #portrait:hryc_neutral
Ej!
~race_delay = true
-> END
