import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import App from './App';
import './i18n';
import { getToken } from './utils/auth';
import i18n from './i18n';
import LanguageProvider from './i18n/LanguageProvider';
import { SystemSettingsProvider } from './context/SystemSettingsProvider';
import "./styles/fontSizes.css";



async function startApp() {
  const token = getToken();

  if (token) {
    try {
      const res = await fetch("https://localhost:7123/api/settings", {
        headers: { Authorization: `Bearer ${token}` }
      });

      if (res.ok) {
        const data = await res.json();
        const lang = data.language || 'de';
        await i18n.changeLanguage(lang);
        console.log("Sprache gesetzt auf:", lang);
      } else {
        console.warn("Antwort nicht ok, Standardsprache wird verwendet.");
      }
    } catch (err) {
      console.warn("Fehler beim Laden der Sprache:", err);
    }
  } else {
    console.log("Kein Token vorhanden – Sprache wird nicht von API geladen.");
  }

  createRoot(document.getElementById("root")!).render(
    <StrictMode>
      <LanguageProvider>
        <SystemSettingsProvider>
          <App />
        </SystemSettingsProvider>
      </LanguageProvider>
    </StrictMode>
  );
}

startApp();

