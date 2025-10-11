INCLUDE ../../../globals.ink

INCLUDE english/Q1_14.ink
INCLUDE polish/Q1_14.ink

{english:
    -> English_Dialogues
  - else:
    -> Polish_Dialogues
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