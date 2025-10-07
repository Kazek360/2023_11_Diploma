INCLUDE ../../globals.ink

INCLUDE english/P_2_1.ink

INCLUDE polish/P_2_1.ink

{english:
    -> English_Dialogues
  - else:
    -> Polish_Dialogues
}

=== English_Dialogues ===
{prologue == 8:
    -> Prologue_2_1_english
  - else:
    SOMETHING WENT WRONG
    ->END
}

=== Polish_Dialogues ===
{prologue == 8:
    -> Prologue_2_1_polish
  - else:
    SOMETHING WENT WRONG
    ->END
}