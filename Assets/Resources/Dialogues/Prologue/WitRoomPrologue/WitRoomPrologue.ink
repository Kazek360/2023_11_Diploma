INCLUDE ../../globals.ink

INCLUDE english/P_3_5.ink

INCLUDE polish/P_3_5.ink

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
{prologue == 18:
    -> Prologue_3_5_english
  - else:
    SOMETHING WENT WRONG
    ->END
}

=== Polish_Dialogues ===
{prologue == 18:
    -> Prologue_3_5_polish
  - else:
    SOMETHING WENT WRONG
    ->END
}