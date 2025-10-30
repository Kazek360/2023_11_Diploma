INCLUDE ../../../globals.ink

INCLUDE english/Q1_1.ink
INCLUDE polish/Q1_1.ink

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
{Q1 == 1:
    -> Q1_1_english
  - else:
    SOMETHING WENT WRONG
    ->END
}

=== Polish_Dialogues ===
{Q1 == 1:
    -> Q1_1_polish
  - else:
    SOMETHING WENT WRONG
    ->END
}