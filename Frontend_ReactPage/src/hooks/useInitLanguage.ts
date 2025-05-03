import { useEffect, useRef } from "react";
import i18n from "../i18n";
import { getToken } from "../utils/auth";

// Globale Flag, um erneutes Laden zu verhindern
let languageInitialized = false;

export const useInitLanguage = () => {
  const isMounted = useRef(false);

  useEffect(() => {
    if (languageInitialized || isMounted.current) return;
    isMounted.current = true;

    const token = getToken();
    const localLang = localStorage.getItem("language");

    const loadLanguage = async () => {
      if (!token && localLang) {
        await i18n.changeLanguage(localLang);
        console.log("Sprache aus localStorage:", localLang);
        languageInitialized = true;
        return;
      }

      try {
        const res = await fetch("https://localhost:7123/api/settings", {
          headers: { Authorization: `Bearer ${token}` },
        });

        if (res.ok) {
          const data = await res.json();
          const lang = data.language || "de";
          await i18n.changeLanguage(lang);
          localStorage.setItem("language", lang);
          console.log("Sprache aus API:", lang);
          languageInitialized = true;
        } else {
          console.warn("Backendantwort nicht ok. Standardsprache wird verwendet.");
        }
      } catch (err) {
        console.warn("Sprache konnte nicht geladen werden:", err);
      }
    };

    loadLanguage();
  }, []);
};
