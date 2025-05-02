export interface TokenConfig {
    loginType: 'Terminal' | 'Mitarbeiter' | 'Admin' | 'QR';
    validityDuration: number;
  }
  
  export interface SystemSettings {
    language: string;
    fontSize: 'small' | 'normal' | 'large';
    backgroundImageUrl?: string;
    autoSendReports: boolean;
    downloadOnly: boolean;
    tokenConfigs: TokenConfig[];
  }
  