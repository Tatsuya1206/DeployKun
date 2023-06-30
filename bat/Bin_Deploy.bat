@ECHO OFF
@REM ##########################################
@REM �X�V����Ă��� .dll,.pdb���f�v���C
@REM ��1�����F�ړ���BIN�t�H���_�p�X
@REM ��2�����F�ړ���A�v���P�[�V�����p�X
@REM ##########################################

@REM �R�s�[�� �f�B���N�g���ݒ�
SET SOURCE_DIR=%1

@REM �R�s�[�� �f�B���N�g���ݒ�
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