INCLUDE ../../globals.ink

INCLUDE english/P_1_2.ink
INCLUDE english/Flavour.ink

INCLUDE polish/P_1_2.ink
INCLUDE polish/Flavour.ink

{english:
    -> English_Dialogues
  - else:
    -> Polish_Dialogues
}

=== English_Dialogues ===
{prologue == 2:
    -> Prologue_1_2_english
  - else:
    -> HrycFlavourPrologue_english
}

=== Polish_Dialogues ===
{prologue == 2:
    -> Prologue_1_2_polish
  - else:
    -> HrycFlavourPrologue_polish
}