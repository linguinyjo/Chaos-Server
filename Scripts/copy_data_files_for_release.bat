@echo off
setlocal enabledelayedexpansion

:: Source and destination base paths
set "SOURCE_BASE=C:\Chaos-Server\Data\Configuration"
set "DEST_BASE=C:\Chaos-Release\Data-Latest\Configuration"

:: Folders to copy (excluding Templates with Maps)
set "FOLDERS=LootTables MapInstances MetaData WorldMaps WorldMapsNodes"

:: Create exclude file for Templates
echo Maps\>exclude.txt

:: Clear destination directory first
echo Clearing destination directory...
if exist "%DEST_BASE%" (
    rmdir /s /q "%DEST_BASE%"
)
mkdir "%DEST_BASE%"

:: Copy each folder
for %%F in (%FOLDERS%) do (
    echo Copying %%F...
    if exist "%SOURCE_BASE%\%%F" (
        xcopy "%SOURCE_BASE%\%%F" "%DEST_BASE%\%%F" /E /I /Y
    ) else (
        echo Warning: Folder %%F does not exist in source!
    )
)

:: Copy Templates folder with exclusion
echo Copying Templates excluding Maps...
xcopy "%SOURCE_BASE%\Templates" "%DEST_BASE%\Templates" /E /I /Y /EXCLUDE:exclude.txt

:: Clean up exclude file
del exclude.txt

echo Folder migration complete.
pause