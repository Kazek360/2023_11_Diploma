INCLUDE ../../../globals.ink

INCLUDE english/Q1_9.ink
INCLUDE polish/Q1_9.ink

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
{Q1 == 9:
    -> Q1_9_english
  - else:
    SOMETHING WENT WRONG
    ->END
}

=== Polish_Dialogues ===
{Q1 == 9:
    -> Q1_9_polish
  - else:
    SOMETHING WENT WRONG
    ->END
}