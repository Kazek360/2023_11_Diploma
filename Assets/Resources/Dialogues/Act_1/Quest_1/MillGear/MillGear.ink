INCLUDE ../../../globals.ink

INCLUDE english/Q1_6.ink
INCLUDE polish/Q1_6.ink

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
{Q1 == 6:
    -> Q1_6_english
  - else:
    SOMETHING WENT WRONG
    ->END
}

=== Polish_Dialogues ===
{Q1 == 6:
    -> Q1_6_polish
  - else:
    SOMETHING WENT WRONG
    ->END
}