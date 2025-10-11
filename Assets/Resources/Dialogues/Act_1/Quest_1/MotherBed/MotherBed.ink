INCLUDE ../../../globals.ink

INCLUDE english/Q1_1.ink
INCLUDE polish/Q1_1.ink

{english:
    -> English_Dialogues
  - else:
    -> Polish_Dialogues
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