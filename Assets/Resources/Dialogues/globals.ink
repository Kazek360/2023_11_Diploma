//External functions
EXTERNAL addItem(id, amount)
EXTERNAL removeItem(id, amount)
EXTERNAL changeNestorState(id, isAppearing)
EXTERNAL startClip()
EXTERNAL stopClip()

//localization flag
VAR english = true

//quest progress
VAR prologue = 0

//First race with Hryc flags
VAR hryc_race_reward = false
VAR hryc_race_win = false

//Dad flags
VAR dad_dinner_eaten = false
VAR mill_thief_truth = false