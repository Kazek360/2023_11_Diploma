INCLUDE ../../../globals.ink

INCLUDE english/Q1_16.ink
INCLUDE polish/Q1_16.ink

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
{Q1 == 16:
    -> Q1_16_english
  - else:
    SOMETHING WENT WRONG
    ->END
}

=== Polish_Dialogues ===
{Q1 == 16:
    -> Q1_16_polish
  - else:
    SOMETHING WENT WRONG
    ->END
}