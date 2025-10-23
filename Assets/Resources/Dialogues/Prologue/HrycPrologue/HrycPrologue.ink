INCLUDE ../../globals.ink

INCLUDE english/P_1_2.ink
INCLUDE english/Flavour.ink

INCLUDE polish/P_1_2.ink
INCLUDE polish/Flavour.ink

{languageIndex:
  - 0:
    -> English_Dialogues
  - 1: 
    -> Polish_Dialogues
  - else:
    Wrong Language Index
    ->END
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