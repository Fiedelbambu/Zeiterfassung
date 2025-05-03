import { createContext, useState } from "react";
import { SystemSettings } from "../types/settingsTypes";

export const SystemSettingsContext = createContext<{
  settings: SystemSettings | null;
  setSettings: (settings: SystemSettings) => void;
} | null>(null);

export const SystemSettingsProvider = ({ children }: { children: React.ReactNode }) => {
  const [settings, setSettings] = useState<SystemSettings | null>(null);

  return (
    <SystemSettingsContext.Provider value={{ settings, setSettings }}>
      {children}
    </SystemSettingsContext.Provider>
  );
};
