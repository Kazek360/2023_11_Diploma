== Q1_16_english ==

{ mill_thief_truth:
        ->Mill_Truth_Told_english
    - else:
        ->Mill_Lie_Told_english
}

== Mill_Truth_Told_english ==
#speaker:Wit #portrait:wit_happy
No i misja “obiad” zakończyła się sukcesem! Mogę wracać do domu!

#speaker:Nestor #portrait:nestor_instruction
Nie pamiętasz, o co jeszcze prosiła cię mama?

* [Pamiętam!]
    #speaker:Wit #portrait:wit_confident
    Miałem podziękować Hrycowi za ratunek.
    
    #speaker:Nestor #portrait:nestor_smiling
    Zgadza się.
    
    #speaker:Wit #portrait:wit_confident
    Idę do niego na pola!
    
* [Nie pamiętam…]
    #speaker:Nestor #portrait:nestor_instruction
    Miałeś podziękować mu za wezwanie pomocy, po naszej potyczce z wilkami. 

    #speaker:Wit #portrait:wit_confident
    No tak! Dzięki Nestor, zupełnie wyleciało mi z głowy.
-
->END

== Mill_Lie_Told_english ==
#speaker:Wit #portrait:wit_scared
Trzeba ostrzec Hryca, może uda się to wspólnie jakoś odkręcić?
->END