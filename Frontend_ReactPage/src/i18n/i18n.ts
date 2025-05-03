import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

import translationDE from './locales/de.json';
import translationEN from './locales/en.json';

i18n
  .use(initReactI18next)
  .init({
    resources: {
      de: { translation: translationDE },
      en: { translation: translationEN },
    },
    lng: 'en',
    fallbackLng: 'en',
    interpolation: { escapeValue: false  },
  });

export default i18n;
