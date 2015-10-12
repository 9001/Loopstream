setlocal enabledelayedexpansion enableextensions
set LIST=
for %%x in (%baseDir%\*.ogg) do set LIST=!LIST! %%x
set LIST=%LIST:~1%
echo %LIST%
