PUT THIS RELEASE FOLDER AND ALL OF ITS CONTENTS INTO YOUR HIDMACROS FOLDER FOR THE APPLICATION TO WORK!

Main-Settings.xml will just be for saving settings and such. It will not be used for saving macros.
Universal.xml will be a file with macros that will be used on every application. If there is a macro in one of application specific programs, it will be overridden.

Under the applications folder, every single file should have the format of _____.xml with the underscores being the .exe file name. For example: brave.xml or unity.xml
The program will automagically detect based on that file name to know what to switch too. If a program is not found in there, it will be assigned the Universal.xml macros from the root folder.
.xml files in the applications folder only need to contain <Config><Macros></Macros></Config>

The settings only saves across for the variables listed below:
Language
ScriptLanguage
ProcBegin
ProcEnd
MinimizeToTray
StartMinimized
AllowScriptGUI