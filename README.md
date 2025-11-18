# TestingWindowsService v0.1.0

Servicio de Windows para monitoreo automático. Se ejecuta como un servicio del sistema operativo y registra eventos cada 30 segundos en un archivo de log.

## Compilar Localmente

```bash
dotnet build -c Release
```

## Instalar como Servicio de Windows

```powershell
# Desde PowerShell como administrador
sc.exe create TestingWindowsService binPath="C:\ruta\a\TestingWindowsService.exe"
```

O usando directamente:

```powershell
# Iniciar el servicio
net start TestingWindowsService

# Detener el servicio
net stop TestingWindowsService

# Desinstalar el servicio
sc.exe delete TestingWindowsService
```

## Verificar los Logs

Los logs se guardan en:
```
%APPDATA%\TestingWindowsService\service.log
```

Normalmente:
```
C:\Users\[NombreUsuario]\AppData\Roaming\TestingWindowsService\service.log
```

## Crear una Release en GitHub

1. Asegúrate de que todos los cambios estén committeados
2. Crea un tag con la versión:
   ```bash
   git tag -a v0.1.0 -m "Release version 0.1.0"
   git push origin v0.1.0
   ```
3. El workflow de GitHub Actions compilará automáticamente el proyecto y creará:
   - Un `.exe` compilado para instalación
   - Un `.zip` con todo lo necesario

## Características

- ✅ Se ejecuta como servicio de Windows
- ✅ Logs detallados guardados en `%APPDATA%`
- ✅ Compilación automática en releases
- ✅ ZIP descargable desde GitHub Actions
- ✅ Fácil instalación y desinstalación

## Requisitos

- Windows 10/11 o Windows Server
- .NET 8.0 Runtime

## Uso

El servicio se inicia automáticamente después de instalarlo y genera eventos de monitoreo cada 30 segundos en el archivo de log.
