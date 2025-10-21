INCLUDE ../../globals.ink

INCLUDE english/P_1_6.ink

INCLUDE polish/P_1_6.ink

{english:
    -> English_Dialogues
  - else:
    -> Polish_Dialogues
}

=== English_Dialogues ===
{prologue == 6:
    -> Prologue_1_6_english
  - else:
    SOMETHING WENT WRONG
    ->END
}

=== Polish_Dialogues ===
{prologue == 6:
    -> Prologue_1_6_polish
  - else:
    SOMETHING WENT WRONG
    ->END
}