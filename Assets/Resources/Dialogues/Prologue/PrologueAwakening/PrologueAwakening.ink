INCLUDE ../../globals.ink

INCLUDE english/P_3_1.ink

INCLUDE polish/P_3_1.ink

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