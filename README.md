# Random Spell Merchant

I always felt it was weird that spell merchants have every spell in their inventory, so I decided to change that. Random Spell Merchant is a Skyrim Synthesis patcher that randomizes which spell tomes merchants sell and lets you customize how the lists are generated.

Instead of every merchant having nearly every spell available, spells are divided by school, tier, and rarity. The patcher then rebuilds the merchant inventories using Skyrim's own leveled-list system. It can also include spells from supported mods that are enabled in your load order.

The project is written in **C#** using **Synthesis** and **Mutagen**.

## Important notes

**Supported spell mods:** My currently documented support includes vanilla Skyrim/DLC, Anniversary Edition / Creation Club content, Mysticism, Apocalypse, Odin, Triumvirate, Lost Grimoire, and Bound Armory Extravaganza. Some extra spell IDs, including some possibly from Animated Armory, are also present. There are additional mod-specific arrays in the source, but I haven't confirmed every one of those as fully supported.

**Unsupported spell mods:** If you use another spell mod, its spells may no longer appear in the **vanilla merchants'** inventories that this patcher changes. Mod-added merchants are not currently changed by the patcher and should continue using their existing lists, unless another mod changes them.

**Missing spell warnings:** If the patcher says some spell tomes are missing, that doesn't necessarily mean something is broken. Spell mods sometimes rename or replace Editor IDs, and the patcher looks up the IDs listed in its code against the records available in your current load order. Mysticism is a common example because it replaces many vanilla spell records. Missing IDs are skipped.

## Which version should I use?

- **Main ESP:** For anyone who wants the default setup without running Synthesis.
- **Synthesis patcher:** Recommended if you want to change rarity, the number of rolls, higher-tier restrictions, or generate the lists specifically for the supported spell mods in your load order.

**You only need one version.**

## Installation

**Main ESP:** Download and install the main file through your mod manager, or however you normally install mods.

**Synthesis patcher:** Download the patcher from the files, or add it through Synthesis. Run the patcher after your load order is finalized so it can check which supported mods are enabled.

For more information about setting up Synthesis, see the [Synthesis installation guide on Nexus Mods](https://www.nexusmods.com/skyrim/articles/52942).

## How it works

The basic idea is to use Skyrim's leveled lists and their **Chance None** setting to control how frequently spells appear.

Each spell tier and school has three rarity groups: **Common, Rare, and Very Rare**. Each group has its own Chance None modifier. For example, if Common is set to 50, each roll on that Common list has a 50% chance to contain nothing and a 50% chance to select a spell tome.

I then give each list a number of rolls to determine how many chances it gets to add a spell to the merchant's inventory. The number of rolls depends on how many spells are in the list, its rarity, and its spell tier. Rarer spells and higher-tier spells get fewer rolls.

### How the number of rolls is calculated

First, the base number of rolls is calculated from the number of spells in the list:

| Rarity | Base rolls |
| --- | --- |
| Common | `(Number of spells / 2) × Roll Multiplier` |
| Rare | `(Number of spells / 2 / 2) × Roll Multiplier` |
| Very Rare | `(Number of spells / 2 / 3) × Roll Multiplier` |

Then that number is reduced based on spell tier:

| Spell tier | Final roll calculation |
| --- | --- |
| Novice | `Base rolls` |
| Apprentice | `Base rolls / Spell Tier Roll Divisor` |
| Adept | `Base rolls / (Spell Tier Roll Divisor × 2)` |
| Expert | `Base rolls / (Spell Tier Roll Divisor × 3)` |
| Master | `Base rolls / (Spell Tier Roll Divisor × 4)` |

The final number is always **rounded down**, with a **minimum of 1 roll** as long as the list contains at least one spell. Empty lists get 0 rolls. This means that even an extremely rare or high-tier spell still has at least one chance to appear if it's in a nonempty list.

### Customizable settings

The Synthesis patcher has an automatically generated settings interface where you can change:

- **Common, Rare, and Very Rare Chance None:** How likely each rarity group is to produce nothing on a roll.
- **Overall Roll Multiplier:** How many rolls the lists get overall.
- **Spell Tier Roll Divisor:** How much higher spell tiers reduce the number of rolls.
- **Spell Level Restrictions:** Whether to use the spell-skill restrictions on the generated leveled lists.

You can change these however you want. With larger groups of spell mods, I'd suggest starting with my recommended settings, as they should work reasonably well.

If you want more Common spells, increasing the Overall Roll Multiplier should give you more spells without changing the balance between rarities too much.

With smaller spell lists, I'd suggest increasing both Chance None and the Overall Roll Multiplier. This gives those lists more chances to roll, while the higher Chance None helps keep the actual number of spell tomes from getting too high. Otherwise, smaller rare or high-tier lists tend to end up with the minimum of one roll, which reduces the variation between them.

The patcher also prints an **estimated number of books** for each merchant group so you can experiment with the settings until the balance looks right to you. These are estimates, not guaranteed inventory counts.

## Merchants affected

The patcher currently changes the following groups of existing merchants:

1. **Court wizards:** General spell lists and a specialty list with more spells from their particular school.
2. **College of Winterhold merchants:** School-specific inventories, with additional specialty rolls and chances for higher-tier spells.
3. **General merchants:** Merchants such as Belethor. These use a simpler general spell setup and currently have fewer balancing options than the specialist merchants.
4. **Talvas:** Talvas-specific lists along with Destruction and Conjuration specialty rolls.
5. **Enthir:** Vampire, Illusion, and Destruction specialty rolls.
6. **Florentius:** Vampire Hunter specialty rolls.
7. **Feran:** Vampire specialty rolls.
8. **Nelacar:** Restoration and Conjuration specialty rolls.

These are specific merchant records handled by the patcher, not a search that automatically adds support for every merchant in the game.

## Compatibility

The patcher replaces selected vanilla merchant spell-tome leveled lists with newly generated lists. Mods that modify or replace those same lists may conflict or have their changes overwritten by the generated patch.

The patcher also depends on spell-tome **Editor IDs**. If another mod changes those IDs, some tomes may not be found and will be skipped. The patcher checks the active load order for supported spell mods and uses records available from that load order.

**Mod-added merchants are not currently supported.** They should keep their existing inventory behavior. I may add support for merchants from Beyond Bruma, Beyond Reach, Wyrmstooth, and other mods later, especially since I already use some of these in my own mod list.

## Supported spell mods

These are the mods I have listed as currently supported or included in my setup:

- Vanilla Skyrim, DLC, and Anniversary Edition / Creation Club spells
- Mysticism
- Apocalypse
- Odin
- Triumvirate — these spells are placed in Very Rare lists so they can appear randomly; the patcher doesn't change Triumvirate's own special merchants
- Lost Grimoire
- Bound Armory Extravaganza

There are also a few additional spell-tome entries, including bound-weapon spells that may come from Animated Armory. I still need to confirm exactly which mod provides all of them.

## For developers / adding more spells

It is actually fairly easy to add new spells to the patcher, although the current setup requires editing the C# code.

I have arrays containing spell-tome **Editor IDs** for each mod, sorted by spell tier, magic school, and rarity. Those arrays get combined when the patcher builds the new lists.

For a bigger spell mod, you can create a separate set of arrays and add them to the combining logic. I have already created sections for some additional spell mods that aren't in my confirmed support list, so check before making new ones.

For smaller mods, you can add spells to the arrays whose names start with `additional`. Just put the spell tome's Editor ID in the correct school, tier, and rarity group. The additional arrays are already included when the lists are built.

I plan to make this easier by allowing you to enter additional spell tomes through the Synthesis settings, rather than having to edit the code.

## Planned additions

- Allow additional spell tomes to be entered through the Synthesis settings.
- Add more supported spell mods if requested.
- Potentially add support for modded merchants, including ones from Beyond Bruma, Beyond Reach, and Wyrmstooth.
- Publish my separate **Spell Getter** diagnostic patcher. It is intended to look through merchant inventories, identify the spells they contain, and write that information to a text file. It was largely AI-generated, so I still want to test and clean it up before presenting it as a finished tool.

## Development notes

This is my first mod, and I really didn't know what I was doing when I started. I used AI quite a bit to learn how Synthesis and the Skyrim records worked, as well as for repetitive typing tasks like building the spell arrays. The balancing choices and overall design are mine, and I worked through the code to get the patcher behaving how I wanted it to.

I also didn't know about **Rarer Spell Tomes**, another mod that does something similar, until I was about halfway through. I decided to keep working on this one because I wanted the customizable system and support for spells from other mods.
