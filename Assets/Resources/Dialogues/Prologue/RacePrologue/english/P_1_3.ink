=== Prologue_1_3_english ===

{ hryc_race_reward:
        ->RaceWithReward
    - else:
        ->RaceWithoutReward_english
}

== RaceWithoutReward_english ==
{ hryc_race_win: ->WitWinsSat | ->HrycWinsSat}


-> END