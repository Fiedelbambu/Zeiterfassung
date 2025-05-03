import { createContext } from 'react';

export const LanguageContext = createContext<{
  language: string;
  setLanguage: (lang: string) => void;
}>({
  language: 'de',
  setLanguage: () => {},
});
