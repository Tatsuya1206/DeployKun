@ECHO OFF
@REM ##########################################
@REM GRANDIT再起動
@REM 第1引数：アプリケーションパス
@REM 第2引数：対象ブラウザ
@REM ##########################################

@REM 自分で作成した、ローカルGRANDITのURLを設定
SET GR_URL=%1

IF "%2"=="1" (
    @REM chrome強制終了
    TASKKILL /F /IM chrome.exe /T
    @REM chrome起動
    START chrome.exe %GR_URL%
    
) ELSE IF "%2"=="2" (
    @REM Edge強制終了
    taskkill /im msedge.exe /f
    @REM Edge起動
    START msedge.exe %GR_URL%
    
) ELSE (
    @REM Edge強制終了
    taskkill /im msedge.exe /f
    @REM 強制的にIEモードでEdgeを起動
    START msedge.exe "--ie-mode-force" "--internet-explorer-integration=iemode" %GR_URL%
)

EXIT /B 0
