# Valheim Reelable Harpoon

A simple Valheim mod that lets you pull targets with the harpoon by pressing a combination of keys (current Run+Use). This is my first Valheim mod, so I decided to try something that seemed simple. I based my design philosophy around the description of Wendigo's mod [HarpoonReelin](https://thunderstore.io/c/valheim/p/Wendigo/HarpoonReelIn/). So, all this does is patch a couple methods: ```SE_Harpooned.UpdateStatusEffect```and ```Utils.Pull```. 

## What does it do?

When the correct combination of keys is pressed while harpooning, the hooked creature will be pulled towards you. This is accomplished by decreasing the distance between the target and the harpoon's current endpoint. Whenever the harpoon hits a target, it sets how far it is away from the player. So, by deliberately decreasing that distance, we can simulate a pulling behavior without having to move around. I've also allowed players to choose whether they can pull targets "up." Vanilla harpoons only pull targets along the x & z axes so only forward, backward, left, and right. By modifying the ```noUpForce``` parameter to the ```Utils.Pull``` method, harpoons can affect a target's y position.

## Configuration

There are a few configuration options available right now. They should all be self-explanatory. The file is called "ztag96.ValheimReelableHarpoon.cfg." You can also make changes to the config mid-session, depending on the config's lock status. I plan to come back to this README and visualize the different options. I also aim to make the keybindings customizable. The config files should sync across a server, and admins have the ability to lock edits to the config. Keeping the PullSpeed not much higher than 1 facilitates a "lore-friendly" experience.

## Dependencies

All you need is [BepInEx](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/). This was made on v5.4+ if you care.

## Conflicts

This mod will almost definitely conflict with [HarpoonReelin](https://thunderstore.io/c/valheim/p/Wendigo/HarpoonReelIn/) and any other mods that add any sort of Reeling behavior, especially those that modify ```SE_Harpooned``` members. Let me know if you find any other conflicts. 

## AI Disclosure

No vibe coding was done here. Generative AI was used for research into the BepInEx, Harmony, Valheim, and other libraries after solo research failed.
