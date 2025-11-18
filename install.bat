@echo off
REM Script para instalar el servicio de Windows TestingWindowsService
REM Requiere permisos de administrador

setlocal enabledelayedexpansion

REM Verificar permisos de administrador
net session >nul 2>&1
if %errorlevel% neq 0 (
    echo.
    echo ===============================================
    echo ERROR: Este script requiere permisos de ADMINISTRADOR
    echo ===============================================
    echo.
    echo Por favor, ejecuta este script como administrador:
    echo 1. Abre CMD o PowerShell como administrador
    echo 2. Navega a la carpeta del servicio
    echo 3. Ejecuta: install.bat
    echo.
    pause
    exit /b 1
)

if "%1"=="" (
    REM Si no se pasa ruta, usar el EXE en la carpeta actual
    set "EXE_PATH=%~dp0TestingWindowsService.exe"
) else (
    set "EXE_PATH=%1"
)

if not exist "!EXE_PATH!" (
    echo.
    echo ===============================================
    echo ERROR: El archivo !EXE_PATH! no existe
    echo ===============================================
    echo.
    echo Uso: install.bat [ruta_del_exe]
    echo Ejemplo: install.bat "C:\Apps\TestService\TestingWindowsService.exe"
    echo.
    pause
    exit /b 1
)

echo.
echo ===============================================
echo Instalando servicio TestingWindowsService...
echo ===============================================
echo.
echo Ruta del ejecutable: !EXE_PATH!
echo.

REM Detener servicio si ya existe
echo Verificando si el servicio ya existe...
sc.exe query TestingWindowsService >nul 2>&1
if !errorlevel! equ 0 (
    echo El servicio ya existe, deteniendo...
    net stop TestingWindowsService >nul 2>&1
    timeout /t 2 /nobreak >nul
    echo Eliminando servicio anterior...
    sc.exe delete TestingWindowsService >nul 2>&1
    timeout /t 2 /nobreak >nul
)

REM Crear directorio de logs
echo Creando directorio de logs...
if not exist "%ProgramData%\TestingWindowsService" (
    mkdir "%ProgramData%\TestingWindowsService"
    echo Directorio creado: %ProgramData%\TestingWindowsService
)

REM Crear el servicio
echo.
echo Registrando servicio...
sc.exe create TestingWindowsService binPath="!EXE_PATH!" start=auto displayname="Testing Windows Service"

if !errorlevel! equ 0 (
    echo.
    echo ===============================================
    echo EXITO: Servicio instalado correctamente
    echo ===============================================
    echo.
    echo Iniciando servicio...
    net start TestingWindowsService
    
    if !errorlevel! equ 0 (
        echo.
        echo ===============================================
        echo Servicio iniciado exitosamente
        echo ===============================================
        echo.
        echo Estado del servicio:
        sc.exe query TestingWindowsService
        echo.
        echo Los logs se guardan en:
        echo %ProgramData%\TestingWindowsService\service.log
        echo.
        timeout /t 35 /nobreak
        echo.
        echo Ver logs en tiempo real:
        echo PowerShell: Get-Content "%ProgramData%\TestingWindowsService\service.log" -Wait
        echo.
    ) else (
        echo.
        echo ERROR: No se pudo iniciar el servicio
        echo Revisa que la ruta del ejecutable sea correcta
        echo.
    )
) else (
    echo.
    echo ===============================================
    echo ERROR: No se pudo instalar el servicio
    echo ===============================================
    echo.
    echo Posibles causas:
    echo - No tienes permisos de administrador
    echo - La ruta del ejecutable es incorrecta
    echo - El puerto ya está en uso
    echo.
)

pause
