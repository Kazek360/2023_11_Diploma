INCLUDE ../../globals.ink

{ hryc_race_reward_knife or hryc_race_reward_slingshot:
    { hryc_race_reward_knife:
        ->KnifeWin
    }
    { hryc_race_reward_slingshot:
        ->SlingWin
    }
    - else:
        ->SatWin
}

== SatWin ==
{ hryc_race_winner_player: ->WitWinsSat | ->HrycWinsSat}

== KnifeWin ==
{ hryc_race_winner_player: ->WitWinsKnife | ->HrycWinsKnife}

== SlingWin ==
{ hryc_race_winner_player: ->WitWinsSling | ->HrycWinsSling}

== WitWinsSat ==
#speaker:Wit #portrait:wit_smirk
Ha. I kto tu jest lepszy?

#speaker:Hryc #portrait:hryc_neutral
Tym razem ci się udało…

#speaker:Hryc #portrait:hryc_neutral
…ale to tylko dlatego że nie byłem przygotowany.

#speaker:Wit #portrait:wit_smirk
Po prostu przyznaj że jestem lepszy.

#speaker:Hryc #portrait:hryc_neutral
Wolałbym czyścić oborę.

#speaker:Hryc #portrait:hryc_neutral
Następnym razem mi się uda.
-> END

== WitWinsKnife ==
#speaker:Wit #portrait:wit_smirk
Wygląda na to że nożyk dalej zostanie w mojej kieszeni

#speaker:Hryc #portrait:hryc_neutral
Za którymś razem mi się uda i go w końcu odzyskam.

#speaker:Wit #portrait:wit_fun
Chyba śnisz…

#speaker:Hryc #portrait:hryc_neutral
Jeszcze zobaczymy
-> END

== WitWinsSling ==
#speaker:Wit #portrait:wit_smirk
Wygrałem! Oddawaj proce.

#speaker:Hryc #portrait:hryc_neutral
Tym razem miałeś szczęście.

#speaker:Hryc #portrait:hryc_neutral
Następnym razem mi się uda.
-> END

== HrycWinsSat ==
#speaker:Hryc #portrait:hryc_neutral
Widzisz? I kto teraz jest lepszy?

*[Przyznaj, że Hryc jest lepszy]
    #speaker:Wit #portrait:wit_surprised
    Dobra, wygrałeś.
    
    #speaker:Wit #portrait:wit_smirk
    Następnym razem mi się uda

    #speaker:Hryc #portrait:hryc_neutral
    Widzisz? To nie było takie trudne.

    #speaker:Hryc #portrait:hryc_neutral
    Jestem zwyczajnie lepszy od ciebie.

*[Milcz]
    #speaker:Hryc #portrait:hryc_neutral
    Nic nie mówisz, bo wiesz że przegrałeś.
-
-> END

== HrycWinsKnife ==
#speaker:Hryc #portrait:hryc_neutral
(Wyciąga rękę. Na twarzy ma drwiący uśmieszek)

#speaker:Wit #portrait:wit_angry
Masz. I tak zamierzałem ci go jutro oddać.
-> END

== HrycWinsSling ==
#speaker:Hryc #portrait:hryc_neutral
Chyba twoja proca zostanie u mnie. Nie martw się, dobrze się nią zaopiekuję.

#speaker:Wit #portrait:wit_angry
Jak ją połamiesz to, to...

#speaker:Hryc #portrait:hryc_neutral
To co?

#speaker:Wit #portrait:wit_angry
To jajco. Masz jej pilnować.

#speaker:Hryc #portrait:hryc_neutral
A myślisz, że co ja robię.
-> END
