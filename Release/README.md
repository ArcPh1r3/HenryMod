# HenryMod
- Adds Henry, a stickman who fights with a combination of sword and gun
- Designed primarily to use as a base for creating your own custom characters, codebase is modular and easy to understand while easily supporting multiple characters and even enemies eventually- in other words, Example Survivor 2
- Has a bunch of alternate skills and unlockable skins

[![](https://cdn.discordapp.com/attachments/469291841859092488/815574716630695946/unknown.png)]()

[![](https://raw.githubusercontent.com/ArcPh1r3/HenryMod/main/Release/Readme/screens1.png)]()
[![](https://raw.githubusercontent.com/ArcPh1r3/HenryMod/main/Release/Readme/screens2.png)]()

[![](https://cdn.discordapp.com/attachments/469291841859092488/807476979061489664/texHenryIcon.png)]()

join the discord to share feedback: https://discord.gg/Xacmgm7KXM

## Tutorial

In-depth character creation tutorial can be found here: https://github.com/ArcPh1r3/HenryTutorial/blob/master/README.md  
This tutorial is fixed to work with the current update, and will be maintained.

## Survivors of the Void Update
Henry is back!  

In the update for SotV some things were cut for now (namely nemry), and some things might be missing or a little off (namely sound and camera stuff). Let me know in the discord link above if you come across any of this.  
Aside from that, gameplay wise he's back up and running.

Also, as a gift from rob: 
[![](https://raw.githubusercontent.com/ArcPh1r3/HenryMod/main/Release/Readme/screens3Skins.png)]()
These skins are outfits and enemies from the game rob's been working on since he left ror2 modding. Enable them in config "Extra Skins".

Check out his game via his youtube channel here:  
[![epic game development](http://img.youtube.com/vi/TN9A5lcT_hc/0.jpg)](http://www.youtube.com/watch?v=TN9A5lcT_hc)

Thanks for stickin around, have fun, love ya - Timesweeper

## Skills

#### Primary

[![](https://cdn.discordapp.com/attachments/469291841859092488/826844941706854460/unknown.png)]()  
[![](https://cdn.discordapp.com/attachments/469291841859092488/826843527089881088/unknown.png)]()  
[![](https://cdn.discordapp.com/attachments/469291841859092488/826843622326534214/unknown.png)]()  

#### Secondary

[![](https://cdn.discordapp.com/attachments/469291841859092488/826843434664067133/unknown.png)]()  
[![](https://cdn.discordapp.com/attachments/469291841859092488/826843546274758706/unknown.png)]()  
[![](https://cdn.discordapp.com/attachments/469291841859092488/826843647613337640/unknown.png)]()  

#### Utility

[![](https://cdn.discordapp.com/attachments/469291841859092488/826843457137803304/unknown.png)]()  
[![](https://cdn.discordapp.com/attachments/469291841859092488/826843568357376030/unknown.png)]()

#### Special

[![](https://cdn.discordapp.com/attachments/469291841859092488/826843480089296956/unknown.png)]()  
[![](https://cdn.discordapp.com/attachments/469291841859092488/826843588474044426/unknown.png)]()  
[![](https://cdn.discordapp.com/attachments/469291841859092488/830875746431008818/unknown.png)]()


#### Ancient Scepter

* Bomb > More Bombs! | Cooldown is halved and carry capacity is increased to 4
* Bazooka > Armageddon | Fire two Bazookas at once
* Rampage > Super Saiyan | Become real Super Sand

## Credits
rob - everything  
PapaZach- Skill icons  
OK - Mod icon  
Rein - A ton of help getting things up and running again  
TheTimesweeper - Gupdate fix and maintaining  
~~Linkin Park - Music~~

## Changelog

`2.1.2`
 - grandmastery now achievable with eclipse and inferno
 - fixed eclipse not saving progress

`2.1.1`
 - moved unlockable code to r2api, fixing achievement issue with recent update 
 - fixed achievements in logbook marked as ??? even with henry unlocked

`2.1.0`
- Fixed for survivors of the void
  - secret character removed for now
  - sounds had to be redone, may be too loud/quiet in some places
  - let me know if any other issues arise
- Added 4 new skins in config, courtesy of rob and the game he's working on.
- made melee hitboxes consistently face foward while sprinting, making these attacks more consistent while sprinting sideways
- more things that I'm forgetting I'm sure

`2.0.0`
- Updated some skill effects
- Tweaked aim origin some more

`1.3.6`
- Fixed Rampage effect config for real this time

`1.3.5`
- Tweaked aim origin to help shots land more consistently
- Fixed Rampage effect config not actually doing anything

`1.3.4`
- Added config option to disable Rampage's visual effects, for those having issues with it
- Stinger now properly generates Fury when used with Sword

`1.3.3`
- Fixed Super Saiyan breaking and becoming unusable when going to a new stage

`1.3.2`
- Restored Ancient Scepter functionality

`1.3.1`
- Fixed to work on latest RoR2 update

`1.3.0`
- Added a new special: Rampage
- Added more Boxing Gloves sounds
- Rewrote emote camera stuff to make it more smooth

`1.2.6`
- Fixed all achievements being broken after some internal code refactors

`1.2.5`
- Fixed `200% Motivated` achievement counting the hidden Drizzle/Monsoon items and not granting the unlock

`1.2.4`
- Added more Stinger sounds
- Added Uzi reload animation
- Added new Pistol skill icon
- Updated Boxing Gloves high attack speed animation
- Pistol base duration 0.4s > 0.3s
- Uzi damage 50% > 75%
- Uzi proc coefficient 0.5 > 0.75
- Bazooka max damage 1400% > 1600%

`1.2.3`
- Fixed `200% Motivated` achievement not working properly due to the moon scene name being changed internally
- Removed bundled MMHook and added a dependency to Standalone MMHook

`1.2.2`
- Fixed achievements
- Fixed item displays
- Restored old Roll behavior

`1.2.1`
- Fixed the mod to work on the latest RoR2 version
- Unfortunately a few things had to be dropped, mainly item displays and achievements, with plans to add them back in at a later time
- Roll armor buff removed for now- invulnerability now lasts for the entire duration

`1.2.0`
- Added Ancient Scepter upgrade for Bazooka
- Added new utility: Shotgun
- Added custom run animation for Boxing Gloves
- Added new animations for high attack speed Boxing Gloves
- Added combo attacks for Boxing Gloves' Stinger, only usable in close range
- Added a custom indicator for Stinger targeting
- Made Stinger targeting much more consistent
- Fixed Pistol being held sideways
- Roll invulnerability duration halved

`1.1.2`
- Networked Bazooka

`1.1.1`
- Added alternate animation for fully charged Bazooka shots
- Remade strafe animations
- Remade Pistol animation
- Tweaked Dante skin's texture
- Pistol fire rate increased
- Bazooka damage 400-1200% > 600-1400%
- Bazooka crosshair adjusted

`1.1.0`
- Added Pistol primary
- Added Bazooka special
- Renamed Pistol secondary to Handgun
- Bomb cooldown 8s > 10s
- Sword damage 350% > 280%
- Boxing Gloves damage 280% > 240%
- Handgun damage 480% > 420%
- Stinger damage 500% > 450%
- Adjusted volume on some sounds
- Networked Bomb sound

`1.0.0`
- Tweaked slash and punch animations
- Added a new skin
- Stinger now only appears usable when locked onto a target
- Stinger now takes priority over other skills

`0.1.2`
- Networked emotes
- Added a new mod icon by OK

`0.1.1`
- Actually added new skill icons oops

`0.1.0`
- Added new skill icons, thanks to PapaZach
- Increased Stinger range

`0.0.9`
- Added new secondary- Uzi
- Added Ancient Scepter support
- Added Rest emote(bound to 1 by default)
- Added Dance emote(bound to 3 by default)
- Added placeholder skill icons for Boxing Gloves and Stinger
- Updated Stinger visuals, sfx and fixed a rare bug
- Increased Boxing Gloves damage from 240% to 280%
- Made Pistol and Bomb able to be used separately from primary without interrupting it
- Updated a bunch of animations
- Fixed gun item displays being visible even when there is no gun
- Fixed Bomb sound falling off from too short a distance

`0.0.8`
- Fixed Stinger getting stuck forever, softlocking the whole character
- Lowered Stinger knockback
- Lowered Stinger cooldown from 4s to 3s

`0.0.7`
- Added Grand Mastery skin
- Remade sword and boxing gloves VFX
- Secondary now counts as a combat skill
- Added a new melee secondary skill
- Added doppelganger
- Added aim and landing animations
- Separated gunshot animation from sword swings so they play more smoothly together

`0.0.6`
- Completed item displays
- Updated Boxing Gloves hit effect

`0.0.5`
- Added a handful of item displays
- Added work in progress Boxing Gloves primary
- Added sword effects
- Updated bomb effects
- Changed sword from bendy to fluid smear

`0.0.4`
- Added custom sounds
- Added a custom Bomb projectile
- Increased primary hitbox size
- Animation tweaks, added second primary attack
- Added character select animation
- Updated mastery skin, added unique model
- Tweaked bandana physics, added collision
- Fixed teleporter particles being huge
- Fixed primary not dealing damage with too much attack speed

`0.0.3`
- Added skill icons
- Added keywords
- Made secondary agile

`0.0.2`
- Added custom skills
- Added ragdoll
- Added skin and achievement icons
- Animation tweaks

`0.0.1`
- Initial release