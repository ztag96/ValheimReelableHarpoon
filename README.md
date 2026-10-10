# Valheim Reelable Harpoon

A simple Valheim mod that lets you pull targets with the harpoon by pressing a combination of keys (currently Run+Use or Shift+E). This is my first Valheim mod, so I decided to try something that seemed simple. I based my design philosophy around the description of Wendigo's mod [HarpoonReelin](https://thunderstore.io/c/valheim/p/Wendigo/HarpoonReelIn/). So, all this does is patch a couple methods: ```SE_Harpooned.UpdateStatusEffect```and ```Utils.Pull```.

![video1](https://media4.giphy.com/media/v1.Y2lkPTc5MGI3NjExNnFmdHN1djJ5Y3lranA4cnQ3ZmthdGJremZwdWU2cHc1c3AyOWFkeiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/duIsOcucZt6omQ30Om/giphy.gif)

## How to Use

1) Harpoon a creature normally.
2) Hold Shift+E, or Run+Use, to the pull creature towards you.
* Your stamina will drain while pulling a creature. The drain rate is the same as vanilla.
* You can only pull a creature up to the MinDistance which is set in the configuration.

## What does it do?

The harpoon's distance from the player is saved when it attaches to a creature. By deliberately decreasing that distance, we can simulate pulling. I've also allowed players to choose whether they can pull targets "up." Vanilla harpoons only pull targets along the x & z axes, so only forward, backward, left, and right. By modifying the ```noUpForce``` parameter to the ```Utils.Pull``` method, harpoons can affect a target's y position.

## Configuration

The config file can be found at ```<valheim_install_path>/BepInEx/config/ztag96.ValheimReelableHarpoon.cfg``` You can also make changes to the config mid-session. The config files should sync across a server, and admins have the ability to lock edits to the config. Keeping the PullSpeed not much higher than 1 facilitates a "lore-friendly" experience.

|Option|Description|Default Value|
|------|-----------|-------------|
|IsLocked|Whether the config is locked and cannot be edited.|true|
|PullSpeed|The speed at which the harpoon will reel in.|1|
|MinDistance|The closest a creature can be reeled.|5|
|CanPullUp|If the harpoon can pull creatures up or down.|true|

## Dependencies

All you need is [BepInEx](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/). This was made on v5.4+ if you care. Confirmed Compatibility is only with Valheim 1.0+.

## Conflicts

This mod will almost definitely conflict with [HarpoonReelin](https://thunderstore.io/c/valheim/p/Wendigo/HarpoonReelIn/) and any other mods that add any sort of Reeling behaior, especially those that modify ```SE_Harpooned``` members. Let me know if you find any other conflicts.

## Bugs?

Post an Issue on the [GitHub Repository](https://github.com/ztag96/ValheimReelableHarpoon).

## AI Disclosure

No vibe coding here. Generative AI was used for research into the BepInEx, Harmony, Valheim, and other libraries after solo research failed. Debugging was also assisted by AI.
