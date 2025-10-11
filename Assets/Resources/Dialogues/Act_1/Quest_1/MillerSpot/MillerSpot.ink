INCLUDE ../../../globals.ink

INCLUDE english/Q1_9.ink
INCLUDE polish/Q1_9.ink

{english:
    -> English_Dialogues
  - else:
    -> Polish_Dialogues
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