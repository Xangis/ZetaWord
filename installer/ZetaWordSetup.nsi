;Include Modern UI

  !include "MUI2.nsh"
  !include "FileAssociation.nsh"

Name "ZetaWord 1.05"
OutFile "ZetaWord1.05Setup.exe"
InstallDir "$PROGRAMFILES\Zeta Centauri\ZetaWord"
InstallDirRegKey HKLM "Software\ZetaWord" "Install_Dir"
RequestExecutionLevel admin
!define MUI_ICON "ZetaPad32.ico"
!define MUI_UNICON "ZetaPad32.ico"

;Version Information

  VIProductVersion "1.0.5.0"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "ZetaWord"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "Zeta Centauri"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "Copyright 2012 Zeta Centauri"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "ZetaWord Installer"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" "1.0.5.0"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductVersion" "1.0.5.0"

;Interface Settings

  !define MUI_ABORTWARNING

;Pages

  !insertmacro MUI_PAGE_LICENSE "License.txt"
;  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
      !define MUI_FINISHPAGE_NOAUTOCLOSE
      !define MUI_FINISHPAGE_RUN
      !define MUI_FINISHPAGE_RUN_CHECKED
      !define MUI_FINISHPAGE_RUN_TEXT "Launch ZetaWord"
      !define MUI_FINISHPAGE_RUN_FUNCTION "LaunchProgram"
      !define MUI_FINISHPAGE_SHOWREADME ""
      !define MUI_FINISHPAGE_SHOWREADME_NOTCHECKED
      !define MUI_FINISHPAGE_SHOWREADME_TEXT "Create Desktop Shortcut"
      !define MUI_FINISHPAGE_SHOWREADME_FUNCTION finishpageaction
  !insertmacro MUI_PAGE_FINISH
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES

;Languages
 
  !insertmacro MUI_LANGUAGE "English"


; The stuff to install
Section "ZetaWord"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File "ZetaWord.exe"
  File "License.txt"
  File "ZetaPad32.ico"
  
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\ZetaWord "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ZetaWord" "DisplayName" "ZetaWord"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ZetaWord" "DisplayVersion" "1.05"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ZetaWord" "Publisher" "Zeta Centauri"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ZetaWord" "DisplayIcon" "$INSTDIR\ZetaPad32.ico"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ZetaWord" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ZetaWord" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ZetaWord" "NoRepair" 1
  WriteUninstaller "uninstall.exe"

  ${registerExtension} "$INSTDIR\ZetaWord.exe" ".txt" "Text File"
  ${registerExtension} "$INSTDIR\ZetaWord.exe" ".ini" "INI File"
  ${registerExtension} "$INSTDIR\ZetaWord.exe" ".config" "Config File"
  ${registerExtension} "$INSTDIR\ZetaWord.exe" ".log" "Log File"
  ${registerExtension} "$INSTDIR\ZetaWord.exe" ".me" "Text File"
  ${registerExtension} "$INSTDIR\ZetaWord.exe" ".rtf" "Rich Text File"
  ${registerExtension} "$INSTDIR\ZetaWord.exe" ".asc" "ASCII Text File"
   
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

  CreateDirectory "$SMPROGRAMS\Zeta Centauri\ZetaWord"
  CreateShortCut "$SMPROGRAMS\Zeta Centauri\ZetaWord\ZetaWord.lnk" "$INSTDIR\ZetaWord.exe" "" "" 0
  ;CreateShortCut "$SMPROGRAMS\Zeta Centauri\ZetaWord\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  WriteINIStr "$SMPROGRAMS\Zeta Centauri\ZetaWord\ZetaWord Website.url" "InternetShortcut" "URL" "http://zetacentauri.com/software_zetaword.htm"
  
SectionEnd

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ZetaWord"
  DeleteRegKey HKLM SOFTWARE\ZetaWord

  ; Remove files and uninstaller
  Delete $INSTDIR\ZetaWord.exe
  Delete $INSTDIR\License.txt
  Delete $INSTDIR\uninstall.exe
  Delete $INSTDIR\ZetaPad32.ico

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\Zeta Centauri\ZetaWord\*.*"
  Delete "$DESKTOP\ZetaWord.lnk"
  Delete "$SMPROGRAMS\Zeta Centauri\ZetaWord\ZetaWord Website.url"
  ;DeleteINISec "$SMPROGRAMS\Zeta Centauri\ZetaWord\ZetaWord Website.url" "InternetShortcut"

  ; Remove directories used
  RMDir "$SMPROGRAMS\Zeta Centauri\ZetaWord"
  RMDir "$SMPROGRAMS\Zeta Centauri"
  RMDir "$INSTDIR"

  ${unregisterExtension} ".txt" "Text File"
  ${unregisterExtension} ".ini" "INI File"
  ${unregisterExtension} ".config" "Config File"
  ${unregisterExtension} ".log" "Log File"
  ${unregisterExtension} ".me" "Text File"
  ${unregisterExtension} ".rtf" "Rich Text File"
  ${unregisterExtension} ".asc" "ASCII Text File"

SectionEnd

Function LaunchProgram
  ExecShell "" "$SMPROGRAMS\Zeta Centauri\ZetaWord\ZetaWord.lnk"
FunctionEnd

Function finishpageaction
  CreateShortcut "$DESKTOP\ZetaWord.lnk" "$instdir\ZetaWord.exe"
FunctionEnd
