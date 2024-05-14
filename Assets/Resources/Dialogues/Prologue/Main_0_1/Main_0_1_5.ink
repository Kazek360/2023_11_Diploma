INCLUDE ../../globals.ink
-> MainQuest1_5

== MainQuest1_5 ==
#speaker:Wit #portrait:wit_surprised
Gdzie… gdzie ja jestem?

#speaker:Wit #portrait:wit_surprised
Halo?

#speaker:Wit #portrait:wit_surprised
Jest tu kto?

#speaker:Wit #portrait:wit_surprised
Ktokolwiek?

*[(Idź w stronę światła)]
    #speaker:Tajemniczy głos w oddali #portrait:none
    Nie idź w stronę światła.
*[(Czekaj)]
    #speaker: #portrait:none
    (Nic się nie dzieje)
    **[(Idź w stronę światła)]
        #speaker:Tajemniczy głos w oddali #portrait:none
        Nie idź w stronę światła.
    **[Pomocy!]
        #speaker:Tajemniczy głos w oddali #portrait:none
        Słyszę cię.
*[Pomocy!]
    #speaker:Tajemniczy głos w oddali #portrait:none
    Słyszę cię.
-
#speaker:Wit #portrait:wit_surprised    
Halo? Jest tu kto?

#speaker:Tajemniczy głos #portrait:none
Jestem. Idź w moją stronę

#speaker:Wit #portrait:wit_surprised
Kim jesteś?

#speaker:Tajemniczy głos #portrait:none
To nie jest teraz ważne. Masz coraz mniej czasu.
Idź za moim głosem


*[(Idź w stronę światła)]
    ~death_choice = "death"
*[(Idź za tajemniczym głosem)]
    ~death_choice = "life"
-
->END