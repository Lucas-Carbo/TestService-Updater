@echo off
REM Script para desinstalar el servicio de Windows

echo Deteniendo servicio TestingWindowsService...
net stop TestingWindowsService

echo Desinstalando servicio...
sc.exe delete TestingWindowsService

if errorlevel 1 (
    echo Error al desinstalar el servicio
    exit /b 1
) else (
    echo Servicio desinstalado exitosamente
)

pause
