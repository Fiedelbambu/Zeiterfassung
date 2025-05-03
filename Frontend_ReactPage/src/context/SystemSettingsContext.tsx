// src/context/SystemSettingsContext.tsx
import React, { createContext, useContext, useEffect, useState } from 'react';
import { SystemSettings } from '../types/settingsTypes';
import { getToken } from '../utils/auth';

interface SystemSettingsContextType {
  settings: SystemSettings | null;
  updateSettings: (newSettings: SystemSettings) => void;
}

const SystemSettingsContext = createContext<SystemSettingsContextType>({
  settings: null,
  updateSettings: () => {},
});

export const useSystemSettings = () => useContext(SystemSettingsContext);

export const SystemSettingsProvider = ({ children }: { children: React.ReactNode }) => {
  const [settings, setSettings] = useState<SystemSettings | null>(null);

  useEffect(() => {
    const fetchSettings = async () => {
      try {
        const res = await fetch("https://localhost:7123/api/settings", {
          headers: { Authorization: `Bearer ${getToken()}` },
        });
        if (res.ok) {
          const data = await res.json();
          setSettings(data);
        }
      } catch (err) {
        console.warn("Fehler beim Laden der Systemeinstellungen", err);
      }
    };
    fetchSettings();
  }, []);

  const updateSettings = (newSettings: SystemSettings) => {
    setSettings(newSettings);
  };

  return (
    <SystemSettingsContext.Provider value={{ settings, updateSettings }}>
      {children}
    </SystemSettingsContext.Provider>
  );
};
