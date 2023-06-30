@ECHO OFF
@REM ##########################################
@REM 更新されている .dll,.pdbをデプロイ
@REM 第1引数：移動元BINフォルダパス
@REM 第2引数：移動先アプリケーションパス
@REM ##########################################

@REM コピー元 ディレクトリ設定
SET SOURCE_DIR=%1

@REM コピー先 ディレクトリ設定
SET DESTINATION_DIR=%2

SET EXIT_CODE=0

SET LOG_DIR=%~dp0\Log
SET SYS_DATE=%date:/=%
SET SYS_TIME=%time%
SET SYS_DATETIME=%SYS_DATE%_%SYS_TIME%
SET LOG_FILE=BIN_DEPLOY%SYS_DATE%.log

IF NOT EXIST %LOG_DIR% (
    MKDIR %LOG_DIR%
    TYPE NUL > %LOG_DIR%\%LOG_FILE%
)

ROBOCOPY %SOURCE_DIR% %DESTINATION_DIR%/bin *.dll *.pdb /R:1 /W:5 >>%LOG_DIR%\%LOG_FILE% 2>&1
if %ERRORLEVEL% neq 0 (
    SET EXIT_CODE=%ERRORLEVEL%
)

EXIT /B %EXIT_CODE%