:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Global variables
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


CALL %* 
EXIT /B


:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Global functions
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::




REM REM REM REM
REM REM REM REM
REM REM
REM REM Initialize setup environment variables
REM REM
REM REM REM REM
REM REM REM REM
:Initialization
@echo off
SET SOLUTIONCONFIG=%1
SET ROOT_DIR=%2
SET BUILD_DIR=%3
SET ATTACKSERVICES_DIR=%4
SET OUTPUTFILE=outer.txt


IF "%~1"=="" (
  echo "Usage: %0 DEBUG|RELEASE"
  exit /b 1
)

IF /I NOT "%SOLUTIONCONFIG%" == "debug" (
  IF /I NOT "%SOLUTIONCONFIG%" == "release" (
    echo "Usage: %0 DEBUG|RELEASE"
    exit /b 1
  )
)

IF NOT EXIST "BUILDS" mkdir BUILDS
IF NOT EXIST "BUILDS\DEBUG"   mkdir BUILDS\DEBUG 2> nul
IF NOT EXIST "BUILDS\RELEASE" mkdir BUILDS\RELEASE 2> nul


SET PLUGIN_DIR=%BUILD_DIR%\plugins\
SET DLL_DIR=%BUILD_DIR%\dll\
SET DATA_DIR=%BUILD_DIR%\data\
SET TEMPLATES_DIR=%BUILD_DIR%\templates\
SET PAYLOADS_DIR=%BUILD_DIR%\payloads\
SET IMAGES_DIR=%BUILD_DIR%\images\

if exist "%OUTPUTFILE%" (
  del %OUTPUTFILE%
)

EXIT /B 0




REM REM REM REM
REM REM REM REM
REM REM
REM REM Create directory variables
REM REM
REM REM REM REM
REM REM REM REM

:CreateDirectoryStructure

ECHO Creating new Mianry release %BUILD_DIR%

MKDIR %BUILD_DIR%
MKDIR %PLUGIN_DIR%
MKDIR %DLL_DIR%
MKDIR %ATTACKSERVICES_DIR%
MKDIR %DATA_DIR%
MKDIR %TEMPLATES_DIR%
MKDIR %PAYLOADS_DIR%
MKDIR %IMAGES_DIR%

EXIT /B 0




REM REM REM REM
REM REM REM REM
REM REM
REM REM Copy Minary files
REM REM
REM REM REM REM
REM REM REM REM

:CopyMinaryFiles
ECHO[
ECHO Copying Minary ...

SET MINARY_ROOT=%ROOT_DIR%Minary\bin\%SOLUTIONCONFIG%\
SET MINARY_DEFAULTDATA_ROOT=%ROOT_DIR%Minary\DEFAULT_DATA\
SET MINARYLIB_ROOT=%ROOT_DIR%Libs\MinaryLib\bin\%SOLUTIONCONFIG%\


ECHO copy %MINARY_ROOT%Minary.exe %BUILD_DIR%
COPY %MINARY_ROOT%Minary.exe %BUILD_DIR% >>%OUTPUTFILE%

ECHO copy %MINARY_ROOT%Minary.exe.config %BUILD_DIR% 
COPY %MINARY_ROOT%Minary.exe.config %BUILD_DIR% >>%OUTPUTFILE%

ECHO copy %ROOT_DIR%Minary\Minary.manifest %BUILD_DIR%
COPY %ROOT_DIR%Minary\Minary.manifest %BUILD_DIR% >>%OUTPUTFILE%



ECHO[
ECHO Copying MinaryLib ...

ECHO copy %MINARYLIB_ROOT%MinaryLib.dll %BUILD_DIR%dll\
COPY %MINARYLIB_ROOT%MinaryLib.dll %BUILD_DIR%dll\ >>%OUTPUTFILE%

ECHO copy %MINARYLIB_ROOT%LibGit2Sharp.dll %BUILD_DIR%dll\
COPY %MINARYLIB_ROOT%LibGit2Sharp.dll %BUILD_DIR%dll\ >>%OUTPUTFILE%

ECHO xcopy /s /c /i %MINARYLIB_ROOT%NativeBinaries %BUILD_DIR%dll\
XCOPY /s /c /i %MINARYLIB_ROOT%NativeBinaries %BUILD_DIR%\dll\NativeBinaries\ >>%OUTPUTFILE%


ECHO[
ECHO Copying Default data files ...

ECHO COPY %MINARY_DEFAULTDATA_ROOT%MacVendors.txt %BUILD_DIR%data\ 
COPY %MINARY_DEFAULTDATA_ROOT%MacVendors.txt %BUILD_DIR%data\  >>%OUTPUTFILE%

ECHO copy %ROOT_DIR%Minary\Minary.ico %IMAGES_DIR%
COPY %ROOT_DIR%Minary\Minary.ico %IMAGES_DIR% >>%OUTPUTFILE%

EXIT /B 0






:: a function to verify if a file exists and
:: Ret val  0 : File exists && Size is zero
:: Ret val  1 : File exists && Size zero is not zero
:: Ret val -1 : File does not exist
:: Ret val -2 : Error!

:FileExistsNotZero
SET FILENAME=%1

IF EXIST %FILENAME% (  
  for /F %%A in ("%FILENAME%") do ( 
    If %%~zA equ 0 (
      EXIT /B 0
    )
  )
  EXIT /B 1
) ELSE (
  EXIT /B -1
)
EXIT /B -2