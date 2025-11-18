# CHANGELOG

## [0.1.0] - 2025-01-18

### Added
- Initial release
- Windows Service implementation with .NET 8
- Automatic logging to `%APPDATA%\TestingWindowsService\service.log`
- GitHub Actions workflow for automated releases
- Installation and uninstallation scripts
- Support for 30-second monitoring intervals

### Features
- Runs as Windows service
- Logs events every 30 seconds
- Automatic ZIP packaging on releases
- Easy setup via batch scripts
