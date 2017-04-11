# Game Design

## Combat

Combat is an encounter that happens among at least two characters on a person-by-person scale.

Combat is always among two or more sides, fighting each other.

### Initiative

At the start of combat all participants roll for initiative. This is based on a base initiative, levels, bonuses, items and some rng.

Combat happens in turns, starting with the participant with the highest initiative, and then taking turn by result.

The person currentlt taking turn is known as the "Attacker", while enemies are known as "Defenders". If the attacker has friends in the combat, they are known as "allies". During a full round, each character can perform one major attack action and one major defence action. In case of more than two participants, additional actions are always minor.

It is normally only possible to perform one action per turn.

### A round in combat

#### The attacker starts his timer
The attacker have a timer, that starts ticking down when it is his turn. He has until it hits zero to choose an action, though the countdown ends when the timer hits zero.

#### The defender reactis to the attacker.
While the attacker choose what to do, the defender gets to choose a reaction to the attackers action. The defender cannot see how long time the attacker have left to make a decision.
After the attacker has chosen what to do, the defender gets a short time where action is partially known. How long depends on the defenders reaction as well as the attackers commitment.
When there are more than one defender, it is not always known who the attacker will attack. This may allow the defender to try and get an attack in on the attacker, but failing to defend can have lethal consequenses.

#### Allies makes a move
Since allies aren't under attack, it doesn't have to think much about defence. It does however also not have the option to do any attacks. They mostly try to get an overview of the battle.

#### The actions are executed
The attacker starts by taking his action, countered by the primary defender, if any. Other defenders then get to take their actions. Allies then take their action.
All characters that have performed actions that alter initiative have their values updated, but keep their order.

#### Next turn
The next person on the initiative list gets to take an action, with others responding accordingly.
People that flee, surrender, are knocked uncontious or died are removed from the initiative list, and take no more turns. This may or may not end combat.

#### End of round
Af the round has ended, initiative is reordered based on the new initiative scores.

### Ending combat

When a character tries to flee or surrender, his allies get a chance to do the same, before the next turn takes place. If all do so, combat ends.
Should combat not end due to some choosing to remain and fight, those fleeing combat gets a small bonus.

In case a character is knocked uncountios or dies, other characters gets a chance to flee or surrender as well.

Combat also ends when only one side remains able to perform any actions.

### Actions in combat

#### Attack actions
##### Normal attack
Creates a normal attack using normal strength, that doesn't require initiative
Can be used to attack in one of three states:
1) High
2) Middle
3) Low

##### Focused attack
But more strength into attacking one part of the body. Costs initiative.
Can be used to attack in one of three states:
1) High
2) Middle
3) Low

##### Reach Attack
When a character have created distance, but still only is one distance away. Only allowed for weapons with reach. Cost initiative.

##### Ranged attack
Create a ranged attack. Cost initiative. Requires a ranged weapon. Enemies still adjacent treats this as not taking an attack action.

##### Close gap
Assuming it is possible to close the gap, the gap can be closed by one distance for each initiative spent. A normal attack can be made afterwards for an initiative cost.

#### Defence actions
##### Normal defence
This is used for a normal defence against an attack that uses a normal amount of strenght, and doesn't take any initiative.
It can be used to defend in one of three states:
1) High
2) Middle
3) Low

##### Focused defence
This defence can handle strong attacks, but leaves the rest of the body unprotected. Cost initiative.
Can be used to defend in one of three states:
1) High
2) Middle
3) Low

##### Create distance
Creating distance means that the you leave the range of the attacker.
Creating distance will always create space to the attacker, but any attacks made during this acction hits for full.

##### Defend ally
Redirects the defence to an ally, lowering damage dealt to an area, but leaving other defences open.
Can be used to defend an ally in one of three states:
1) High
2) Middle
3) Lower

##### Counterattack
Directly attack the attacker. This is only possible if the attacker performs a non-attacking action, as it otherwise will result in a more fatal hit, or a total miss. Since the counterattack is an reaction to an attack, only the first person to attempt a counterattack hits, and will have no effect for characters in initiative order below the attacked character.

#### Allies actions
##### Focus on character
Puts focus on selected character. By having focus on this character, info about their actions will become available when they make their choice.

#### Major General actions
##### Consider
Gives initiative, but allows no other action, making the person a vulnerable target. Not possible as a minor action.

#### Minor General actions
##### Flee
Attempt to flee the battle. Fleeing allows all attacks made aggainst the target to hit.
Fleeing also negatively affects allies' initiative.

##### Surrender
Tries to end combat, removing the surrenderer if the attacker chooses to accept. Rejecting results in combat continuing as normal.
Attempting to surrender affects allies' initiative negatively.

##### Change weapon
Change to another weapon. Different weapons may have different actions available.
Changing weapon can take multiple turns, though never longer than to the end of a round.

##### Raise/Lower shield
Having a raised shield may lead to better defence, but greatly limit attacking oppotunities.
Raising/Lowering a shield normally takes one turn.

##### Remove/Equip shield.
Sometimes both hands are needed to handle weapon, in which case removing the shield is needed. Or if a hand is free, it is possible to equip a shield in a lowered state.
Removing7Equipping a shield takes until the end of a round, and will nullify an attack/defence action if applicable.

## Warfare

## Unsorted / Unformatted Content
TODO:
Move path finder into unity-independant library to test runtime performance and reuse (+ evt. upload as separate project on github)
Think about current assignment of tasks for classes
Allow player to move though some things that can reserve tile, as long as the player dont stop on it => force player to move to unreserved tile instead (Automatically)
Make tiles reservable, so only one character can be inside it at a time, but still allow pathfinding to move to it/find a way around.
* Make the path finder work so that it can replan route if a tile is blocked and impassable by character.
Separate player from mono behavior

FEATURES:
Inventory, Journal, World map, Character info screens
Basic NPC characters
Basic AI
Basic combat
Basic enemy AI
World Editor
World save / load
Basic quest system
Party mechanics
Skills / Abilities
Basic diplomacy system
Basic intrigue system
House system
Optimal: Basic multiplayer connectivity
Trade system
Currency system
War / army / navy system
Close combat system
* 1v1 sword fighting
* Group fighting
* Melee / ranged attack
Sieges

Design notes:
Inventory:
* Grid based
* Drag and Drop
* Image of character
* Ability to switch between character inventory when in a Party
* Ability to equip characters from screen
* Infobox with info about item
* Inventory limitations
* Item id system

Journal:
* Generel info gathered
* Active quests
* Overview over world events, local events and family events

World map:
* Click and Drag
* Info about known location
* Can also be used for travelling between locations not directly connected
* Map modes
* Character tracker
* Hex based, graph paths
* Hidden and off-road locations not all characters can enter

Diplomacy:
* Relations
* Allies and Enemies

Intrigue:
* Known plots

House:
* Head of House
* House members
* House allies, friends, enemies and wars

// Character browser should be a part of the journal that is already used to keep track of information
Character browser:
* Find character
* location
* Known allies / enemies/ wars
* Dynasty browser

War browser:
* Browse current known wars
* Influence war if involved

Character info screen:
* Icon
* Skills
* Abilities
* Current equipment