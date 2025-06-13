# Loopstream
internet radio streaming rig

* see [./stuff/](https://github.com/9001/Loopstream/tree/master/stuff) for plugins and misc

## Maintainers:
`tools.dfc` contains utilities used by Loopstream at runtime, including stuff like `lame.exe` which *probably* shouldn't be in the source repo. So when making commits or pull requests, make sure to leave `tools.dfc` unmodified unless you need to modify the other resource files (web wizard or sfx).

### how to release
* do a release build in visual studio 2013
* run `loopstraem.exe sign` and upload the `.exe.exe`
* release name must be a doujin album / song title
  * very important
