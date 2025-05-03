import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import App from './App';
import './i18n';
import { getToken } from './utils/auth';
import i18n from './i18n';
import LanguageProvider from './i18n/LanguageProvider';

async function startApp() {
  try {
    const res = await fetch("https://localhost:7123/api/settings", {
      headers: {
        Authorization: `Bearer ${getToken()}`
      },
    });

    if (res.ok) {
      const data = await res.json();
      const lang = data.language || 'de';
      await i18n.changeLanguage('de');
      console.log("Sprache gesetzt auf:", lang);
    } else {
      console.warn("Antwort nicht ok, Standardsprache wird verwendet.");
    }
  } catch (err) {
    console.warn("Fehler beim Laden der Sprache:", err);
  }


  createRoot(document.getElementById("root")!).render(
    <StrictMode>
      <LanguageProvider>
        <App />
      </LanguageProvider>
    </StrictMode>
  );
}

startApp();
