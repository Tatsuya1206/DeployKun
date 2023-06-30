@ECHO OFF
@REM ##########################################
@REM 更新されている .jsをデプロイ
@REM 第1引数：移動元ファイルパス
@REM 第2引数：移動先アプリケーションパス
@REM 第3引数：機能ID
@REM ##########################################

@REM コピー元 ディレクトリ設定
SET FILE_DIR=%1

@REM コピー先 ディレクトリ設定
SET DESTINATION_DIR=%2
SET JS_DIR=\js

IF "%3"=="" (
    SET FIND_TARGET_ID=*
) ELSE (
    @REM 引数で指定された機能IDのファイルのみ対象
    SET FIND_TARGET_ID=%3
)

SET EXIT_CODE=0

SET LOG_DIR=%~dp0\Log
SET SYS_DATE=%date:/=%
SET SYS_TIME=%time%
SET SYS_DATETIME=%SYS_DATE%_%SYS_TIME%
SET LOG_FILE=JS_DEPLOY%SYS_DATE%.log

IF NOT EXIST %LOG_DIR% (
    MKDIR %LOG_DIR%
    TYPE NUL > %LOG_DIR%\%LOG_FILE%
)

FOR /F "delims=" %%a in ('WHERE /R %FILE_DIR% %FIND_TARGET_ID%.js') DO ( 
    XCOPY %%a %DESTINATION_DIR%%JS_DIR% /Y /D >>%LOG_DIR%\%LOG_FILE% 2>&1
    if %ERRORLEVEL% neq 0 (
        SET EXIT_CODE=%ERRORLEVEL%
    )
)

EXIT /B %EXIT_CODE%