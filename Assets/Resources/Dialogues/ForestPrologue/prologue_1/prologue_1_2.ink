INCLUDE ../../globals.ink

{ hryc_race_reward:
        ->RaceWithReward
    - else:
        ->RaceWithoutReward
}

== RaceWithoutReward ==
{ hryc_race_win: ->WitWinsSat | ->HrycWinsSat}

== RaceWithReward ==
{ hryc_race_win: ->WitWinsReward | ->HrycWinsReward}

== WitWinsSat ==
#speaker:Wit #portrait:wit_smirk
Ha! I kto tu jest szybszy?

#speaker:Hryc #portrait:hryc_neutral
Tym razem ci się udało…

#speaker:Hryc #portrait:hryc_neutral
…ale to tylko dlatego że nie byłem dostatecznie przygotowany.

#speaker:Wit #portrait:wit_smirk
Po prostu przyznaj, że jestem od ciebie lepszy.

#speaker:Hryc #portrait:hryc_neutral
Wolałbym czyścić oborę.

-> END

== WitWinsReward ==
#speaker:Wit #portrait:wit_smirk
Chyba twoja proca ma nowego właściciela. Nie martw się, dobrze się nią zaopiekuję.

#speaker:Hryc #portrait:hryc_neutral
Jak ją połamiesz to... to...

#speaker:Wit #portrait:wit_fun
To co?

#speaker:Hryc #portrait:hryc_neutral
To jajco. Masz jej pilnować.

-> END

== HrycWinsSat ==
#speaker:Hryc #portrait:hryc_neutral
Wygrałem! Mówiłem ci, że nikt mnie nie prześcignie!

*[Dobra, dobra. To dlatego że wcześniej się nadźwigałem.]
    
    #speaker:Wit #portrait:wit_smirk
    Gdybym był wypoczęty, to bym cię pokonał z palcem w nosie.

    #speaker:Hryc #portrait:hryc_neutral
    Widzisz? Jednak dobra drzemka to klucz do zwycięstwa.

*[*Milcz*]
    #speaker:Hryc #portrait:hryc_neutral
    Co, zatkało kakao? Widzisz, jednak dobra drzemka to klucz do zwycięstwa.
-
-> END

== HrycWinsReward ==
#speaker:Hryc #portrait:hryc_neutral
Wygrałem! Oddawaj scyzoryk!

#speaker:Wit #portrait:wit_angry
Pofarciło ci się i tyle. Gdybym był wypoczęty, to bym cię pokonał z palcem w nosie.

#speaker:Hryc #portrait:hryc_neutral
Widzisz? Jednak dobra drzemka to klucz do zwycięstwa.

-> END
