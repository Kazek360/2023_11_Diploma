INCLUDE ../../globals.ink

INCLUDE english/P_3_2.ink
INCLUDE english/P_3_4.ink

INCLUDE polish/P_3_2.ink
INCLUDE polish/P_3_4.ink

{english:
    -> English_Dialogues
  - else:
    -> Polish_Dialogues
}

=== English_Dialogues ===
{prologue:
  - 15:
    -> Prologue_3_2_english
  - 17: 
    -> Prologue_3_4_english
  - else:
    SOMETHING WENT WRONG
    ->END
}
=== Polish_Dialogues ===
{prologue:
  - 15:
    -> Prologue_3_2_polish
  - 17: 
    -> Prologue_3_4_polish
  - else:
    SOMETHING WENT WRONG
    ->END
}