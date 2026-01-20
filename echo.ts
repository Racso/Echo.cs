/**
 * Echo - A flexible logging library
 * TypeScript port of Echo.cs core functionality (excluding Unity-specific features)
 */

// ============================================================================
// Enums
// ============================================================================

export enum LogLevel {
    None = 0,
    Error = 1,
    Warn = 2,
    Info = 3,
    Debug = 4
}

enum LogMode {
    Always = 0,
    Once = 1
}

export enum SystemColor {
    None,
    LabelOnly,
    LabelAndMessage
}

// ============================================================================
// Interfaces
// ============================================================================

export interface EchoLogWriter {
    writeLog(level: LogLevel, system: string, message: string): void;
}

// ============================================================================
// Configuration
// ============================================================================

export class LogWriterConfig {
    timestamp: boolean = true;
    levelLabels: boolean = true;
    levelColors: boolean = true;
    systemColor: SystemColor = SystemColor.LabelOnly;
}

// ============================================================================
// Helpers
// ============================================================================

class Helpers {
    static fnv1a32(str: string, hash: number = 2166136261): number {
        for (let i = 0; i < str.length; i++) {
            hash ^= str.charCodeAt(i);
            hash = Math.imul(hash, 16777619);
        }
        return hash >>> 0; // Convert to unsigned 32-bit integer
    }

    static getElementFromHash<T>(collection: T[], stringToHash: string): T {
        const hash = Helpers.fnv1a32(stringToHash);
        const index = hash % collection.length;
        return collection[index];
    }
}

class WritersHelpers {
    static getLevelLabel(level: LogLevel): string {
        switch (level) {
            case LogLevel.Debug: return "DEBUG";
            case LogLevel.Info: return "INFO";
            case LogLevel.Warn: return "WARN";
            case LogLevel.Error: return "ERROR";
            default: return "???";
        }
    }

    static readonly systemColors: string[] = [
        "\x1b[31m", // Red
        "\x1b[32m", // Green
        "\x1b[34m", // Blue
        "\x1b[33m", // Yellow
        "\x1b[36m", // Cyan
        "\x1b[35m"  // Magenta
    ];

    static readonly levelColors: Map<LogLevel, string> = new Map([
        [LogLevel.Debug, "\x1b[37m"],  // White
        [LogLevel.Info, "\x1b[36m"],   // Cyan
        [LogLevel.Warn, "\x1b[33m"],   // Yellow
        [LogLevel.Error, "\x1b[31m"]   // Red
    ]);

    static readonly resetColor = "\x1b[0m";
    static readonly grayColor = "\x1b[90m";
}

// ============================================================================
// Internal Classes
// ============================================================================

class HashesManager {
    private hashes: Set<number> = new Set();

    tryAdd(system: string, message: string): boolean {
        let hash = Helpers.fnv1a32(message);
        hash = Helpers.fnv1a32(system, hash);
        if (this.hashes.has(hash)) {
            return false;
        }
        this.hashes.add(hash);
        return true;
    }

    clear(): void {
        this.hashes.clear();
    }
}

class LoggerCore {
    private readonly logWriter: EchoLogWriter;
    private readonly echoSettings: EchoSettings;
    private readonly hashes: HashesManager;

    constructor(config: EchoSettings, hashes: HashesManager, logger: EchoLogWriter) {
        if (!config) throw new Error("config cannot be null");
        if (!hashes) throw new Error("hashes cannot be null");
        if (!logger) throw new Error("logger cannot be null");
        
        this.echoSettings = config;
        this.hashes = hashes;
        this.logWriter = logger;
    }

    private isEnabled(system: string, level: LogLevel): boolean {
        return level <= this.echoSettings.getSystemLevel(system);
    }

    private shouldLogOnce(system: string, message: string): boolean {
        return this.hashes.tryAdd(system, message);
    }

    clearHashes(): void {
        this.hashes.clear();
    }

    private write(level: LogLevel, mode: LogMode, system: string, message: string): void {
        if (mode === LogMode.Always || this.shouldLogOnce(system, message)) {
            this.logWriter.writeLog(level, system, message);
        }
    }

    writeIfEnabled(level: LogLevel, mode: LogMode, system: string, message: string): void;
    writeIfEnabled(level: LogLevel, mode: LogMode, system: string, format: string, ...params: any[]): void;
    writeIfEnabled(level: LogLevel, mode: LogMode, system: string, formatOrMessage: string, ...params: any[]): void {
        if (this.isEnabled(system, level)) {
            let message: string;
            if (params.length > 0) {
                // Format the string using the parameters
                message = this.formatString(formatOrMessage, ...params);
            } else {
                message = formatOrMessage;
            }
            this.write(level, mode, system, message);
        }
    }

    private formatString(format: string, ...params: any[]): string {
        return format.replace(/\{(\d+)\}/g, (match, index) => {
            const idx = parseInt(index);
            return idx < params.length ? String(params[idx]) : match;
        });
    }
}

// ============================================================================
// Public Classes
// ============================================================================

export class EchoSettings {
    private systemLevels: Map<string, LogLevel> = new Map();
    private _defaultLevel: LogLevel = LogLevel.Warn;
    private updateCallbacks: (() => void)[] = [];

    get defaultLevel(): LogLevel {
        return this._defaultLevel;
    }

    onUpdated(callback: () => void): void {
        this.updateCallbacks.push(callback);
    }

    private triggerUpdate(): void {
        for (const callback of this.updateCallbacks) {
            callback();
        }
    }

    setSystemLevel(system: string, level: LogLevel): void {
        this.throwIfInvalidSystem(system);
        this.systemLevels.set(system, level);
        this.triggerUpdate();
    }

    clearSystemLevel(system: string): void {
        this.throwIfInvalidSystem(system);
        if (this.systemLevels.delete(system)) {
            this.triggerUpdate();
        }
    }

    getSystemLevel(system: string): LogLevel {
        this.throwIfInvalidSystem(system);
        return this.systemLevels.get(system) ?? this._defaultLevel;
    }

    tryGetSystemLevel(system: string): { success: boolean; level?: LogLevel } {
        this.throwIfInvalidSystem(system);
        const level = this.systemLevels.get(system);
        return level !== undefined ? { success: true, level } : { success: false };
    }

    clearSystemLevels(): void {
        this.systemLevels.clear();
        this.triggerUpdate();
    }

    setDefaultLevel(level: LogLevel): void {
        this._defaultLevel = level;
        this.triggerUpdate();
    }

    getAllSystemLevels(): ReadonlyMap<string, LogLevel> {
        return this.systemLevels;
    }

    private throwIfInvalidSystem(system: string): void {
        if (!system || system.length === 0) {
            throw new Error("System name cannot be null or empty.");
        }
    }
}

export class EchoLogger {
    private readonly loggerCore: LoggerCore;

    constructor(loggerCore: LoggerCore) {
        if (!loggerCore) throw new Error("loggerCore cannot be null");
        this.loggerCore = loggerCore;
    }

    // Debug Methods
    debug(system: string, message: string): void;
    debug(system: string, format: string, ...params: any[]): void;
    debug(system: string, formatOrMessage: string, ...params: any[]): void {
        this.loggerCore.writeIfEnabled(LogLevel.Debug, LogMode.Always, system, formatOrMessage, ...params);
    }

    debug1(system: string, message: string): void;
    debug1(system: string, format: string, ...params: any[]): void;
    debug1(system: string, formatOrMessage: string, ...params: any[]): void {
        this.loggerCore.writeIfEnabled(LogLevel.Debug, LogMode.Once, system, formatOrMessage, ...params);
    }

    // Info Methods
    info(system: string, message: string): void;
    info(system: string, format: string, ...params: any[]): void;
    info(system: string, formatOrMessage: string, ...params: any[]): void {
        this.loggerCore.writeIfEnabled(LogLevel.Info, LogMode.Always, system, formatOrMessage, ...params);
    }

    info1(system: string, message: string): void;
    info1(system: string, format: string, ...params: any[]): void;
    info1(system: string, formatOrMessage: string, ...params: any[]): void {
        this.loggerCore.writeIfEnabled(LogLevel.Info, LogMode.Once, system, formatOrMessage, ...params);
    }

    // Warn Methods
    warn(system: string, message: string): void;
    warn(system: string, format: string, ...params: any[]): void;
    warn(system: string, formatOrMessage: string, ...params: any[]): void {
        this.loggerCore.writeIfEnabled(LogLevel.Warn, LogMode.Always, system, formatOrMessage, ...params);
    }

    warn1(system: string, message: string): void;
    warn1(system: string, format: string, ...params: any[]): void;
    warn1(system: string, formatOrMessage: string, ...params: any[]): void {
        this.loggerCore.writeIfEnabled(LogLevel.Warn, LogMode.Once, system, formatOrMessage, ...params);
    }

    // Error Methods
    error(system: string, message: string): void;
    error(system: string, format: string, ...params: any[]): void;
    error(system: string, formatOrMessage: string, ...params: any[]): void {
        this.loggerCore.writeIfEnabled(LogLevel.Error, LogMode.Always, system, formatOrMessage, ...params);
    }

    error1(system: string, message: string): void;
    error1(system: string, format: string, ...params: any[]): void;
    error1(system: string, formatOrMessage: string, ...params: any[]): void {
        this.loggerCore.writeIfEnabled(LogLevel.Error, LogMode.Once, system, formatOrMessage, ...params);
    }
}

export class EchoSystemLogger {
    private readonly loggerCore: LoggerCore;
    private readonly system: string;

    constructor(loggerCore: LoggerCore, system: string) {
        if (!loggerCore) throw new Error("loggerCore cannot be null");
        if (!system) throw new Error("system cannot be null");
        this.loggerCore = loggerCore;
        this.system = system;
    }

    // Debug Methods
    debug(message: string): void;
    debug(format: string, ...params: any[]): void;
    debug(formatOrMessage: string, ...params: any[]): void {
        this.loggerCore.writeIfEnabled(LogLevel.Debug, LogMode.Always, this.system, formatOrMessage, ...params);
    }

    debug1(message: string): void;
    debug1(format: string, ...params: any[]): void;
    debug1(formatOrMessage: string, ...params: any[]): void {
        this.loggerCore.writeIfEnabled(LogLevel.Debug, LogMode.Once, this.system, formatOrMessage, ...params);
    }

    // Info Methods
    info(message: string): void;
    info(format: string, ...params: any[]): void;
    info(formatOrMessage: string, ...params: any[]): void {
        this.loggerCore.writeIfEnabled(LogLevel.Info, LogMode.Always, this.system, formatOrMessage, ...params);
    }

    info1(message: string): void;
    info1(format: string, ...params: any[]): void;
    info1(formatOrMessage: string, ...params: any[]): void {
        this.loggerCore.writeIfEnabled(LogLevel.Info, LogMode.Once, this.system, formatOrMessage, ...params);
    }

    // Warn Methods
    warn(message: string): void;
    warn(format: string, ...params: any[]): void;
    warn(formatOrMessage: string, ...params: any[]): void {
        this.loggerCore.writeIfEnabled(LogLevel.Warn, LogMode.Always, this.system, formatOrMessage, ...params);
    }

    warn1(message: string): void;
    warn1(format: string, ...params: any[]): void;
    warn1(formatOrMessage: string, ...params: any[]): void {
        this.loggerCore.writeIfEnabled(LogLevel.Warn, LogMode.Once, this.system, formatOrMessage, ...params);
    }

    // Error Methods
    error(message: string): void;
    error(format: string, ...params: any[]): void;
    error(formatOrMessage: string, ...params: any[]): void {
        this.loggerCore.writeIfEnabled(LogLevel.Error, LogMode.Always, this.system, formatOrMessage, ...params);
    }

    error1(message: string): void;
    error1(format: string, ...params: any[]): void;
    error1(formatOrMessage: string, ...params: any[]): void {
        this.loggerCore.writeIfEnabled(LogLevel.Error, LogMode.Once, this.system, formatOrMessage, ...params);
    }
}

export class Echo {
    private readonly loggerCore: LoggerCore;
    private readonly loggers: Map<string, EchoLogger | EchoSystemLogger> = new Map();
    private readonly _settings: EchoSettings;

    constructor(writer: EchoLogWriter) {
        if (!writer) throw new Error("writer cannot be null");
        const hashes = new HashesManager();
        this._settings = new EchoSettings();
        this.loggerCore = new LoggerCore(this._settings, hashes, writer);
    }

    getLogger(): EchoLogger {
        const key = "";
        if (!this.loggers.has(key)) {
            this.loggers.set(key, new EchoLogger(this.loggerCore));
        }
        return this.loggers.get(key) as EchoLogger;
    }

    getSystemLogger(system: string): EchoSystemLogger {
        if (!system || system.length === 0) {
            throw new Error("System name cannot be null or empty.");
        }

        if (!this.loggers.has(system)) {
            this.loggers.set(system, new EchoSystemLogger(this.loggerCore, system));
        }
        return this.loggers.get(system) as EchoSystemLogger;
    }

    get settings(): EchoSettings {
        return this._settings;
    }
}

// ============================================================================
// Built-in Log Writers
// ============================================================================

class ConsoleLogWriter implements EchoLogWriter {
    private readonly config: LogWriterConfig;

    constructor(config: LogWriterConfig) {
        if (!config) throw new Error("config cannot be null");
        this.config = config;
    }

    writeLog(level: LogLevel, system: string, message: string): void {
        let output = "";

        if (this.config.timestamp) {
            const now = new Date();
            const timestamp = this.formatTimestamp(now);
            output += `${WritersHelpers.grayColor}[${timestamp}]${WritersHelpers.resetColor} `;
        }

        if (this.config.levelLabels) {
            const levelLabel = WritersHelpers.getLevelLabel(level);
            if (this.config.levelColors) {
                const color = WritersHelpers.levelColors.get(level) ?? "";
                output += `${color}[${levelLabel}]${WritersHelpers.resetColor} `;
            } else {
                output += `[${levelLabel}] `;
            }
        }

        const systemColor = this.config.systemColor === SystemColor.LabelOnly || 
                          this.config.systemColor === SystemColor.LabelAndMessage
                          ? Helpers.getElementFromHash(WritersHelpers.systemColors, system)
                          : "";

        if (systemColor) {
            output += `${systemColor}[${system}]${WritersHelpers.resetColor} `;
        } else {
            output += `[${system}] `;
        }

        if (this.config.systemColor === SystemColor.LabelAndMessage && systemColor) {
            output += `${systemColor}${message}${WritersHelpers.resetColor}`;
        } else {
            output += message;
        }

        console.log(output);
    }

    private formatTimestamp(date: Date): string {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        const hours = String(date.getHours()).padStart(2, '0');
        const minutes = String(date.getMinutes()).padStart(2, '0');
        const seconds = String(date.getSeconds()).padStart(2, '0');
        const milliseconds = String(date.getMilliseconds()).padStart(3, '0');
        return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}.${milliseconds}`;
    }
}

// ============================================================================
// Public API Helpers
// ============================================================================

export class EchoConsole {
    static new(config?: LogWriterConfig): Echo {
        const writerConfig = config ?? new LogWriterConfig();
        const writer = new ConsoleLogWriter(writerConfig);
        return new Echo(writer);
    }
}
