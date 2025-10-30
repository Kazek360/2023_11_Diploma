INCLUDE ../../../globals.ink

INCLUDE english/Q1_14.ink
INCLUDE polish/Q1_14.ink

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
{Q1 == 14:
    -> Q1_12_english
  - else:
    SOMETHING WENT WRONG
    ->END
}

=== Polish_Dialogues ===
{Q1 == 14:
    -> Q1_12_polish
  - else:
    SOMETHING WENT WRONG
    ->END
}