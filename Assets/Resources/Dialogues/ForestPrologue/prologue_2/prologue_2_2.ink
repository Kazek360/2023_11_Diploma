INCLUDE ../../globals.ink

-> Prologue_2_2

=== Prologue_2_2 ===

~changeNestorState("forest_prologue", true)

#speaker:Wit #portrait:wit_surprised
Eee… dzień dobry?

#speaker:Tajemnicza Postać #portrait:nestor_neutral
...

#speaker:Wit #portrait:wit_angry
Hej, do ciebie mówię! Ty! B-bez nóg!

#speaker:Tajemnicza Postać #portrait:nestor_neutral
...

#speaker:Wit #portrait:wit_thinking
Tak, ty. Widziałem cię wcześniej jak się czaiłeś w szuwarach.

#speaker:Tajemnicza Postać #portrait:nestor_neutral
...

#speaker:Tajemnicza Postać #portrait:nestor_talking
Ty mnie widzisz?

*[Tak]
*[Nie]
    #speaker:Tajemnicza Postać #portrait:nestor_angry
    ...
    
    #speaker:Wit #portrait:wit_neutral
    Oczywiście, że cię widzę.
    
-
#speaker:Tajemnicza Postać #portrait:nestor_sad
To jest gorzej niż przewidywałem…

#speaker:Wit #portrait:wit_surprised
Co znaczy gorzej? Kim jesteś?

#speaker:Tajemnicza Postać #portrait:nestor_instruction
Hmm... możesz mnie nazywać Nestor.

#speaker:Wit #portrait:wit_smirk
Nestor? To twoje prawdziwe imię, czy ksywa? 

#speaker:Nestor #portrait:nestor_neutral
...

->NestorQuestions  

== NestorQuestions ==

*[Kim jesteś]
    #speaker:Nestor #portrait:nestor_talking
    Mówiłem ci już, jestem Nestor.
->NestorQuestions

*[Dlaczego miałbym cię nie widzieć?]
    #speaker:Nestor #portrait:nestor_instruction
    Ponieważ normalnie ludzie nie widzą takich jak ja.
    #speaker:Wit #portrait:wit_thinking
    Takich jak ty… czyli jakich? Nie rozumiem.

    #speaker:Nestor #portrait:nestor_neutral
    ...
->NestorQuestions

*[Co znaczy gorzej?]
    #speaker:Nestor #portrait:nestor_thinking
    To znaczy że było blisko.
    #speaker:Wit #portrait:wit_surprised
    Blisko czego?
    #speaker:Nestor #portrait:nestor_angry
    Gdybyś nie ganiał po drzewie jak wiewiórka to nie musiałbym cię ściągać z drugiej strony.
    #speaker:Wit #portrait:wit_surprised
    Drugiej strony… jeziora?
    #speaker:Nestor #portrait:nestor_talking
    Można tak powiedzieć.
->NestorQuestions
*->
#speaker:Nestor #portrait:nestor_thinking
Słuchaj Witek, miałeś dużo szczęścia

#speaker:Wit #portrait:wit_surprised
Skąd wiesz jak się nazywam? Zaraz! Czy ty mnie śledzisz?

~changeNestorState("forest_prologue", false)
->END