INCLUDE ../../globals.ink

INCLUDE english/P_2_2.ink

INCLUDE polish/P_2_2.ink

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
{prologue == 9:
    -> Prologue_2_2_english
  - else:
    SOMETHING WENT WRONG
    ->END
}

=== Polish_Dialogues ===
{prologue == 9:
    -> Prologue_2_2_polish
  - else:
    SOMETHING WENT WRONG
    ->END
}