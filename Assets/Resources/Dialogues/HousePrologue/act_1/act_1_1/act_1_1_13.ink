INCLUDE ../../../globals.ink

{ mill_thief_truth:
        ->Mill_Truth_Told
    - else:
        ->Mill_Lie_Told
}

== Mill_Truth_Told ==
#speaker:Wit #portrait:wit_surprised
No i misja “obiad” zakończyła się sukcesem! Mogę wracać do domu!

#speaker:Nestor #portrait:nestor_neutral
Nie pamiętasz, o co jeszcze prosiła cię mama?

* [Pamiętam!]
    #speaker:Wit #portrait:wit_surprised
    Miałem podziękować Hrycowi za ratunek.
    
    #speaker:Nestor #portrait:nestor_neutral
    Zgadza się.
    
    #speaker:Wit #portrait:wit_surprised
    Idę do niego na pola!
    
* [Nie pamiętam…]
    #speaker:Nestor #portrait:nestor_neutral
    Miałeś podziękować mu za wezwanie pomocy, po naszej potyczce z wilkami. 

    #speaker:Wit #portrait:wit_surprised
    No tak! Dzięki Nestor, zupełnie wyleciało mi z głowy.
-
->END

== Mill_Lie_Told ==
#speaker:Wit #portrait:wit_surprised
Trzeba ostrzec Hryca, może uda się to wspólnie jakoś odkręcić?
->END