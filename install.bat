@echo off
REM Script para instalar el servicio de Windows

setlocal enabledelayedexpansion

if "%1"=="" (
    echo Uso: install.bat [ruta_del_exe]
    echo Ejemplo: install.bat C:\ruta\a\TestingWindowsService.exe
    exit /b 1
)

if not exist "%1" (
    echo Error: El archivo %1 no existe
    exit /b 1
)

echo Instalando servicio TestingWindowsService...
sc.exe create TestingWindowsService binPath="%1" start=auto displayname="Testing Windows Service"

if !errorlevel! equ 0 (
    echo Servicio instalado exitosamente
    echo Iniciando servicio...
    net start TestingWindowsService
) else (
    echo Error al instalar el servicio
    exit /b 1
)

pause
