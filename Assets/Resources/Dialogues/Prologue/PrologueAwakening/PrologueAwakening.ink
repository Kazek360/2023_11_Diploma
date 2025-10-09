INCLUDE ../../globals.ink

INCLUDE english/P_3_1.ink

INCLUDE polish/P_3_1.ink

{english:
    -> English_Dialogues
  - else:
    -> Polish_Dialogues
}

=== English_Dialogues ===
{prologue == 14:
    -> Prologue_3_1_english
  - else:
    SOMETHING WENT WRONG
    ->END
}

=== Polish_Dialogues ===
{prologue == 14:
    -> Prologue_3_1_polish
  - else:
    SOMETHING WENT WRONG
    ->END
}