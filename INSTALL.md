# Scripts de instalación

## install.bat

Script para instalar el servicio de Windows automáticamente.

**Requisitos:**
- Ejecutar como administrador
- Tener el archivo `TestingWindowsService.exe` compilado

**Uso:**
```bash
install.bat "C:\ruta\a\TestingWindowsService.exe"
```

El script:
1. Registra el servicio en Windows
2. Lo configura para iniciar automáticamente
3. Lo inicia inmediatamente

## uninstall.bat

Script para desinstalar y detener el servicio.

**Uso:**
```bash
uninstall.bat
```

El script:
1. Detiene el servicio
2. Lo desregistra de Windows
