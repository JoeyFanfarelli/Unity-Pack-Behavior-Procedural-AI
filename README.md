# Unity-Pack-Behavior-Simple-AI
 Script of interest: NPCMovement.

 In this demo, I provide a procedural form of intelligence to NPCs, akin to animal pack behavior. The overarching idea is that NPCs will flee when they feel outnumbered and become bolder as they gain numbers until they decide they are strong enough to chase the player.
 Raycasts are used in conjunction with overlapshere for npc vision.  
 Navmeshes are used to allow the npc agents to navigate the environment.

 NPCs flee from the player. When they see another NPC (in range), they form a pack (if none exists) or join their pack. 
 If the NPC's pack has less than two members, they still flee. 
 At 2 and 3, they ignore the player. 
 Above 3, they chase the player. 
 
 
