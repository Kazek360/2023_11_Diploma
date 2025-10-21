INCLUDE ../../../globals.ink

INCLUDE english/Q1_7.ink
INCLUDE polish/Q1_7.ink

{english:
    -> English_Dialogues
  - else:
    -> Polish_Dialogues
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