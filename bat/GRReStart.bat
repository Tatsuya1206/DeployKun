@ECHO OFF
@REM ##########################################
@REM GRANDIT�ċN��
@REM ��1�����F�A�v���P�[�V�����p�X
@REM ��2�����F�Ώۃu���E�U
@REM ##########################################

@REM �����ō쐬�����A���[�J��GRANDIT��URL��ݒ�
SET GR_URL=%1

IF "%2"=="1" (
    @REM chrome�����I��
    TASKKILL /F /IM chrome.exe /T
    @REM chrome�N��
    START chrome.exe %GR_URL%
    
) ELSE IF "%2"=="2" (
    @REM Edge�����I��
    taskkill /im msedge.exe /f
    @REM Edge�N��
    START msedge.exe %GR_URL%
    
) ELSE (
    @REM Edge�����I��
    taskkill /im msedge.exe /f
    @REM �����I��IE���[�h��Edge���N��
    START msedge.exe "--ie-mode-force" "--internet-explorer-integration=iemode" %GR_URL%
)

EXIT /B 0
