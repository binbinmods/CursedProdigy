# Seren, the Cursed Prodigy

A hero mod, introducing Seren, a <InsertDescription.> 

This currently does not include any events or quests related to Seren other than those available to all heroes or all heroes of a specific class.

### Known Bugs
- The damage description always appears as if the card is empowered, even if the card would not be

A couple of notes:
## Notes:
- I understand that things are going to be janky at times, and there are definitely bugs that will be worked out
- **What to do if Seren is not unlocked:** Due to some jankiness of the way the code works, Seren is unlocked only for the profile that is open when you launch the game (and for new profiles). So if they aren't unlocked in the correct profile, switch to that profile, close the game and re-open it and they will be unlocked. I'll fix this in the future, but most people won't notice it. You can also just use the profile editor to fix it.
- There are **no character events** for Seren at this time beyond the ones that are available to all characters of a given class (such as pet trainers or healers being able to remove cards at Rest areas).
- Seren's selection location (in the Hero Selection screen) is intentionally in position 5 (the far right). I have not yet automated the process of placing characters, and this is to accommodate other heroes. If you wish to change this, you can access the `cursedprodigy.json` file and the `OrderInList` property with whatever you wish.

This mod relies on [Obeliskial Content](https://across-the-obelisk.thunderstore.io/package/meds/Obeliskial_Content/).

![Summary](https://raw.githubusercontent.com/binbinmods/Bloodrager/refs/heads/main/Assets/gorkSummary.png?token=GHSAT0AAAAAAC7MWJUCABIHA5RA5KNJWWHAZ64I5QQ)



## Installation (manual)

1. Install [Obeliskial Essentials](https://across-the-obelisk.thunderstore.io/package/meds/Obeliskial_Essentials/) and [Obeliskial Content](https://across-the-obelisk.thunderstore.io/package/meds/Obeliskial_Content/).
2. Click _Manual Download_ at the top of the page.
3. In Steam, right-click Across the Obelisk and select _Manage_->_Browse local files_.
4. Extract the archive into the game folder. Your _Across the Obelisk_ folder should now contain a `BepInEx` folder and a `doorstop\libs` folder.
5. Run the game. If everything runs correctly, you will see this mod in the list of registered mods on the main menu.
6. Press F5 to open/close the Config Manager and F1 to show/hide mod version information.
7. Note: I am not certain about these install instructions. In the worst case, just copy `com.binbin.CursedProdigy.dll` into the `BepInEx\plugins` folder, and the `Seren` folder (the one with the subfolders containing the json files) into `BepInEx\config\Obeliskial\importing`

## Installation (automatic)

1. Download and install [Thunderstore Mod Manager](https://www.overwolf.com/app/Thunderstore-Thunderstore_Mod_Manager) or [r2modman](https://across-the-obelisk.thunderstore.io/package/ebkr/r2modman/).
2. Click **Install with Mod Manager** button on top of the page.
3. Run the game via the mod manager.

## Support

This has been updated for Across the Obelisk version 1.5.0.1.

Hope you enjoy it and if have any issues, ping me in Discord or make a post in the **modding #support-and-requests** channel of the [official Across the Obelisk Discord](https://discord.gg/across-the-obelisk-679706811108163701).

## Donation

Please do not donate to me. If you wish to support me, I would prefer it if you just gave me feedback. 