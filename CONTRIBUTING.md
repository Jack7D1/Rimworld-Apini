# CONTRIBUTING TO APINI

## General rules

* **All defnames must contain apini specific names** To avoid defname collisions with other mods all our defnames should begin with 'Apini', 'Azuri', 'Npini', 'Moobee'. Using Apini as a default is a good option.

* **Test your changes** Testing your changes locally is incredibly important. Make sure when launching rimworld does not throw any errors.

* **All PRs MUST be to the staging branch** This is so we can update the mod in large updates all at once which are much easier to test and balance.
* If your changes get merged onto staging they are available to those who wish to manually download and play the staging branch, however will not appear on the steam copy until a PR is made to merge staging into master.

### In addition

* Don't be afraid to ask for help! You can find community links in the README.


## Testing Battery
* The test fails is an error is encountered.
* **This is a very intensive and time consuming test, as such contributors should not be expected to perform it in its entirety.**
This is intended for maintainers to perform before merging staging into master to catch any issues that may be encountered by players on release.

### Ignore these errors
* Anything that mentions a butcher spot

### Start here
1. Disable all mods
2. Perform Vanilla test
3. Perform Expansion test

### Vanilla test:
1. Enable Development mode
2. Enable Apini mod and all dependancies
3. Autosort mods and restart
4. Perform Standard test

### Expansion test:
1. Enable all mods claimed as supported on steam page, as well as the royalty content (if owned)
2. Autosort mods and restart
3. Follow Primary Checklist

### Standard test:
1. Create new colony without quickstart with 'Rebuild the Hive' scenario (200x200 size)
2. Perform Autotest list
3. Create new colony without quickstart with 'Settled Azuri Swarm' scenario (200x200 size)
4. Perform Autotest list
5. Create new colony without quickstart with 'Necropini Special Forces' scenario (200x200 size)
6. Perform Autotest list
7. Run Autotest Battle Royale All PawnKinds for 1 round


### Autotest list:
1. Run Autotest Make Colony(full) for ~two in game days
2. Run Autotest Make Colony(animals) for ~one in game hour
3. Run Test generate pawn x1000
