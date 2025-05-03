import { createContext, useEffect, useState } from "react";
import { SystemSettings } from "../types/settingsTypes";
import { getToken } from "../utils/auth";

export const SystemSettingsContext = createContext<SystemSettings | null>(null);

export const SystemSettingsProvider = ({ children }: { children: React.ReactNode }) => {
  const [settings, setSettings] = useState<SystemSettings | null>(null);

  useEffect(() => {
    const token = getToken();
    if (!token) return; // Nur wenn eingeloggt
  
    const loadSettings = async () => {
      try {
        const res = await fetch("https://localhost:7123/api/settings", {
          headers: { Authorization: `Bearer ${token}` }
        });
        if (!res.ok) return;
  
        const data = await res.json();
        setSettings(data);
      } catch (err) {
        console.warn("SystemSettings konnten nicht geladen werden", err);
      }
    };
  
    loadSettings();
  }, []);
  
  useEffect(() => {
    // Schriftgröße anwenden, wenn Settings geladen sind
    if (settings?.fontSize) {
      document.body.classList.remove("font-size-small", "font-size-normal", "font-size-large");

      switch (settings.fontSize) {
        case 1:
          document.body.classList.add("font-size-small");
          break;
        case 2:
          document.body.classList.add("font-size-normal");
          break;
        case 3:
          document.body.classList.add("font-size-large");
          break;
      }
    }
  }, [settings?.fontSize]);

  

 // if (!settings) return null; // Optionale Ladeanzeige

  return (
    <SystemSettingsContext.Provider value={settings}>
      {children}
    </SystemSettingsContext.Provider>
  );
};
