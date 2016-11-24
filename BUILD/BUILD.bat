@ECHO OFF

SET YEAR=%date:~-4,4%
SET MONTH=%date:~-7,2%
if "%MONTH:~0,1%" == " " set MONTH=0%MONTH:~1,1%
SET DAY=%date:~-10,2%
if "%DAY:~0,1%" == " " set DAY=0%DAY:~1,1%

SET HOURS=%time:~-0,2%
if "%HOURS:~0,1%" == " " set HOURS=0%HOURS:~1,1%
SET MINUTES=%time:~-8,2%
if "%MINUTES:~0,1%" == " " set MINUTES=0%MINUTES:~1,1%
SET SECONDS=%time:~-5,2%
if "%SECONDS:~0,1%" == " " set SECONDS=0%SECONDS:~1,1%

SET TIMESTAMP=%YEAR%%MONTH%%DAY%_%HOURS%%MINUTES%%SECONDS%
SET SOLUTIONCONFIG=%1
SET ROOT_DIR=..\
SET BUILD_DIR=BUILDS\%SOLUTIONCONFIG%\%TIMESTAMP%\
SET ATTACKSERVICES_DIR=%BUILD_DIR%\attackservices\

CLS

ECHO BUILD_DIR: %BUILD_DIR%
IF exist "%BUILD_DIR%" (
  echo The release %BUILD_DIR% exists already. Process aborted.
  GOTO :END
)


REM REM REM REM
REM REM REM REM
REM REM
REM REM Copy Minary files to build directory
REM REM
REM REM REM REM
REM REM REM REM

CALL Scripts\Minary.cmd :Initialization %SOLUTIONCONFIG% %ROOT_DIR% %BUILD_DIR% %ATTACKSERVICES_DIR%
IF %ERRORLEVEL% NEQ 0 GOTO :END

CALL Scripts\Minary.cmd :CreateDirectoryStructure
IF %ERRORLEVEL% NEQ 0 GOTO :END

:: Copy Minary data
CALL Scripts\Minary.cmd :CopyMinaryFiles 
IF %ERRORLEVEL% NEQ 0 GOTO :END



REM REM REM REM
REM REM REM REM
REM REM
REM REM Copy attack service files to build directory
REM REM
REM REM REM REM
REM REM REM REM

:: Copy APE attack services data
CALL Scripts\Attackservices\Ape.cmd :CopyAttackServiceFiles %ROOT_DIR% %ATTACKSERVICES_DIR%
IF %ERRORLEVEL% NEQ 0 GOTO :END

:: Copy ApeSniffer attack services data
CALL Scripts\Attackservices\ApeSniffer.cmd :CopyAttackServiceFiles %ROOT_DIR% %ATTACKSERVICES_DIR%
IF %ERRORLEVEL% NEQ 0 GOTO :END

:: Copy ArpScan attack services data
CALL Scripts\Attackservices\ArpScan.cmd :CopyAttackServiceFiles %ROOT_DIR% %ATTACKSERVICES_DIR%
IF %ERRORLEVEL% NEQ 0 GOTO :END

:: Copy HttpReverseProxy attack services data
CALL Scripts\Attackservices\HttpReverseProxy.cmd :CopyAttackServiceFiles %ROOT_DIR% %ATTACKSERVICES_DIR%
IF %ERRORLEVEL% NEQ 0 GOTO :END



REM REM REM REM
REM REM REM REM
REM REM
REM REM Copy plugins to build directory
REM REM
REM REM REM REM
REM REM REM REM

:: Copy Browser Attack plugin data
CALL Scripts\Plugins\BrowserExploitKit.cmd :CopyPluginFiles

:: Copy DNS Poisoning plugin data
CALL Scripts\Plugins\DnsPoisoning.cmd :CopyPluginFiles

:: Copy DNS Requests plugin data
CALL Scripts\Plugins\DnsRequests.cmd :CopyPluginFiles

:: Copy Firewall plugin data
CALL Scripts\Plugins\Firewall.cmd :CopyPluginFiles

:: Copy HTTP accounts plugin data
CALL Scripts\Plugins\HttpAccounts.cmd :CopyPluginFiles

:: Copy HTTP host mapping plugin data
CALL Scripts\Plugins\HttpHostMapping.cmd :CopyPluginFiles

:: Copy HTTP Requests plugin data
CALL Scripts\Plugins\HttpRequests.cmd :CopyPluginFiles

:: Copy Inject payload plugin data
CALL Scripts\Plugins\InjectPayload.cmd :CopyPluginFiles

:: Copy IP Accounting plugin data
CALL Scripts\Plugins\IpAccounting.cmd :CopyPluginFiles

:: Copy Sessions
CALL Scripts\Plugins\Sessions.cmd :CopyPluginFiles

:: Copy SSLStrip proxy plugin data
CALL Scripts\Plugins\SslStrip.cmd :CopyPluginFiles

:: Copy Systems plugin data
CALL Scripts\Plugins\Systems.cmd :CopyPluginFiles

GOTO :END

:ERROR
ECHO An error occurred while building new packet.
EXIT /B 1

:END

::CALL Scripts\Minary.cmd :FileExistsNotZero

ECHO Build process finished successfully
EXIT /B 0
