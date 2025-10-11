INCLUDE ../../../globals.ink

INCLUDE english/brother_room_1.ink
INCLUDE english/brother_room_2.ink

INCLUDE polish/brother_room_1.ink
INCLUDE polish/brother_room_2.ink

{english:
    -> English_Dialogues
  - else:
    -> Polish_Dialogues
}

=== English_Dialogues ===
{brother_room_interacted:
    -> brother_room_2_english
  - else:
    -> brother_room_1_english
}

=== Polish_Dialogues ===
{brother_room_interacted:
    -> brother_room_2_polish
  - else:
    -> brother_room_1_polish
}