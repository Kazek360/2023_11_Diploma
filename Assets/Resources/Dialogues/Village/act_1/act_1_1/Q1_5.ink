INCLUDE ../../../globals.ink

{ dad_dinner_eaten: ->Dad_Angry | ->Dad_Happy}

== Dad_Angry ==

#speaker:Tata Wita #portrait:father_neutral
…Czy ta babka ziemniaczana jest nadgryziona?

#speaker:Wit #portrait:wit_surprised
Eee-

#speaker:Tata Wita #portrait:father_neutral
Wit, nieładnie. Nie uczyli cię w szkółce niedzielnej? Siódme nie kradnij!

#speaker:Nestor #portrait:nestor_surprised
A nie mówiłem?

#speaker:Wit #portrait:wit_surprised
Przepraszam tato.

#speaker:Tata Wita #portrait:father_neutral
Matka da ci za to solidny szlaban, kiedy tylko jej o tym powiem.

#speaker:Wit #portrait:wit_surprised
Nie! Proszę tatku! Zrobię wszystko, tylko nie mów mamie!

#speaker:Tata Wita #portrait:father_neutral
…Wszystko?
Synu, mam dla ciebie bojowe zadanie.

#speaker:Wit #portrait:wit_surprised
O nie-

#speaker:Tata Wita #portrait:father_neutral
Znów koła się zacięły i do pierona nie wiem dlaczego.
Jesteś mały, przeciśniesz się za mechanizm i sprawdzisz co tam się dzieje?

#speaker:Wit #portrait:wit_surprised
Zrobię co w mojej mocy, tatku.

~removeItem("father_dinner", 1)

->END

== Dad_Happy ==
* [Potrzebujesz pomocy?]

    #speaker:Tata Wita #portrait:father_neutral
    Właściwie, to nie pogardziłbym pomocną parą rąk. Znów koła się zacięły i do pierona nie wiem dlaczego.

    Jesteś mniejszy ode mnie, wciśniesz się tam i zobaczysz, co się popsuło?
    
    #speaker:Wit #portrait:wit_surprised
    Już się robi, tatku.
    
* [Mogę już sobie pójść]

    #speaker:Tata Wita #portrait:father_neutral
    Nie tak prędko, młody. Mam dla ciebie bojowe zadanie.
    
    #speaker:Wit #portrait:wit_surprised
    Ech… czego ci trzeba, tatku?

    #speaker:Tata Wita #portrait:father_neutral
    Właśnie nie wiem. Mechanizm nie chce ruszyć, jakby go coś blokowało, do pierona. Wciśnij się na tył i zobacz co się zepsuło.
    
    #speaker:Wit #portrait:wit_surprised
     Dobrze tatku.

- 
~removeItem("father_dinner", 1)
->END