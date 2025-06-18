INCLUDE ../../globals.ink

->FatherDinner

== FatherDinner ==

#speaker:Wit #portrait:wit_happy
Babka ziemniaczana! Wygląda tak przepysznie! 

* [Zjedz]
    #speaker:Wit #portrait:wit_smirk
    Tata nie zauważy, jak ugryzę trochę z boku…
    
    #speaker:Nestor #portrait:nestor_angry
    Co ty wyprawiasz!?
    
    #speaker:Wit #portrait:wit_frightened
    N-nic! 
    
    #speaker:Nestor #portrait:nestor_angry
    Zostaw tą babkę! Nie uczyli cię w szkółce niedzielnej!? Siódme nie kradnij!
    
    #speaker:Wit #portrait:wit_angry
    Dobra, dobra! Już nie ruszam! Chciałem tylko trochę spróbować…Musisz tak wyskakiwać na mnie znienacka!?
    
    #speaker:Nestor #portrait:nestor_instruction
    Od tego jestem. Nie będę tylko biernie patrzył na twoje głupie pomysły.
    
    ~dad_dinner_eaten = true
    ~removeItem("father_dinner", 1)
    ~addItem("eaten_father_dinner", 1)
    
* [Zostaw]
    #speaker:Wit #portrait:wit_neutral
    Nie! Muszę oprzeć się pokusie! To w końcu obiad taty.
-

->END