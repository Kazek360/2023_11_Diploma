INCLUDE ../../../globals.ink

INCLUDE english/Q1_7.ink
INCLUDE polish/Q1_7.ink

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
{Q1 == 7:
    -> Q1_7_english
  - else:
    SOMETHING WENT WRONG
    ->END
}

=== Polish_Dialogues ===
{Q1 == 7:
    -> Q1_7_polish
  - else:
    SOMETHING WENT WRONG
    ->END
}