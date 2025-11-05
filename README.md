# SamSWAT's Anime Iron Sight 4.0 Port
This mods adds new Rear Iron Sights with anime art at the back, so you can fulfill your dream to raid with your Waifu.

## Features
+ Adds 24 rear sights with anime girls on them. 8 for MCX and 16 for MBUS weapons.
+ Rear sights are sold by the Mechanic and on the Flea Market
+ Easily add new and edit current iron sights by editing JSON files (guide below)

## Known Issues
+ Added sights does not spawn in raids
+ Internationalization support not working

## Installation
1. Download the latest versions on the Releases page
2. Extract the downloaded ZIP file on the root of your SPT Installation
3. Start up the server

## How to add new sights or edit them
This mod has a small JSON interpreter that allows you to add and edit new back iron sights into the game.
### items.jsonc
All you have to do is open `db/items.jsonc` with your preferred text editor and add the follow structure for your iron sight:
```json
{
  "id": "YOUR_ITEM_MONGOID",
  "clone_from_tpl": "ITEM_TO_CLONE_FROM_MONGOID",
  "bundle_path": "path/to/your/bundle.bundle",
  "price": 4000, // Item price. This will be used on Handbook and Mechanic
  "ergonomics": 3 // Ergo boost. You can set this to any value, but play nice ;) (or dont, I'm not the SPT police)
}
```
### Locales
Then, you need to add the Name, ShortName and Description of your item, otherwise it will have a weird name.<br>
To do that, open `db/locales/en.json` with your preferred text editor and add the follow structure:
```json
"YOUR_ITEM_MONGOID": {
    "name": "Your beautiful item name",
    "short_name": "Short name",
    "description": "Very detailed and incredible item description that will give the player a lot of information and totally not be useless"
}
```

### Contribuiting
(WIP)<br>
To contribuite to this project, please follow good coding standards. Make sure your commits
follow [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) and that all PRs are done to the `develop` branch

## Credits
+ [**Rexana**](https://forge.sp-tarkov.com/user/38525/rexana) - For giving the idea.
+ [**GrooveyPenguinX**](https://forge.sp-tarkov.com/user/30890/grooveypenguinx) - For making [WTT-BundleMaster](https://github.com/GrooveypenguinX/WTT-BundleMaster), used to fix the original bundles
+ [**SamSWAT**](https://forge.sp-tarkov.com/user/200/samswat) - For making the [original mod and bundles](https://forge.sp-tarkov.com/mod/316/anime-rear-sights)
+ [**ApertureAwesome**](https://steamcommunity.com/profiles/76561198224517144) - [Girls Frontline M4 SOPMOD II](https://steamcommunity.com/sharedfiles/filedetails/?id=1628771637).
+ [**ApertureAwesome**](https://steamcommunity.com/profiles/76561198224517144) - [Ram & Rem](https://steamcommunity.com/sharedfiles/filedetails/?id=1636845240)
+ [**Farengar**](https://steamcommunity.com/profiles/76561198020115276), [**tac.error**](https://steamcommunity.com/profiles/76561198057580968), [**Rymd**](https://steamcommunity.com/profiles/76561198104088584) - [Ai Fuyuumi](https://steamcommunity.com/sharedfiles/filedetails/?id=1484273642) 
+ [**Sumerikan**](https://www.reddit.com/user/SUMERIKAN/) - [Art for Anti 1 & 2](https://www.reddit.com/r/PhantomForces/comments/cenaum/animu_sight_anti/)
+ And one more unknown artist...