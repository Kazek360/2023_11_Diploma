INCLUDE ../../globals.ink

INCLUDE english/P_1_4.ink
INCLUDE polish/P_1_4.ink

{english:
    -> English_Dialogues
  - else:
    -> Polish_Dialogues
}

=== English_Dialogues ===
{prologue == 4:
    -> Prologue_1_4_english
  - else:
    SOMETHING WENT WRONG
    ->END
}

=== Polish_Dialogues ===
{prologue == 4:
    -> Prologue_1_4_polish
  - else:
    SOMETHING WENT WRONG
    ->END
}