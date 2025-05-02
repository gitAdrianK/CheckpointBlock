[h1]Checkpoint Blocks[/h1]
[hr]
[h2]Description[/h2]
Block that checkpoints your progress. Resets you to the last touched checkpoint or the mapstart if no checkpoint block has been touched yet.
Resets you when the reset block is touched.
[hr]
[h2]Colors[/h2]
[table]
[tr][th]Name[/th][th]Red[/th][th]Green[/th][th]Blue[/th][/tr]
[tr][td]Set 1 - Checkpoint[/td][td]1[/td][td]238[/td][td]124[/td][/tr]
[tr][td]Set 1 - Single use Checkpoint[/td][td]5[/td][td]238[/td][td]124[/td][/tr]
[tr][td]Set 1 - Reset[/td][td]2[/td][td]238[/td][td]124[/td][/tr]
[tr][td]Set 2 - Checkpoint[/td][td]3[/td][td]238[/td][td]124[/td][/tr]
[tr][td]Set 2 - Single use Checkpoint[/td][td]6[/td][td]238[/td][td]124[/td][/tr]
[tr][td]Set 2 - Reset[/td][td]2[/td][td]238[/td][td]124[/td][/tr]
[/table]
[hr]
[h2]Level Tags[/h2]
[b]CheckpointsIgnoreStart[/b] - If you don't want the player to be reset to the start and just reset once at least one checkpoint has been touched.
[hr]
[h2]Information for Creators[/h2]
Only a single pixel of the checkpoint should be placed. The reset point is the bottom center of the block.
The checkpoint block should [b]NOT[/b] be placed against a wall as the King potentially will be inside the wall on reset.
To provide your own texture for checkpoint set 1 a [b]checkpoint.png[/b] should be placed inside your maps folder (next to the level.png at the root level of the map folder). For set 2 a [b]checkpoint2.png[/b]. Worldsmith should automatically convert these to .xnb files.
[hr]
[h2]GitHub Repository[/h2]
View the source code [url=https://github.com/gitAdrianK/CheckpointBlock]here[/url]
