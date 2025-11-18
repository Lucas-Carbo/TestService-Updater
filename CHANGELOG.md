# CHANGELOG

## [0.3.4] - 2025-11-18

### Added
- Enhanced console output with decorative borders and ASCII boxes
- Memory usage monitoring in each event log
- Event counter in formatted output
- Detailed startup and shutdown messages
- Better log formatting with visual separators

### Fixed
- Changed log directory from `%APPDATA%` to `C:\ProgramData` for proper Local System access
- Improved service startup information display

### Improved
- Better logging clarity with visual hierarchy
- More informative event logging with timestamps and memory usage
- Cleaner service lifecycle messages

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
