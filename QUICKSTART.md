# Guía Rápida - TestingWindowsService

## 📦 Estructura del Proyecto

```
TestingWindowsService/
├── .github/workflows/
│   └── release.yml          # Workflow automático de releases
├── Program.cs               # Código principal del servicio
├── TestingWindowsService.csproj
├── install.bat              # Script para instalar el servicio
├── uninstall.bat            # Script para desinstalar el servicio
├── README.md                # Documentación principal
└── CHANGELOG.md             # Historial de cambios
```

## 🚀 Inicio Rápido

### 1. Compilar Localmente

```bash
dotnet build -c Release
```

### 2. Instalar como Servicio

```powershell
# Como administrador
.\install.bat ".\bin\Release\net8.0-windows\TestingWindowsService.exe"
```

### 3. Verificar que está corriendo

```powershell
# Ver servicios
Get-Service TestingWindowsService

# Ver logs
Get-Content "$env:APPDATA\TestingWindowsService\service.log" -Wait
```

### 4. Desinstalar

```powershell
.\uninstall.bat
```

## 📦 Crear una Release en GitHub

1. Hacer commit de los cambios:
```bash
git add .
git commit -m "Update service v0.2.0"
```

2. Crear tag:
```bash
git tag -a v0.2.0 -m "Release version 0.2.0"
git push origin v0.2.0
```

3. El workflow de GitHub Actions se ejecutará automáticamente y:
   - Compilará el proyecto
   - Creará un ZIP
   - Subirá ambos assets a la release

## 📋 Logs

Los logs se guardan en:
```
%APPDATA%\TestingWindowsService\service.log
```

Ejemplo en Windows 11:
```
C:\Users\[TuNombre]\AppData\Roaming\TestingWindowsService\service.log
```

Contenido del log:
```
[2025-01-18 10:30:45] [Information] Service.TestingWindowsService - Servicio iniciado
[2025-01-18 10:31:15] [Information] Service.TestingWindowsService - [Evento #1] Servicio monitorando...
[2025-01-18 10:31:45] [Information] Service.TestingWindowsService - [Evento #2] Servicio monitorando...
```

## ⚙️ Configuración

Para cambiar el intervalo de monitoreo, edita `Program.cs`:

```csharp
await Task.Delay(30000, stoppingToken); // Cambiar 30000 por ms deseados
```

## 🔧 Troubleshooting

### El servicio no inicia
- Verifica que tienes permisos de administrador
- Comprueba que el path del EXE es correcto en el install.bat

### Los logs no aparecen
- Verifica que `%APPDATA%\TestingWindowsService\` existe
- Revisa permisos de escritura en esa carpeta

### Error "El servicio ya existe"
```powershell
sc.exe delete TestingWindowsService
# Luego ejecutar install.bat de nuevo
```

## 📝 Notas

- El servicio se ejecuta con permisos del usuario Local System
- Inicia automáticamente en boot
- Se configura como servicio automático
