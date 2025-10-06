INCLUDE ../../globals.ink

INCLUDE english/P_1_7.ink

INCLUDE polish/P_1_7.ink

{english:
    -> English_Dialogues
  - else:
    -> Polish_Dialogues
}

=== English_Dialogues ===
{prologue == 7:
    -> Prologue_1_7_english
  - else:
    SOMETHING WENT WRONG
    ->END
}

=== Polish_Dialogues ===
{prologue == 7:
    -> Prologue_1_7_polish
  - else:
    SOMETHING WENT WRONG
    ->END
}