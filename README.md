# TestingWindowsService v0.1.0

Servicio de Windows para monitoreo automático. Se ejecuta como un servicio del sistema operativo y registra eventos cada 30 segundos en un archivo de log.

## Características

- ✅ Se ejecuta como servicio de Windows
- ✅ Logs detallados guardados en `%APPDATA%`
- ✅ Compilación automática en releases
- ✅ ZIP descargable desde GitHub Actions
- ✅ Fácil instalación y desinstalación
- ✅ Actualización automática desde otro servicio

## Compilar Localmente

```bash
dotnet build -c Release
```

## Instalar como Servicio de Windows

```powershell
# Desde PowerShell como administrador
.\install.bat ".\bin\Release\net8.0\TestingWindowsService.exe"
```

O manualmente:

```powershell
# Iniciar el servicio
net start TestingWindowsService

# Detener el servicio
net stop TestingWindowsService

# Desinstalar el servicio
sc.exe delete TestingWindowsService
```

## Desinstalar

```powershell
.\uninstall.bat
```

## Verificar los Logs

Los logs se guardan en:
```
C:\ProgramData\TestingWindowsService\service.log
```

Puedes verlos con:
```powershell
Get-Content "C:\ProgramData\TestingWindowsService\service.log" -Wait
```

## Crear una Release en GitHub

1. Asegúrate de que todos los cambios estén committeados
2. Crea un tag con la versión:
   ```bash
   git tag -a v0.2.0 -m "Release version 0.2.0"
   git push origin v0.2.0
   ```
3. El workflow de GitHub Actions compilará automáticamente el proyecto y creará:
   - Un `.zip` con todos los binarios (para actualizaciones automáticas)
   - Un `.exe` compilado para instalación directa

## 📦 Descarga desde GitHub API

Para descargar la última release en formato ZIP:

```powershell
# Obtener información de la última release
$release = Invoke-RestMethod -Uri "https://api.github.com/repos/Lucas-Carbo/TestService-Updater/releases/latest"

# Descargar el ZIP
$zipUrl = ($release.assets | Where-Object {$_.name -like "*.zip"}).browser_download_url
Invoke-WebRequest -Uri $zipUrl -OutFile "update.zip"

# Extraer y actualizar
Expand-Archive -Path "update.zip" -DestinationPath "C:\update\path" -Force
```

## Requisitos

- Windows 10/11 o Windows Server
- .NET 8.0 Runtime

## Uso Típico

El servicio se inicia automáticamente después de instalarlo y genera eventos de monitoreo cada 30 segundos en el archivo de log.

Para detenerlo:
```powershell
net stop TestingWindowsService
```

Para iniciarlo nuevamente:
```powershell
net start TestingWindowsService
```
