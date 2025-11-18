@echo off
REM Script para desinstalar el servicio de Windows TestingWindowsService
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
    echo 3. Ejecuta: uninstall.bat
    echo.
    pause
    exit /b 1
)

echo.
echo ===============================================
echo Desinstalando servicio TestingWindowsService...
echo ===============================================
echo.

REM Verificar si el servicio existe
sc.exe query TestingWindowsService >nul 2>&1
if !errorlevel! neq 0 (
    echo.
    echo AVISO: El servicio no está instalado
    echo.
    pause
    exit /b 0
)

REM Detener el servicio
echo Deteniendo servicio...
net stop TestingWindowsService
timeout /t 3 /nobreak >nul

REM Eliminar el servicio
echo Eliminando servicio del registro de Windows...
sc.exe delete TestingWindowsService

if !errorlevel! equ 0 (
    echo.
    echo ===============================================
    echo EXITO: Servicio desinstalado correctamente
    echo ===============================================
    echo.
    echo Los archivos permanecen en su ubicación original.
    echo Para eliminar los logs, borra:
    echo %ProgramData%\TestingWindowsService
    echo.
) else (
    echo.
    echo ===============================================
    echo ERROR: No se pudo desinstalar el servicio
    echo ===============================================
    echo.
    echo Asegúrate de:
    echo - Ejecutar como administrador
    echo - Que el servicio no esté en uso
    echo.
)

pause
