import { createContext, useEffect, useState } from "react";
import { SystemSettings } from "../types/settingsTypes";
import { getToken } from "../utils/auth";

export const SystemSettingsContext = createContext<SystemSettings | null>(null);

export const SystemSettingsProvider = ({ children }: { children: React.ReactNode }) => {
  const [settings, setSettings] = useState<SystemSettings | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = getToken();
    if (!token) {
      setLoading(false); // Kein Token → kein Versuch
      return;
    }

    const loadSettings = async () => {
      try {
        const res = await fetch("https://localhost:7123/api/settings", {
          headers: { Authorization: `Bearer ${token}` }
        });
        if (!res.ok) {
          setLoading(false);
          return;
        }

        const data = await res.json();
        setSettings(data);
      } catch (err) {
        console.warn("SystemSettings konnten nicht geladen werden", err);
      } finally {
        setLoading(false);
      }
    };

    loadSettings();
  }, []);

  // Schriftgröße anwenden
  useEffect(() => {
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

  //Hintergrundbild anwenden
  useEffect(() => {
    if (settings?.backgroundImageUrl) {
      document.body.style.backgroundImage = `url(${settings.backgroundImageUrl})`;
      document.body.style.backgroundSize = "cover";
      document.body.style.backgroundRepeat = "no-repeat";
      document.body.style.backgroundPosition = "center center";
    }
  }, [settings?.backgroundImageUrl]);
  

  // Anzeige für Ladephase
  if (loading) return <div>Lade Systemeinstellungen...</div>;

  // Wenn kein Token oder keine Settings verfügbar
  if (!settings) return <>{children}</>;

  return (
    <SystemSettingsContext.Provider value={settings}>
      {children}
    </SystemSettingsContext.Provider>
  );
};
