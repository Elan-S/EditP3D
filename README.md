# EditP3D

EditP3D is a viewer and exporter for Radical Entertainment's Pure3D (`.p3d`) files, with a focus on the Prototype series.

The application provides a node-based interface for browsing Pure3D files and allows supported meshes, animations, and textures to be previewed directly within the editor.

## Supported Games

| Game        | Support Level |
| ----------- | ------------- |
| Prototype   | Full          |
| Prototype 2 | Partial       |

Prototype 2 support is currently incomplete. Some files, nodes, or asset types may not load, preview, or export correctly.

## Features

### Mesh Preview

Preview supported geometry directly within the application.

Previewable mesh nodes include:

* Geometry
* PolySkins
* CompositeDrawables

Selecting a supported node displays the corresponding model in the preview window.

### Animation Preview

Animations can be previewed after compatible geometry has been selected.

Select the geometry you want to use, then select an animation node to play the animation on the selected model.

### Texture Preview

Select a supported texture node to display the texture in the preview window.

### Node Inspection

Pure3D files are displayed as a navigable node tree. Selecting a node displays its available properties and allows supported node types to be previewed or exported.

## Controls

### Previewing Assets

Click a node in the node tree to preview it.

Previewable nodes include:

* Geometry
* PolySkins
* CompositeDrawables
* Textures
* Animations, when compatible geometry has already been selected

Additional preview-specific controls can be viewed after selecting a geometry node by pressing `H`.

### Selecting Multiple Nodes

Multiple nodes can be selected using:

* Category selection
* `Ctrl` + click
* `Shift` + click

This allows multiple assets to be exported together.

## Exporting

Select one or more nodes and press `Ctrl+X` to export the current selection.

Multiple-node exporting is supported through category selection or by using `Ctrl` or `Shift` while selecting nodes in the node tree.

An option to export all animations for the currently selected geometry is also available at the top of the application.

## Importing and Saving

EditP3D currently has very limited and experimental importing and saving capabilities.

### Animation Importing

Animations can be partially imported, but the feature is largely untested.

Imported animation data may be incorrect or incompatible with the game. Attempting to use modified animations in-game may produce broken animations, invalid data, or outright crashes.

Use animation importing only for experimentation, and keep backups of any original files.

### Saving Pure3D Files

Saving complete `.p3d` files is currently mostly unsupported.

Saving a single node is less risky than saving an entire file, but single-node saving should still be considered experimental. Saved data may be incomplete, incorrectly reconstructed, or incompatible with the game.

Do not overwrite original game files without keeping a backup.

## Cutscene Preview

EditP3D includes a very basic cutscene preview.

To preview a cutscene, select a cutscene file ending in:

```text
_fig.p3d
```

The selected file should be located in the game's cutscenes folder so that the application can locate its related Pure3D files.

Cutscene preview support is experimental and may not reproduce every part of a cutscene correctly.

## Planned Features

Future development is planned to include:

* Expanded Prototype 2 support
* Support for additional Radical Entertainment games
* Archive viewing and dumping
* `.rz` file decompression

## Credits

EditP3D is based on the original [Gibbed.Prototype](https://github.com/gibbed/Gibbed.Prototype) tools created by Rick "Gibbed" Gibbed.

Special thanks to **Nixson**, who provided significant help with research into the Prototype Pure3D format, as well as dumping and reconstructing the game's fightdata classes.

## License

This project is distributed under the zlib license.

See [`license.txt`](license.txt) for the complete license text.

## Disclaimer

This is an unofficial community project and is not affiliated with or endorsed by Radical Entertainment, Activision, or the publishers and developers of the supported games.
