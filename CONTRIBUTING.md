# CONTRIBUTING TO APINI

#### By including your written code / assets in a pull request; you agree to submit your code / assets completely under the LICENSE.

## General rules

* **All defnames shall contain apini specific names** To avoid defname collisions with other mods all our defnames should begin with 'Apini', 'Azuri', 'Npini', 'Moobee'. Using Apini as a default is a good option. 

* **Test your changes** Testing your changes locally is incredibly important. Make sure when launching rimworld does not throw any errors.

* Code contributions shall comply with the requirments outlined in the Code Standards and Formatting section.

* Code shall be commented with a high level overview of its function and purpose. Small snippets without comments may be acceptable.

### In addition
* Don't be afraid to ask for help! You can find community links in the README.


### Code Standards and Formatting

#### XML
* Line 1 of all XML files shall be "<?xml version="1.0" encoding="utf-8"?>"
* Children elements shall be indented with 4 spaces more than their parent, the tab character shall not be used. Consider using a writer that turns a tab press into 4 spaces.
* Both the start and end tags of an element shall have the same indentation.
* Abstract parents class structures shall be used for large chunks of code that is repetitive and takes up lots of space, reducing readability. 

#### C#
* All using statements shall be at the beginning of the file.
* All files shall use the Apini namespace.
* Nested statements shall be indented with 4 spaces more than their parent, the tab character shall not be used. Consider using a writer that turns a tab press into 4 spaces.
* Repetitive code shall use loops to increase readability.



## Testing Battery
###### Not required for individual contributions, only for maintainer merges.
* The test fails is an error is encountered, unless it can be definitively proven that the error is not caused by Apini, in which case the test will be deferred to the future, a release shall not be made until this error has been resolved. If the game/mod in question has not been fixed in over a month and this error persists the 
* **This is a very intensive and time consuming test, contributors should not be expected to perform it.**
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
3. Follow Standard test

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
