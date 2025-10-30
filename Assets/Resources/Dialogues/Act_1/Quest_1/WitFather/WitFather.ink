INCLUDE ../../../globals.ink

INCLUDE english/Q1_5.ink
INCLUDE english/Q1_8.ink
INCLUDE english/Q1_17.ink

INCLUDE polish/Q1_5.ink
INCLUDE polish/Q1_8.ink
INCLUDE polish/Q1_17.ink

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
{Q1:
  - 5:
    -> Q1_5_english
  - 8: 
    -> Q1_8_english
  - 17: 
    -> Q1_17_english
  - else:
    SOMETHING WENT WRONG
    ->END
}
=== Polish_Dialogues ===
{Q1:
  - 5:
    -> Q1_5_polish
  - 8: 
    -> Q1_8_polish
  - 17: 
    -> Q1_17_polish
  - else:
    SOMETHING WENT WRONG
    ->END
}