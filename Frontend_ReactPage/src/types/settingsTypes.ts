
// This file defines the types used in the settings module of the application.
//         body: JSON.stringify(settings),
export type FontSizeOption = 'small' | 'normal' | 'large';

export type LoginMethod = 'Passwort' | 'Terminal' | 'NFC' | 'QR' | 'App';

export interface TokenConfig {
  loginType: LoginMethod;
  validityDuration: string; // Format: "HH:mm:ss"
}

export interface SystemSettings {
  language: string;
  fontSize: FontSizeOption;
  backgroundImageUrl?: string;
  autoSendReports: boolean;
  downloadOnly: boolean;
  sendOnDay: number;
  reportWithSignatureField: boolean;
  tokenConfigs: TokenConfig[];
  tokenMaxLifetime: string;
  qrTokenSingleUse: boolean;
  enableReminder: boolean;
  remindAfterDays: number;
  errorTypesToCheck: string[];
  holidaySource: string;
  holidayRegionCode: string;
  autoSyncHolidays: boolean;
}

  
  