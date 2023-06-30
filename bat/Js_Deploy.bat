@ECHO OFF
@REM ##########################################
@REM �X�V����Ă��� .js���f�v���C
@REM ��1�����F�ړ����t�@�C���p�X
@REM ��2�����F�ړ���A�v���P�[�V�����p�X
@REM ��3�����F�@�\ID
@REM ##########################################

@REM �R�s�[�� �f�B���N�g���ݒ�
SET FILE_DIR=%1

@REM �R�s�[�� �f�B���N�g���ݒ�
SET DESTINATION_DIR=%2
SET JS_DIR=\js

IF "%3"=="" (
    SET FIND_TARGET_ID=*
) ELSE (
    @REM �����Ŏw�肳�ꂽ�@�\ID�̃t�@�C���̂ݑΏ�
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